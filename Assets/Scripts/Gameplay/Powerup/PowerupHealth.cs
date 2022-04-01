using Photon.Pun;
using UnityEngine;

namespace Gameplay {
    public class PowerupHealth : Powerup {

        [SerializeField] 
        private float healthRestored = 50.0f;

        public override void OnTriggerEnter(Collider other) {
            if (!photonView.IsMine)
                return;
            
            PlayerTank playerTank = other.GetComponent<PlayerTank>();
            if (playerTank == null)
                return;
            
            playerTank.photonView.RPC(nameof(PlayerTank.GiveHealth), RpcTarget.All, healthRestored);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
