using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Health {
    public class Health : MonoBehaviour {
        [Header("Options")] [SerializeField] private int MAXHEALTH;
        private int _currentHealth;
        public bool isInvincible;
        private bool _isDead;

        public int CurrentHealth => _currentHealth;
        [Header("UI")] [SerializeField] private Slider healthBar;

        // Start is called before the first frame update
        void Start() {
            _currentHealth = MAXHEALTH;
            OnTakenDamage();
        }

        private void OnTakenDamage() {
            healthBar.value = _currentHealth;
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