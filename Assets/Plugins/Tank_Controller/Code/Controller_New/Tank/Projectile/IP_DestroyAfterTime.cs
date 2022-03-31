using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IndiePixel.Core
{
    public class IP_DestroyAfterTime : MonoBehaviour 
    {
        #region Variables
        public float m_LifeSpan = 5f;
        public UnityEvent onTimeOut = new UnityEvent();
        private float startTime;
        #endregion


        #region Main methods
    	// Use this for initialization
    	void Start () 
        {
            startTime = Time.time;
    	}
    	
    	// Update is called once per frame
    	void Update () 
        {
            if(Time.time >= startTime + m_LifeSpan)	
            {
                if(onTimeOut != null)
                {
                    onTimeOut.Invoke();
                }
                Destroy(gameObject);
            }
    	}
        #endregion
    }
}
