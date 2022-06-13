using System;
using UnityEngine;

namespace Enemy {
    public class Enemy : MonoBehaviour {
        [SerializeField] private Health.Health health;

        private void Update() {
            if(health.CurrentHealth == 0)
                Destroy(gameObject);
        }
    }
}