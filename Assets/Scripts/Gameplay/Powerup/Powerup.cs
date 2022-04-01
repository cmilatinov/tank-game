using Multiplayer;
using Photon.Pun;
using UnityEngine;

namespace Gameplay {
    [RequireComponent(typeof(PhotonView))]
    public class Powerup : MonoBehaviour {
        [SerializeField] 
        public PowerupSpawner spawner;

        [SerializeField] 
        public PhotonView photonView;

        public virtual void Reset() {
            photonView = GetComponent<PhotonView>();
        }

        public virtual void OnTriggerEnter(Collider other) { }

        public virtual void OnDestroy() {
            if (spawner != null && photonView.IsMine) {
                spawner.OnPowerupPickup();
            }
        }
    }
}
