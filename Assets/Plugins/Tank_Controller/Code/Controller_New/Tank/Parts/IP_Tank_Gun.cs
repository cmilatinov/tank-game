using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IndiePixel.Projectiles;

namespace IndiePixel.Controllers
{
    public class IP_Tank_Gun : MonoBehaviour 
    {
        #region Variables
        [Header("Main Properties")]
        public IP_Tank_Projectile m_Bullet;
        public GameObject m_MuzzleFlash;
        public Transform m_MuzzlePosition;
        #endregion


        #region Main Methods
        #endregion


        #region Helper Methods
        public virtual void FireGun()
        {
            Debug.Log("Firing Projectile");
            if(m_Bullet && m_MuzzlePosition)
            {
                Instantiate(m_Bullet, m_MuzzlePosition.position, Quaternion.LookRotation(m_MuzzlePosition.forward));
            }
        }
        #endregion
    }
}
