using Multiplayer;
using Photon.Pun;
using UnityEngine;

namespace Gameplay {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PhotonView))]
    public class TankProjectile : MonoBehaviour {

        [Header("Parameters")] 
        [SerializeField]
        private float speed = 20.0f;

        [SerializeField] 
        public float damage = 30.0f;

        [Header("References")]
        [SerializeField] 
        private new Rigidbody rigidbody;

        [SerializeField]
        private GameObject projectileHitPrefab;
        
        [SerializeField] 
        private PhotonView photonView;

        private void Reset() {
            rigidbody = GetComponent<Rigidbody>();
            photonView = GetComponent<PhotonView>();
        }

        private void Start() {
            rigidbody.velocity = transform.forward * speed;
            Destroy(gameObject, 5.0f);
        }

        private void OnCollisionEnter(Collision other) {
            Instantiate(projectileHitPrefab, other.contacts[0].point, Quaternion.identity);
            Destroy(gameObject);
            
            if (PhotonNetwork.IsMasterClient) {
                Player player = other.gameObject.GetComponent<Player>();
                if (player != null) {
                    player.photonView.RPC(nameof(Player.TakeDamage), RpcTarget.All, damage);
                }
                
                DestructibleWall wall = other.gameObject.GetComponent<DestructibleWall>();
                if (wall != null) {
                    wall.photonView.RPC(nameof(DestructibleWall.TakeDamage), RpcTarget.All, damage);
                }
            }
        }
    }
}
