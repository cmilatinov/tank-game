using System;
using Photon.Pun;
using UnityEngine;

namespace Gameplay {
    [RequireComponent(typeof(PhotonView))]
    public class Killable : MonoBehaviour, IPunObservable {

        [Header("Killable")] 
        [SerializeField]
        public float maxHealth = 200;

        [SerializeField] 
        public float health;

        [SerializeField] 
        public PhotonView photonView;

        protected virtual void Reset() {
            photonView = GetComponent<PhotonView>();
        }

        protected virtual void Start() {
            health = maxHealth;
        }

        public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(health);
            } else if (stream.IsReading) {
                health = (float) stream.ReceiveNext();
            }
        }
    }
}
