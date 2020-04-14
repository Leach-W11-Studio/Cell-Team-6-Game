using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using PolyNav;

//using UnityEditor;

[RequireComponent(typeof(CompositeCollider2D))]
[RequireComponent(typeof(PolyNav.PolyNavObstacle))]
[RequireComponent(typeof(EdgeCollider2D))]
public class ProceduralWall : MonoBehaviour
{
    public bool curvy = true;
    public float radius;
    public Vector2 areaSize;
    public List<GameObject> wallPrefabs = new List<GameObject>();
    public float density;
    public float objectDilation;
    public Vector2 randomDelta;

    private Vector2 lastScale;
    private Vector2 lastRandomDelta;
    private float lastDensity;
    private float lastDilation;
    private List<Vector2> points {
        get {
            return new List<Vector2>(GetComponent<EdgeCollider2D>().points);
        }
    }

    
    void Start() {
        GetComponent<EdgeCollider2D>().enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
    }

    public void Generate() {
        Reset();
        if (curvy) {
            GenerateFromSpline();
        }
        else {
            GenerateFromArea();
        }
    }

    private void GenerateFromSpline() {
        Debug.Log("Generating procedural wall from spline");
        // Iterate over points in spline starting from second point
        int count = 0;
        foreach (Vector2 splinePoint in points) {
            if (count == 0) {count++; continue;}
            Vector2 direction = (splinePoint - points[count - 1]).normalized;
            Vector2 perp = Vector2.Perpendicular(direction);
            // Loop "density" times
            for (int i = 1; i < density; i++) {
                // Interpolate to point in line from previous point to this point
                float percent = i/density;
                Debug.Log(percent);
                Vector2 step = Vector2.Lerp(points[count - 1], splinePoint, percent);
                Debug.DrawLine(transform.TransformPoint(splinePoint), transform.TransformPoint(step), Color.red);
                // Loop "density" times
                for (int r = 1; r < density; r++) {
                    // generate a random number within -radius and radius
                    float offset = Random.Range(-radius, radius);
                    Debug.Log("Offset is: " + offset);
                    // Get vector of point on perpendicular of line and offet by the random number
                    Vector2 position = transform.TransformPoint(Vector2.Lerp(step, step + perp, offset / radius));
                    // Intsntiate a random prefab from the lis at this point
                    SpawnAtPoint(position.x, position.y);
                }
            }

            count++;
        }
    }

    private void GenerateFromArea() {
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
                SpawnAtPoint(x, y);
            }
        }
    }

    public void Reset() {
        float children = transform.childCount;
        for(int i = 0; i < children; i++) {
            if (Application.isEditor) {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            else {
                Destroy(transform.GetChild(0).gameObject);
            }
        }
    }

    [System.Obsolete("CreateWall is obsolete. Use Generate() instead")]
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

    private GameObject SpawnAtPoint(float x, float y) {
        Quaternion randomQuat = Quaternion.Euler(0, 0, Random.Range(-180, 180));
        Vector2 moddedPosition = (new Vector2(x,y)) + new Vector2(Random.Range(-randomDelta.x, randomDelta.x), Random.Range(-randomDelta.y, randomDelta.y));
        GameObject wallPiece = Instantiate<GameObject>(wallPrefabs[Random.Range(0, wallPrefabs.Count)], moddedPosition, randomQuat, transform);
        wallPiece.transform.localScale = wallPiece.transform.localScale * new Vector2(Random.Range(1, randomDelta.x), Random.Range(1, randomDelta.y)) * objectDilation;
        return wallPiece;
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