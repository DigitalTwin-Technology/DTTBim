// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using UnityEditor;
using UnityEngine;
using DTTBim.DataStructs;
using DTTUnityCore.DataStructs;

[CustomEditor(typeof(BimNodeObject))]
public class BimNodeObjectEditor : Editor
{
    BimNodeObject _target;

    private void OnEnable()
    {
        _target = (BimNodeObject)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.ObjectField("Header", _target.Header, typeof(DTTGltFastHeader), true);
        EditorGUILayout.ObjectField("Parent", _target.Parent, typeof(BimNodeParent), true);

        GUILayout.Box($"Node Id {_target.Id}");
        GUILayout.Box($"Node Index {((MetaDataBimNode)_target.Data).NodeIndex}");
    }
}
