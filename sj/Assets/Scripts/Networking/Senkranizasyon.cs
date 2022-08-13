using Photon.Bolt;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class Senkranizasyon : Photon.Bolt.EntityBehaviour<IMain>
{
    public bool takýmlý;
    public int takým;
    [SerializeField] private Silah silah;
    [SerializeField] private MeshRenderer m;
    [SerializeField] private TMP_Text nick;
    [SerializeField] private Transform silahTransform,silahScaleTransform;
    [SerializeField] private MonoBehaviour[] Kapatilcaklar;
    [SerializeField] private GameObject[] KapatilcakObje;
    [SerializeField] private PlayerInput input;
    [SerializeField] [Range(1f, 20f)] private float ýþýnlanmaMesafesi = 25f,spawnProtectionT = 5f;
    [SerializeField] [Range(0.01f, 50f)] private float pozisyonLerpHýzý = 2f;
    public Rigidbody rb;
    [SerializeField] private Transform t;
    public int IDEffecter;
    public string NickEffecter = "",GunEffecter;
    public bool spawnProtection = true;
    public override void Initialized() // Bolt Awake
    {
        state.Death = -1;

        #region renk
        Color color;
        color = Color.red;
        var renkKodu = "#"+PlayerPrefs.GetString("Renk");
        ColorUtility.TryParseHtmlString(renkKodu, out color);
#if UNITY_EDITOR
        Debug.Log("Oyuncu Renk Kodu :" + renkKodu);
#endif
        state.Color = color;
        #endregion

        state.Nick = PlayerPrefs.GetString("sj");
        spawnProtection = true;
        base.Initialized();
        if (!entity.IsOwner)
        {
            input.enabled = false;
            foreach(MonoBehaviour m in Kapatilcaklar)
            {
                m.enabled = false;
            }
            foreach (GameObject m in KapatilcakObje)
            {
                m.SetActive(false);
            }
        }
        changeID();

        GetCrate g = GetCrate.Create();
        g.PlayerID = state.ID;
        g.ItemID = silah.defSilah;
        g.Send();

        StartCoroutine(SpawnProtection_());
    }
    public override void Attached() // Bolt Start
    {
        Color a = state.Color;
#if UNITY_EDITOR
        Debug.Log("Oyuncu Rengi RGB :" + a);
#endif
        m.material.color = a;
        nick.text = state.Nick;
        takým = state.Team; 
        StartCoroutine(Ayarlayici());
    } 

    public IEnumerator SpawnProtection_()
    {
        if (NickEffecter != "")
        {
            state.Death++;
            Died p = Died.Create();
            p.EffectedID = state.ID;
            p.EffectiveID = IDEffecter;

            p.Gun = GunEffecter;
            p.EffectedNick = state.Nick;
            p.EffectiveNick = NickEffecter;
            p.Send();
        }

        NickEffecter = "";

        spawnProtection = true;
        yield return new WaitForSeconds(spawnProtectionT);
        spawnProtection = false;
#if UNITY_EDITOR
        Debug.Log("spawn protection bitti");
#endif
    }
    public void changeID()
    {
        nick.text = state.Nick;
        BoltEntity[] e = FindObjectsOfType<BoltEntity>();
        int a = Random.Range(0, 9999999);
        state.ID = a;

        if (!takýmlý)
            takým = a;

        state.Team = takým;


        if (e.Length > 1)
        {
            foreach (BoltEntity y in e)
            {
                if (y.StateIs<IMain>())
                {
                    BoltEntity sj = GetComponent<BoltEntity>();
                    if (y.GetState<IMain>().ID == state.ID && y != sj)
                    {
                        changeID();
                    }
                }
            }
        }
    }
    public override void SimulateOwner()
    {
        base.SimulateOwner();
        t.position = new Vector3(t.position.x, t.position.y,0);
        state.Velocity = rb.velocity;
        state.Position = new Vector3(t.position.x, t.position.y,0);
        state.SilahRot = silahTransform.rotation;
        state.Scale = silahScaleTransform.localScale;
        Debug.Log(silahScaleTransform.localScale);

    }
    private void FixedUpdate()
    {
        if (!entity.IsOwner)
        {
            rb.velocity = state.Velocity;
            if (Vector3.Distance(state.Position, rb.position) > ýþýnlanmaMesafesi)
                rb.MovePosition(state.Position);
            else
                rb.MovePosition(Vector3.Slerp(rb.position, state.Position, pozisyonLerpHýzý * Time.fixedDeltaTime));

            silahScaleTransform.localScale = state.Scale;
            silahTransform.rotation = state.SilahRot;
        }
    }
    private IEnumerator Ayarlayici()
    {
        yield return new WaitForSeconds(2f);
        VisualisePlayer p = VisualisePlayer.Create();
        p.Send();
    }
    public void Ayarla(Color a)
    {
        Debug.Log(a);
        m.material.color = a;
        nick.text = state.Nick;
        takým = state.Team;
    }

    public void SilahSenkranizasyon()
    {

        ChangeWeapon c = ChangeWeapon.Create();
        c.ID = state.ID;
        c.Weapon = silah.silahID;
        c.Send();
    }
    public void SilahModelDeðiþ(int silahID)
    {
        foreach (GameObject s in silah.silahObjeleri)
            s.SetActive(false);
        silah.silahObjeleri[silahID].SetActive(true);
    }

    public void Vurul(Vector3 vurus)
    {
        rb.AddForce(vurus);
    }
}
