using Photon.Pun;
using UnityEngine;

namespace Gameplay {
    [RequireComponent(typeof(PhotonView))]
    public class DestructibleWall : Killable {
        
        [Header("Parameters")]
        [SerializeField] 
        private float damagedThreshold = 30.0f;
        
        [Header("References")]
        [SerializeField] 
        private Material damagedMaterial;

        [SerializeField] 
        private GameObject toggle;
        
        private Material _originalMaterial;
        private MeshRenderer _meshRenderer;
        
        protected override void Start() {
            base.Start();
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
            _originalMaterial = _meshRenderer.sharedMaterial;
        }

        private void UpdateState() {
            if (health < damagedThreshold) {
                _meshRenderer.sharedMaterial = damagedMaterial;
            } else {
                _meshRenderer.sharedMaterial = _originalMaterial;
            }

            toggle.SetActive(health > 0.0f);
        }

        [PunRPC]
        public void TakeDamage(float damage) {
            health -= damage;
            UpdateState();
        }
    }
}
