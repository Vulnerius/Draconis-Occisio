using UnityEngine;

namespace Enemy {
    public class DestroyMultipleTornado : MonoBehaviour {
        [SerializeField] private float lifeTime;

        private void Awake() {
            Destroy(gameObject, lifeTime-.2f);
        }
    }
}