using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages object pooling
/// </summary>
public class SimplePool : MonoBehaviour
{
    /// <summary>
    /// We want to view the pool in editor
    /// </summary>
    [SerializeField] List<GameObject> pool = new List<GameObject>();
    
    [SerializeField] int poolSize = 50;
    [SerializeField] int maxPoolSize = 100;
    /// <summary>
    /// Prefab object to pool/instantiate
    /// </summary>
    [SerializeField] GameObject prefab;
  
    //Create an pool poolsize prefabs.
    void Start()
    {
        pool = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < poolSize; i++)
        {
            tmp = Instantiate(prefab, transform);
            tmp.SetActive(false);
            pool.Add(tmp);
        }
    }
    /// <summary>
    /// Gets or spawns a new object
    /// </summary>
    /// <returns></returns>
    public T GetObject<T>() where T : MonoBehaviour
    {
        for(int i = 0; i < poolSize; i++)
        {
            if(!pool[i].activeInHierarchy)
            {
                return pool[i].GetComponent<T>();
            }
        }
        //if we hit our max size return null
        if (poolSize >= maxPoolSize)
        {
            // Debug.LogWarning("Max pool size reached");
            return null;
        }
        //otherwise we can expand the pool more
        GameObject newObj = Instantiate(prefab);
        newObj.SetActive(false);
        pool.Add(newObj);
        poolSize++;
        return newObj.GetComponent<T>();
    }

    /// <summary>
    /// Releases the object back to the pool. 
    /// </summary>
    /// <param name="obj">object to release</param>
    public void ReleaseObject(GameObject obj)
    {
        if (pool.Contains(obj))
        {
            obj.transform.SetParent(transform);
            obj.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Tried to release an object not managed by this pool.");
        }
    }
}
