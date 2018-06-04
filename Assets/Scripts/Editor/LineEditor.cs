using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LineChanger))]
public class LineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LineChanger lineChanger = (LineChanger)target;

        if (GUILayout.Button("Изменить линию", EditorStyles.miniButtonMid))
        {
            lineChanger.ChangePhase();
        }
    }

}
