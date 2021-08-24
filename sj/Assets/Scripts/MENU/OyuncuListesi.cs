using System.Collections.Generic;
using Bolt.Matchmaking;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class OyuncuListesi : MonoBehaviour
{
    [SerializeField] [Range(0.5f, 5)] private float yenilemeSüresi = .5f;
    [SerializeField] private GameObject oyuncuListesi;
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject butonPrefab;
    private List<GameObject> butonlar = new List<GameObject>();
    public List<string> kd;
    private float mevcutSüre = 0;

    private bool açıkmı = false;

    private void Awake()
    {
        Debug.Log("BoltMatchmaking.CurrentSession.IsDedicatedServer = " + BoltMatchmaking.CurrentSession.IsDedicatedServer);

        oyuncuListesi.SetActive(false);
    }

    private void OyuncuListesiInput(InputAction.CallbackContext context)
    {
        açıkmı = context.ReadValueAsButton();
        oyuncuListesi.SetActive(açıkmı);
    }

    private void Update()
    {
        mevcutSüre += Time.deltaTime;
        if (mevcutSüre >= yenilemeSüresi)
        {
            mevcutSüre -= yenilemeSüresi;

            UpdateList();
        }
    }
    private void ResetList()
    {
        int length = butonlar.Count;
        for (int i = 0; i < length; i++) // listedeki objeler yok edildi
            Destroy(butonlar[i]);

        butonlar.Clear(); // liste temizlendi
    }
    private void UpdateList()
    {
        ResetList();

        foreach (BoltEntity entity in FindObjectsOfType<BoltEntity>())
        {
            string name = entity.GetState<IMain>().NICK; // nick verisini çekiyorum manuel olarak

            // oyuncu listesini buton yaptým ilerde týklayarak kick vote açma falan eklicem
            GameObject oluşturulanButon = Instantiate(butonPrefab, content);
            oluşturulanButon.GetComponentInChildren<TMP_Text>().text = name;

            butonlar.Add(oluşturulanButon); // butonu listeye ekledik
        }
    }
}