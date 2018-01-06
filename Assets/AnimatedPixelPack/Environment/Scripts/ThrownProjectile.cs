using UnityEngine;
using System.Collections;

namespace AnimatedPixelPack
{
    public class ThrownProjectile : HorizontalProjectile
    {
        // Editor Properties
        [Header("Thrown")]
        [Tooltip("Should the sprite be generated from the MainItem (false will use OffItem)")]
        public bool IsMainItem = true;
        [Tooltip("The angle to rotate the sprite when thrown")]
        public float StartRotation = 0f;
        [Tooltip("The angle to rotate the sprite when it has landed")]
        public float EndRotation = 0f;

        // Members
        private bool isEndRotationSet;

        protected override void Start()
        {
            base.Start();
            
            // Get the sprite from the hand of the character
            Sprite weaponSprite = this.GetComponentInChildren<SpriteRenderer>().sprite;
            BoxCollider2D weaponBox = this.GetComponentInChildren<BoxCollider2D>();
            if (this.Owner != null)
            {
                SpriteRenderer[] parts = this.Owner.GetComponentsInChildren<SpriteRenderer>();
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i].name == (this.IsMainItem ? "MainItem" : "OffItem"))
                    {
                        weaponSprite = parts[i].sprite;
                        weaponBox = parts[i].gameObject.GetComponent<BoxCollider2D>();
                        break;
                    }
                }
            }

            // Update our sprite to match the one we are throwing
            if (weaponSprite != null)
            {
                SpriteRenderer sr = this.GetComponentInChildren<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sprite = weaponSprite;
                }
            }

            // Update our collision box based on the one we are throwing
            if (weaponBox != null)
            {
                BoxCollider2D bc = this.GetComponentInChildren<BoxCollider2D>();
                if (bc != null)
                {
                    bc.offset = weaponBox.offset;
                    bc.size = weaponBox.size;
                }

                // Since we updated the collider we need to re-ignore the collisions
                WeaponProjectile.IgnoreOwnerCollisions(this, this.Owner);

                this.RenderTransform.localPosition = -weaponBox.offset;
                this.RenderTransform.RotateAround(this.transform.position, Vector3.forward, this.StartRotation * this.DirectionX);
            }
        }

        protected override void OnCollisionEnter2D(Collision2D c)
        {
            base.OnCollisionEnter2D(c);

            // If we hit our first ground collision, set the end rotation
            if (this.isStopped && !this.isEndRotationSet)
            {
                this.isEndRotationSet = true;

                if (!this.ShouldFallWhenCollided)
                {
                    float rotation = this.EndRotation - this.RenderTransform.localEulerAngles.z;
                    this.RenderTransform.RotateAround(this.transform.position + this.RotationPoint, Vector3.forward, rotation * this.DirectionX);
                }
            }
        }
    }
}
