using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour {
    [SerializeField] private GameObject fireBall;
    [SerializeField] private GameObject watershield; 
    private Animator Animator;
    private PlayerControls Controls;

    private void Awake() {
        Controls = new PlayerControls();
        Animator = gameObject.GetComponentInChildren<Animator>();
        InitAttackActions(Controls);
        Controls.Enable();
    }

    private void InitAttackActions(PlayerControls controls) {
        controls.Attacks.FireBall.performed += _ => StartCoroutine(InstantiateFireBall());

        controls.Attacks.WaterShield.performed += _ => StartCoroutine(InstantiateWaterShield());
    }

    private IEnumerator InstantiateWaterShield() {
        Animator.Play("DefendHit");
        yield return new WaitForSeconds(.4f);
        var fireB = Instantiate(watershield, transform.position + new Vector3(0,1,0), Quaternion.identity);
        Destroy(fireB, 3f);
        yield return new WaitForSeconds(.2f);
        Animator.Play("Idle03");
    }

    private IEnumerator InstantiateFireBall() {
        //TODO:
        Animator.Play("Attack01");
        yield return new WaitForSeconds(.4f);
        var fireB = Instantiate(fireBall, transform.position + new Vector3(0,0,1.2f), Quaternion.LookRotation(transform.forward));
        fireB.GetComponent<Rigidbody>().AddForce(15 * transform.forward, ForceMode.Impulse);
        Destroy(fireB, 3f);
        yield return new WaitForSeconds(.4f);
        Animator.Play("Idle03");
    }

    private void OnDisable() {
        Controls.Disable();
    }
}