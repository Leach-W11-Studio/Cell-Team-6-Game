﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

using UnityEditor;

[RequireComponent(typeof(CompositeCollider2D))]
[RequireComponent(typeof(PolyNav.PolyNavObstacle))]
public class ProceduralWall : MonoBehaviour
{
    public Vector2 areaSize;
    public List<GameObject> wallPrefabs = new List<GameObject>();
    public float density;
    public float objectDilation;
    public Vector2 randomDelta;

    private Vector2 lastScale;
    private Vector2 lastRandomDelta;
    private float lastDensity;
    private float lastDilation;

    void Start() {
    }
    // Update is called once per frame
    void Update()
    {
    }

    public void CreateWall() {
        if (wallPrefabs.Count == 0) { return; }
        
        float startX = transform.position.x - (areaSize.x/2);
        float endX = transform.position.x + (areaSize.x/2);
        float startY = transform.position.y - (areaSize.y/2);
        float endY = transform.position.y + (areaSize.y/2);
        float children = transform.childCount;
        // Vector2 positionDelta = new Vector2((density * areaSize.x), density * areaSize.y);
        for(int i = 0; i < children; i++) {
            if (Application.isEditor) {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            else {
                Destroy(transform.GetChild(0).gameObject);
            }
        }
        for(float x = startX; x < endX; x += 1f/density) {
            for (float y = startY; y < endY; y += 1f/density) {
                Quaternion randomQuat = Quaternion.Euler(0, 0, Random.Range(-180, 180));
                Vector2 moddedPosition = (new Vector2(x,y)) + new Vector2(Random.Range(-randomDelta.x, randomDelta.x), Random.Range(-randomDelta.y, randomDelta.y));
                GameObject wallPiece = Instantiate<GameObject>(wallPrefabs[Random.Range(0, wallPrefabs.Count)], moddedPosition, randomQuat, transform);
                wallPiece.transform.localScale = wallPiece.transform.localScale * new Vector2(Random.Range(1, randomDelta.x), Random.Range(1, randomDelta.y)) * objectDilation;
            }
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, areaSize);
        // if (lastScale != areaSize ||
        //     lastRandomDelta != randomDelta ||
        //     lastDilation != objectDilation ||
        //     lastDensity != density) {
        //         CreateWall();
        //     }
        // lastScale = areaSize;
        // lastRandomDelta = randomDelta;
        // lastDensity = density;
        // lastDilation = objectDilation;
    }

    
}

[CanEditMultipleObjects]
[CustomEditor(typeof(ProceduralWall))]
public class ProceduralWallEditor : Editor {
    public override void OnInspectorGUI () {
        serializedObject.Update();
        MonoBehaviour mono = (MonoBehaviour)target;
        ProceduralWall scriptTarget = mono.GetComponent<ProceduralWall>();
        PolyNavObstacle pObstacle = mono.GetComponent<PolyNavObstacle>();
        CompositeCollider2D collider = mono.GetComponent<CompositeCollider2D>();
        base.OnInspectorGUI();

        if (GUILayout.Button("Build Wall")) {
            scriptTarget.CreateWall();
        }

        serializedObject.ApplyModifiedProperties();
    }
}