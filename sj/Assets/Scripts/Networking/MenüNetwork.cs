using System.Collections;
using System.Collections.Generic;
using Photon.Bolt.Matchmaking;
using Photon.Bolt;
using TMPro;
using UdpKit;
using UdpKit.Platform.Photon;
using UnityEngine;
using UnityEngine.UI;

public class MenüNetwork : Photon.Bolt.GlobalEventListener
{
    [SerializeField] private GameObject autoDisconnect;
    [Header("Server Açma", order = 1)]
    [Space(order = 2)]

    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private TMP_InputField odaAdıInputField;
    [SerializeField] private Button serverButton;
    [SerializeField] private GameObject oynaEkranı, refreshText;
    [SerializeField] private string[] haritalar;
    private string harita;

    [Space(order = 0)]
    [Header("Server Listeleme", order = 1)]
    [Space(order = 2)]

    [SerializeField] private Transform serverList;
    [SerializeField] private GameObject buttonPrefab;

    [SerializeField] [Range(2, 10)] private float refreshTime = 3;
    private bool autoRefresh = true, refresh = false;
    private Coroutine lastCoroutine;

    private NetworkCallbacks networkCallbacks;
    private List<Button> joinGameButtons = new List<Button>();
    private float timer;

    [Space(order = 0)]
    [Header("Oyuncu Adı", order = 1)]
    [Space(order = 2)]

    [SerializeField] private TMP_InputField oyuncuAdıInputField;

    private void Awake()
    {
        BoltLauncher.StartClient();
        networkCallbacks = FindObjectOfType<NetworkCallbacks>();

        if (!PlayerPrefs.HasKey("Oyuncu Adı"))
            PlayerPrefs.SetString("Oyuncu Adı", "Adsız");
        else
            oyuncuAdıInputField.text = PlayerPrefs.GetString("Oyuncu Adı");
    }

    #region RefreshCallback
    public void ToggleAutoRefresh(bool toggle)
    {
        autoRefresh = toggle;
    }
    public void RefreshRooms() => refresh = true;
    private void Update()
    {
        if (autoRefresh)
        {
            if (timer >= refreshTime)
            {
                refresh = true;
                timer -= refreshTime;
            }
            timer += Time.deltaTime;
        }
        else timer = refreshTime;

        if (refresh)
        {
            if (lastCoroutine != null)
                StopCoroutine(lastCoroutine);
            lastCoroutine = StartCoroutine(Refresh());
            refresh = false;
        }
    }

    private IEnumerator Refresh()
    {
        networkCallbacks.State_SelectRoom();

        refreshText.SetActive(true);
        yield return new WaitForSeconds(1);
        refreshText.SetActive(false);

        if (oynaEkranı.activeSelf)
        {
            yield return new WaitForSeconds(1);
            serverButton.interactable = true;
        }
    }

    #endregion

    #region ServerSide
    public void StartServer() => StartCoroutine(WaitAndStartServer());
    private IEnumerator WaitAndStartServer()
    {
        BoltLauncher.Shutdown();
        while (BoltNetwork.IsClient)
        {
            yield return null;
        }
        BoltLauncher.StartServer();
    }

    public void MapDropdown()
    {
        harita = haritalar[dropdown.value];
        Debug.Log("Harita değiştirildi! Yeni harita: " + harita);
    }
    public override void BoltStartDone()
    {
        if (BoltNetwork.IsServer)
        {
            if (harita == null)
                harita = haritalar[0];

            string roomName;
            if (odaAdıInputField.text != "" && odaAdıInputField.text != " ")
                roomName = odaAdıInputField.text;
            else
                roomName = "Oda " + Random.Range(0, 10) + Random.Range(0, 10) + Random.Range(0, 10) + Random.Range(0, 10);

            PlayerPrefs.SetString("Room Name", roomName);

            BoltMatchmaking.CreateSession(sessionID: roomName, sceneToLoad: harita);

            Debug.Log("Session oluşturuluyor. Oda Adı: " + roomName + " Sahne: " + harita);
        }
    }

    public override void SessionCreationFailed(UdpSession session, UdpSessionError errorReason)
    {
        Debug.Log("Oda açılamadı, tekrar deneniyor...");
        BoltStartDone();
    }

    #endregion

    #region RoomsListing

    public void UpdateRooms(string label, PhotonSession photonSession)
    {
        Button joinGameButtonClone = Instantiate(buttonPrefab, serverList).GetComponent<Button>();
        joinGameButtonClone.gameObject.SetActive(true);
        joinGameButtonClone.onClick.AddListener(() => JoinGame(photonSession)); // server girme tuşu ayarlama
        joinGameButtonClone.onClick.AddListener(() => PlayerPrefs.SetString("Room Name", label));
        joinGameButtonClone.onClick.AddListener(() => Instantiate(autoDisconnect,Vector3.zero,Quaternion.identity));

        TMP_Text odaAdı = joinGameButtonClone.GetComponentInChildren<TMP_Text>();
        odaAdı.text = label; // oda adını tuşa yazdırma

        joinGameButtons.Add(joinGameButtonClone); // oda girme tuşunu listeye ekleme
    }

    private void JoinGame(UdpSession session) => BoltMatchmaking.JoinSession(session);
    public void ClearSessionList()
    {
        foreach (Button buton in joinGameButtons)
            Destroy(buton.gameObject);

        joinGameButtons.Clear();
    }

    #endregion
}