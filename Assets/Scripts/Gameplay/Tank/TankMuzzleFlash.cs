using UnityEngine;

namespace Gameplay {
    public class TankMuzzleFlash : MonoBehaviour {
        [Header("Parameters")]
        [SerializeField] 
        private float lifetime = 3.0f;

        private float _startTime;

        private void Start() {
            _startTime = Time.time;
        }

        private void Update() {
            if (Time.time - _startTime > lifetime) {
                Destroy(gameObject);
            }
        }
    }
}
