using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AnimatedPixelPack
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Character : MonoBehaviour
    {
        // Editor Properties
        [Header("Character")]
        [Tooltip("Transform used to check if the character is touching the ground")]
        public Transform GroundChecker;
        [Tooltip("Layer that contains all the 'ground' colliders")]
        public LayerMask GroundLayer;
        [Tooltip("Speed of the character when running")]
        public float RunSpeed = 250;
        [Tooltip("Factor of run speed to lose(-)/gain(+) when pressing the modifier. Use for run boost or sneak.")]
        [Range(-1, 1)]
        public float RunModifierFactor = -0.75f;
        [Tooltip("Factor of velocity to lose(-)/gain(+) when blocking (if enabled)")]
        [Range(-1, 1)]
        public float BlockingMoveFactor = -0.75f;
        [Tooltip("Should the character be allowed to double jump")]
        public bool AllowDoubleJump = true;
        [Tooltip("Should the character be allowed to control direction while in the air")]
        public bool AllowAirControl = true;
        [Tooltip("Force applied to character when pressing jump")]
        public float JumpPower = 550;
        [Tooltip("Should the character be allowed to jump while sliding down a wall")]
        public bool AllowWallJump = true;
        [Tooltip("Transform used to check if the character is sliding down a wall forwards")]
        public Transform WallCheckerFront;
        [Tooltip("Transform used to check if the character is sliding down a wall backwards")]
        public Transform WallCheckerBack;
        [Tooltip("Layer that contains all the 'wall' colliders")]
        public LayerMask WallLayer;
        [Tooltip("Factor of velocity to lose(-)/gain(+) when sliding down a wall")]
        [Range(-1, 1)]
        public float WallSlideFactor = -0.25f;
        [Tooltip("Force applied to character horizontally when jumping off a wall")]
        public float WallJumpHorizontalPower = 100;
        [Tooltip("Allow the character to jump down oneway platforms (containing a PlatformEffector2D with oneway set to true)")]
        public bool AllowJumpDownPlatforms = true;
        [Tooltip("The time to wait after jumping down before re-enabling the platform")]
        public float JumpDownTimeout = 1;
        [Tooltip("Allow the character to climb ladder trigger colliders (named or tagged 'ladder')")]
        public bool UseLadders = true;
        [Tooltip("Allow the character to move slower in water trigger colliders (named or tagged 'water')")]
        public bool UseWater = true;
        [Tooltip("Factor of velocity to lose(-)/gain(+) when moving in water")]
        [Range(-1, 1)]
        public float WaterMoveFactor = -0.75f;
        [Tooltip("Allow the character to slide on ice trigger colliders (named or tagged 'ice')")]
        public bool UseIce = true;
        [Tooltip("Amount of friction to use for ice")]
        public float IceFriction = 1f;
        [Tooltip("Should the character ignore all the mecanim animation states and remain static (useful for character select screens)")]
        public bool IgnoreAnimationStates = false;
        [Tooltip("Health of the character")]
        public int MaxHealth = 100;
        [Tooltip("Gravity scale applied to the RigidBody2D on start up")]
        public float GravityScale = 3;
        [Tooltip("Is the character a zombie")]
        public bool IsZombified = false;
        [Tooltip("A particle system to spawn when entering water")]
        public ParticleSystem SplashEmitter;
        [Tooltip("A particle system to start when underwater")]
        public ParticleSystem BubbleEmitter;
        [Tooltip("A particle system to spawn when jumping")]
        public ParticleSystem DustEmitter;
        [Tooltip("The velocity the character must be travelling to create a dust cloud when landing")]
        public float DustCloudThreshold = -10;

        [Header("Weapon")]
        [Tooltip("Type of weapon character is carrying. Used in animations.")]
        public WeaponType EquippedWeaponType;
        [Tooltip("Can the character block?")]
        public bool IsBlockEnabled = false;
        [Tooltip("Transform position used as the spawn point for projectiles")]
        public Transform LaunchPoint;
        [Tooltip("Projectile to spawn when casting")]
        public WeaponProjectile CastProjectile;
        [Tooltip("Projectile to spawn when throwing from main hand")]
        public WeaponProjectile ThrowMainProjectile;
        [Tooltip("Projectile to spawn when throwing from off hand")]
        public WeaponProjectile ThrowOffProjectile;
        [Tooltip("Transform position used to spawn an effect")]
        public Transform EffectPoint;
        [Tooltip("Type of effect to spawn during cast")]
        public WeaponEffect Effect;

        // Script Properties
        public int CurrentHealth { get; private set; }
        public bool IsDead { get { return this.CurrentHealth <= 0; } }
        public Direction CurrentDirection { get; private set; }
        public float ModifiedSpeed
        {
            get
            {
                return this.RunSpeed * this.GetMultiplier(this.RunModifierFactor);
            }
        }
        public bool IsAttacking
        {
            get
            {
                AnimatorStateInfo state = this.animatorObject.GetCurrentAnimatorStateInfo(3);
                return state.IsName("Attack") || state.IsName("Quick Attack");
            }
        }

        public enum WeaponType
        {
            None = 0,
            Staff = 1,
            Sword = 2,
            Bow = 3,
            Gun = 4
        }

        public enum Direction
        {
            Left = -1,
            Right = 1
        }

        [Flags]
        public enum Action
        {
            Jump = 1,
            RunModified = 2,
            QuickAttack = 4,
            Attack = 8,
            Cast = 16,
            ThrowOff = 32,
            ThromMain = 64,
            Consume = 128,
            Block = 256,
            Hurt = 512,
            JumpDown = 1024,
            Crouch = 2048
        }

        // Members
        private Animator animatorObject;
        private Rigidbody2D body2D;
        private bool isGrounded = true;
        private bool isOnWall = false;
        private bool isOnWallFront = false;
        private bool isOnLadder = false;
        private bool isInWater = false;
        private bool isOnIce = false;
        private bool isBlocking = false;
        private bool isCrouching = false;
        private bool isJumpPressed;
        private bool isJumpingDown;
        private bool isReadyForDust;
        private int jumpCount = 0;
        private bool isRunningNormal = false;
        private float groundRadius = 0.1f;
        private float wallDecayX = 0.006f;
        private float wallJumpX = 0;
        private WeaponEffect activeEffect;
        private Direction startDirection = Direction.Right;

        /// <summary>
        /// Instantiate a new character with the supplied parameters
        /// </summary>
        /// <param name="instance">The instance to use as the base</param>
        /// <param name="startDirection">Direction the new character should be facing</param>
        /// <param name="position">The position to spawn at</param>
        /// <returns>The new character</returns>
        public static Character Create(Character instance, Direction startDirection, Vector3 position)
        {
            Character c = GameObject.Instantiate<Character>(instance);
            c.transform.position = position;
            c.startDirection = startDirection;
            return c;
        }

        void Start()
        {
            // Grab the editor objects
            this.body2D = this.GetComponent<Rigidbody2D>();
            this.animatorObject = this.GetComponent<Animator>();

            // Apply the gravity scale because 2D physics jumping look too floaty without extra gravity
            if (this.body2D != null)
            {
                this.body2D.gravityScale = this.GravityScale;
            }

            // Setup the character
            this.CurrentHealth = this.MaxHealth;
            this.ApplyDamage(0);
            this.body2D.centerOfMass = new Vector2(0f, 0.4f);
            if (this.startDirection != Direction.Right)
            {
                this.ChangeDirection(this.startDirection);
            }
            else
            {
                this.CurrentDirection = this.startDirection;
            }

            // Setup the underwater FX
            if (this.BubbleEmitter != null && this.BubbleEmitter.isPlaying)
            {
                this.BubbleEmitter.Stop();
                this.BubbleEmitter.gameObject.SetActive(false);
            }

            // Warn the user if they have forgotten to setup the layers correctly
            if ((this.GroundLayer & (1 << this.gameObject.layer)) != 0)
            {
                Debug.LogWarningFormat(this, "The character has its GroundLayer set incorrectly.\r\nThe GroundLayer matches the Character's main Layer, so it will not jump/fall correctly\r\nPlease update either the GroundLayer or the Layer of the character.");
            }

            // Perform an initial ground check
            this.isGrounded = this.CheckGround();
        }

        void FixedUpdate()
        {
            // Check if we are touching the ground using the rigidbody
            this.isGrounded = this.CheckGround();

            // Check if we are touching a wall when wall jump is allowed
            bool isOnWallFront = false;
            bool isOnWallBack = false;
            if (this.AllowWallJump && !this.isGrounded && this.body2D.velocity.y <= 0)
            {
                isOnWallFront = Physics2D.OverlapCircle(WallCheckerFront.position, this.groundRadius, this.WallLayer);
                isOnWallBack = Physics2D.OverlapCircle(WallCheckerBack.position, this.groundRadius, this.WallLayer);
            }

            this.isOnWall = (isOnWallFront || isOnWallBack);
            this.isOnWallFront = (this.isOnWall && isOnWallFront);
        }

        void OnCollisionEnter2D(Collision2D c)
        {
            // Check if we hit the ground (going downwards)
            if ((this.GroundLayer.value & 1 << c.gameObject.layer) != 0 &&
                c.contacts[0].normal.y > 0.8f)
            {
                // If we did, check to see if we either just jumped or are falling fast enough for dust
                if (this.isReadyForDust || this.body2D.velocity.y < this.DustCloudThreshold)
                {
                    this.CreateDustCloud(c.contacts[0].point);
                }

                // We hit the ground after jumping
                this.isReadyForDust = false;
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            // Check if they are splashing into water
            if (this.UseWater && this.IsTrigger(other.gameObject, "water"))
            {
                Vector2 pos = this.GroundChecker.position;
                pos.y = other.bounds.center.y + other.bounds.extents.y;
                this.CreateSplash(pos);

                // Play bubbles
                if (this.BubbleEmitter != null && !this.BubbleEmitter.isPlaying)
                {
                    this.BubbleEmitter.gameObject.SetActive(true);
                    this.BubbleEmitter.Play();
                }
            }
        }

        void OnTriggerStay2D(Collider2D other)
        {
            // Check for ladders
            if (this.UseLadders && this.IsTrigger(other.gameObject, "ladder"))
            {
                this.body2D.isKinematic = true;
                this.isOnLadder = true;
                this.animatorObject.SetBool("IsOnLadder", this.isOnLadder);
            }

            // Check for water
            if (this.UseWater && this.IsTrigger(other.gameObject, "water"))
            {
                this.isInWater = true;
                this.animatorObject.SetBool("IsInWater", this.isInWater);
            }

            // Check for ice
            if (this.UseIce && this.IsTrigger(other.gameObject, "ice"))
            {
                this.isOnIce = true;
                this.animatorObject.SetBool("IsOnIce", this.isOnIce);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            // Check for ladders
            if (this.UseLadders && this.IsTrigger(other.gameObject, "ladder"))
            {
                this.body2D.isKinematic = false;
                this.isOnLadder = false;
                this.animatorObject.SetBool("IsOnLadder", this.isOnLadder);
            }

            // Check for water
            if (this.UseWater && this.IsTrigger(other.gameObject, "water"))
            {
                this.isInWater = false;
                this.animatorObject.SetBool("IsInWater", this.isInWater);

                if (this.BubbleEmitter != null && this.BubbleEmitter.isPlaying)
                {
                    this.BubbleEmitter.Stop();
                    this.BubbleEmitter.gameObject.SetActive(false);
                }
            }

            // Check for ice
            if (this.UseIce && this.IsTrigger(other.gameObject, "ice"))
            {
                this.isOnIce = false;
                this.animatorObject.SetBool("IsOnIce", this.isOnIce);
            }
        }

        /// <summary>
        /// Perform movement for the character
        /// This should be called from the FixedUpdate() method as it makes changes to physics properties
        /// </summary>
        /// <param name="axis">The x and y values used for running and climbing ladders</param>
        /// <param name="isHorizontalStillPressed">True if the user is still actively pressing the horizontal axis</param>
        public void Move(Vector2 axis, bool isHorizontalStillPressed)
        {
            // Quit early if dead
            if (this.IsDead)
            {
                this.body2D.velocity = new Vector2(0, this.body2D.velocity.y);
                return;
            }

            if (this.IgnoreAnimationStates)
            {
                this.isGrounded = true;
                this.animatorObject.SetBool("IsGrounded", this.isGrounded);
                return;
            }

            // Get the input and speed
            if (this.isGrounded || this.AllowAirControl)
            {
                float horizontal = axis.x;

                // Check if we are jumping off a wall when using AirControl
                if (this.AllowWallJump && !this.isGrounded && !Mathf.Approximately(this.wallJumpX, 0))
                {
                    if (!isHorizontalStillPressed)
                    {
                        // Wall jumps with AirControl emulate a user pressing the direction,
                        // so that they jump off at an angle. You can't just rely on the 
                        // rigidbody force since we overwrite it with the user input.
                        this.wallJumpX = Mathf.Lerp(this.wallJumpX, 0, Time.deltaTime);
                        horizontal = this.wallJumpX;
                    }
                    else
                    {
                        this.wallJumpX = 0;
                    }
                }

                // Set the new velocity for the character based on the run modifier
                float speed = (this.isRunningNormal ? this.RunSpeed : this.ModifiedSpeed);
                Vector2 newVelocity = new Vector2(horizontal * speed * Time.deltaTime, this.body2D.velocity.y);

                if (this.isOnIce)
                {
                    // If on ice we should slide
                    newVelocity.x = Mathf.Lerp(this.body2D.velocity.x, newVelocity.x, this.IceFriction * Time.deltaTime);
                }

                this.body2D.velocity = newVelocity;
            }

            // If they pressed jump, then add some Y velocity
            if (this.isJumpPressed)
            {
                float xPower = 0;
                if (this.AllowWallJump && this.isOnWall)
                {
                    // Add horizontal power for wall jumps
                    xPower = this.WallJumpHorizontalPower * (int)this.CurrentDirection;
                    this.wallJumpX = xPower * this.wallDecayX;

                    // Show the dust cloud when we jump off a wall
                    this.CreateDustCloud(this.GroundChecker.position);
                }
                else
                {
                    this.wallJumpX = 0;
                }

                this.body2D.velocity = new Vector2(this.body2D.velocity.x, 0);
                this.body2D.AddForce(new Vector2(xPower, this.JumpPower));
                this.isJumpPressed = false;
                this.isReadyForDust = true;

                // If we are double jumping, play an extra rolling animation,
                // so that the jump looks cooler.
                if (this.jumpCount == 2)
                {
                    this.animatorObject.Play("Roll", LayerMask.NameToLayer("FX"));
                }
            }
            else if (this.isOnLadder)
            {
                // Ladder climbing means we use the Y axis
                float vertical = axis.y;
                this.body2D.velocity = new Vector2(this.body2D.velocity.x, vertical * this.RunSpeed * Time.deltaTime);
            }
            else if (this.isOnWall && !this.isGrounded)
            {
                // If they are sliding down a wall, slow them down
                if (this.body2D.velocity.y < 0 &&
                    (this.body2D.velocity.x >= 0 && this.CurrentDirection < 0 ||
                     this.body2D.velocity.x <= 0 && this.CurrentDirection > 0))
                {
                    this.body2D.velocity = new Vector2(this.body2D.velocity.x, this.body2D.velocity.y * this.GetMultiplier(this.WallSlideFactor));
                }
            }
            else if (this.isBlocking)
            {
                // Blocking changes the speed of the character
                this.body2D.velocity = new Vector2(this.body2D.velocity.x * this.GetMultiplier(this.BlockingMoveFactor), this.body2D.velocity.y);
            }

            if (this.isInWater)
            {
                // Water also changes the speed of the character
                float waterFactor = this.GetMultiplier(this.WaterMoveFactor);
                this.body2D.velocity = new Vector2(this.body2D.velocity.x * waterFactor, this.body2D.velocity.y);
            }

            // Update the animator
            this.animatorObject.SetBool("IsGrounded", this.isGrounded);
            this.animatorObject.SetBool("IsOnWall", this.isOnWall);
            this.animatorObject.SetInteger("WeaponType", (int)this.EquippedWeaponType);
            this.animatorObject.SetBool("IsZombified", this.IsZombified);
            this.animatorObject.SetFloat("AbsY", Mathf.Abs(this.body2D.velocity.y));
            this.animatorObject.SetFloat("VelocityY", this.body2D.velocity.y);
            this.animatorObject.SetFloat("VelocityX", Mathf.Abs(this.body2D.velocity.x));
            this.animatorObject.SetBool("HasMoveInput", isHorizontalStillPressed);
            this.animatorObject.SetBool("IsCrouching", this.isCrouching && this.isGrounded);

            // Flip the sprites if necessary
            if (this.isOnWall)
            {
                if (this.isOnWallFront && !this.isOnLadder)
                {
                    this.ChangeDirection(this.CurrentDirection == Direction.Left ? Direction.Right : Direction.Left);
                }
            }
            else if (this.body2D.velocity.x != 0)
            {
                this.ChangeDirection(this.body2D.velocity.x < 0 ? Direction.Left : Direction.Right);
            }
        }

        /// <summary>
        /// Perform the specified actions for the character
        /// </summary>
        /// <param name="action">A combined set of flags for all the actions the character should perform</param>
        public void Perform(Action action)
        {
            // Quit early if dead
            if (this.IsDead)
            {
                return;
            }

            // Check if we are blocking
            this.isBlocking = IsAction(action, Action.Block) && this.IsBlockEnabled;

            // Check if we are crouching
            this.isCrouching = IsAction(action, Action.Crouch);

            // Check for the running modifier key
            this.isRunningNormal = !IsAction(action, Action.RunModified);

            // Reset the jump count if we are on the ground
            if (this.isGrounded)
            {
                this.jumpCount = 0;
            }

            // Check for jumping down since we need to remove the regular jump flag if we are
            if (IsAction(action, Action.JumpDown))
            {
                Collider2D ground = this.CheckGround();
                if (ground != null)
                {
                    PlatformEffector2D fx = ground.GetComponent<PlatformEffector2D>();
                    if (fx != null && fx.useOneWay)
                    {
                        ground.enabled = false;
                        action &= ~Action.Jump;

                        StartCoroutine(this.EnableAfter(this.JumpDownTimeout, ground));
                    }
                }
            }

            // Now check the rest of the keys for actions
            if (IsAction(action, Action.Jump) && !this.isJumpPressed)
            {
                // Prevent them jumping on ladders
                if (!this.isOnLadder)
                {
                    if (this.isGrounded || (this.AllowWallJump && this.isOnWall))
                    {
                        this.isJumpPressed = true;
                        this.jumpCount = 1;
                    }
                    else if (this.AllowDoubleJump && this.jumpCount <= 1)
                    {
                        this.isJumpPressed = true;
                        this.jumpCount = 2;
                    }
                }
            }
            else if (IsAction(action, Action.QuickAttack))
            {
                this.TriggerAction("TriggerQuickAttack");
            }
            else if (IsAction(action, Action.Attack))
            {
                this.TriggerAction("TriggerAttack");
            }
            else if (IsAction(action, Action.Cast))
            {
                this.TriggerAction("TriggerCast");
            }
            else if (IsAction(action, Action.ThrowOff))
            {
                this.TriggerAction("TriggerThrowOff");
            }
            else if (IsAction(action, Action.ThromMain))
            {
                this.TriggerAction("TriggerThrowMain");
            }
            else if (IsAction(action, Action.Consume))
            {
                this.TriggerAction("TriggerConsume");
            }
            else if (this.isBlocking && !this.animatorObject.GetBool("IsBlocking"))
            {
                this.TriggerAction("TriggerBlock");
            }
            else if (IsAction(action, Action.Hurt))
            {
                // Apply some damage to test the animation
                this.ApplyDamage(10);
            }

            // Reset the blocking animation if they let go of the block button
            if (!this.isBlocking)
            {
                this.animatorObject.SetBool("IsBlocking", this.isBlocking);
            }
        }

        /// <summary>
        /// Reduce the health of the character by the specified amount
        /// </summary>
        /// <param name="damage">The amount of damage to apply</param>
        /// <param name="direction">The direction that the damage came from (left < 0 > right)</param>
        /// <returns>True if the character dies from this damage, False if it remains alive</returns>
        public bool ApplyDamage(int damage, float direction = 0)
        {
            if (!this.IsDead)
            {
                this.animatorObject.SetFloat("LastHitDirection", direction * (int)this.CurrentDirection);

                // Update the health
                this.CurrentHealth = Mathf.Clamp(this.CurrentHealth - damage, 0, this.MaxHealth);
                this.animatorObject.SetInteger("Health", this.CurrentHealth);

                if (damage != 0)
                {
                    // Show the hurt animation
                    this.TriggerAction("TriggerHurt", false);
                }

                if (this.CurrentHealth <= 0)
                {
                    // Since the player is dead, remove the corpse
                    StartCoroutine(this.DestroyAfter(1, this.gameObject));
                }
            }

            return this.IsDead;
        }

        private void TriggerAction(string action, bool isCombatAction = true)
        {
            // Update the animator object
            this.animatorObject.SetTrigger(action);
            this.animatorObject.SetBool("IsBlocking", this.isBlocking);

            if (isCombatAction)
            {
                // Combat actions also trigger an additional parameter to move correctly through states
                this.animatorObject.SetTrigger("TriggerCombatAction");
            }
        }

        private void ChangeDirection(Direction newDirection)
        {
            if (this.CurrentDirection == newDirection)
            {
                return;
            }

            // Swap the direction of the sprites
            Vector3 rotation = this.transform.localRotation.eulerAngles;
            rotation.y -= 180;
            this.transform.localEulerAngles = rotation;
            this.CurrentDirection = newDirection;

            SpriteRenderer[] sprites = this.GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < sprites.Length; i++)
            {
                Vector3 position = sprites[i].transform.localPosition;
                position.z *= -1;
                sprites[i].transform.localPosition = position;
            }
        }

        private void OnCastEffect()
        {
            // If we have an effect start it now
            if (this.Effect != null)
            {
                this.activeEffect = WeaponEffect.Create(this.Effect, this.EffectPoint);
            }
        }
        private void OnCastEffectStop()
        {
            // If we have an effect stop it now
            if (this.activeEffect != null)
            {
                this.activeEffect.Stop();
                this.activeEffect = null;
            }
        }

        private void OnCastComplete()
        {
            // Stop the active effect once we cast
            this.OnCastEffectStop();

            // Create the projectile
            this.LaunchProjectile(this.CastProjectile);
        }

        private void OnThrowMainComplete()
        {
            // Create the projectile for the main hand
            this.LaunchProjectile(this.ThrowMainProjectile);
        }

        private void OnThrowOffComplete()
        {
            // Create the projectile for the off hand
            this.LaunchProjectile(this.ThrowOffProjectile);
        }

        private void LaunchProjectile(WeaponProjectile projectile)
        {
            // Create the projectile
            if (projectile != null)
            {
                WeaponProjectile.Create(
                    projectile,
                    this,
                    this.LaunchPoint,
                    (this.CurrentDirection == Direction.Left ? -1 : 1));
            }
        }

        private Collider2D CheckGround()
        {
            // Check if we are touching the ground using the rigidbody
            return Physics2D.OverlapCircle(GroundChecker.position, this.groundRadius, this.GroundLayer);
        }

        private void CreateSplash(Vector2 point)
        {
            if (this.SplashEmitter != null)
            {
                // Create a cloud of dust
                ParticleSystem splash = GameObject.Instantiate<ParticleSystem>(this.SplashEmitter);
                splash.transform.position = point;
                splash.Play();
                StartCoroutine(this.DestroyAfter(splash.duration, splash.gameObject));
            }
        }

        private void CreateDustCloud(Vector2 point)
        {
            if (this.DustEmitter != null)
            {
                // Create a cloud of dust
                ParticleSystem dust = GameObject.Instantiate<ParticleSystem>(this.DustEmitter);
                dust.transform.position = point;
                dust.Play();
                StartCoroutine(this.DestroyAfter(dust.duration, dust.gameObject));
            }
        }

        private bool IsTrigger(GameObject other, string name)
        {
            name = name.ToLower();

            if ((other.tag != null && other.tag.ToLower() == name) ||
                (other.name != null && other.name.ToLower() == name))
            {
                return true;
            }

            return false;
        }

        private bool IsAction(Action value, Action flag)
        {
            return (value & flag) != 0;
        }

        private float GetMultiplier(float factor)
        {
            if (Mathf.Sign(factor) < 0)
            {
                return 1 + factor;
            }
            else
            {
                return factor;
            }
        }

        private IEnumerator DestroyAfter(float seconds, GameObject gameObject)
        {
            yield return new WaitForSeconds(seconds);

            GameObject.Destroy(gameObject);
        }

        private IEnumerator EnableAfter(float seconds, Behaviour obj)
        {
            yield return new WaitForSeconds(seconds);

            obj.enabled = true;
        }
    }
}