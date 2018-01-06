using UnityEngine;
using System.Collections;

namespace AnimatedPixelPack
{
    [RequireComponent(typeof(Character))]
    public class CharacterInput : MonoBehaviour
    {
        // Members
        private Character character;
        private Vector2 axis;

        void Start()
        {
            this.character = this.GetComponent<Character>();
            this.axis = new Vector2();
        }

        /// <summary>
        /// Fixed update should be used to perform physics actions such as moving
        /// </summary>
        void FixedUpdate()
        {
            this.axis.x = Input.GetAxis("Horizontal");
            this.axis.y = Input.GetAxis("Vertical");

            // Check if the user is actually holding down a direction.
            // We use this for knowing when to slide on ice or wall jump correctly.
            bool isHorizontalStillPressed = Input.GetAxisRaw("Horizontal") != 0;

            // Move the character using the axis as the input
            this.character.Move(this.axis, isHorizontalStillPressed);
        }

        /// <summary>
        /// Update should be used to perform user input actions such as triggering an attack
        /// You can edit this function to replace the Keyboard buttons with your own input scheme.
        /// For example, you could use on screen buttons to detect input.
        /// </summary>
        void Update()
        {
            // Start out with no actions to perform
            Character.Action actionFlags = 0;

            // Check if we are modifying the run speed
            actionFlags |= (Input.GetAxisRaw("Vertical") < 0 ? Character.Action.RunModified : 0);

            // Check if we are crouching (which is the same as run speed modifer in our demo)
            actionFlags |= (Input.GetAxisRaw("Vertical") < 0 ? Character.Action.Crouch : 0);

            // Check if we are jumping
            actionFlags |= (Input.GetButtonDown("Jump") ? Character.Action.Jump : 0);

            // Check if we are jumping down off a platform
            actionFlags |= (Input.GetButtonDown("Jump") && Input.GetAxisRaw("Vertical") < 0 ? Character.Action.JumpDown : 0);

            // Check if they are holding down block
            actionFlags |= (Input.GetKey(KeyCode.B) ? Character.Action.Block : 0);

            // Check the other actions
            actionFlags |= (Input.GetButtonDown("Fire1") ? Character.Action.QuickAttack : 0);
            actionFlags |= (Input.GetButtonDown("Fire2") ? Character.Action.Attack : 0);
            actionFlags |= (Input.GetButtonDown("Fire3") ? Character.Action.Cast : 0);
            actionFlags |= (Input.GetKeyDown(KeyCode.Z) ? Character.Action.ThrowOff : 0);
            actionFlags |= (Input.GetKeyDown(KeyCode.X) ? Character.Action.ThromMain : 0);
            actionFlags |= (Input.GetKeyDown(KeyCode.C) ? Character.Action.Consume : 0);
            actionFlags |= (Input.GetKeyDown(KeyCode.H) ? Character.Action.Hurt : 0);

            // Now trigger the current actions on the character
            this.character.Perform(actionFlags);
        }
    }
}
