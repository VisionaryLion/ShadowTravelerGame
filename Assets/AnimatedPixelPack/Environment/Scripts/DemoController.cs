using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace AnimatedPixelPack
{
    public class DemoController : MonoBehaviour
    {
        // Editor Properties
        public Canvas UICanvas;
        public FloatingText Title;
        public Character[] Characters;
        public CharacterInputSimpleAI[] Enemies;
        public bool SpawnEnemies = false;
        public float TimeBetweenSpawn = 5;
        public Transform SpawnPlayerPoint;

        // Members
        private Character selectedCharacter;
        private int selectedIndex = -1;
        private float timeToNextSpawn;

        void Start()
        {
            // Select a character by default
            this.selectedIndex = 0;
            this.SelectCharacter(this.selectedIndex);
        }

        void Update()
        {
            if (this.Characters.Length <= 0)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Equals))
            {
                // Select next character
                this.selectedIndex++;
                if (this.selectedIndex >= this.Characters.Length)
                {
                    this.selectedIndex = 0;
                }

                this.SelectCharacter(this.selectedIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Minus))
            {
                // Select previous character
                this.selectedIndex--;
                if (this.selectedIndex < 0)
                {
                    this.selectedIndex = this.Characters.Length - 1;
                }

                this.SelectCharacter(this.selectedIndex);
            }

            // Check if we should spawn enemies yet based on the time
            if (this.SpawnEnemies)
            {
                this.timeToNextSpawn -= Time.deltaTime;
                if (this.timeToNextSpawn <= 0)
                {
                    this.timeToNextSpawn = this.TimeBetweenSpawn;

                    this.SpawnEnemy();
                }
            }
        }

        private void SelectCharacter(int index)
        {
            if (index >= 0 && index < this.Characters.Length)
            {
                Vector3 position = new Vector3();
                Character.Direction direction = Character.Direction.Right;
                if (this.selectedCharacter != null)
                {
                    // Remove previous character
                    position = this.selectedCharacter.transform.position;
                    direction = this.selectedCharacter.CurrentDirection;
                    GameObject.Destroy(this.selectedCharacter.gameObject);
                }
                else if (this.SpawnPlayerPoint != null)
                {
                    position = this.SpawnPlayerPoint.position;
                }

                // Spawn new character
                this.selectedCharacter = Character.Create(this.Characters[this.selectedIndex], direction, position);

                // Add some floating text to show the character name
                Vector3 titlePosition = this.selectedCharacter.transform.position + (Vector3.up * 1f);
                string text = this.selectedCharacter.name.Replace("_", " ").Replace("(Clone)", "");
                FloatingText.Create(this.Title, this.UICanvas, titlePosition, text);

                SimpleFollowCamera cam = Camera.main.GetComponent<SimpleFollowCamera>();
                if (cam != null)
                {
                    cam.FollowTarget = this.selectedCharacter.transform;
                }
            }
        }

        private void SpawnEnemy()
        {
            // Make a new enemy
            CharacterInputSimpleAI ai = this.Enemies[Random.Range(0, this.Enemies.Length)];
            Character enemy = Character.Create(ai.GetComponent<Character>(), Character.Direction.Left, new Vector3(5, 5, 0));
            enemy.gameObject.AddComponent<CharacterInputSimpleAI>();
        }
    }
}