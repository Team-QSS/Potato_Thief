using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager_LWS : MonoBehaviour
{
    public static MapManager_LWS mapMaker = null;
    List<GameObject> levers = new List<GameObject>();
    List<GameObject> doors = new List<GameObject>();
    List<List<GameObject>> objectsList = new List<List<GameObject>>();

    ObjectPoolingManager_LWS poolingManager;
    InteractingObjectGroup_LWS[] groups;

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
        poolingManager = ObjectPoolingManager_LWS.manager;
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

    public void SetDoor()
    {
        GameObject door;

        door = MakeObject(ObstacleObject.door, new Vector2(5, -2));

        groups = new InteractingObjectGroup_LWS[] {
            new InteractingObjectGroup_LWS(ObstacleObject.lever, 0),
            new InteractingObjectGroup_LWS(ObstacleObject.lever, 1)
        };

        MakeInteraction(door, groups);

        door = MakeObject(ObstacleObject.door, new Vector2(0, -2));

        groups = new InteractingObjectGroup_LWS[] {
            new InteractingObjectGroup_LWS(ObstacleObject.lever, 0),
            new InteractingObjectGroup_LWS(ObstacleObject.lever, 2)
        };

        MakeInteraction(door, groups);

        door = MakeObject(ObstacleObject.door, new Vector2(-5, -2));

        groups = new InteractingObjectGroup_LWS[] {
            new InteractingObjectGroup_LWS(ObstacleObject.lever, 1),
        };

        MakeInteraction(door, groups);
    }

    private void MakeInteraction(GameObject obj, InteractingObjectGroup_LWS[] interactions)
    {
        InteractedObstacle_LWS InteractedObstacle;
        InteractedObstacle = obj.GetComponent<InteractedObstacle_LWS>();

        for (int i = 0; i < interactions.Length; i++)
        {
            List<GameObject> targetList = objectsList[(int)interactions[i].obstacleType];
            GameObject targetObject = targetList[interactions[i].obstacleKey];
            InteractedObstacle.targetObjects.Add(targetObject);
        }
    }

    public void SendMessage(object[] contents) => ReceiveMessage(contents);

    private void ReceiveMessage(object[] contents)
    {
        ObstacleObject[] targetObjects = (ObstacleObject[])contents[0];
        int key = (int)contents[1];
        bool status = (bool)contents[2];

        levers[key].GetComponent<Obstacle_LWS>().SetStatus(status);

        for (int i = 0; i < targetObjects.Length; i++)
        {
            List<GameObject> targetList = objectsList[(int)targetObjects[i]];
            print("target  Enum : " + targetObjects[i]);

            for (int j = 0; j < targetList.Count; j++)
            {
                targetList[j].GetComponent<InteractedObstacle_LWS>().TargetStatusChange();
            }
        }
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