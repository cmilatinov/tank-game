using Enums;
using Gameplay;
using IndiePixel.Cameras;
using IndiePixel.Controllers;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Multiplayer {
    public class PlayerTankSpawner : Spawner {

        public static PlayerTankSpawner Instance;

        [SerializeField]
        private GameObject[] playerPrefabs;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            SpawnPlayer();
        }

        public void SpawnPlayer() {
            // Get random spawn point
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            
            // Spawn player
            GameObject playerPrefab = playerPrefabs[(PlayerTank.PlayerNumber - 1) % playerPrefabs.Length];
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
            
            // Setup camera
            TankCamera tankCamera = GameObject.FindWithTag(Tag.MainCamera).GetComponent<TankCamera>();
            tankCamera.m_Target = player.transform;
            
            // Setup reticle
            TankInputs tankInputs = player.GetComponent<TankInputs>();
            tankInputs.m_Reticle = GameObject.FindWithTag(Tag.Reticle).transform;
        }

    }
}
