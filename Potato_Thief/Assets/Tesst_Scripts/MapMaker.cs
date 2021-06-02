using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    public static MapMaker mapMaker = null;
    List<GameObject> levers = new List<GameObject>();
    List<GameObject> doors = new List<GameObject>();

    private void Start()
    {
        if(mapMaker == null)
        {
            mapMaker = this;
        }

        levers = new List<GameObject>();
        doors = new List<GameObject>();
    }

    public void MakeMap()
    {
        SetLever();
        SetDoor();
    }

    public void SetLever()
    {
        GameObject lever;

        lever = Manager.manager.InstantiateLever();
        lever.transform.localPosition = new Vector2(0, 0);
        levers.Add(lever);

        lever = Manager.manager.InstantiateLever();
        lever.transform.localPosition = new Vector2(5, 0);
        levers.Add(lever);

        lever = Manager.manager.InstantiateLever();
        lever.transform.localPosition = new Vector2(-5, 0);
        levers.Add(lever);
    }

    public void SetDoor()
    {
        GameObject door;
        Door doorScript;

        door = Manager.manager.InstantiateDoor();
        door.transform.localPosition = new Vector2(5, -2);
        doorScript = door.GetComponent<Door>();
        doorScript.targetObjects.Add(levers[0]);
        doorScript.targetObjects.Add(levers[1]);
        doors.Add(door);

        door = Manager.manager.InstantiateDoor();
        door.transform.localPosition = new Vector2(0, -2);
        doorScript = door.GetComponent<Door>();
        doorScript.targetObjects.Add(levers[0]);
        doorScript.targetObjects.Add(levers[2]);
        doors.Add(door);

        door = Manager.manager.InstantiateDoor();
        door.transform.localPosition = new Vector2(-5, -2);
        doorScript = door.GetComponent<Door>();
        doorScript.targetObjects.Add(levers[1]);
        doors.Add(door);
    }

    public void SendMessage(object[] contents)
    {
        ReceiveMessage(contents);
    }

    public void ReceiveMessage(object[] contents)
    {
        Door doorScript;

        int key = (int)contents[0];
        bool status = (bool)contents[1];

        print("Receive Data : " + key + "," + status);

        levers[key].GetComponent<Lever>().SetStatus(status);

        for (int i = 0; i < doors.Count; i++)
        {
            doorScript = doors[i].GetComponent<Door>();
            doorScript.TargetStatusChange();
        }
    }
}
