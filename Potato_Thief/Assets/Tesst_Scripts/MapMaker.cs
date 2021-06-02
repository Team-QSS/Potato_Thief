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
        if (mapMaker == null)
            mapMaker = this;
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

    void MakeInteraction(GameObject door, params int[] interactions)
    {
        Door doorScript;
        doorScript = door.GetComponent<Door>();

        for (int i = 0; i < interactions.Length; i++)
        {
            doorScript.targetObjects.Add(levers[interactions[i]]);
        }
    }

    public void SetDoor()
    {
        GameObject door;

        door = Manager.manager.InstantiateDoor(new Vector2(5, -2));
        MakeInteraction(door, 1, 0);
        doors.Add(door);

        door = Manager.manager.InstantiateDoor(new Vector2(0, -2));
        MakeInteraction(door, 0, 2);
        doors.Add(door);

        door = Manager.manager.InstantiateDoor(new Vector2(-5, -2));
        MakeInteraction(door, 1);
        doors.Add(door);
    }

    public void SendMessage(object[] contents) => ReceiveMessage(contents);

    public void ReceiveMessage(object[] contents)
    {
        int key = (int)contents[0];
        bool status = (bool)contents[1];

        levers[key].GetComponent<Lever>().SetStatus(status);

        for (int i = 0; i < doors.Count; i++)
            doors[i].GetComponent<Door>().TargetStatusChange();

    }
}
