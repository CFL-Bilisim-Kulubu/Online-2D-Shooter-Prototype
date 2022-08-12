using Photon.Bolt;
using UnityEngine;
using System.Collections;

public class BossBattleGameRuler : GlobalEventListener
{
    [SerializeField] private bool isHost;
    [SerializeField] private BoxCollider spawnArea;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Color BossColor;
    [Header("Boss Settings")]
    [SerializeField] private float bossDrag, bossJumpForce, bossMovementSpeed;
    [SerializeField] private int bossGunID;
    private BoltEntity[] every;


    private Senkranizasyon s;
    private Color defaultColor;
    private float normalDrag, normalJumpForce, normalMovementSpeed;



    private void Awake()
    {
        isHost = BoltNetwork.IsServer;
        //renk verisini çekme
        defaultColor = Color.red;
        var renkKodu = "#" + PlayerPrefs.GetString("Renk");
        ColorUtility.TryParseHtmlString(renkKodu, out defaultColor);
    }
    public override void SceneLoadLocalDone(string scene, IProtocolToken protocolToken)
    {
        Spawn(0);

        Senkranizasyon[] senk;
        senk = FindObjectsOfType<Senkranizasyon>();
        foreach (Senkranizasyon snk in senk)
        {
            snk.SilahSenkranizasyon();
        }
    }
    public void Spawn(int team)
    {
        if (!spawnArea) spawnArea = GameObject.FindGameObjectWithTag("Spawn Area").GetComponent<BoxCollider>();

        Vector3 random = new Vector3(spawnArea.bounds.extents.x * Random.Range(-1f, 1f),
                                     spawnArea.bounds.extents.y * Random.Range(-1f, 1f),
                                     0);

        //Vector3 spawn = spawnPoint.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));

        GameObject g = BoltNetwork.Instantiate(playerPrefab, new Vector3(spawnArea.bounds.center.x, spawnArea.bounds.center.y, 0) + random, spawnArea.transform.rotation);
        Debug.Log("Yeni Oyuncu Oyuna Baðlandý ve Spawnlandý");

        s = g.GetComponent<Senkranizasyon>();

        s.takým = isHost ? 1 : team;
        s.takýmlý = true;
        s.state.IsBoss = false;

        s.changeID(); // takýmý ayarla

        if (isHost)
        {
            NewBoss n = NewBoss.Create();
            n.NewID = s.state.ID;
            n.OldID = 0;
            n.Send();
        }

    }
    

    public void OnSomeoneDied(Died evnt)
    {
        if (!isHost)
            return;

#if UNITY_EDITOR
        Debug.Log("Host Biziz");
#endif

        every = FindObjectsOfType<BoltEntity>();
        foreach(BoltEntity entity in every)
        {

            if (!entity.StateIs<IMain>())
                continue;

            if (entity.GetState<IMain>().IsBoss) 
            {
#if UNITY_EDITOR
                Debug.Log("Boss mu Öldü Kontrol Ediliyor");
#endif
                //daha optimize olsun diye ilk boss mu diye bakýyoz sadece
                if (entity.GetState<IMain>().ID == evnt.EffectedID)
                {
                    Debug.Log("Boss Öldü");

                    NewBoss n = NewBoss.Create();
                    n.NewID = evnt.EffectiveID;
                    n.OldID = evnt.EffectedID;
                    n.Send();
                    Debug.Log(evnt.EffectiveID + " ve " + evnt.EffectedID);
                }
                    
            }
        }
    }
    
    /* BUNLAR CLIENT SIDED */
    public void makeBoss(Senkranizasyon se,Silah si)
    {
        //normal dragi, hýzý ve zýplama gücünü depolama
        normalMovementSpeed = se.gameObject.GetComponent<Conttrollrer>().SonHýz;
        normalJumpForce = se.gameObject.GetComponent<Conttrollrer>().ZýplamaGücü;
        normalDrag = se.gameObject.GetComponent<Rigidbody>().drag;  

        se.gameObject.GetComponent<Rigidbody>().drag = bossDrag;
        se.gameObject.GetComponent<Conttrollrer>().ZýplamaGücü = bossJumpForce;
        se.gameObject.GetComponent<Conttrollrer>().SonHýz = bossMovementSpeed;
        se.gameObject.GetComponentInChildren<SilahDeðiþtirici>().AktiveEt();


        GetCrate g = GetCrate.Create();
        g.PlayerID = se.state.ID;
        g.ItemID = bossGunID;
        g.Send();

        //renk deðiþtirme
        ChangeColor cc = ChangeColor.Create();
        cc.ID = se.state.ID;
        cc.Color = BossColor;
        cc.Send();
    }
    public void makeNormal(Senkranizasyon se,Silah si)
    {
        se.gameObject.GetComponent<Rigidbody>().drag = normalDrag;
        se.gameObject.GetComponent<Conttrollrer>().ZýplamaGücü = normalJumpForce;
        se.gameObject.GetComponent<Conttrollrer>().SonHýz = normalMovementSpeed;
        se.gameObject.GetComponentInChildren<SilahDeðiþtirici>().DeAktiveEt();


        GetCrate g = GetCrate.Create();
        g.PlayerID = se.state.ID;
        g.ItemID = si.defSilah;
        g.Send();

        //renk deðiþtirme
        ChangeColor cc = ChangeColor.Create();
        cc.ID = se.state.ID;
        cc.Color = defaultColor;
        cc.Send();
    }
}