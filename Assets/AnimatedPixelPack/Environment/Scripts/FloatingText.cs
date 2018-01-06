using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace AnimatedPixelPack
{
    public class FloatingText : MonoBehaviour
    {
        // Editor Properties
        public float maxLifeTime = 5;
        public float speed = 1;
        public float waveCount = 3;
        public float waveSize = 1;

        // Members
        private Text textComponent;
        private float lifeTime;
        private float startX;

        /// <summary>
        /// Instantiate a new instance of the floating text class using the supplied parameters
        /// </summary>
        /// <param name="instance">The instance to use as the base</param>
        /// <param name="canvas">The UI canvas to use for parenting</param>
        /// <param name="start">The start location</param>
        /// <param name="text">The text to display</param>
        /// <returns>The new floating text instance</returns>
        public static FloatingText Create(FloatingText instance, Canvas canvas, Vector3 start, string text)
        {
            FloatingText ft = GameObject.Instantiate<FloatingText>(instance);
            ft.transform.SetParent(canvas.transform, false);
            ft.transform.position = start;
            ft.textComponent = ft.GetComponent<Text>();
            ft.textComponent.text = text;
            return ft;
        }

        void Start()
        {
            this.startX = this.transform.position.x;
        }

        void Update()
        {
            this.lifeTime += Time.deltaTime * this.speed;

            // Move in a sine wave pattern
            float x = this.startX + Mathf.Sin((this.lifeTime / this.maxLifeTime) * this.waveCount * Mathf.PI) * this.waveSize;
            float y = this.transform.position.y + Time.deltaTime;

            this.transform.position = new Vector3(x, y, this.transform.position.z);

            // Fade out the color
            Color color = this.textComponent.color;
            color.a = 1 - (this.lifeTime / this.maxLifeTime);
            this.textComponent.color = color;

            if (this.lifeTime >= this.maxLifeTime)
            {
                // End of life
                Destroy(this.gameObject);
            }
        }
    }
}