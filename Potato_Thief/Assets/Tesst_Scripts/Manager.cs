using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager manager = null;
    public GameObject mapMaker;

    public GameObject leverPrefebs;
    public GameObject doorPrefebs;

    Queue<GameObject> leverPool = new Queue<GameObject>();
    Queue<GameObject> doorPool = new Queue<GameObject>();

    int key;

    private void Start()
    {
        if(manager == null)
        {
            manager = this;
        }



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

        mapMaker.GetComponent<MapMaker>().MakeMap();
    }

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

        lever.GetComponent<Lever>().key = key;
        key++;
        return lever;
    }

    public void DestroyLever(GameObject lever)
    {
        leverPool.Enqueue(lever);
        lever.SetActive(false);
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
        door.GetComponent<Door>().key = key;
        key++;
        return door;
    }

    public void DestroyDoor(GameObject door)
    {
        doorPool.Enqueue(door);
        door.SetActive(false);
    }
}
