using System.Collections;
using UnityEngine;

public class DunyaSiniri : MonoBehaviour
{
    [SerializeField] private Collider spawnArea;
    public bool acik;
    private void Awake() => acik = true;
    private void OnTriggerEnter(Collider other)
    {
        if (acik)
        {
            Vector3 random = new Vector3(spawnArea.bounds.extents.x * Random.Range(-1f, 1f),
                                                     spawnArea.bounds.extents.y * Random.Range(-1f, 1f),
                                                     spawnArea.bounds.extents.z * Random.Range(-1f, 1f));

            //Vector3 spawn = spawnPoint.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
            other.transform.position = spawnArea.bounds.center + random;

            Debug.Log("biri öldü sj");
            Rigidbody rb = other.GetComponent<Rigidbody>();
            Senkranizasyon s = other.GetComponent<Senkranizasyon>();
            if (rb)
                rb.velocity = Vector3.zero;
            if (s.entity.IsOwner)
            {
                s.StartCoroutine("SpawnProtection_");
                acik = true;
            }
        }
    }
}
