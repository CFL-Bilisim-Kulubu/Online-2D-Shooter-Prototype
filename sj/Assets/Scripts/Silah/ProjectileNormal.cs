using Bolt;
using UnityEngine;

public class ProjectileNormal : Bolt.EntityBehaviour<IMermi>
{
    [SerializeField] private float hiz,hasar,yokolmaSuresi;
    private int tak�m;
    [SerializeField] private Transform t;
    public Senkranizasyon s;
    private Vector3 v = Vector3.one;
    public override void Initialized()
    {
        v = Vector3.one * hiz;
        base.Initialized();
        entity.DestroyDelayed(yokolmaSuresi);
    }
    public void Tak�m()
    {
        tak�m = s.tak�m;
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
            OnTriggerEnter(hit.collider);
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
        if (entity.IsOwner)
        {
            Debug.Log(other.gameObject);
            Debug.Log("mermi carpti ama sj");

            Senkranizasyon e = other.GetComponent<Senkranizasyon>();
            e = other.gameObject.GetComponent<Senkranizasyon>();
            Debug.Log(e);

            if(e != null)
            {
                Debug.Log("mermi carpti");
                send(e);
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
        p.Team = tak�m;

        Debug.Log(p.EffectedID + " " + p.EffectiveID + " takm:" + tak�m);

        p.Send();
    }
}