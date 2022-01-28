using System.Collections.Generic;
using Photon.Bolt.Matchmaking;
using Photon.Bolt;
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
    private float mevcutSüre = 0;

    private bool açıkmı = false;

    private void Awake()
    {
        Debug.Log("BoltMatchmaking.CurrentSession.IsDedicatedServer = " + BoltMatchmaking.CurrentSession.IsDedicatedServer);

        oyuncuListesi.SetActive(false);
    }

    public void OyuncuListesiInput(InputAction.CallbackContext context)
    {
        açıkmı = context.ReadValueAsButton();
        oyuncuListesi.SetActive(açıkmı);
        UpdateList();
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
#if UNITY_EDITOR
        print("Oyuncu listesi güncellendi");
#endif
        ResetList();

        foreach (BoltEntity entity in FindObjectsOfType<BoltEntity>())
        {
            if (!entity.StateIs<IMain>())
                return;
            string text = entity.GetState<IMain>().Nick + " " + entity.GetState<IMain>().Kill + " " + entity.GetState<IMain>().Death; // nick verisini çekiyorum manuel olarak

            // oyuncu listesini buton yaptım ilerde tıklayarak kick vote açma falan eklicem
            GameObject oluşturulanButon = Instantiate(butonPrefab, content);
            oluşturulanButon.GetComponentInChildren<TMP_Text>().text = text;



            butonlar.Add(oluşturulanButon); // butonu listeye ekledik
        }
    }
}