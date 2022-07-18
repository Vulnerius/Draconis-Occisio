using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class MovementController : MonoBehaviour {
        [SerializeField] private GameObject wizardPrefab;
        [SerializeField] private Transform cameraPosition;
        [SerializeField] private CharacterController characterController;

        [Tooltip("The force applied to the rigidbody when walking")] [SerializeField]
        public float walkSpeed;
        private float initWalkSpeed;

        [Tooltip("The time it takes to turn into a new walking direction. Should be set lower than 0.2")]
        [SerializeField] public float turnTime;

        [Tooltip("The force applied vertically to the rigidbody when jumping")] 
        [SerializeField] public float jumpingForce;
        
        private PlayerControls m_Controls;
        private PlayerStates m_States;
        public PlayerStates States => m_States;
        private bool canJump;

        private void Awake() {
            initWalkSpeed = walkSpeed;
            m_States = new PlayerStates();
            m_Controls = new PlayerControls();
            InitMovementActions(m_Controls);
            m_Controls.Enable();
        }

        private void Update() {
            if (m_States.jumped && !m_States.ability)
                StartCoroutine(DoJump());
            if (m_States.moveEnabled && !m_States.ability)
                Walk(walkSpeed);
        }

        private void InitMovementActions(PlayerControls controls) {
            controls.Movement.Move.started += ctx => {
                m_States.moveEnabled = true; 
                PlayerAnimationState.isWalking = true;
            };

            controls.Movement.Move.performed += ctx => {
                m_States.move = ctx.ReadValue<Vector2>();
                PlayerAnimationState.isWalking = true;
            };
            controls.Movement.Move.canceled += _ => {
                m_States.move = Vector2.zero;
                m_States.moveEnabled = false;
                PlayerAnimationState.isWalking = false;
            };

            controls.Movement.Jump.performed += _ => {
                m_States.jumped = true;
                PlayerAnimationState.isJumping = true;
            };
            controls.Movement.Jump.canceled += _ => {
                m_States.jumped = false;
            };

            controls.Movement.Sprint.performed += _ => {
                walkSpeed *= 2; 
                PlayerAnimationState.isRunning = true;
            };
            controls.Movement.Sprint.canceled += _ => { walkSpeed /= 2;   PlayerAnimationState.isRunning = false;};
        }

        private void OnJump(Vector3 jumpVector) {
            if (!canJump) return;
            jumpVector.y -= 10.0f * Time.deltaTime;
            characterController.Move(jumpVector);
        }

        private IEnumerator DoJump() {
            m_States.moveEnabled = false;
            OnJump(new Vector3(0,jumpingForce,0));
            canJump = false;
            yield return new WaitForSeconds(.5f);
            PlayerAnimationState.isJumping = false;
            m_States.moveEnabled = true;
        }

        private void Walk(float speed) {
            if (m_States.move.magnitude <= .1f) return;
            
            canJump = true;
            float targetAngle = Mathf.Atan2(m_States.move.x, m_States.move.y) * Mathf.Rad2Deg + cameraPosition.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(gameObject.transform.eulerAngles.y, targetAngle,
                ref m_States.TurnVelocity, turnTime);
            gameObject.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            characterController.SimpleMove(moveDirection * m_States.move.magnitude * speed);
        }

        private void OnDisable() {
            m_Controls.Disable();
        }
        
    }

}