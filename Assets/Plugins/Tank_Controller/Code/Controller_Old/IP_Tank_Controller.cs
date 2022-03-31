using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TankDemo
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(IP_Tank_Inputs))]
    public class IP_Tank_Controller : MonoBehaviour
    {
        #region Variables
        [Header("Movement Properties")]
        public float tankSpeed = 15f;
        public float tankRotationSpeed = 20f;

        [Header("Turret Properties")]
        public Transform turretTransform;
        public float turretLagSpeed = 0.5f;

        [Header("Reticle Properties")]
        public Transform reticleTransform;
            


        private Rigidbody rb;
        private IP_Tank_Inputs input;
        private Vector3 finalTurretLookDir;
        #endregion


        #region Builtin Methods
        // Use this for initialization
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            input = GetComponent<IP_Tank_Inputs>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(rb && input)
            {
                HandleMovement();
                HandleTurret();
                HandleReticle();
            }
        }
        #endregion



        #region Custom Methods
        protected virtual void HandleMovement()
        {
            //Move Tank forward
            Vector3 wantedPosition = transform.position + (transform.forward * input.ForwardInput * tankSpeed * Time.deltaTime);
            rb.MovePosition(wantedPosition);

            //Rotate the Tank
            Quaternion wantedRotation = transform.rotation * Quaternion.Euler(Vector3.up * (tankRotationSpeed * input.RotationInput * Time.deltaTime));
            rb.MoveRotation(wantedRotation);
        }

        protected virtual void HandleTurret()
        {
            if(turretTransform)
            {
                Vector3 turretLookDir = input.ReticlePosition - turretTransform.position;
                turretLookDir.y = 0f;

                finalTurretLookDir = Vector3.Lerp(finalTurretLookDir, turretLookDir, Time.deltaTime * turretLagSpeed);
                turretTransform.rotation = Quaternion.LookRotation(finalTurretLookDir);
            }
        }

        protected virtual void HandleReticle()
        {
            if(reticleTransform)
            {
                reticleTransform.position = input.ReticlePosition;
            }
        }
        #endregion
    }
}
