using System;
using System.Collections;
using UnityEngine;
using Spine.Unity;
using Spine;
using DungeonEscape.AI;

namespace DungeonEscape.Monsters
{
    [RequireComponent(typeof(Rigidbody2D))]
    /// <summary>
    /// Base class for all monsters.
    /// </summary>
    public class Monster : MonoBehaviour
    {
        [Serializable]
        public class Health
        {
            public float maxHealth = 10f;
            public float currentHealth = 10f;
        }

        public Health health = new Health();

        public string monsterName = "Monster1";
        public float attackDuration = 1f;
        public string animationName = "";

        /// <summary>
        /// The name of the monster.
        /// </summary>
        public string MonsterName => monsterName;

        public float CurrentHealth { get => health.currentHealth; set => health.currentHealth = value; }

        public float MaxHealth { get => health.maxHealth; set => health.maxHealth = value; }

        public Spine.AnimationState AnimationState { get; private set; }

        public SkeletonAnimation SkeletonAnimation { get; private set; }

        /// <summary>
        /// How long the attack animation lasts.
        /// </summary>
        public float AttackDuration => attackDuration;

        public EnemyAI EnemyAI => GetComponent<EnemyAI>();

        /// <summary>
        /// The room the monster is in.
        /// </summary>
        public Room MonsterRoom { get; set; }

        public bool IsWalking { get; set; } = false;

        /// <summary>
        /// Is the monster currently attacking?
        /// </summary>
        public bool IsAttacking { get; set; } = false;

        /// <summary>
        /// Is the monster dead?
        /// </summary>
        public bool IsDead { get; set; }

        Rigidbody2D rb;

        public float deathDelay = 2.5f;
        public GameObject body;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            SkeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
            AnimationState = SkeletonAnimation.AnimationState;
            CurrentHealth = MaxHealth;
            IsDead = false;
            IsWalking = true;
        }

        private void Update()
        {
            if (IsWalking && !IsDead)
            {
                AnimationState.SetAnimation(0, "Walk", true);
                IsWalking = false;
            }
            FlipSprite();
            animationName = AnimationState.ToString();
        }

        /// <summary>
        /// OnHit is called when the monster takes damage.
        /// </summary>
        private void OnHit(float damage)
        {
            CurrentHealth -= damage;

            if (CurrentHealth <= 0 && !IsDead)
            {
                StartCoroutine(Die());
            }
            else
            {
                StartCoroutine(GetHurt());
            }
        }

        private IEnumerator GetHurt()
        {
            yield return new WaitForSeconds(0.5f);
        }

        /// <summary>
        /// Kills the monster and removes it.
        /// </summary>
        public IEnumerator Die()
        {
            AnimationState.SetAnimation(0, "Dead", false);
            IsDead = true;
            yield return new WaitForSeconds(deathDelay);
            MonsterRoom.OnMonsterDead(this);
            Destroy(gameObject);
        }

        private void FlipSprite()
        {
            if (!IsDead && !IsAttacking)
            {
                if (rb.velocity.x >= 0.01f)
                {
                    body.transform.localScale = new Vector3(1, 1, 1);
                }
                else if (rb.velocity.x <= -0.01f)
                {
                    body.transform.localScale = new Vector3(-1, 1, 1);
                }
            }
        }

        public IEnumerator Attack()
        {
            if (!IsAttacking)
            {
                IsAttacking = true;
                AnimationState.SetAnimation(0, "Attack", false);
                attackDuration = AnimationState.GetCurrent(0).Animation.Duration;
                print(AttackDuration);
                yield return new WaitForSeconds(AttackDuration);
                IsAttacking = false;
                IsWalking = true;
            }
        }
    }
}
