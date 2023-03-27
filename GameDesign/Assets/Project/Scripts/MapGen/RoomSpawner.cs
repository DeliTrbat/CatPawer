using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    private RoomTemplates templates;

    public bool spawned = false;

    private Component[] placedRoomSpawnCheckers;
    private Component[] placedRoomSpawners;
    private RoomSpawnAreaChecker placedRoomSpawnAreaChecker;
    private RoomSpawnAreaChecker parentSpawnAreaChecker;

    public RoomTemplates roomQueue;
    private GameObject room;

    void Start()
    {
        Invoke("SpawnRoom", 0.5f);
    }

    private void SpawnRoom()
    {
        if (!spawned)
        {
            templates = GameObject.Find("Room Templates").GetComponent<RoomTemplates>();
            room = templates.GetNextGameObject();
            try
            {
                placedRoomSpawners = room.GetComponentsInChildren<RoomSpawner>();       //exception will be fixed
            }
            catch
            {
                return;
            }
            placedRoomSpawnCheckers = room.GetComponentsInChildren<RoomSpawnChecker>();
            placedRoomSpawnAreaChecker = room.GetComponentInChildren<RoomSpawnAreaChecker>();

            placedRoomSpawnAreaChecker.removable = true;

            // aligns a spawnchecker from the placeable prefab to this spawn point 
            Vector3 pos = this.transform.position - placedRoomSpawnCheckers[0].transform.localPosition;
            StartCoroutine(TryPlacePrefab(room, pos, 0));
        }
    }

    IEnumerator TryPlacePrefab(GameObject room, Vector3 position, int roomSpawnCheckerIndex)
    {
        if (this.spawned || roomSpawnCheckerIndex > placedRoomSpawnCheckers.Length)
            yield break;

        GameObject placedPrefab;
        for (int angle = 90; angle <= 360; angle += 90)
        {
            placedPrefab = Instantiate(room, position, Quaternion.identity);
            placedPrefab.transform.RotateAround(this.transform.position, Vector3.up, angle);

            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;


            if (IsPrefabStillPlaced(placedPrefab))
            {
                Transform child = transform.parent.Find("AreaChecker");
                if (child != null)
                {
                    RoomSpawnAreaChecker areaChecker = child.GetComponent<RoomSpawnAreaChecker>();
                    if (areaChecker != null)
                    {
                        areaChecker.removable = false;
                    }
                }
                placedRoomSpawnAreaChecker.removable = false;
                break;
            }
        }
        if(!this.spawned)
            StartCoroutine(TryPlacePrefab(room, position, roomSpawnCheckerIndex++));
    }

    bool IsPrefabStillPlaced(GameObject placedPrefab)
    {
        try
        {
            if (placedPrefab.activeSelf)
            {
                placedRoomSpawnAreaChecker.removable = false;
                this.spawned = true;
            }
        }
        catch (MissingReferenceException)
        {
            this.spawned = false;
        }
        return this.spawned;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoomSpawnChecker"))
        {
            this.spawned = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        print(collision);
    }
}
