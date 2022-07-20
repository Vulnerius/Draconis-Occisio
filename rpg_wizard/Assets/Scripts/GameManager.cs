using System;
using System.Collections;
using DefaultNamespace;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {
    [SerializeField] public CineMachineSwitcher switcher;

    public int currentEnemyIdx;

    void Update() {
        if (Keyboard.current.fKey.wasPressedThisFrame)
            switcher.SwitchLock();
    }

    private void FixedUpdate() {
        if (!ReferenceTable.CurrentEnemy) StartCoroutine(SpawnNewDragon());
        if (!ReferenceTable.CurrentEnemy.gameObject.activeSelf)
            StartCoroutine(SpawnNewDragon());
    }

    private IEnumerator SpawnNewDragon() {
        ReferenceTable.DragonSpawner.SpawnDragon(++currentEnemyIdx);
        yield return new WaitForFixedUpdate();
    }
}