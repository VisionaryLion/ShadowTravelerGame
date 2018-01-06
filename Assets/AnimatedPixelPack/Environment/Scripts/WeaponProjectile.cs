using UnityEngine;
using System.Collections;

namespace AnimatedPixelPack
{
    public abstract class WeaponProjectile : MonoBehaviour
    {
        // Editor Properties
        [Header("Weapon")]
        [Tooltip("The amount of damage to apply to the Character it hits")]
        public int Damage = 50;
        [Tooltip("How many seconds should the projectile live for (it is destroyed after this time)")]
        public float LifeTime = 3;
        [Tooltip("Should the projectile collide with the ground layer (if not it will fly straight through)")]
        public bool ShouldCollideWithGround;
        [Tooltip("Should the projectile fall to the ground when it hits a wall")]
        public bool ShouldFallWhenCollided = false;
        [Tooltip("Layer that contains all the 'ground' colliders (if left as nothing it will be set from the character)")]
        public LayerMask OverrideGroundLayer;
        [Tooltip("A ParticleSystem to play when a collision occurs")]
        public ParticleSystem CollideEmitter;
        [Tooltip("Should the projectile be mirrored when travelling the opposite direction")]
        public bool ShouldFlipDirection = true;

        // Members
        protected Character Owner { get; private set; }
        protected int DirectionX { get; private set; }
        protected Animator animatorObject;
        protected bool isStopped;

        /// <summary>
        /// Instantiate a new instance of the WeaponProjectile class using the supplied parameters
        /// </summary>
        /// <param name="instance">The instance to use as the base</param>
        /// <param name="owner">The character that owns this projectile</param>
        /// <param name="launchPoint">Where to spawn the projectile</param>
        /// <param name="directionX">The direction to move</param>
        /// <returns>The new projectile</returns>
        public static WeaponProjectile Create(WeaponProjectile instance, Character owner, Transform launchPoint, int directionX)
        {
            WeaponProjectile projectile = GameObject.Instantiate<WeaponProjectile>(instance);
            projectile.Owner = owner;

            // Set the start position
            Vector2 position = launchPoint.position;
            projectile.transform.position = position;
            projectile.DirectionX = directionX;

            // Make sure we can't collide with the person who shot this projectile
            WeaponProjectile.IgnoreOwnerCollisions(projectile, owner);

            // Flip the sprite if necessary
            if (projectile.ShouldFlipDirection && directionX < 0)
            {
                Vector3 rotation = projectile.transform.localRotation.eulerAngles;
                rotation.y -= 180;
                projectile.transform.localEulerAngles = rotation;
            }

            return projectile;
        }

        protected virtual void Start()
        {
            this.animatorObject = this.GetComponentInChildren<Animator>();

            // Get rid of the projectile after a while if it doesn't hit anything
            StartCoroutine(this.DestroyAfter(this.LifeTime));
        }

        protected virtual void Update()
        {
        }

        protected virtual void OnCollisionEnter2D(Collision2D c)
        {
            if (this.isStopped)
            {
                return;
            }

            LayerMask ground = (this.OverrideGroundLayer == 0 && this.Owner != null ? this.Owner.GroundLayer : this.OverrideGroundLayer);
            bool isOnGround = (ground & (1 << c.gameObject.layer)) != 0;

            if (isOnGround)
            {
                if (this.ShouldCollideWithGround)
                {
                    if (this.animatorObject != null)
                    {
                        this.animatorObject.Play("Hit");
                    }

                    if (this.ShouldFallWhenCollided)
                    {
                        SliderJoint2D joint = this.GetComponent<SliderJoint2D>();
                        if (joint != null)
                        {
                            joint.enabled = false;
                        }
                    }
                    else
                    {
                        Rigidbody2D body = this.GetComponentInChildren<Rigidbody2D>();
                        if (body != null)
                        {
                            body.isKinematic = true;
                        }

                        Collider2D[] colliders = this.GetComponentsInChildren<Collider2D>();
                        for (int i = 0; i < colliders.Length; i++)
                        {
                            colliders[i].enabled = false;
                        }
                    }

                    if (this.CollideEmitter != null)
                    {
                        this.CollideEmitter.Play();
                    }

                    this.isStopped = true;
                }
                else
                {
                    Collider2D[] projectileColliders = this.GetComponentsInChildren<Collider2D>();
                    for (int i = 0; i < projectileColliders.Length; i++)
                    {
                        Physics2D.IgnoreCollision(projectileColliders[i], c.collider);
                    }
                }
            }
            else
            {
                GameObject.Destroy(this.gameObject);
            }

            // Apply damage to any character hit by this projectile
            Character character = c.transform.GetComponent<Character>();
            if (character != null)
            {
                float direction = c.contacts[0].point.x - character.transform.position.x;
                character.ApplyDamage(this.Damage, direction);
            }
        }

        protected static void IgnoreOwnerCollisions(WeaponProjectile projectile, Character owner)
        {
            // Prevent hitting the player who cast it
            if (owner != null)
            {
                Collider2D[] colliders = owner.GetComponentsInChildren<Collider2D>();
                Collider2D[] projectileColliders = projectile.GetComponentsInChildren<Collider2D>();
                for (int i = 0; i < colliders.Length; i++)
                {
                    for (int j = 0; j < projectileColliders.Length; j++)
                    {
                        Physics2D.IgnoreCollision(colliders[i], projectileColliders[j]);
                    }
                }
            }
        }

        protected IEnumerator DestroyAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            GameObject.Destroy(this.gameObject);
        }
    }
}
