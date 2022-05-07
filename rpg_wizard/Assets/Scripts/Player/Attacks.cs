using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour {
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

    private void InstantiateWaterShield() {
        
    }

    private void InstantiateFireBall() {
        
    }

    private void OnDisable() {
        Controls.Disable();
    }
}
