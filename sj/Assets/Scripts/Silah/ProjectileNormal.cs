using Bolt;
using UnityEngine;

public class ProjectileNormal : Bolt.EntityBehaviour<IMermi>
{
    [SerializeField] private float hiz,hasar,yokolmaSuresi;
    [SerializeField] private Transform t;
    public Senkranizasyon s;
    private Vector3 v = Vector3.one;
    public override void Initialized()
    {
        v = Vector3.one * hiz;
        base.Initialized();
        entity.DestroyDelayed(yokolmaSuresi);
    }
    public override void SimulateOwner()
    {
        state.Pozisyon = t.position += t.right * hiz;
          
        RaycastHit hit;

        if (Physics.Raycast(transform.position, t.right, out hit, hiz))
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
            BoltEntity e = null;
            e = other.GetComponent<BoltEntity>();
            if(e != null)
            {
               send(e);
            }
            entity.DestroyDelayed(0f);
        } 
    }
    public void send(BoltEntity e)
    {
        ProjectileDamage p = ProjectileDamage.Create();
        p.Damage = hasar;
        p.Rot = t.right;
        
        p.EffectedID = e.GetState<IMain>().ID;
        p.EffectiveID = s.state.ID;

        p.EffectedNick = e.GetState<IMain>().NICK;
        p.EffectiveNick = s.state.NICK;

        p.Send();
    }
}