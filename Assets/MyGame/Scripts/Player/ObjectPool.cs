using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    public static ObjectPool instance;
    private int amountBulletToPool = 20;
   [SerializeField] private GameObject bulletPrefab;
    private List<GameObject> pooledObjects = new List<GameObject>();
    private void Awake()
    {
        instance = this; 
    }
    private void Start()
    {
      for (int i =0; i < amountBulletToPool; i ++) {
        GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);    
            obj.transform.parent = transform;
        }  
    }

   public GameObject GetPooledObject()
    {
        for (int i =0; i < pooledObjects.Count; i ++) { 
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
