using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DunyaSiniri : MonoBehaviour
{
    [SerializeField] private Collider spawnArea;
    [SerializeField] [Tooltip("onDie eventleri ölme döngüsünün sonunda çaðýrýlýr")] private UnityEvent onDie;
    public void OnTriggerEnter(Collider other)
    {
        Vector3 random = new Vector3(spawnArea.bounds.extents.x * Random.Range(-1f, 1f),
            spawnArea.bounds.extents.y * Random.Range(-1f, 1f),
            spawnArea.bounds.extents.z * Random.Range(-1f, 1f));
        
        //Vector3 spawn = spawnPoint.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
        other.transform.position = spawnArea.bounds.center + random;
        
        Debug.Log("biri öldü sj");
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        Senkranizasyon s = other.gameObject.GetComponent<Senkranizasyon>();
        if(!s)
        {
            Debug.Log("Oyuncu Olmayan Objenin Dünya Limitiyle Temasý ya da Oyuncu Tanýmlanamadý \n\n Obje Adý: " + other.gameObject.name);

            return;
        }
        Debug.Log("Oyuncu Ölümü Algýlandý (PLAYER WORLD LIMIT OUT)");

        if (rb)                         
            rb.velocity = Vector3.zero;

        if (s.entity.IsOwner)
        {
            s.StartCoroutine("SpawnProtection_");
        }

        try
        {
            onDie.Invoke();
        }
        catch(UnityException e)
        {
            Debug.Log("On Die event çaðýrýlma hatasý! \nHata:" + e);
        }
    }
}
