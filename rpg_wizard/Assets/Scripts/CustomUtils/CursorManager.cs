using UnityEngine;

namespace CustomUtils {
    /// <summary>
    /// static class for visible and moving or invisible and locked Cursor
    /// </summary>
    public static class CursorManager {
        public enum CursorEvent {
            Invisible,
            Visible
        }

        /// <summary>
        /// sets the Cursor either visible and movable or invisible and locked
        /// defaulting to visible and movable Cursor
        /// </summary>
        /// <param name="cursorVisibility"></param>
        public static void SetCursor(CursorEvent cursorVisibility) {
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