using UnityEngine;
using System.Collections;

namespace AnimatedPixelPack
{
    public class SimpleFollowCamera : MonoBehaviour
    {
        // Editor Properties
        public Transform FollowTarget;
        public float SmoothFactor = 1;
        public float LookAheadDistance = 2;
        public float LookAheadCenterSpeed = 1;
        public float LookAheadMovementThreshold = 0.1f;

        // Members
        private Vector3 previousFollowTargetPosition;
        private Vector3 dampVelocity;
        private Vector3 lookAtPosition;

        void Start()
        {
            this.previousFollowTargetPosition = this.transform.position;

            // If we don't have a follow target, find the first available player
            if (this.FollowTarget == null)
            {
                GameObject go = GameObject.FindGameObjectWithTag("Player");
                if (go != null)
                {
                    this.FollowTarget = go.transform;
                }
            }
        }

        void Update()
        {
            if (this.FollowTarget != null)
            {
                // Check if the player has changed direction
                float movementX = (this.FollowTarget.position - this.previousFollowTargetPosition).x;
                if (Mathf.Abs(movementX) > this.LookAheadMovementThreshold)
                {
                    // If so we need to look ahead in the other direction
                    this.lookAtPosition = this.LookAheadDistance * Vector3.right * Mathf.Sign(movementX);
                }
                else
                {
                    // Otherwise just gently focus on the look ahead position
                    this.lookAtPosition = Vector3.MoveTowards(this.lookAtPosition, Vector3.zero, Time.deltaTime * this.LookAheadCenterSpeed);
                }

                // Move the camera towards the target position
                Vector3 target = this.FollowTarget.position + this.lookAtPosition;
                Vector3 newPos = Vector3.SmoothDamp(this.transform.position, target, ref this.dampVelocity, this.SmoothFactor);

                // Remember to keep the camera on the starting z axis value
                newPos.z = this.transform.position.z;

                this.transform.position = newPos;

                // Store the new follow target position so we can check if it changed in the next update call
                this.previousFollowTargetPosition = this.FollowTarget.position;
            }
        }
    }
}
