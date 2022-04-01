using Enums;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Multiplayer {
    public class ZombieTankSpawner : Spawner {

        [SerializeField] 
        private GameObject[] zombiePrefabs;
        
        [SerializeField] 
        private float spawnTime = 20.0f;

        [SerializeField] 
        private int maxSpawns = 5;
        
        private float _lastSpawn;

        private void Update() {
            if (!PhotonNetwork.IsMasterClient)
                return;

            if (Time.time - _lastSpawn > spawnTime) {
                SpawnZombie();
                _lastSpawn = Time.time;
            }
        }

        public void SpawnZombie() {
            // Check can spawn
            GameObject[] zombies = GameObject.FindGameObjectsWithTag(Tag.Zombie);
            if (zombies.Length >= maxSpawns)
                return;

            // Get random spawn point
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            
            // Spawn zombie
            GameObject zombiePrefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];
            PhotonNetwork.InstantiateRoomObject(zombiePrefab.name, spawnPoint.position, spawnPoint.rotation, 0);
        }
    }
}
