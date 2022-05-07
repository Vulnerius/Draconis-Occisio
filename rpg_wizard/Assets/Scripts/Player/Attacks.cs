using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour {
    [SerializeField] private GameObject fireBall;
    private PlayerControls Controls;

    private void Awake() {
        Controls = new PlayerControls();
        InitAttackActions(Controls);
        Controls.Enable();
    }

    private void InitAttackActions(PlayerControls controls) {
        controls.Attacks.FireBall.performed += _ => InstantiateFireBall();

        controls.Attacks.WaterShield.performed += _ => InstantiateWaterShield();
    }

    private void InstantiateWaterShield() { }

    private void InstantiateFireBall() {
        //TODO:
        var fireB = Instantiate(fireBall, transform.position + new Vector3(0,0,1.2f), Quaternion.LookRotation(transform.forward));
        fireB.GetComponent<Rigidbody>().AddForce(5 * transform.forward, ForceMode.Impulse);
        Destroy(fireB, 3f);
    }

    private void OnDisable() {
        Controls.Disable();
    }
}