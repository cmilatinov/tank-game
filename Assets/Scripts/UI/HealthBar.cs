using Multiplayer;
using UnityEngine;

namespace Gameplay {
    public class HealthBar : MonoBehaviour {
        
        [Header("Parameters")]
        [SerializeField]
        private float selfYOffset;

        [Header("References")] 
        [SerializeField]
        private GameObject healthBar;

        [SerializeField] 
        private Killable killable;

        private Camera _camera;

        private void Start() {
            _camera = Camera.main;
            var pos = transform.localPosition;
            transform.localPosition = new Vector3(pos.x, killable.photonView.IsMine ? selfYOffset : pos.y, pos.z);
        }

        private void Update() {
            transform.rotation = Quaternion.LookRotation(_camera.transform.position - transform.position);

            var scale = healthBar.transform.localScale;
            healthBar.transform.localScale = new Vector3(killable.health / killable.maxHealth, scale.y, scale.z);
        }
    }
}
