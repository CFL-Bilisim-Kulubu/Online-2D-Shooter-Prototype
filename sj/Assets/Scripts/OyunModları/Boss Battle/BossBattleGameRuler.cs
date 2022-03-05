using Photon.Bolt;
using UnityEngine;

public class BossBattleGameRuler : GlobalEventListener
{
    [SerializeField] private bool isHost;
    [SerializeField] private BoxCollider spawnArea;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Color BossColor;
    [Header("Boss Settings")]
    [SerializeField] private float bossDrag;
    [SerializeField] private int bossGunID;
    
    private Senkranizasyon s;
    private Color defaultColor;
    private float normalDrag;



    private void Awake()
    {
        isHost = BoltNetwork.IsServer;
        //renk verisini �ekme
        defaultColor = Color.red;
        var renkKodu = "#" + PlayerPrefs.GetString("Renk");
        ColorUtility.TryParseHtmlString(renkKodu, out defaultColor);
    }
    public override void SceneLoadLocalDone(string scene, IProtocolToken protocolToken)
    {
        Spawn(0);
    }
    public void Spawn(int team)
    {
        if (!spawnArea) spawnArea = GameObject.FindGameObjectWithTag("Spawn Area").GetComponent<BoxCollider>();

        Vector3 random = new Vector3(spawnArea.bounds.extents.x * Random.Range(-1f, 1f),
                                     spawnArea.bounds.extents.y * Random.Range(-1f, 1f),
                                     0);

        //Vector3 spawn = spawnPoint.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));

        GameObject g = BoltNetwork.Instantiate(playerPrefab, new Vector3(spawnArea.bounds.center.x, spawnArea.bounds.center.y, 0) + random, spawnArea.transform.rotation);
        Debug.Log("Yeni Oyuncu Spawnlad�m");

        s = g.GetComponent<Senkranizasyon>();

        s.tak�m = isHost ? 1 : team;
        s.tak�ml� = true;
        s.state.IsBoss = false;

        s.changeID(); // tak�m� ayarla

        if (isHost)
        {
            NewBoss n = NewBoss.Create();
            n.NewID = s.state.ID;
            n.OldID = 0;
            n.Send();
        }

    }
    

    public override void OnEvent(Died evnt)
    {
        if (!isHost)
            return;

        BoltEntity[] every = FindObjectsOfType<BoltEntity>();
        foreach(BoltEntity entity in every)
        {
                if (!entity.StateIs<IMain>())
                    return;
                if(entity.GetState<IMain>().IsBoss)
                {
                    Debug.Log("buldum");
                    //daha optimize olsun diye ilk boss mu diye bak�yoz sadece
                    if (entity.GetState<IMain>().ID == evnt.EffectedID)
                    {
                        NewBoss n = NewBoss.Create();
                        n.NewID = evnt.EffectiveID;
                        n.OldID = evnt.EffectedID;
                        n.Send();
                        Debug.Log(evnt.EffectiveID + " ve " + evnt.EffectedID);
                    }
                }

            
        }
    }
    

    public void makeBoss(Senkranizasyon se,Silah si)
    {
        normalDrag = se.gameObject.GetComponent<Rigidbody>().drag;//normal dragi depolama
        se.gameObject.GetComponent<Rigidbody>().drag = bossDrag;
        GetCrate g = GetCrate.Create();
        g.PlayerID = se.state.ID;
        g.ItemID = bossGunID;
        g.Send();

        //renk de�i�tirme
        se.state.Color = BossColor;
        se.Ayarla(BossColor);
        VisualisePlayer p = VisualisePlayer.Create();
        p.Send();
    }
    public void makeNormal(Senkranizasyon se,Silah si)
    {
        se.gameObject.GetComponent<Rigidbody>().drag = normalDrag;
        GetCrate g = GetCrate.Create();
        g.PlayerID = se.state.ID;
        g.ItemID = si.defSilah;
        g.Send();

        //renk de�i�tirme
        se.state.Color = defaultColor;
        se.Ayarla(defaultColor);
        VisualisePlayer p = VisualisePlayer.Create();
        p.Send();
    }
}