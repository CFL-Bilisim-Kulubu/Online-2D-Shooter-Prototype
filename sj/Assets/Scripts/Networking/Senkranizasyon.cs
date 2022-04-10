using Photon.Bolt;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class Senkranizasyon : Photon.Bolt.EntityBehaviour<IMain>
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
    [SerializeField] [Range(1f, 20f)] private float ışınlanmaMesafesi = 25f,spawnProtectionT = 5f;
    [SerializeField] [Range(0.01f, 50f)] private float pozisyonLerpHızı = 2f;
    public Rigidbody rb;
    [SerializeField] private Transform t;
    [SerializeField] private bool fps = false;
    public int IDEffecter;
    public string NickEffecter = "",GunEffecter;
    public bool spawnProtection = true;
    public override void Initialized() // Bolt Awake
    {
        state.Death = 0;
        #region renk
        Color color;
        color = Color.red;
        var renkKodu = "#"+PlayerPrefs.GetString("Renk");
        ColorUtility.TryParseHtmlString(renkKodu, out color);
        Debug.Log(renkKodu);
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
        StartCoroutine(SpawnProtection_());
    }
    public override void Attached() // Bolt Start
    {
        Color a = state.Color;
        Debug.Log(a);
        m.material.color = a;
        nick.text = state.Nick;
        takım = state.Team; 
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
        Debug.Log("spawn protection bitti");
    }
    public void changeID()
    {
        nick.text = state.Nick;
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
        t.position = new Vector3(t.position.x, t.position.y, fps ? t.position.z : 0);
        state.Velocity = rb.velocity;
        state.Position = new Vector3(t.position.x, t.position.y, fps ? t.position.z : 0);
        state.SilahRot = silahTransform.rotation;

        if (fps)
        {
            state.Rot = t.rotation;
            t.rotation = state.Rot;
        }
    }
    private void FixedUpdate()
    {
        if (!entity.IsOwner)
        {
            rb.velocity = state.Velocity;
            if (Vector3.Distance(state.Position, rb.position) > ışınlanmaMesafesi)
                rb.MovePosition(state.Position);
            else
                rb.MovePosition(Vector3.Slerp(rb.position, state.Position, pozisyonLerpHızı * Time.fixedDeltaTime));
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
        takım = state.Team;
    }

    public void SilahSenkranizasyon()
    {
        if (entity.IsOwner)
        {
            ChangeWeapon c = ChangeWeapon.Create();
            c.ID = state.ID;
            c.Weapon = silah.silahID;
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
