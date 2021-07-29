using Bolt;
using UnityEngine;

public class Callbacks : GlobalEventListener
{
    [SerializeField] private BoxCollider spawnArea;
    [SerializeField] private GameObject playerPrefab;

    [System.Obsolete]
    public override void SceneLoadLocalDone(string scene)
    {

        if (!spawnArea) spawnArea = GameObject.FindGameObjectWithTag("Spawn Area").GetComponent<BoxCollider>();

        Vector3 random = new Vector3(spawnArea.bounds.extents.x * Random.Range(-1, 1),
                                     spawnArea.bounds.extents.y * Random.Range(-1, 1),
                                     spawnArea.bounds.extents.z * Random.Range(-1, 1));

        //Vector3 spawn = spawnPoint.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
        BoltNetwork.Instantiate(playerPrefab, spawnArea.bounds.center + random, spawnArea.transform.rotation);
        Debug.Log("Yeni Oyuncu Spawnladım");
    }
}