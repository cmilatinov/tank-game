using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndiePixel.Cameras {
    public class TankCamera : MonoBehaviour {
        #region Variables

        public Transform m_Target;
        public float m_Height = 10f;
        public float m_Distance = 5f;
        public float m_Angle = 45f;

        private Vector3 refVelocity;

        #endregion


        #region Main Methods

        // Use this for initialization
        void Start() {
            HandleCamera();
        }

        // Update is called once per frame
        void Update() {
            HandleCamera();
        }

        #endregion


        #region Helper Methods

        protected virtual void HandleCamera() {
            if (m_Target) {
                Vector3 worldPosition = (Vector3.forward * -m_Distance) + (Vector3.up * m_Height);
                Debug.DrawLine(m_Target.position, worldPosition, Color.blue);

                Vector3 rotatedVector = Quaternion.AngleAxis(m_Angle, Vector3.up) * worldPosition;
                Debug.DrawLine(m_Target.position, rotatedVector, Color.red);

                Vector3 flatTargetPosition = m_Target.position;
                flatTargetPosition.y = 0f;
                Vector3 finalPosition = flatTargetPosition + rotatedVector;

                Debug.DrawLine(m_Target.position, finalPosition, Color.green);
                transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, 0.5f);
                transform.LookAt(m_Target.position);
            }
        }

        void OnDrawGizmos() {
            Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
            if (m_Target) {
                Gizmos.DrawLine(transform.position, m_Target.position);
                Gizmos.DrawSphere(m_Target.position, 2f);
            }

            Gizmos.DrawSphere(transform.position, 2f);

            Gizmos.color = Color.white;
        }

        #endregion
    }
}
