using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileableWall : MonoBehaviour
{

    BoxCollider2D boxCollider2D = null;
    SpriteRenderer spriteRenderer = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if (!boxCollider2D) {
            boxCollider2D = GetComponent<BoxCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        boxCollider2D.size = new Vector2(spriteRenderer.size.x, spriteRenderer.size.y);
    }
       
}
