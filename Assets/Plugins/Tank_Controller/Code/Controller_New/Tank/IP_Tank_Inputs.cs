using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace IndiePixel.Controllers {
    public class IP_Tank_Inputs : MonoBehaviour {
        #region Variables

        public float vInput;
        public float hInput;
        public float hArrows;


        public bool lMouseDown = false;
        public bool lMouseHeldDown = false;
        public bool rMouseDown = false;
        public bool rMouseHeldDown = false;

        
        public Transform m_Reticle;
        public LayerMask m_ReticleLayerMask;


        private PhotonView photonView;
        
        public Vector2 MouseScreenPosition {
            get { return Input.mousePosition; }
        }

        private Vector3 worldPosition;

        public Vector3 WorldPosition {
            get { return worldPosition; }
        }

        #endregion

        private Camera _camera;


        #region Main Methods

        private void Start() {
            _camera = Camera.main;
            photonView = GetComponent<PhotonView>();
        }

        void Update() {
            if (photonView.IsMine) {
                HandleInputs();
                HandleMousePositions();
                HandleMouseButtons();
            }
        }

        #endregion


        #region Helper Methods

        protected virtual void HandleInputs() {
            vInput = Input.GetAxis("Vertical");
            hInput = Input.GetAxis("Horizontal");

            if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) {
                hArrows = -1f;
            }
            else if (!Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)) {
                hArrows = 1f;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)) {
                hArrows = 0f;
            }
            else {
                hArrows = 0f;
            }
        }

        protected virtual void HandleMousePositions() {
            Vector2 screenPos = MouseScreenPosition;
            RaycastHit hit;
            Ray curRay = _camera.ScreenPointToRay(screenPos);

            if (Physics.Raycast(curRay, out hit, 1000, m_ReticleLayerMask.value)) {
                worldPosition = hit.point;
                if (m_Reticle) {
                    m_Reticle.position = hit.point;
                }
            }
        }

        protected virtual void HandleMouseButtons() {
            lMouseDown = Input.GetMouseButtonDown(0);
            lMouseHeldDown = Input.GetMouseButton(0);
            rMouseDown = Input.GetMouseButtonDown(1);
            rMouseHeldDown = Input.GetMouseButton(1);
        }

        #endregion
    }
}
