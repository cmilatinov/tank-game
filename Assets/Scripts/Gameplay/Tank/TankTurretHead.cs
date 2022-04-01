using UnityEngine;

namespace IndiePixel.Controllers {
    public class TankTurretHead : MonoBehaviour {
        [Header("Main Properties")] 
        public float mRotationSpeed = 4.0f;

        public void HandleRotation(TankInputs input) {
            Vector3 lookDir = input.WorldPosition - transform.position;
            lookDir.y = 0.0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * mRotationSpeed);
        }
    }
}
