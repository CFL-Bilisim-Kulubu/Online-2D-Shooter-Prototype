using Bolt;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class Senkranizasyon : Bolt.EntityBehaviour<IMain>
{
    [SerializeField] private TMP_Text nick;
    [SerializeField] private Transform silah;
    [SerializeField] private MonoBehaviour[] Kapatilcaklar;
    [SerializeField] private GameObject[] KapatilcakObje;
    [SerializeField] private PlayerInput input;
    [SerializeField] [Range(1f, 20f)] private float ýþýnlanmaMesafesi = 25f;
    [SerializeField] [Range(0.01f, 50f)] private float pozisyonLerpHýzý = 2f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform t;
    public int IDEffecter;
    public string NickEffecter = "";
    public bool spawnProtection = true;
    public override void Initialized() // Bolt Awake
    {
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
        else
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
        changeID();
        StartCoroutine(SpawnProtection_());
    }
    public IEnumerator SpawnProtection_()
    {
        if (NickEffecter != "")
        {
            Died p = Died.Create();
            p.EffectedID = state.ID;
            p.EffectiveID = IDEffecter;

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
    private void changeID()
    {
        nick.text = state.NICK;
        BoltEntity[] e = FindObjectsOfType<BoltEntity>();
        state.ID = Random.Range(0, 9999999);
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
        state.silahRot = silah.rotation;
    }
    private void FixedUpdate()
    {
        if (!entity.IsOwner)
        {
            nick.text = state.NICK;
            rb.velocity = state.Velocity;
            if (Vector3.Distance(state.Pozisyon, rb.position) > ýþýnlanmaMesafesi)
                rb.MovePosition(state.Pozisyon);
            else
                rb.MovePosition(Vector3.Slerp(rb.position, state.Pozisyon, pozisyonLerpHýzý * Time.fixedDeltaTime));
            silah.rotation = state.silahRot;
        }
    }

    public void Vurul(Vector3 vurus)
    {
        rb.AddForce(vurus);
    }
}
