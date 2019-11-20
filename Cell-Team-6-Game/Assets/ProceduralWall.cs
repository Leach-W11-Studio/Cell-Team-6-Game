using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[RequireComponent(typeof(SpriteRenderer))]
public class ProceduralWall : MonoBehaviour
{
    public List<GameObject> wallPrefabs = new List<GameObject>();
    public float density;
    public float objectDilation;
    public Vector2 randomDelta;

    SpriteRenderer spriteRenderer;

    private Vector2 lastScale;
    private Vector2 lastRandomDelta;
    private float lastDensity;
    private float lastDilation;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CompositeCollider2D collider = GetComponent<CompositeCollider2D>();
        collider.GenerateGeometry();
    }
    // Update is called once per frame
    void Update()
    {
    }

    public void CreateWall() {
        if (wallPrefabs.Count == 0) { return; }
        if (!spriteRenderer) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.sprite = null;
        
        float startX = transform.position.x - (spriteRenderer.size.x/2);
        float endX = transform.position.x + (spriteRenderer.size.x/2);
        float startY = transform.position.y - (spriteRenderer.size.y/2);
        float endY = transform.position.y + (spriteRenderer.size.y/2);
        float children = transform.childCount;
        // Vector2 positionDelta = new Vector2((density * spriteRenderer.size.x), density * spriteRenderer.size.y);
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
        if (!spriteRenderer) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        Gizmos.DrawWireCube(transform.position, spriteRenderer.size);
        // if (lastScale != spriteRenderer.size ||
        //     lastRandomDelta != randomDelta ||
        //     lastDilation != objectDilation ||
        //     lastDensity != density) {
        //         CreateWall();
        //     }
        // lastScale = spriteRenderer.size;
        // lastRandomDelta = randomDelta;
        // lastDensity = density;
        // lastDilation = objectDilation;
    }

    
}

[CustomEditor(typeof(ProceduralWall))]
public class ProceduralWallEditor : Editor {
    public override void OnInspectorGUI () {
        ProceduralWall scriptTarget = (ProceduralWall)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("Build Wall")) {
            scriptTarget.CreateWall();
            CompositeCollider2D collider = scriptTarget.gameObject.GetComponent<CompositeCollider2D>();
            collider.GenerateGeometry();
        }
    }
}