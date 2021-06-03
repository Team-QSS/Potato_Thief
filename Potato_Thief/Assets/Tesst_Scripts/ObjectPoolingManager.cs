using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolEnum
{
    leverPool = 0,
    doorPool = 1,
    Max
}

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager manager = null;
    public GameObject mapMaker;

    public GameObject leverPrefebs;
    public GameObject doorPrefebs;

    private Queue<GameObject> leverPool = new Queue<GameObject>();
    private Queue<GameObject> doorPool = new Queue<GameObject>();

    private List<GameObject> prefebList = new List<GameObject>();
    private List<Queue<GameObject>> poolList = new List<Queue<GameObject>>();

    private int key;

    private void Start()
    {
        if (manager == null)
            manager = this;

        key = 0;

        InitializePoolList();
        InitializePrefebList();

        GameObject obj;

        for (int objkind = 0; objkind < (int)PoolEnum.Max; objkind++)
        {
            for (int i = 0; i < 10; i++)
            {
                obj = Instantiate(prefebList[objkind]);
                DestroyObject((PoolEnum)objkind, obj);
            }
        }

        mapMaker.GetComponent<MapManager>().MakeMap();
    }

    public void InitializePoolList()
    {
        poolList.Add(leverPool);
        poolList.Add(doorPool);
    }
    public void InitializePrefebList()
    {
        prefebList.Add(leverPrefebs);
        prefebList.Add(doorPrefebs);
    }

    public void DestroyObject(PoolEnum poolEnum, GameObject obj)
    {
        poolList[(int)poolEnum].Enqueue(obj);
        obj.SetActive(false);
    }

    #region InstantiateObject

    private GameObject GetObject(PoolEnum poolEnum)
    {
        GameObject obj = null;
        Queue<GameObject> pool = poolList[(int)poolEnum];

        if (pool.Count > 0)
            obj = pool.Dequeue();

        if (obj == null)
        {
            obj = Instantiate(prefebList[(int)poolEnum]);
        }
        obj.SetActive(true);

        obj.GetComponent<Obstacle>().SetKey(key);
        key++;
        return obj;
    }

    public GameObject InstantiateObject(PoolEnum poolEnum)
    {
        return GetObject(poolEnum);
    }

    public GameObject InstantiateObject(PoolEnum poolEnum, Vector2 position)
    {
        GameObject obj = GetObject(poolEnum);
        obj.transform.localPosition = position;
        return obj;
    }

    public GameObject InstantiateObject(PoolEnum poolEnum, Quaternion rotation)
    {
        GameObject obj = GetObject(poolEnum);
        obj.transform.rotation = rotation;
        return obj;
    }

    public GameObject InstantiateObject(PoolEnum poolEnum, Vector2 position, Quaternion rotation)
    {
        GameObject obj = GetObject(poolEnum);
        obj.transform.localPosition = position;
        obj.transform.rotation = rotation;

        return obj;
    }
    #endregion 
}
