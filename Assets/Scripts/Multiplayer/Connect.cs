using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Multiplayer {
    public class Connect : MonoBehaviourPunCallbacks {
        private void Start() {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster() {
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby() {
            SceneManager.LoadScene("Lobby");
        }
    }
}
