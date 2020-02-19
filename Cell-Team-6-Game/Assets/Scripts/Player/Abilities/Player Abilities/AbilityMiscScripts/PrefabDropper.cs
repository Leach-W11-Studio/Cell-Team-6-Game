using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabDropper : MonoBehaviour
{
    public GameObject prefabToDrop;
    public GameObject noteIcon;
    public float iconOffset = 2;
    private GameObject noteInstance;
    private void OnDestroy()
    {
        Instantiate(prefabToDrop, transform.position, Quaternion.identity);
        Destroy(noteInstance);
    }

    private void Start()
    {
        noteInstance = Instantiate(noteIcon);
    }

    private void Update()
    {
        noteInstance.transform.position = new Vector2(transform.position.x, transform.position.y + iconOffset);
    }
}
