using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralWall : MonoBehaviour
{
    public List<GameObject> wallPrefabs = new List<GameObject>();
    public float density;
    public Vector2 randomDelta;

    private List<GameObject> wallPieces = new List<GameObject>();
    private Vector3 lastScale;
    private Vector2 lastRandomDelta;

    void Start() {

        lastScale = transform.lossyScale;
        lastRandomDelta = randomDelta;
        float startX = transform.position.x - (transform.lossyScale.x/2);
        float endX = transform.position.x + (transform.lossyScale.x/2);
        float startY = transform.position.y - (transform.lossyScale.y/2);
        float endY = transform.position.y + (transform.lossyScale.y/2);
        Vector2 positionDelta = new Vector2(transform.lossyScale.x/density, transform.lossyScale.y/density);
        foreach (GameObject wallPiece in wallPrefabs) {
            Destroy(wallPiece);
        }
        wallPieces.Clear();
        for(float x = startX; x < endX; x += positionDelta.x) {
            for (float y = startY; y < endY; y += positionDelta.y) {
                Quaternion randomQuat = Quaternion.Euler(0, 0, Random.Range(-180, 180));
                Vector2 moddedPosition = (new Vector2(x,y)) * new Vector2(Random.Range(-randomDelta.x, randomDelta.x), Random.Range(-randomDelta.y, randomDelta.y));
                GameObject wallPiece = Instantiate<GameObject>(wallPrefabs[Random.Range(0, wallPrefabs.Count -1)], moddedPosition, randomQuat);
                wallPieces.Add(wallPiece);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
