using System;
using UnityEngine;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Characters.ThirdPerson;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {
        public Vector2 InputVectorMove;
        public Vector2 InputVectorLook;
        public bool InputBool;
        [SerializeField]private ThirdPersonUserControl _control;
        [SerializeField] private FreeLookCam _cam;

        private void Update()
        {
            _control.move = InputVectorMove;
            _cam.look = InputVectorLook;
        }

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            InputVectorMove = virtualMoveDirection;
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            InputVectorLook = virtualLookDirection;
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            InputBool = virtualJumpState;
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            InputBool = virtualSprintState;
        }
        
    }

}
