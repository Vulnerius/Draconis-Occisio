using System;
using System.Collections;
using CustomUtils;
using Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Health {
    public class Health : MonoBehaviour {
        [Header("Options")] [SerializeField] private int MAXHEALTH;
        [SerializeField] private SoundSource hitSound;
        private int _currentHealth;
        public bool isInvincible;
        private bool _isDead;

        public int CurrentHealth => _currentHealth;
        [Header("UI")]
        [SerializeField] private Slider healthBar;
        private Canvas healthBarCanvas;
        
        void Start() {
            _currentHealth = MAXHEALTH;
            healthBarCanvas = healthBar.GetComponentInParent<Canvas>();
            healthBar.maxValue = MAXHEALTH;
            healthBar.value = _currentHealth;
            UpdateUI();
        }

        private void Update() {
            StartCoroutine(CheckHealthChanged(_currentHealth));
        }

        private IEnumerator CheckHealthChanged(int currentHealth) {
            yield return new WaitForSeconds(2f);
            if (_currentHealth == currentHealth && !gameObject.CompareTag("Player"))
                healthBarCanvas.enabled = false;
        }

        private void UpdateUI() {
            if(!healthBarCanvas.enabled){
                healthBarCanvas.enabled = true;
                healthBarCanvas.transform.LookAt(ReferenceTable.Player.transform);
            }
            healthBar.value = _currentHealth;
        }

        public void GetDamagedInstantly(int value) {
            if (isInvincible || _isDead) return;
            PlayHitSound();
            _currentHealth -= value;
            ClampHealth();
            _isDead = CheckIfDead();
        }

        private void PlayHitSound() {
            hitSound.Play(transform);
        }

        private void ClampHealth() {
            _currentHealth = Mathf.Clamp(_currentHealth, 0, MAXHEALTH);
            UpdateUI();
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