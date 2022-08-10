using UnityEngine;

namespace Player {
    /// <summary>
    /// Handling Movement for the Player
    /// </summary>
    public class MovementController : MonoBehaviour {
        [SerializeField] private Transform cameraPosition;
        [SerializeField] private CharacterController characterController;

        [Tooltip("The force applied to the rigidbody when walking")] [SerializeField]
        public float walkSpeed;

        [Tooltip("The time it takes to turn into a new walking direction.")]
        [SerializeField] public float turnTime;
        
        private PlayerControls m_Controls;
        private PlayerStates m_States;
        public PlayerStates States => m_States;

        /// <summary>
        /// Instantiating PlayerStates and Controls
        /// Initializing MovementActions
        /// enabling InputAction
        /// </summary>
        private void Awake() {
            m_States = new PlayerStates();
            m_Controls = new PlayerControls();
            InitMovementActions(m_Controls);
            m_Controls.Enable();
        }

        /// <summary>
        /// checking is Player is Flying
        /// checking if Player casted an ability
        /// </summary>
        private void Update() {
            if (PlayerAnimationState.isLevitating)
                Fly(walkSpeed);
            if (m_States.moveEnabled && !m_States.ability)
                Walk(walkSpeed);
            
        }

        /// <summary>
        /// moving the wizard up and in pressed movement direction
        /// </summary>
        /// <param name="f">movement Speed in air</param>
        private void Fly(float f) {
            var flyDirection = Time.deltaTime * Walk(f);
            characterController.Move(new Vector3(flyDirection.x, .1f, flyDirection.z));
        }

        /// <summary>
        /// assigning functionality to InputActions
        /// </summary>
        /// <param name="controls">InputAction</param>
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

            controls.Movement.Sprint.performed += _ => {
                walkSpeed *= 2; 
                PlayerAnimationState.isRunning = true;
            };
            controls.Movement.Sprint.canceled += _ => { walkSpeed /= 2;   PlayerAnimationState.isRunning = false;};

            controls.Movement.Levitate.performed += _ => PlayerAnimationState.isLevitating = true;
            controls.Movement.Levitate.canceled += _ => PlayerAnimationState.isLevitating = false;
        }
        
        /// <summary>
        /// calculating the targetAngle to move towards
        /// transferring the angle to a Vector3
        /// moving the gameObject towards the direction
        /// </summary>
        /// <param name="speed">MovementSpeed</param>
        /// <returns>moveDirection as Vector3</returns>
        private Vector3 Walk(float speed) {
            if (m_States.move.magnitude <= .1f) return Vector3.zero;
            
            float targetAngle = Mathf.Atan2(m_States.move.x, m_States.move.y) * Mathf.Rad2Deg + cameraPosition.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(gameObject.transform.eulerAngles.y, targetAngle,
                ref m_States.TurnVelocity, turnTime);
            gameObject.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            characterController.SimpleMove(moveDirection * m_States.move.magnitude * speed);
            return moveDirection;
        }

        private void OnDisable() {
            m_Controls.Disable();
        }
        
    }

}