using System;
using System.Collections;
using CustomUtils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

namespace Player {
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

        private void Awake() {
            Controls = new PlayerControls();
            InitAttackActions(Controls);
            Controls.Enable();
            StartCoroutine(GetStates());
        }

        private IEnumerator GetStates() {
            if (gameObject.GetComponent<MovementController>().States == null)
                yield return new WaitForSeconds(.1f);
            m_States = gameObject.GetComponent<MovementController>().States;
        }

        private void InitAttackActions(PlayerControls controls) {
            controls.Attacks.FireBall.performed += _ => StartCoroutine(InstantiateFireBall());

            controls.Attacks.WaterShield.performed += _ => StartCoroutine(InstantiateWaterShield());
        }

        private IEnumerator InstantiateWaterShield() {
            if (hasDefenseCoolDown) yield break;
            ReferenceTable.GameManager.setText("shield spawn");
            m_States.ability = true;
            PlayerAnimationState.isDefending = true;
            hasDefenseCoolDown = true;

            yield return new WaitForSeconds(.4f);
            PlayerAnimationState.isDefending = false;
            m_States.ability = false;

            var fireB = Instantiate(watershield, transform.position + Vector3.up, Quaternion.identity);
            Destroy(fireB, 3f);
            watershieldUI.SetActive(true);
            StartCoroutine(CoolDownShield());
            StartCoroutine(RemainTimeShield());
        }

        private IEnumerator RemainTimeShield() {
            var shieldMaxValue = 2.8f;
            watershieldRemainingTime.gameObject.SetActive(true);
            watershieldRemainingTime.maxValue = shieldMaxValue;
            while (shieldMaxValue > 0) {
                watershieldRemainingTime.value = 3 - shieldMaxValue;
                shieldMaxValue -= .1f;
                yield return new WaitForSeconds(.1f);
            }
            watershieldRemainingTime.gameObject.SetActive(false);
        }

        private IEnumerator CoolDownShield() {
            StartCoroutine(UpdateShieldCdText());
            yield return new WaitForSeconds(shieldCoolDown);
            watershieldUI.SetActive(false);
            hasDefenseCoolDown = false;
        }

        private IEnumerator UpdateShieldCdText() {
            var tickCd = shieldCoolDown;
            while (tickCd > 0) {
                watershieldCDText.text = $"{tickCd:0.00}";
                tickCd -= .1f;
                yield return new WaitForSeconds(.1f);
            }
        }

        private IEnumerator InstantiateFireBall() {
            if (hasAttackCoolDown) yield break;
            ReferenceTable.GameManager.setText("tornado spawn");
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

        private IEnumerator CoolDownFireBall() {
            StartCoroutine(UpdateFireballCdText());
            yield return new WaitForSeconds(fireBallCoolDown);
            fireBallUI.SetActive(false);
            hasAttackCoolDown = false;
        }

        private IEnumerator UpdateFireballCdText() {
            var tickCd = fireBallCoolDown;
            while (tickCd > 0) {
                fireballCDText.text = $"{tickCd:0.00}";
                tickCd -= .1f;
                yield return new WaitForSeconds(.1f);
            }
        }

        private void OnDisable() {
            Controls.Disable();
        }
    }
}