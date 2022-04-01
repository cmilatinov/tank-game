using System;
using Photon.Pun;
using UnityEngine;

namespace Gameplay {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PhotonView))]
    public class TankProjectile : MonoBehaviour, IPunObservable {

        [Header("Parameters")] 
        [SerializeField]
        private float speed = 20.0f;

        [SerializeField] 
        public float damage = 30.0f;

        [SerializeField] 
        public int numBounces = 0;

        [Header("References")]
        [SerializeField] 
        private new Rigidbody rigidbody;

        [SerializeField]
        private GameObject projectileHitPrefab;
        
        [SerializeField] 
        public PhotonView photonView;

        private void Reset() {
            rigidbody = GetComponent<Rigidbody>();
            photonView = GetComponent<PhotonView>();
        }

        private void Start() {
            rigidbody.velocity = transform.forward * speed;
            Destroy(gameObject, 5.0f);
        }

        private void Update() {
            if (!PhotonNetwork.IsMasterClient || numBounces <= 0)
                return;

            Ray ray = new Ray(transform.position, rigidbody.velocity.normalized);
            if (Physics.Raycast(ray, out var hit, 3.0f * rigidbody.velocity.magnitude * Time.deltaTime + 1.0f, 1 << Layer.Walls)) {
                numBounces--;
                rigidbody.velocity = Vector3.Reflect(rigidbody.velocity, hit.normal);
            }
        }

        private void OnCollisionEnter(Collision other) {
            Instantiate(projectileHitPrefab, other.contacts[0].point, Quaternion.identity);
            Destroy(gameObject);

            if (PhotonNetwork.IsMasterClient) {
                Killable killable = other.gameObject.GetComponent<Killable>();
                if (killable != null) {
                    killable.photonView.RPC(nameof(Killable.TakeDamage), RpcTarget.All, damage);
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(damage);
                stream.SendNext(numBounces);
            } else if (stream.IsReading) {
                damage = (float)stream.ReceiveNext();
                numBounces = (int)stream.ReceiveNext();
            }
        }

        [PunRPC]
        public void SetBounces(int bounces) {
            numBounces = bounces;
        }
    }
}
