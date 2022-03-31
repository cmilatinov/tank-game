using Photon.Pun;
using UnityEngine;

namespace IndiePixel.Controllers {
    [RequireComponent(typeof(IP_Tank_Inputs))]
    [RequireComponent(typeof(Rigidbody))]
    public class IP_Tank_Controller : MonoBehaviour {
        #region Variables

        [Header("Main Properties")]
        public float m_ForwardSpeed = 10f;
        public float m_RotationSpeed = 90f;

        [Header("Tank Parts")] 
        public IP_Tank_TurretHead m_TurretHead;

        [Header("Projectile")] 
        public GameObject m_ProjectilePrefab;
        public GameObject m_MuzzleFlashPrefab;
        public Transform m_Barrel;
        public float m_FireRate = 1.0f;
        public float m_RecoilForce = 10.0f;

        private IP_Tank_Inputs inputs;
        private Rigidbody rb;
        private Animator animator;

        private Vector3 moveDirection;
        private float wantedAngle;
        private Quaternion moveRotation;
        private PhotonView photonView;
        private float lastFire;
        #endregion
        

        #region Main Methods

        void Start() {
            inputs = GetComponent<IP_Tank_Inputs>();
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            photonView = GetComponent<PhotonView>();

            moveRotation = Quaternion.identity;
        }

        void Update() {
            if (photonView.IsMine) {
                HandleTranslation();
                HandleRotation();

                if (m_TurretHead) {
                    HandleTurretHead();
                }

                HandleGun();
            }
        }
        #endregion


        #region Helper Methods

        protected virtual void HandleTranslation() {
            moveDirection = transform.forward * (inputs.vInput * m_ForwardSpeed);
            rb.velocity = Vector3.Slerp(rb.velocity, moveDirection, 10.0f * Time.deltaTime);
        }

        protected virtual void HandleRotation() {
            wantedAngle += inputs.hInput * m_RotationSpeed * Time.deltaTime;
            moveRotation = Quaternion.Slerp(moveRotation, Quaternion.Euler(0f, wantedAngle, 0f), Time.deltaTime * 10f);
            rb.MoveRotation(moveRotation);
        }

        protected virtual void HandleTurretHead() {
            m_TurretHead.HandleRotation(transform.forward, inputs);
        }

        protected virtual void HandleGun() {
            if (inputs.lMouseHeldDown && Time.time - lastFire > (1 / m_FireRate) && photonView.IsMine) {
                lastFire = Time.time;
                PhotonNetwork.Instantiate(m_ProjectilePrefab.name, m_Barrel.position, m_Barrel.rotation);
                rb.AddForce(-m_RecoilForce * m_Barrel.forward, ForceMode.Impulse);
                photonView.RPC(nameof(Shoot), RpcTarget.All);
            }
        }

        #endregion
        
        [PunRPC]
        public void Shoot() {
            Instantiate(m_MuzzleFlashPrefab, m_Barrel.position, m_Barrel.rotation);
        }
    }
}
