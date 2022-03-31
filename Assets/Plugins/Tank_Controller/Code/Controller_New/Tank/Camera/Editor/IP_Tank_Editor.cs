using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace IndiePixel.Cameras
{
    [CustomEditor(typeof(IP_Tank_Camera))]
    public class IP_Tank_Editor : Editor
    {
        #region Variables
        private IP_Tank_Camera targetCamera;
        #endregion



        #region Main Methods
    	// Use this for initialization
    	void OnEnable () 
        {
            targetCamera = (IP_Tank_Camera)target;
    	}
    	
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();



            serializedObject.ApplyModifiedProperties();
        }

        void OnSceneGUI()
        {
            if(targetCamera.m_Target)
            {
                Transform camTarget = targetCamera.m_Target;


                Handles.DrawWireDisc(camTarget.position, Vector3.up, targetCamera.m_Distance);
                Handles.color = new Color(0f, 1f, 1f, 0.15f);
                Handles.DrawSolidDisc(camTarget.position, Vector3.up, targetCamera.m_Distance);


                Handles.color = Color.red;
                targetCamera.m_Distance = Handles.ScaleSlider(targetCamera.m_Distance, camTarget.position, -camTarget.forward, Quaternion.identity, targetCamera.m_Distance, 1f);
                targetCamera.m_Distance = Mathf.Clamp(targetCamera.m_Distance, 1.0f, float.MaxValue); 


                Handles.color = Color.blue;
                targetCamera.m_Height = Handles.ScaleSlider(targetCamera.m_Height, camTarget.position, Vector3.up, Quaternion.identity, targetCamera.m_Height, 1f);
                targetCamera.m_Height = Mathf.Clamp(targetCamera.m_Height, 1.0f, float.MaxValue); 


                GUIStyle labelStyle = new GUIStyle();
                labelStyle.fontSize = 15;
                labelStyle.normal.textColor = Color.red;
                labelStyle.alignment = TextAnchor.MiddleCenter;
                Handles.Label(camTarget.position + (-camTarget.forward * targetCamera.m_Distance) + Vector3.up * 4f, "Distance", labelStyle);


                labelStyle.normal.textColor = Color.blue;
                labelStyle.alignment = TextAnchor.MiddleRight;
                Handles.Label(camTarget.position + (Vector3.up * targetCamera.m_Height) + Vector3.up * 4f, "    Height", labelStyle);

            }
        }
        #endregion
    }
}
