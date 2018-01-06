using UnityEngine;
using System.Collections;

namespace AnimatedPixelPack
{
    [RequireComponent(typeof(Character))]
    public class CharacterInputSimpleAI : MonoBehaviour
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
            // For our demo, we just make sure the AI character always walks left,
            // We set the 'allow air control' property on the prefabs to false so that
            // it will fall straight down at the start.
            this.axis.x = -1;
            this.axis.y = 0;

            bool isHorizontalStillPressed = true;

            // Move the character using the axis as the input
            this.character.Move(this.axis, isHorizontalStillPressed);
        }

        void OnTriggerEnter2D(Collider2D c)
        {
            // Check if we collided with a main item weapon
            if ((c.tag != null && c.tag == "MainItem") ||
                (c.name != null && c.name == "MainItem"))
            {
                // Take some damage if we are attacked
                Character hurtBy = c.GetComponentInParent<Character>();
                if (hurtBy != null && hurtBy.IsAttacking)
                {
                    // Apply damage to this character
                    float direction = c.transform.position.x - this.transform.position.x;
                    this.character.ApplyDamage(100, direction);
                }
            }
        }
    }
}
