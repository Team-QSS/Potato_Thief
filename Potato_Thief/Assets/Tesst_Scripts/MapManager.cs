using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public static MapManager mapMaker = null;
    List<GameObject> levers = new List<GameObject>();
    List<GameObject> doors = new List<GameObject>();
    List<List<GameObject>> objectsList = new List<List<GameObject>>();

    ObjectPoolingManager poolingManager;

    private void Start()
    {
        if (mapMaker == null)
            mapMaker = this;
    }

    public void MakeMap()
    {
        InitializeBeforeMakeMap();
        SetLever();
        SetDoor();
    }

    private void InitializeBeforeMakeMap()
    {
        poolingManager = ObjectPoolingManager.manager;
        InitializeObjectsList();
    }

    private void InitializeObjectsList()
    {
        objectsList.Add(levers);
        objectsList.Add(doors);
    }

    private void SetLever()
    {
        MakeObject(ObstacleObject.lever, new Vector2(5, 0));

        MakeObject(ObstacleObject.lever, new Vector2(0, 0));

        MakeObject(ObstacleObject.lever, new Vector2(-5, 0));
    }

    private void SetDoor()
    {
        GameObject door;

        door = MakeObject(ObstacleObject.door, new Vector2(5, -2));
        MakeInteractionDoorToLever(door, 1, 0);

        door = MakeObject(ObstacleObject.door, new Vector2(0, -2));
        MakeInteractionDoorToLever(door, 0, 2);

        door = MakeObject(ObstacleObject.door, new Vector2(-5, -2));
        MakeInteractionDoorToLever(door, 1);
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

    public void SendMessage(object[] contents) => ReceiveMessage(contents);

    private void ReceiveMessage(object[] contents)
    {
        int key = (int)contents[0];
        bool status = (bool)contents[1];

        levers[key].GetComponent<Obstacle>().SetStatus(status);

        for (int i = 0; i < doors.Count; i++)
            doors[i].GetComponent<Door>().TargetStatusChange();

    }

    #region MakeObject

    private GameObject MakeObject(ObstacleObject obstacleObject)
    {
        GameObject obj = poolingManager.InstantiateObject(obstacleObject);
        objectsList[(int)obstacleObject].Add(obj);
        return obj;
    }
    private GameObject MakeObject(ObstacleObject obstacleObject, Vector2 position)
    {
        GameObject obj = poolingManager.InstantiateObject(obstacleObject, position);
        objectsList[(int)obstacleObject].Add(obj);
        return obj;
    }
    private GameObject MakeObject(ObstacleObject obstacleObject, Quaternion rotation)
    {
        GameObject obj = poolingManager.InstantiateObject(obstacleObject, rotation);
        objectsList[(int)obstacleObject].Add(obj);
        return obj;
    }

    private GameObject MakeObject(ObstacleObject obstacleObject, Vector2 position, Quaternion rotation)
    {
        GameObject obj = poolingManager.InstantiateObject(obstacleObject, position, rotation);
        objectsList[(int)obstacleObject].Add(obj);
        return obj;
    }

    #endregion
}
