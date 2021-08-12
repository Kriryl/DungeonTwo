using UnityEngine;
using Pathfinding;
using DungeonEscape.Monsters;

namespace DungeonEscape.AI
{
    [RequireComponent(typeof(Monster))]
    public class EnemyAI : MonoBehaviour
    {
        public Transform target;
        public float speed = 3f;
        public float nextWayPointDistance = 3f;
        public float autoUpdate = 0.5f;
        public float attackRange = 2.5f;

        Path path;
        int currentWayPoint = 0;
        bool reachedEndOfPath = false;

        bool isDead = false;

        Seeker seeker;
        Rigidbody2D rb;
        Monster monster;

        private void Start()
        {
            monster = GetComponent<Monster>();
            seeker = GetComponent<Seeker>();
            rb = GetComponent<Rigidbody2D>();
            InvokeRepeating(nameof(UpdatePath), 0f, autoUpdate);
        }

        private void UpdatePath()
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(rb.position, target.position, OnPathComplete);
            }
        }

        private void FixedUpdate()
        {
            isDead = monster.IsDead;
            if (isDead) { return; }
            if (path != null)
            {
                FollowPath();
            }
            CheckDistanceFromTarget();
        }

        private void FollowPath()
        {
            if (currentWayPoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            reachedEndOfPath = false;

            Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
            Vector2 force = speed * Time.deltaTime * direction;

            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
            if (distance < nextWayPointDistance)
            {
                currentWayPoint++;
            }
        }

        private void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                path = p;
                currentWayPoint = 0;
            }
        }

        private void CheckDistanceFromTarget()
        {
            float distance = Vector2.Distance(rb.position, target.position);
            if (distance <= attackRange)
            {
                StartCoroutine(monster.Attack());
            }
        }
    }
}
