using System.Collections;
using Sound;
using UnityEngine;
using UnityEngine.VFX;

namespace Health {
    /// <summary>
    /// HealthPackage behaviour
    /// </summary>
    public class HealthPackage : MonoBehaviour {
        [SerializeField] private SoundSource sound;
        [SerializeField] private int healAmount;
        [SerializeField] private float coolDown;
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private VisualEffect effect;
        [SerializeField] private ParticleSystem particleLight;

        /// <summary>
        /// increases the colliding gameObjects currentHealth
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.GetComponentInParent<Health>() == null) return;

            other.gameObject.GetComponentInParent<Health>().IncreaseHealth(healAmount);
            sound.Play(transform);
            StartCoroutine(CoolDownSelf());
        }

        /// <summary>
        /// Disables the GameObject
        /// starts cooldown
        /// Enables the GameObject after Cooldown
        /// </summary>
        /// <returns>Waiting for cooldown</returns>
        private IEnumerator CoolDownSelf() {
            Disable();
            yield return new WaitForSeconds(coolDown);
            Enable();
        }

        /// <summary>
        /// enabling boxCollider, visualEffect and ParticleSystem
        /// </summary>
        private void Enable() {
            boxCollider.enabled = true;
            effect.gameObject.SetActive(true);
            particleLight.gameObject.SetActive(true);
        }

        /// <summary>
        /// disabling boxCollider, visualEffect and ParticleSystem
        /// </summary>
        private void Disable() {
            boxCollider.enabled = false;
            effect.gameObject.SetActive(false);
            particleLight.gameObject.SetActive(false);
        }
    }
}