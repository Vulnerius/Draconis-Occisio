using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class Attacks : MonoBehaviour {
    [SerializeField] private GameObject fireBall;
    [SerializeField] private GameObject watershield; 
    private Animator Animator;
    private PlayerControls Controls;
    private PlayerStates m_States;

    private void Awake() {
        Controls = new PlayerControls();
        Animator = gameObject.GetComponentInChildren<Animator>();
        InitAttackActions(Controls);
        Controls.Enable();
        StartCoroutine(GetStates());
        
    }

    private IEnumerator GetStates() {
        if (gameObject.GetComponent<MovementController>().States == null)
            yield return new WaitForSeconds(1f);
        m_States = gameObject.GetComponent<MovementController>().States;
    }

    private void InitAttackActions(PlayerControls controls) {
        controls.Attacks.FireBall.performed += _ => {
            m_States.ability = true;
            StartCoroutine(InstantiateFireBall());
        };
        //controls.Attacks.FireBall.canceled += _ => m_States.ability = false;
        
        controls.Attacks.WaterShield.performed += _ => {
            m_States.ability = true;
            StartCoroutine(InstantiateWaterShield());
        };
        //controls.Attacks.WaterShield.canceled += _ => m_States.ability = false;
    }

    private IEnumerator InstantiateWaterShield() {
        Animator.Play("DefendHit");
        yield return new WaitForSeconds(.4f);
        var fireB = Instantiate(watershield, transform.position + new Vector3(0,1,0), Quaternion.identity);
        Destroy(fireB, 3f);
        yield return new WaitForSeconds(.2f);
        m_States.ability = false;
    }

    private IEnumerator InstantiateFireBall() {
        //TODO:
        Animator.Play("Attack01");
        yield return new WaitForSeconds(.4f);
        var fireB = Instantiate(fireBall, transform.position + new Vector3(0,0,2f), Quaternion.LookRotation(transform.forward));
        fireB.GetComponent<Rigidbody>().AddForce(15 * transform.forward, ForceMode.Impulse);
        Destroy(fireB, 3f);
        yield return new WaitForSeconds(.4f);
        m_States.ability = false;
    }

    private void OnDisable() {
        Controls.Disable();
    }
}