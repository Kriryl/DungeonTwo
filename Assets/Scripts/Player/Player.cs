using Assets.HeroEditor4D.Common.CharacterScripts;
using HeroEditor4D.Common.Enums;
using System;
using UnityEngine;
using DungeonEscape.Enums;
using HeroEditor4D.Common;
using DungeonEscape.Monsters;
using TMPro;
using System.Collections.Generic;

namespace DungeonEscape
{
    namespace PlayerController
    {
        public class Player : MonoBehaviour
        {
            public Character4D character;
            public Character characterSprites;
            public Collider2D currentSword;

            [Serializable]
            public class Movement
            {
                public float playerSpeed = 25f;
                public bool isAttacking = false;
                public bool IsRunning { get; set; } = false;
                public Vector2 InitDirection { get; } = Vector2.down;
                public bool Init { get; set; } = false;
            }

            [Serializable]
            public class Health
            {
                public float maxHealth = 10f;
            }

            [Serializable]
            public class Gear
            {
                public Item.Sword sword;
            }

            [Serializable]
            public class SwordDirection
            {
                public Collider2D left;
                public Collider2D right;
                public Collider2D up;
                public Collider2D down;

                public Collider2D[] Directions
                {
                    get
                    {
                        Collider2D[] colliders = { left, right, up, down };
                        return colliders;
                    }
                }
            }

            [Serializable]
            public class Properties
            {
                public float damage = 1f;
            }

            public Movement movement = new Movement();
            public Health health = new Health();
            public Gear gear = new Gear();
            public SwordDirection swordDirection = new SwordDirection();
            public Properties properties = new Properties();
            [Header("Objective settings")]
            public TextMeshProUGUI objectiveDisplay;
            public Objective objective = new Objective("Hello");

            /// <summary>
            /// The amount of damage the player deals.
            /// </summary>
            public float Damage { get => properties.damage; set => properties.damage = value; }

            /// <summary>
            /// The current objective as text (read only).
            /// </summary>
            public string CurrentObjective => objective.Message;

            /// <summary>
            /// The current sword the player is holding (read only).
            /// </summary>
            public Item.Sword Sword => gear.sword;

            /// <summary>
            /// Is the player currently attacking?
            /// </summary>
            public bool IsAttacking { get => movement.isAttacking; set => movement.isAttacking = value; }

            /// <summary>
            /// Contains all the items the player has.
            /// </summary>
            public List<Item> Inventory { get; } = new List<Item>();

            private Rigidbody2D rb;

            private void Start()
            {
                rb = GetComponent<Rigidbody2D>();
                if (!movement.Init)
                {
                    character.SetDirection(movement.InitDirection);
                    movement.Init = true;
                }
                character.ResetEquipment();
                currentSword = swordDirection.down;
                foreach (Collider2D collider2D in swordDirection.Directions)
                {
                    collider2D.enabled = false;
                }
                print(objective.Message);
            }

            private void Update()
            {
                SetDirection();
                Attack();
                //FlipSprite();
            }

            private void FixedUpdate()
            {
                Move();
            }

            private void OnTriggerEnter2D(Collider2D collision)
            {
                if (collision.GetComponent<Item>())
                {
                    collision.gameObject.BroadcastMessage("OnPickup", this);
                }
                else if (collision.gameObject.GetComponent<Monster>())
                {
                    if (IsAttacking)
                    {
                        collision.gameObject.BroadcastMessage("OnHit", Damage);
                    }
                }
            }

            public void AddToInventory(Sprite sprite, PickupType itemType, Item item)
            {
                if (itemType == PickupType.none) { return; }
                if (itemType == PickupType.equipment)
                {
                    var spriteCollectionWeapons = character.SpriteCollection.MeleeWeapon1H;
                    for (int i = 0; i < spriteCollectionWeapons.Count; i++)
                    {
                        SpriteGroupEntry groupEntry = spriteCollectionWeapons[i];
                        if (sprite == groupEntry.Sprite)
                        {
                            print("Sprite is the same");
                            SpriteGroupEntry weapon = groupEntry;
                            print(groupEntry.ToString() + ", " + i);
                            character.Equip(weapon, EquipmentPart.MeleeWeapon1H);
                            Inventory.Add(item);
                            return;
                        }
                        print(groupEntry.Sprite + ". Sprite is NOT the same.");
                    }
                }
            }

            public void SetDirection()
            {
                Vector2 direction;

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    direction = Vector2.left;
                    ToggleSwordDirection(swordDirection.left);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    direction = Vector2.right;
                    ToggleSwordDirection(swordDirection.right);
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    direction = Vector2.up;
                    ToggleSwordDirection(swordDirection.up);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    direction = Vector2.down;
                    ToggleSwordDirection(swordDirection.down);
                }
                else return;

                character.SetDirection(direction);
            }

            public void ToggleSwordDirection(Collider2D collider)
            {
                foreach (Collider2D collider2D in swordDirection.Directions)
                {
                    collider2D.gameObject.SetActive(false);
                }
                collider.gameObject.SetActive(true);
                currentSword = collider;
            }

            public void Move()
            {
                if (movement.playerSpeed == 0)
                {
                    return;
                }

                float xMove = Input.GetAxis("Horizontal");
                float zMove = Input.GetAxis("Vertical");

                Vector2 newPos = new Vector2(xMove, zMove);
                Vector2 newSpeed = movement.playerSpeed * Time.deltaTime * newPos;

                rb.MovePosition(rb.position + newSpeed);

                bool playerIsIdle = xMove == 0f && zMove == 0f;

                if (playerIsIdle)
                {
                    if (movement.IsRunning)
                    {
                        character.AnimationManager.SetState(CharacterState.Idle);
                        movement.IsRunning = false;
                    }
                }
                else
                {
                    character.AnimationManager.SetState(CharacterState.Run);
                    movement.IsRunning = true;
                }
            }

            public void ApplySword(Item.Sword sword)
            {
                gear.sword = sword;
                Damage = sword.damage;
            }

            public void Attack()
            {
                if (Input.GetMouseButtonDown(0))
                {
                    character.AnimationManager.Slash1H();
                }
            }

            public void Slash()
            {
                IsAttacking = true;
                currentSword.enabled = true;
            }
            
            public void StopSlashing()
            {
                IsAttacking = false;
                currentSword.enabled = false;
            }
        }
    }
}