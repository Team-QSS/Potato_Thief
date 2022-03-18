using InGame;
using UnityEngine;

namespace Editor
{
    using UnityEditor;
    [CustomEditor(typeof(MovingPlane))]
    public class MovingPlaneEditor : Editor
    {
        private void OnSceneGUI()
        {
            var movingPlane = target as MovingPlane;
            if (movingPlane == null) return;
            
            using (var changeCheck = new EditorGUI.ChangeCheckScope())
            {
                var destPosition = movingPlane.transform.InverseTransformPoint(Handles.PositionHandle
                    (movingPlane.transform.TransformPoint(movingPlane.destPosition), movingPlane.transform.rotation));
                if (changeCheck.changed)
                {
                    movingPlane.destPosition = destPosition;
                }
            }
            Handles.color = Color.green;
            Handles.Label(movingPlane.transform.TransformPoint(movingPlane.destPosition), $"Destination : {(Vector2)movingPlane.destPosition}");
        }

        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        private static void OnDrawGizmo(MovingPlane movingPlane, GizmoType gizmoType)
        {
            var defaultPos = movingPlane.defaultPosition;
            var destPos = movingPlane.transform.TransformPoint(movingPlane.destPosition);
            
            Handles.color = Color.cyan;
            Handles.DrawDottedLine(defaultPos, destPos, 2);
            Handles.DrawSolidDisc(defaultPos, movingPlane.transform.forward, 0.1f);
            Handles.DrawSolidDisc(destPos, movingPlane.transform.forward, 0.1f);

            var scale = movingPlane.transform.GetChild(0).GetComponent<Transform>().localScale;
            var pos = movingPlane.transform.TransformPoint(movingPlane.destPosition);
            Handles.DrawWireCube(pos, scale);
        }
    }
}