using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class MovementController : MonoBehaviour {
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject wizardPrefab;
        [SerializeField] private Transform camera;
        [SerializeField] private CharacterController characterController;

        [Tooltip("The force applied to the rigidbody when walking")] [SerializeField]
        public float walkSpeed;
        private float initWalkSpeed;

        [Tooltip("The time it takes to turn into a new walking direction. Should be set lower than 0.2")]
        [SerializeField]
        public float turnTime;

        [Tooltip("The force applied vertically to the rigidbody when jumping")] [SerializeField]
        public float jumpingForce;

        private Animator playerAnimator;
        private PlayerControls m_Controls;
        private PlayerMovement _movement;
        private bool canJump;

        private void Awake() {
            initWalkSpeed = walkSpeed;
            _movement = new PlayerMovement();
            m_Controls = new PlayerControls();
            InitMovementActions(m_Controls);
            m_Controls.Enable();
            playerAnimator = wizardPrefab.GetComponent<Animator>();
        }

        private void Update() {
            if (_movement.jumped)
                StartCoroutine(DoJump());
            if (_movement.moveEnabled)
                Walk(walkSpeed);

            WalkOrIdle();
        }
        
        private IEnumerator DoJump() {
            _movement.moveEnabled = false;
            OnJump(jumpingForce);
            yield return new WaitForSeconds(.5f);
            _movement.moveEnabled = true;
        }

        private void WalkOrIdle() {
            if (!_movement.moveEnabled && !_movement.jumped)
                StartCoroutine(playIdle());
        }

        private IEnumerator playIdle() {
            yield return new WaitForSeconds(0.1f);
            playerAnimator.Play("Idle03");
        }

        private void InitMovementActions(PlayerControls controls) {
            controls.Movement.Move.started += ctx => _movement.moveEnabled = true;

            controls.Movement.Move.performed += ctx => { _movement.move = ctx.ReadValue<Vector2>(); };
            controls.Movement.Move.canceled += _ => {
                _movement.move = Vector2.zero;
                _movement.moveEnabled = false;
            };

            controls.Movement.Jump.performed += _ => {
                _movement.jumped = true;
            };
            controls.Movement.Jump.canceled += _ => _movement.jumped = false;

            controls.Movement.Sprint.performed += _ => {
                walkSpeed *= 2;
                playerAnimator.Play("BattleRunForward");
            };
            controls.Movement.Sprint.canceled += _ => walkSpeed /= 2;
        }

        private void OnJump(float jumpF) {
            playerAnimator.Play("JumpUp");
            characterController.Move(Vector3.up * jumpF * Time.fixedDeltaTime);
        }

        private void Walk(float speed) {
            if (_movement.move.magnitude <= .1f) return;
            if(Math.Abs(initWalkSpeed - walkSpeed) < .1f)
                playerAnimator.Play("WalkForward");
            float targetAngle = Mathf.Atan2(_movement.move.x, _movement.move.y) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(gameObject.transform.eulerAngles.y, targetAngle,
                ref _movement.TurnVelocity, turnTime);
            gameObject.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            characterController.SimpleMove(moveDirection * _movement.move.magnitude * speed);
        }

        private void OnDisable() {
            m_Controls.Disable();
        }
    }

    public class PlayerMovement {
        public Vector2 move;
        public float TurnVelocity;
        public bool moveEnabled;
        public bool jumped;
    }
}