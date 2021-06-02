using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager mapMaker = null;
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

    private void SetLever()
    {
        GameObject lever;

        lever = ObjectPoolingManager.manager.InstantiateLever(new Vector2(0, 0));
        levers.Add(lever);

        lever = ObjectPoolingManager.manager.InstantiateLever(new Vector2(5, 0));
        levers.Add(lever);

        lever = ObjectPoolingManager.manager.InstantiateLever(new Vector2(-5, 0));
        levers.Add(lever);
    }

    private void MakeInteractionDoorToLever(GameObject door, params int[] interactions)
    {
        Door doorScript;
        doorScript = door.GetComponent<Door>();

        for (int i = 0; i < interactions.Length; i++)
        {
            doorScript.targetObjects.Add(levers[interactions[i]]);
        }
    }

    private void SetDoor()
    {
        GameObject door;

        door = ObjectPoolingManager.manager.InstantiateDoor(new Vector2(5, -2));
        MakeInteractionDoorToLever(door, 1, 0);
        doors.Add(door);

        door = ObjectPoolingManager.manager.InstantiateDoor(new Vector2(0, -2));
        MakeInteractionDoorToLever(door, 0, 2);
        doors.Add(door);

        door = ObjectPoolingManager.manager.InstantiateDoor(new Vector2(-5, -2));
        MakeInteractionDoorToLever(door, 1);
        doors.Add(door);
    }

    public void SendMessage(object[] contents) => ReceiveMessage(contents);

    private void ReceiveMessage(object[] contents)
    {
        int key = (int)contents[0];
        bool status = (bool)contents[1];

        levers[key].GetComponent<Obstacle>().SetStatus(status);

        for (int i = 0; i < doors.Count; i++)
            doors[i].GetComponent<Door>().TargetStatusChange();

    }
}
