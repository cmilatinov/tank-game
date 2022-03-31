using System;
using Enums;
using Gameplay;
using IndiePixel.Cameras;
using IndiePixel.Controllers;
using Photon.Pun;
using UnityEngine;

namespace Multiplayer {
    public class PlayerSpawn : MonoBehaviour {

        public static PlayerSpawn Instance;

        [SerializeField]
        private GameObject[] playerPrefabs;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            SpawnPlayer();
        }

        public void SpawnPlayer() {
            GameObject playerPrefab = playerPrefabs[(Player.PlayerNumber - 1) % playerPrefabs.Length];
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
            
            // Setup camera
            IP_Tank_Camera tankCamera = GameObject.FindWithTag(Tag.MainCamera).GetComponent<IP_Tank_Camera>();
            tankCamera.m_Target = player.transform;
            
            // Setup reticle
            IP_Tank_Inputs tankInputs = player.GetComponent<IP_Tank_Inputs>();
            tankInputs.m_Reticle = GameObject.FindWithTag(Tag.Reticle).transform;
        }

    }
}
