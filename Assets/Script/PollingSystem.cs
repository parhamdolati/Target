using System.Collections.Generic;
using UnityEngine;

public class PollingSystem : InitManager
{
    public static PollingSystem Instance;
    [System.Serializable]
    public class Pool
    {
        public string tag; // pool name
        public GameObject prefab; // pool object prefab for instantiate
        public int size; // size of pool
    }

    //list of pools type
    [SerializeField] private List<Pool> pools;
    
    //list of all pool type object
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    
    private void Start()
    {
        Instance = this;
        poolDictionary=new Dictionary<string, Queue<GameObject>>();

        //instantiate objects and save that in queue for use with poolDictionary
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            //instantiate prefab for all pools
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.parent = transform;
                objectPool.Enqueue(obj);
            }
            
            poolDictionary.Add(pool.tag,objectPool);
        }

        isInited = true;
    }

    public GameObject GetFromPool(string tag)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            //if pool queue be empty create new one
            if (poolDictionary[tag].Count <= 0)
            {
                foreach (Pool pool in pools)
                {
                    GameObject objTemp = null;
                    if (pool.tag == tag)
                    {
                        objTemp = Instantiate(pool.prefab);
                        objTemp.transform.parent = transform;
                        poolDictionary[tag].Enqueue(objTemp);
                    }
                }
            }
            
            
            GameObject obj = poolDictionary[tag].Dequeue(); //get first obj in queue
            obj.SetActive(true);
            return obj;
        }

        return null;
    }

    //put game object in specific queue with tag
    public void BackToPool(string tag, GameObject obj)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            obj.SetActive(false);
            //obj.transform.parent = transform;
            poolDictionary[tag].Enqueue(obj);
        }
    }
}
