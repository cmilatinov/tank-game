using Gameplay;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer {
    public class Lobby : MonoBehaviourPunCallbacks {
        
        [Header("References")]
        [SerializeField]
        private InputField roomNameField;
    
        [SerializeField]
        private Button joinButton;
        
        [SerializeField]
        private Button createButton;

        public void OnClickJoin() {
            PhotonNetwork.JoinRoom(roomNameField.text);
        }

        public void OnClickCreate() {
            PhotonNetwork.CreateRoom(roomNameField.text);
        }

        public override void OnJoinedRoom() {
            PlayerTank.PlayerNumber = PhotonNetwork.PlayerList.Length;
            PhotonNetwork.LoadLevel("Game");
        }
        
    }
}
