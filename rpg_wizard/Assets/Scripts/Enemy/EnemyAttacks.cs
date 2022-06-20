using System.Collections;
using Enemy;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    [SerializeField] private GameObject fireBall;
    [SerializeField] private int shootFrequency;
    private GameObject player;

    private float timer;
    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindWithTag("Player");
    }

    private void Awake() {
        
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer > shootFrequency) {
            StartCoroutine(ShootFireBall());
            timer = 0;
        }
    }

    private IEnumerator ShootFireBall() {
        gameObject.transform.LookAt(player.transform.position);
        Vector3 instantiatePoint = transform.position + 7*transform.forward + 2 * Vector3.up;
        Instantiate(fireBall, instantiatePoint, Quaternion.identity);
        yield return new WaitForSeconds(.2f);
    }
}
