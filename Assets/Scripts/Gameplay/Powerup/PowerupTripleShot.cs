using IndiePixel.Controllers;
using Photon.Pun;
using UnityEngine;

namespace Gameplay {
    public class PowerupTripleShot : Powerup {
        [SerializeField] 
        private int numShots = 5; 
        
        public override void OnTriggerEnter(Collider other) {
            if (!photonView.IsMine)
                return;
            
            TankController tankController = other.GetComponent<TankController>();
            if (tankController == null)
                return;
            
            tankController.photonView.RPC(nameof(TankController.AddTripleShots), RpcTarget.All, numShots);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
