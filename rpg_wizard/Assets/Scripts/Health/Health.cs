using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Health {
    public class Health : MonoBehaviour {
        [Header("Options")] [SerializeField] private int MAXHEALTH;
        private int _currentHealth;
        public bool isInvincible;
        private bool _isDead;

        public int CurrentHealth => _currentHealth;
        [Header("UI")]
        [SerializeField] private Slider healthBar;
        [SerializeField] private TextMeshProUGUI healthValueText;
        private Canvas healthBarCanvas;
        // Start is called before the first frame update
        void Start() {
            _currentHealth = MAXHEALTH;
            healthBarCanvas = healthBar.GetComponentInParent<Canvas>();
            OnTakenDamage();
        }

        private void Update() {
            StartCoroutine(CheckHealthChanged(_currentHealth));
        }

        private IEnumerator CheckHealthChanged(int currentHealth) {
            yield return new WaitForSeconds(2f);
            if (_currentHealth == currentHealth && !gameObject.CompareTag("Player"))
                healthBarCanvas.enabled = false;
        }

        private void OnTakenDamage() {
            if(!healthBarCanvas.enabled)
                healthBarCanvas.enabled = true;
            healthBar.value = _currentHealth;
            if (healthValueText != null)
                healthValueText.text = _currentHealth.ToString();
        }

        public void GetDamagedInstantly(int value) {
            if (isInvincible || _isDead) return;
            _currentHealth -= value;
            ClampHealth();
            _isDead = CheckIfDead();
            OnTakenDamage();
        }

        private void ClampHealth() {
            _currentHealth = Mathf.Clamp(_currentHealth, 0, MAXHEALTH);
        }

        public void GetDamagedOverTime(int value, float time) {
            StartCoroutine(GetDamaged(value, time, 3));
        }

        private IEnumerator GetDamaged(int value, float time, int tickRate) {
            var tickTime = time / tickRate;
            var actualTickRate = tickRate;

            while (tickRate > 0) {
                yield return new WaitForSeconds(tickTime);
                GetDamagedInstantly(value / actualTickRate);
                tickRate--;
            }

            GetDamagedInstantly(value % actualTickRate);
        }

        public void IncreaseHealth(int value) {
            if (!_isDead)
                _currentHealth += value;
            ClampHealth();
        }

        public void GetHealthOverTime(int value, float time) {
            StartCoroutine(GetHealing(value, time, 3));
        }

        private IEnumerator GetHealing(int value, float time, int tickRate) {
            var tickTime = time / tickRate;
            var actualTickRate = tickRate;

            while (tickRate > 0) {
                yield return new WaitForSeconds(tickTime);
                IncreaseHealth(value / actualTickRate);
                tickRate--;
            }

            IncreaseHealth(value % actualTickRate);
        }

        public void ResetHealth() {
            _currentHealth = MAXHEALTH;
        }

        private bool CheckIfDead() {
            return _currentHealth <= 0;
        }
    }
}