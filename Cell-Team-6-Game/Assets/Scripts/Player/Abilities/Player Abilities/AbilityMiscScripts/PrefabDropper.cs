using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabDropper : MonoBehaviour
{
    GameObject prefabToDrop;
    private void OnDestroy()
    {
        Instantiate(prefabToDrop, transform.position, Quaternion.identity);
    }
}
