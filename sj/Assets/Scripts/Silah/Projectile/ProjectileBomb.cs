using Bolt;
using UnityEngine;

public class ProjectileBomb : Bolt.EntityBehaviour<IMermi>
{
    [SerializeField] private GameObject debugObject,patlamaInstance;
    [SerializeField] private float hiz, hasar, yokolmaSuresi,alan;
    [SerializeField] private string silahAd;
    private int takým;
    [SerializeField] private Transform t;
    public Senkranizasyon s;
    private Vector3 v = Vector3.one;
    private bool hasarVerildi = false;
    public override void Initialized()
    {
        v = Vector3.one * hiz;
        base.Initialized();
        entity.DestroyDelayed(yokolmaSuresi);
    }
    public void Takým()
    {
        takým = s.takým;
    }
    public override void SimulateOwner()
    {
        state.Pozisyon = t.position += t.right * hiz;

        RaycastHit hit;
#if UNITY_EDITOR
        Debug.DrawRay(transform.position, t.right * hiz * 2, Color.red);
#endif
        if (Physics.Raycast(transform.position, t.right, out hit, hiz * 2))
        {
            if (!hasarVerildi)
            {
                OnTriggerEnter(hit.collider);
            }
        }
    }
    private void Update()
    {
        if (!entity.IsOwner)
        {
            t.position = state.Pozisyon;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (s.gameObject.GetComponent<DebugPlayer>().debug)
        {
            Instantiate(debugObject, this.transform.position, Quaternion.identity);
        }
        if (entity.IsOwner)
        {
            hasarVerildi = true;

            Collider[] bombaCol = Physics.OverlapSphere(transform.position, alan);

            foreach (Collider c in bombaCol)
            {
                Senkranizasyon e;
                e = c.gameObject.GetComponent<Senkranizasyon>();
                if (e != null)
                {
                    send(e);
                }

            }
            entity.DestroyDelayed(0f);
        }
    }
    public void send(Senkranizasyon e)
    {
        ProjectileDamage p = ProjectileDamage.Create();
        p.Damage = hasar;
        p.Rot = t.right;

        p.EffectedID = e.state.ID;
        p.EffectiveID = s.state.ID;

        p.EffectedNick = e.state.NICK;
        p.EffectiveNick = s.state.NICK;
        p.Team = 999999999;
        p.Silah = silahAd;
        p.AreaDamage = true;
        p.Area = alan;
        p.Position = transform.position;

        Debug.Log(p.EffectedID + " " + p.EffectiveID + " takm:" + takým);

        p.Send();
    }
}