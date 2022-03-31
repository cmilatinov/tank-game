using Gameplay;
using Photon.Pun;
using UI;
using UnityEngine;

namespace Gameplay {
    public class Player : Killable {
        public static int PlayerNumber = 0;

        [PunRPC]
        public void TakeDamage(float damage) {
            health -= damage;
            if (photonView.IsMine && health <= 0) {
                GameOver.Instance.ShowGameOver();
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
