using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using IndiePixel.Core;

namespace IndiePixel.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(IP_DestroyAfterTime))]
    public class IP_Tank_Projectile : MonoBehaviour 
    {
        #region Variables
        public float m_StartForce = 100f;
        public GameObject m_HitExplosion;

        public UnityEvent OnHit = new UnityEvent();

        private Rigidbody rb;
        #endregion

        #region Main Methods
    	// Use this for initialization
    	void Start () 
        {
            rb = GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * m_StartForce, ForceMode.Impulse);
    	}

        void OnCollisionEnter(Collision col)
        {
            if(OnHit != null)
            {
                OnHit.Invoke();
                Destroy(this.gameObject);
            }
        }
        #endregion


        #region Helper Methods
        #endregion
    }
}
