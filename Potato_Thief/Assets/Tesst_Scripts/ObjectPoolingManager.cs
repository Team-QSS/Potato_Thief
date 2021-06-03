using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleObject
{
    lever = 0,
    door = 1,
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

        for (int obstacleNum = 0; obstacleNum < (int)ObstacleObject.Max; obstacleNum++)
        {
            for (int i = 0; i < 10; i++)
            {
                obj = Instantiate(prefebList[obstacleNum]);
                DestroyObject((ObstacleObject)obstacleNum, obj);
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

    public void DestroyObject(ObstacleObject obstacleIndex, GameObject obj)
    {
        poolList[(int)obstacleIndex].Enqueue(obj);
        obj.SetActive(false);
    }

    #region InstantiateObject
    private GameObject GetObject(ObstacleObject obstacleIndex)
    {
        GameObject obj = null;
        Queue<GameObject> pool = poolList[(int)obstacleIndex];

        if (pool.Count > 0)
            obj = pool.Dequeue();

        if (obj == null)
        {
            obj = Instantiate(prefebList[(int)obstacleIndex]);
        }
        obj.SetActive(true);

        obj.GetComponent<Obstacle>().SetKey(key);
        key++;
        return obj;
    }

    public GameObject InstantiateObject(ObstacleObject obstacleObject)
    {
        return GetObject(obstacleObject);
    }

    public GameObject InstantiateObject(ObstacleObject obstacleObject, Vector2 position)
    {
        GameObject obj = GetObject(obstacleObject);
        obj.transform.localPosition = position;
        return obj;
    }

    public GameObject InstantiateObject(ObstacleObject obstacleObject, Quaternion rotation)
    {
        GameObject obj = GetObject(obstacleObject);
        obj.transform.rotation = rotation;
        return obj;
    }

    public GameObject InstantiateObject(ObstacleObject obstacleObject, Vector2 position, Quaternion rotation)
    {
        GameObject obj = GetObject(obstacleObject);
        obj.transform.localPosition = position;
        obj.transform.rotation = rotation;

        return obj;
    }
    #endregion 
}
