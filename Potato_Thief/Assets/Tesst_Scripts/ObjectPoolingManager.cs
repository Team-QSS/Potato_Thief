using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager manager = null;
    public GameObject mapMaker;

    public GameObject leverPrefebs;
    public GameObject doorPrefebs;

    readonly private Queue<GameObject> leverPool = new Queue<GameObject>();
    readonly private Queue<GameObject> doorPool = new Queue<GameObject>();

    private int key;

    private void Start()
    {
        if(manager == null)
            manager = this;

        key = 0;

        for (int i = 0; i < 10; i++)
        {
            GameObject lever = Instantiate(leverPrefebs);
            GameObject door = Instantiate(doorPrefebs);

            leverPool.Enqueue(lever);
            doorPool.Enqueue(door);

            lever.SetActive(false);
            door.SetActive(false);
        }

        mapMaker.GetComponent<MapManager>().MakeMap();
    }

    public void DestroyLever(GameObject lever)
    {
        leverPool.Enqueue(lever);
        lever.SetActive(false);
    }
    public void DestroyDoor(GameObject door)
    {
        doorPool.Enqueue(door);
        door.SetActive(false);
    }

    #region InstantiateObjects
    public GameObject InstantiateLever()
    {
        GameObject lever = null;

        if (leverPool.Count > 0)
            lever = leverPool.Dequeue();

        if (lever == null)
        {
            lever = Instantiate(leverPrefebs);
        }
        lever.SetActive(true);

        lever.GetComponent<Obstacle>().SetKey(key);
        key++;
        return lever;
    }
    public GameObject InstantiateLever(Vector2 position)
    {
        GameObject lever = null;

        if (leverPool.Count > 0)
            lever = leverPool.Dequeue();

        if (lever == null)
        {
            lever = Instantiate(leverPrefebs);
        }
        lever.SetActive(true);

        lever.GetComponent<Obstacle>().SetKey(key);
        key++;
        lever.transform.localPosition = position;
        return lever;
    }
    public GameObject InstantiateDoor()
    {
        GameObject door = null;

        if (doorPool.Count > 0)
            door = doorPool.Dequeue();

        if (door == null)
        {
            door = Instantiate(doorPrefebs);
        }

        door.SetActive(true);
        door.GetComponent<Obstacle>().SetKey(key);
        key++;
        return door;
    }
    public GameObject InstantiateDoor(Vector2 position)
    {
        GameObject door = null;

        if (doorPool.Count > 0)
            door = doorPool.Dequeue();

        if (door == null)
        {
            door = Instantiate(doorPrefebs);
        }

        door.SetActive(true);
        door.GetComponent<Obstacle>().SetKey(key);
        key++;
        door.transform.localPosition = position;
        return door;
    }
    #endregion 
}
