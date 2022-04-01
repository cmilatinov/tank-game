using UnityEngine;
using UnityEngine.Events;

namespace Utility {
    public class ForwardTrigger : MonoBehaviour {
        [SerializeField]
        private UnityEvent<Collider> triggerEvent;

        private void OnTriggerEnter(Collider other) {
            triggerEvent?.Invoke(other);
        }
    }
}
