using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

    [CustomEditor(typeof(TerrainGenerator))]
public class GeneratorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainGenerator G = (TerrainGenerator)target;
        base.OnInspectorGUI();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate"))
        {
            //G.generateChunk(0,0);
        }
        if (GUILayout.Button("Clear"))
        {
            //G.ground.ClearAllTiles();
            //G.boundary.ClearAllTiles();
        }
        GUILayout.EndHorizontal();
    }
}
