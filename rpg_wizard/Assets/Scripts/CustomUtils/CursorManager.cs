using UnityEngine;

namespace DefaultNamespace {
    public static class CursorManager {
        public static CursorEvent CurrentCursor;
        
        public enum CursorEvent {
            Invisible,
            Visible
        }

        public static void SetCursor(CursorEvent cursorVisibility) {
            CurrentCursor = cursorVisibility;
            switch (cursorVisibility) {
                case CursorEvent.Visible:
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                case CursorEvent.Invisible:
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;
                default:
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
            }
        }
    }
}