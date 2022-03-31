using System;
using Enums;
using Multiplayer;
using UnityEngine;

namespace UI {
    public class GameOver : MonoBehaviour {

        public static GameOver Instance;
        
        [SerializeField] 
        private Canvas canvas;

        private void Awake() {
            Instance = this;
        }

        private void Reset() {
            canvas = GetComponentInChildren<Canvas>();
        }

        public void ShowGameOver() {
            canvas.gameObject.SetActive(true);
        }

        public void OnClickRespawn() {
            PlayerSpawn.Instance.SpawnPlayer();
            canvas.gameObject.SetActive(false);
        }

    }
}
