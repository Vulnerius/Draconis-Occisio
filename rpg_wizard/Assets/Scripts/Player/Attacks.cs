using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

namespace Player {
    /// <summary>
    /// Handling Abilities Tornado and Shield 
    /// </summary>
    public class Attacks : MonoBehaviour {
        [Header("Fireball")] [SerializeField] private GameObject fireBall;
        [SerializeField] private GameObject fireBallUI;
        [SerializeField] private TextMeshProUGUI fireballCDText;
        [SerializeField] private float fireBallCoolDown;

        [Header("Shield")] [SerializeField] private GameObject watershield;
        [SerializeField] private GameObject watershieldUI;
        [SerializeField] private TextMeshProUGUI watershieldCDText;
        [SerializeField] private Slider watershieldRemainingTime;
        [SerializeField] private float shieldCoolDown;

        private PlayerControls Controls;
        private PlayerStates m_States;

        public bool hasAttackCoolDown;
        public bool hasDefenseCoolDown;

        /// <summary>
        /// Instantiating InputActions and PlayerStates
        /// </summary>
        private void Awake() {
            Controls = new PlayerControls();
            InitAttackActions(Controls);
            Controls.Enable();
            StartCoroutine(GetStates());
        }

        /// <summary>
        /// assigning m_States
        /// </summary>
        /// <returns>Waiting until InputActions are instantiated</returns>
        private IEnumerator GetStates() {
            if (gameObject.GetComponent<MovementController>().States == null)
                yield return new WaitForSeconds(.1f);
            m_States = gameObject.GetComponent<MovementController>().States;
        }

        /// <summary>
        /// Initializing InputActions then assigning functionality
        /// </summary>
        /// <param name="controls">InputAction map</param>
        private void InitAttackActions(PlayerControls controls) {
            controls.Attacks.FireBall.performed += _ => StartCoroutine(InstantiateFireBall());

            controls.Attacks.WaterShield.performed += _ => StartCoroutine(InstantiateWaterShield());
        }

        /// <summary>
        /// performing animation
        /// instantiating the Shield prefab
        /// Starting Coroutines for Cooldown and remainingTime
        /// </summary>
        /// <returns>Waiting</returns>
        private IEnumerator InstantiateWaterShield() {
            if (hasDefenseCoolDown) yield break;
            m_States.ability = true;
            PlayerAnimationState.isDefending = true;
            hasDefenseCoolDown = true;

            yield return new WaitForSeconds(.4f);
            PlayerAnimationState.isDefending = false;
            m_States.ability = false;

            var shield = Instantiate(watershield, transform.position + Vector3.up, Quaternion.identity);
            Destroy(shield, 3f);
            watershieldUI.SetActive(true);
            StartCoroutine(CoolDownShield());
            StartCoroutine(RemainTimeShield());
        }

        /// <summary>
        /// UI code for Slider above the shield
        /// </summary>
        /// <returns>Waiting until shield ran out</returns>
        private IEnumerator RemainTimeShield() {
            var shieldMaxValue = 2.8f;
            watershieldRemainingTime.gameObject.SetActive(true);
            watershieldRemainingTime.maxValue = shieldMaxValue;
            while (shieldMaxValue > 0) {
                watershieldRemainingTime.value = shieldMaxValue;
                shieldMaxValue -= .1f;
                yield return new WaitForSeconds(.1f);
            }
            watershieldRemainingTime.gameObject.SetActive(false);
        }

        /// <summary>
        /// starts the UpdateShield Coroutine
        /// waits for the Cooldown to run out
        /// disables the overlay of the shieldCooldown
        /// enables shield ability
        /// </summary>
        /// <returns></returns>
        private IEnumerator CoolDownShield() {
            StartCoroutine(UpdateShieldCdText());
            yield return new WaitForSeconds(shieldCoolDown);
            watershieldUI.SetActive(false);
            hasDefenseCoolDown = false;
        }

        /// <summary>
        /// updating the shield cooldown text every .1 seconds
        /// </summary>
        /// <returns>Waiting for .1 seconds</returns>
        private IEnumerator UpdateShieldCdText() {
            var tickCd = shieldCoolDown;
            while (tickCd > 0) {
                watershieldCDText.text = $"{tickCd:0.00}";
                tickCd -= .1f;
                yield return new WaitForSeconds(.1f);
            }
        }

        /// <summary>
        /// performing animation
        /// instantiating the tornado prefab
        /// Destroying the tornado after 3 seconds
        /// Starting Coroutine for Cooldown
        /// </summary>
        /// <returns>Waiting</returns>
        private IEnumerator InstantiateFireBall() {
            if (hasAttackCoolDown) yield break;
            PlayerAnimationState.isAttacking = true;
            m_States.ability = true;
            hasAttackCoolDown = true;

            yield return new WaitForSeconds(.4f);
            m_States.ability = false;
            PlayerAnimationState.isAttacking = false;

            var selfTransform = transform;
            var playerTornado = Instantiate(fireBall, selfTransform.position + selfTransform.forward + Vector3.up,
                    Quaternion.identity);
            playerTornado.GetComponent<VisualEffect>().Play();
            Destroy(playerTornado,3f);
            fireBallUI.SetActive(true);
            StartCoroutine(CoolDownFireBall());
        }

        /// <summary>
        /// starting text-update method for tornado
        /// waiting for cooldown time
        /// disabling cooldown overlay
        /// enabling tornado ability
        /// </summary>
        /// <returns></returns>
        private IEnumerator CoolDownFireBall() {
            StartCoroutine(UpdateFireballCdText());
            yield return new WaitForSeconds(fireBallCoolDown);
            fireBallUI.SetActive(false);
            hasAttackCoolDown = false;
        }

        /// <summary>
        /// updating the tornado cooldown text every .1 seconds
        /// </summary>
        /// <returns>Waiting for .1 seconds</returns>
        private IEnumerator UpdateFireballCdText() {
            var tickCd = fireBallCoolDown;
            while (tickCd > 0) {
                fireballCDText.text = $"{tickCd:0.00}";
                tickCd -= .1f;
                yield return new WaitForSeconds(.1f);
            }
        }

        /// <summary>
        /// disabling InputActions
        /// </summary>
        private void OnDisable() {
            Controls.Disable();
        }
    }
}