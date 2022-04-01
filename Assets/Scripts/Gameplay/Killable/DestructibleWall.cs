using Enums;
using Photon.Pun;
using Unity.AI.Navigation;
using UnityEngine;

namespace Gameplay {
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
        private bool _isActive = true;
        
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
            
            var active = health > 0.0f;
            if (_isActive != active) {
                GameObject.FindWithTag(Tag.NavMesh).GetComponent<NavMeshSurface>().BuildNavMesh();
            }

            _isActive = active;
            toggle.SetActive(_isActive);
        }

        [PunRPC]
        public override void TakeDamage(float damage) {
            base.TakeDamage(damage);
            UpdateState();
        }

        public override void OnChangeHealth() {
            UpdateState();
        }
    }
}
