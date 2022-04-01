using System.Linq;
using Enums;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay {
    [RequireComponent(typeof(NavMeshAgent))]
    public class ZombieTank : Killable {

        [Header("Parameters")] 
        [SerializeField]
        private float turretRotationSpeed = 4.0f;

        [SerializeField]
        private float aimRadius = 12.0f;
        
        [SerializeField]
        private float shootRadius = 7.0f;
        
        [SerializeField] 
        private float fireRate = 1.0f;

        [SerializeField] 
        private float damage = 20.0f;
        
        [SerializeField] 
        private float recoilForce = 10.0f;
        
        [Header("References")]
        [SerializeField] 
        private NavMeshAgent agent;

        [SerializeField]
        private GameObject turretHead;
        
        [SerializeField]
        private Transform barrel;

        [SerializeField] 
        private GameObject projectilePrefab;
        
        [SerializeField]
        public GameObject muzzleFlashPrefab;
        
        private float _lastFire;

        protected override void Reset() {
            base.Reset();
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update() {
            if (!photonView.IsMine) {
                agent.enabled = false;
                return;
            }

            agent.enabled = true;
            Transform player = FindClosestPlayer();
            if (player == null)
                return;

            float distance = Vector3.Distance(player.position, transform.position);
            if (distance < shootRadius && HasLineOfSight(player)) {
                agent.destination = transform.position;
                Shoot();
            } else {
                agent.destination = player.position;
            }

            if (distance < aimRadius) {
                FacePlayer(player);
            }
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
        
        private bool HasLineOfSight(Transform player) {
            Vector3 dir = player.position - transform.position;
            Ray ray = new Ray(transform.position + new Vector3(0, 0.5f, 0), dir.normalized);
            Physics.Raycast(ray, out var hit);
            return hit.collider.gameObject.CompareTag(Tag.Player);
        }

        private void FacePlayer(Transform player) {
            Vector3 lookDir = player.position - turretHead.transform.position;
            lookDir.y = 0.0f;
            turretHead.transform.rotation = Quaternion.Slerp(turretHead.transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * turretRotationSpeed);
        }

        private void Shoot() {
            if (Time.time - _lastFire > (1 / fireRate)) {
                _lastFire = Time.time;
                GameObject projectile = PhotonNetwork.Instantiate(projectilePrefab.name, barrel.position, barrel.rotation);
                projectile.GetComponent<TankProjectile>().damage = damage;
                photonView.RPC(nameof(SpawnMuzzle), RpcTarget.All);
                // TODO: Do voodoo magic to get recoil force to work
            }
        }

        [PunRPC]
        private void SpawnMuzzle() {
            Instantiate(muzzleFlashPrefab, barrel.position, barrel.rotation);
        }

        [PunRPC]
        public override void TakeDamage(float damage) {
            base.TakeDamage(damage);
            if (health <= 0.0f && photonView.IsMine) {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
