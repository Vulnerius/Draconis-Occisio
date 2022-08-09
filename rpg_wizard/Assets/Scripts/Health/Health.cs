using System.Collections;
using CustomUtils;
using Sound;
using UnityEngine;
using UnityEngine.UI;

namespace Health {
    public class Health : MonoBehaviour {
        [Header("Options")] [SerializeField] private int MAXHEALTH;
        [SerializeField] private SoundSource hitSound;
        private int _currentHealth;
        public bool isInvincible;
        private bool _isDead;
        private bool hitSoundPlaying;

        public int CurrentHealth => _currentHealth;
        [Header("UI")]
        [SerializeField] private Slider healthBar;
        private Canvas healthBarCanvas;
        
        /// <summary>
        /// setting the Properties
        /// </summary>
        private void Start() {
            _currentHealth = MAXHEALTH;
            healthBarCanvas = healthBar.GetComponentInParent<Canvas>();
            healthBar.maxValue = MAXHEALTH;
            healthBar.value = _currentHealth;
            UpdateUI();
        }

        private void Update() {
            StartCoroutine(CheckHealthChanged(_currentHealth));
        }

        /// <summary>
        /// enabling or disabling the healthBar
        /// </summary>
        /// <param name="currentHealth"></param>
        /// <returns>Waiting</returns>
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

        /// <summary>
        /// reducing the currentHealth by value
        /// </summary>
        /// <param name="value"></param>
        private void GetDamagedInstantly(int value) {
            if (isInvincible || _isDead) return;
            _currentHealth -= value;
            ClampHealth();
            _isDead = CheckIfDead();
        }

        /// <summary>
        /// Playing the attached hitSound
        /// </summary>
        private void PlayHitSound() {
            if (hitSoundPlaying) return;
            hitSound.Play(transform);
            StartCoroutine(HitSoundCoolDown());
        }

        /// <summary>
        /// starting a coolDown so there is only 1 hitSound playing at a time
        /// </summary>
        /// <returns>Waiting for hitSound to complete</returns>
        private IEnumerator HitSoundCoolDown() {
            hitSoundPlaying = true;
            yield return new WaitForSeconds(hitSound.sound.time);
            hitSoundPlaying = false;
        }

        /// <summary>
        /// making sure health is not below 0
        /// </summary>
        private void ClampHealth() {
            _currentHealth = Mathf.Clamp(_currentHealth, 0, MAXHEALTH);
            UpdateUI();
        }

        public void GetDamagedOverTime(int value, float time) {
            StartCoroutine(GetDamaged(value, time, 3));
            PlayHitSound();
        }

        /// <summary>
        /// reducing the currentHealth by value over time
        /// further explained in documentation
        /// </summary>
        /// <param name="value"></param>
        /// <param name="time"></param>
        /// <param name="tickRate"></param>
        /// <returns></returns>
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

        /// <summary>
        /// increasing the currentHealth by value
        /// </summary>
        /// <param name="value"></param>
        public void IncreaseHealth(int value) {
            if (!_isDead)
                _currentHealth += value;
            ClampHealth();
        }

        public void ResetHealth() {
            _currentHealth = MAXHEALTH;
        }

        private bool CheckIfDead() {
            return _currentHealth <= 0;
        }
    }
}