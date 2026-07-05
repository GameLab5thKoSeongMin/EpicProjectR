using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RectTransformToggleMover))]
public class RectTransformToggleMoverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();

        if (GUILayout.Button("Capture Current As Opened Position"))
        {
            RectTransformToggleMover mover = (RectTransformToggleMover)target;
            Undo.RecordObject(mover, "Capture Opened Position");
            mover.CaptureCurrentAsOpenedPosition();
            EditorUtility.SetDirty(mover);
        }
    }
}
