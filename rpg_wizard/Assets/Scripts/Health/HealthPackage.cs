using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace Health {
    public class HealthPackage : MonoBehaviour {
        [SerializeField] private int healAmount;
        [SerializeField] private float coolDown;
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private VisualEffect effect;
        [SerializeField] private ParticleSystem particleLight;
        

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.GetComponentInParent<Health>() == null) return;
            
            other.gameObject.GetComponentInParent<Health>().IncreaseHealth(healAmount);
            Debug.Log(other.gameObject.GetComponentInParent<Health>().CurrentHealth);

            StartCoroutine(CoolDownSelf());
        }

        private IEnumerator CoolDownSelf() {
            Disable();
            yield return new WaitForSeconds(coolDown);
            Enable();
        }

        private void Enable() {
            boxCollider.enabled = true;
            effect.gameObject.SetActive(true);
            particleLight.gameObject.SetActive(true);
        }

        private void Disable() {
            boxCollider.enabled = false;
            effect.gameObject.SetActive(false);
            particleLight.gameObject.SetActive(false);
        }
    }
}