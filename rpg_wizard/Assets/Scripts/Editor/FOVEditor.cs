#if UNITY_EDITOR
using Enemy;
using UnityEditor;
using UnityEngine;

namespace Editor {
    [CustomEditor(typeof(Enemy.Enemy))]
    public class FOVEditor : UnityEditor.Editor {
        
        /// <summary>
        /// creates a circle around the dragon with a triangle-cone as "field of view"
        /// </summary>
        private void OnSceneGUI() {
            Enemy.Enemy enemy = target as Enemy.Enemy;
            FieldOfView fov = enemy.fov;
            Transform selfTransform = enemy.transform;

            Handles.color = Color.white;
            Handles.DrawWireArc(selfTransform.position, Vector3.up, Vector3.forward, 360, fov.SeeRadius);

            Handles.color = Color.gray;
            Handles.DrawWireArc(selfTransform.position, Vector3.up, Vector3.forward, 360, fov.BumpRadius);

            Vector3 viewAngle01 = DirectionFromAngle(selfTransform.eulerAngles.y, -fov.Angle / 2);
            Vector3 viewAngle02 = DirectionFromAngle(selfTransform.eulerAngles.y, fov.Angle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(selfTransform.position, selfTransform.position + viewAngle01 * fov.SeeRadius);
            Handles.DrawLine(selfTransform.position, selfTransform.position + viewAngle02 * fov.SeeRadius);
        }

        private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees) {
            angleInDegrees += eulerY;

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}

#endif