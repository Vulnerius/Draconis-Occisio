using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class MovementController : MonoBehaviour {
        [SerializeField] private GameObject player;
        [SerializeField] private Transform camera;
        [SerializeField] private Rigidbody playerRigidBody;

        [Tooltip("The force applied to the rigidbody when walking")] [SerializeField]
        public float walkSpeed;

        [Tooltip("The time it takes to turn into a new walking direction. Should be set lower than 0.2")]
        [SerializeField]
        public float turnTime;
        
        [Tooltip("The force applied vertically to the rigidbody when jumping")] [SerializeField]
        public float jumpingForce;

        private PlayerControls m_Controls;
        private PlayerMovement _movement;
        private bool canJump;

        private void Awake() {
            _movement = new PlayerMovement();
            m_Controls = new PlayerControls();
            InitMovementActions(m_Controls);
            m_Controls.Enable();
        }

        private void Update() {
            if (_movement.moveEnabled)
                Walk(walkSpeed);
        }

        private void InitMovementActions(PlayerControls controls) {
            controls.Movement.Move.started += ctx => _movement.moveEnabled = true;
            controls.Movement.Move.performed += ctx => _movement.move = ctx.ReadValue<Vector2>();
            controls.Movement.Move.canceled += _ => {
                _movement.move = Vector2.zero;
                _movement.moveEnabled = false;
            };

            controls.Movement.Jump.performed += _ => OnJump(jumpingForce);

            controls.Movement.Sprint.performed += _ => walkSpeed *= 2;
            controls.Movement.Sprint.canceled += _ => walkSpeed /= 2;
        }

        private void OnJump(float jumpF) {
            if (canJump)
                Jump(jumpF);
            else
                canJump = true;
        }

        private void Jump(float jumpForce) {
            playerRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false;
        }


        private void Walk(float speed) {
            if (_movement.move.magnitude <= .1f) return;
            float targetAngle = Mathf.Atan2(_movement.move.x, _movement.move.y) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(gameObject.transform.eulerAngles.y, targetAngle,
                ref _movement.TurnVelocity, turnTime);
            gameObject.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            playerRigidBody.AddForce(moveDirection * _movement.move.magnitude * speed, ForceMode.Acceleration);
        }

        private void OnDisable() {
            m_Controls.Disable();
        }
    }

    public class PlayerMovement {
        public Vector2 move;
        public float TurnVelocity;
        public bool moveEnabled;
    }
}