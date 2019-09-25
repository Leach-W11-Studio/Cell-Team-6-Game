using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectQueue : MonoBehaviour
{
    #region singleton

    public static ObjectQueue Instance;
    private void Awake()
    {
        if(Instance != null) { Destroy(gameObject); }
        else { Instance = this; }

        DontDestroyOnLoad(Instance);
    }

    #endregion

    [System.Serializable]
    public struct Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> objectPools;

    // Start is called before the first frame update
    void Start()
    {
        objectPools = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            objectPools.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!objectPools.ContainsKey(tag))
        {
            Debug.LogWarning("Specified tag " + tag + " doesn't exist");
            return null;
        }

        GameObject toSpawn = objectPools[tag].Dequeue();

        toSpawn.transform.position = position;
        toSpawn.transform.rotation = rotation;
        toSpawn.SetActive(true);

        IShootable shootable = toSpawn.GetComponent<IShootable>();
        shootable.Shoot();

        objectPools[tag].Enqueue(toSpawn);
        return toSpawn;
    }
}
