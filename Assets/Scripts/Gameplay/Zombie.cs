using System.Linq;
using Enums;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay {
    [RequireComponent(typeof(NavMeshAgent))]
    public class Zombie : Killable {

        [Header("References")]
        [SerializeField] 
        private NavMeshAgent agent;

        protected override void Reset() {
            base.Reset();
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update() {
            Transform player = FindClosestPlayer();
            if (player == null)
                return;

            Debug.Log(player.position);
            agent.destination = player.position;
        }

        private Transform FindClosestPlayer() {
            Transform[] players = GameObject.FindGameObjectsWithTag(Tag.Player)
                .Select(g => g.transform)
                .ToArray();
            Transform tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = transform.position;
            foreach (Transform player in players) {
                float dist = (currentPos - player.position).sqrMagnitude;
                if (dist < minDist) {
                    tMin = player;
                    minDist = dist;
                }
            }
            return tMin;
        }

    }
}
