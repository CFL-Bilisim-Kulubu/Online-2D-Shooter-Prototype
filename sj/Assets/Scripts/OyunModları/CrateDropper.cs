using Photon.Bolt;
using UnityEngine;

public class CrateDropper : MonoBehaviour
{
    [SerializeField] private BoxCollider spawnArea;
    [SerializeField] private GameObject cratePrefab;
    [SerializeField] [Range(1,20)] private float spawnTime;
    private float timer;
    private void Awake()
    {
        if (!Photon.Bolt.BoltNetwork.IsServer)
            Destroy(this);
        else
            Debug.Log("Oda kurucu crate spawnlýyor !");
    }
    public void Spawn()
    {
        if (!spawnArea) spawnArea = GameObject.FindGameObjectWithTag("Spawn Area").GetComponent<BoxCollider>();

        Vector3 random = new Vector3(spawnArea.bounds.extents.x * Random.Range(-1f, 1f),
                                     spawnArea.bounds.extents.y * Random.Range(-1f, 1f),
                                     0);

        //Vector3 spawn = spawnPoint.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));

        GameObject g = BoltNetwork.Instantiate(cratePrefab, new Vector3(spawnArea.bounds.center.x, spawnArea.bounds.center.y,0) + random, spawnArea.transform.rotation);
        Debug.Log("Yeni Crate Spawnladým");
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > spawnTime)
        {
            Spawn();

            timer -= spawnTime;
        }
    }

}
