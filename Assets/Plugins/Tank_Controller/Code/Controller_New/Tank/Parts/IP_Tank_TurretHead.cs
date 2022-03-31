using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndiePixel.Controllers {
    public class IP_Tank_TurretHead : MonoBehaviour {
        [Header("Main Properties")] 
        public float mRotationSpeed = 4.0f;

        public void HandleRotation(Vector3 fwdDir, IP_Tank_Inputs input) {
            Vector3 lookDir = input.WorldPosition - transform.position;
            lookDir.y = 0.0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * mRotationSpeed);
        }
    }
}
