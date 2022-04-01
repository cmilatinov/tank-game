using IndiePixel.Controllers;
using Photon.Pun;
using UnityEngine;

namespace Gameplay {
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(TankInputs))]
    [RequireComponent(typeof(Rigidbody))]
    public class TankController : MonoBehaviour, IPunObservable {
        #region Variables

        [Header("Main Properties")]
        public float m_ForwardSpeed = 10f;
        public float m_RotationSpeed = 90f;

        [Header("Tank Parts")] 
        public TankTurretHead m_TurretHead;

        [Header("Projectile")] 
        public GameObject m_ProjectilePrefab;
        public GameObject m_MuzzleFlashPrefab;
        public Transform m_Barrel;
        public float m_FireRate = 1.0f;
        public float m_RecoilForce = 5.0f;
        
        [Header("Triple Shot")]
        public int m_TripleShots = 0;
        public float m_TripleShotAngle = 5.0f;
        public float m_TripleShotOffset = 1.0f;

        [Header("Ricochet")] 
        public int m_RicochetShots = 0;
        public int m_RicochetBounces = 3;

        private TankInputs inputs;
        private Rigidbody rb;
        private Animator animator;

        private Vector3 moveDirection;
        private float wantedAngle;
        private Quaternion moveRotation;
        private float lastFire;
        
        [Header("References")]
        public PhotonView photonView;
        #endregion
        

        #region Main Methods

        void Start() {
            inputs = GetComponent<TankInputs>();
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
            m_TurretHead.HandleRotation(inputs);
        }

        protected virtual void HandleGun() {
            if (inputs.lMouseHeldDown && Time.time - lastFire > (1 / m_FireRate)) {

                if (m_TripleShots > 0) {
                    m_TripleShots--;
                    for (int i = -1; i <= 1; i++) {
                        var shootRotation = Quaternion.Euler(new Vector3(0, i * m_TripleShotAngle, 0));
                        var shootPosition = m_Barrel.position + i * m_TripleShotOffset * m_Barrel.right;
                        GameObject obj = PhotonNetwork.Instantiate(m_ProjectilePrefab.name, shootPosition, m_Barrel.rotation * shootRotation);
                        TankProjectile projectile = obj.GetComponent<TankProjectile>();
                        if (m_RicochetShots > 0) {
                            m_RicochetShots--;
                            projectile.photonView.RPC(nameof(TankProjectile.SetBounces), RpcTarget.All, m_RicochetBounces);
                        }
                        rb.AddForce(-m_RecoilForce * (shootRotation * m_Barrel.forward), ForceMode.Impulse);
                    }
                } else {
                    GameObject obj = PhotonNetwork.Instantiate(m_ProjectilePrefab.name, m_Barrel.position, m_Barrel.rotation);
                    TankProjectile projectile = obj.GetComponent<TankProjectile>();
                    if (m_RicochetShots > 0) {
                        m_RicochetShots--;
                        projectile.photonView.RPC(nameof(TankProjectile.SetBounces), RpcTarget.All, m_RicochetBounces);
                    }
                    rb.AddForce(-m_RecoilForce * m_Barrel.forward, ForceMode.Impulse);
                }
                
                lastFire = Time.time;
                photonView.RPC(nameof(Shoot), RpcTarget.All);
            }
        }

        #endregion
        
        [PunRPC]
        public void Shoot() {
            Instantiate(m_MuzzleFlashPrefab, m_Barrel.position, m_Barrel.rotation);
        }

        [PunRPC]
        public void AddTripleShots(int numShots) {
            m_TripleShots += numShots;
        }

        [PunRPC]
        public void AddRicochetShots(int numShots) {
            m_RicochetShots += numShots;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(m_TripleShots);
                stream.SendNext(m_RicochetShots);
            } else if (stream.IsReading) {
                m_TripleShots = (int)stream.ReceiveNext();
                m_RicochetShots = (int)stream.ReceiveNext();
            }
        }
    }
}
