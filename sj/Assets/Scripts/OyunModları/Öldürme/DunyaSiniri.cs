using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DunyaSiniri : MonoBehaviour
{
    [SerializeField] private Collider spawnArea;
    [SerializeField] [Tooltip("onDie eventleri �lme d�ng�s�n�n sonunda �a��r�l�r")] private UnityEvent onDie;
    public void OnTriggerEnter(Collider other)
    {
        Vector3 random = new Vector3(spawnArea.bounds.extents.x * Random.Range(-1f, 1f),
            spawnArea.bounds.extents.y * Random.Range(-1f, 1f),
            spawnArea.bounds.extents.z * Random.Range(-1f, 1f));
        
        //Vector3 spawn = spawnPoint.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
        other.transform.position = spawnArea.bounds.center + random;
        
        Debug.Log("biri �ld� sj");
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        Senkranizasyon s = other.gameObject.GetComponent<Senkranizasyon>();
        if(!s)
        {
            Debug.Log("Oyuncu Olmayan Objenin D�nya Limitiyle Temas� ya da Oyuncu Tan�mlanamad� \n\n Obje Ad�: " + other.gameObject.name);

            return;
        }
        Debug.Log("Oyuncu �l�m� Alg�land� (PLAYER WORLD LIMIT OUT)");

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
            Debug.Log("On Die event �a��r�lma hatas�! \nHata:" + e);
        }
    }
}
