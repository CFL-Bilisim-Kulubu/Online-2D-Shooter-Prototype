using Bolt;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class Senkranizasyon : Bolt.EntityBehaviour<IMain>
{
    public bool takımlı;
    public int takım;
    [SerializeField] private Silah silah;
    [SerializeField] private MeshRenderer m;
    [SerializeField] private TMP_Text nick;
    [SerializeField] private Transform silahTransform;
    [SerializeField] private MonoBehaviour[] Kapatilcaklar;
    [SerializeField] private GameObject[] KapatilcakObje;
    [SerializeField] private PlayerInput input;
    [SerializeField] [Range(1f, 20f)] private float ışınlanmaMesafesi = 25f;
    [SerializeField] [Range(0.01f, 50f)] private float pozisyonLerpHızı = 2f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform t;
    public int IDEffecter;
    public string NickEffecter = "",GunEffecter;
    public bool spawnProtection = true;
    public override void Initialized() // Bolt Awake
    {
        #region renk
        Color color;
        color = Color.red;
        var renkKodu = "#"+PlayerPrefs.GetString("Renk");
        ColorUtility.TryParseHtmlString(renkKodu, out color);
        Debug.Log(renkKodu);
        state.Color = color;
        #endregion

        state.NICK = PlayerPrefs.GetString("sj");
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
        StartCoroutine(SpawnProtection_());
    }
    public override void Attached() // Bolt Start
    {
        Color a = state.Color;
        Debug.Log(a);
        m.material.color = a;
        nick.text = state.NICK;
        takım = state.Team; 
        StartCoroutine(Ayarlayici());
    } 
    public IEnumerator SpawnProtection_()
    {
        if (NickEffecter != "")
        {
            Died p = Died.Create();
            p.EffectedID = state.ID;
            p.EffectiveID = IDEffecter;

            p.Gun = GunEffecter;
            p.EffectedNick = state.NICK;
            p.EffectiveNick = NickEffecter;
            p.Send();
        }

        NickEffecter = "";

        spawnProtection = true;
        yield return new WaitForSeconds(2.4f);
        spawnProtection = false;
        Debug.Log("spawn protection bitti");
    }
    public void changeID()
    {
        nick.text = state.NICK;
        BoltEntity[] e = FindObjectsOfType<BoltEntity>();
        int a = Random.Range(0, 9999999);
        state.ID = a;

        if (!takımlı)
            takım = a;

        state.Team = takım;


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
        state.Velocity = rb.velocity;
        state.Pozisyon = t.position;
        state.silahRot = silahTransform.rotation;
    }
    private void FixedUpdate()
    {
        if (!entity.IsOwner)
        {
            rb.velocity = state.Velocity;
            if (Vector3.Distance(state.Pozisyon, rb.position) > ışınlanmaMesafesi)
                rb.MovePosition(state.Pozisyon);
            else
                rb.MovePosition(Vector3.Slerp(rb.position, state.Pozisyon, pozisyonLerpHızı * Time.fixedDeltaTime));
            silahTransform.rotation = state.silahRot;
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
        nick.text = state.NICK;
        takım = state.Team;
    }

    public void SilahSenkranizasyon()
    {
        if (entity.IsOwner)
        {
            ChangeWeapon c = ChangeWeapon.Create();
            c.ID = state.ID;
            c.weapon = silah.silahID;
            c.Send();
        }
    }
    public void SilahModelDeğiş(int silahID)
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
