using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;
#if (UNITY_EDITOR)
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(ProceduralWall))]
public class ProceduralWallEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        MonoBehaviour mono = (MonoBehaviour)target;
        ProceduralWall scriptTarget = mono.GetComponent<ProceduralWall>();
        PolyNavObstacle pObstacle = mono.GetComponent<PolyNavObstacle>();
        CompositeCollider2D collider = mono.GetComponent<CompositeCollider2D>();
        base.OnInspectorGUI();

        if (GUILayout.Button("Build Wall"))
        {
            scriptTarget.Generate();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
