using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {
    [SerializeField] private CineMachineSwitcher switcher;
    
    // Update is called once per frame
    void Update() {
        if (Keyboard.current.fKey.wasPressedThisFrame)
            switcher.SwitchLock();
    }
}