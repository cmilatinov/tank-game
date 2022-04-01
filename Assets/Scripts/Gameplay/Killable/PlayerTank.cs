using Photon.Pun;
using UI;
using UnityEngine;

namespace Gameplay {
    public class PlayerTank : Killable {
        public static int PlayerNumber = 0;

        [SerializeField] 
        public int tripleShots = 0;
        
        [PunRPC]
        public override void TakeDamage(float damage) {
            base.TakeDamage(damage);
            if (photonView.IsMine && health <= 0) {
                GameOver.Instance.ShowGameOver();
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
