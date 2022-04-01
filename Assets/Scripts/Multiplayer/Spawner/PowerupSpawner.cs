using Gameplay;
using Photon.Pun;
using UnityEngine;

namespace Multiplayer {
    public class PowerupSpawner : MonoBehaviour {
        
        [SerializeField] 
        private GameObject[] powerupPrefabs;

        [SerializeField] 
        private float spawnTime = 20.0f;
        
        private float _lastSpawn;
        private Powerup _currentPowerup;
        
        private void Update() {
            if (!PhotonNetwork.IsMasterClient)
                return;

            if (_currentPowerup == null && Time.time - _lastSpawn > spawnTime) {
                SpawnPowerup();
            }
        }

        public void SpawnPowerup() {
            GameObject powerupPrefab = powerupPrefabs[Random.Range(0, powerupPrefabs.Length)];
            GameObject powerup = PhotonNetwork.InstantiateRoomObject(powerupPrefab.name, transform.position, transform.rotation, 0);
            Powerup powerupScript = powerup.GetComponent<Powerup>();
            _currentPowerup = powerupScript;
            powerupScript.spawner = this;
        }

        public void OnPowerupPickup() {
            _lastSpawn = Time.time;
            _currentPowerup = null;
        }

    }
}
