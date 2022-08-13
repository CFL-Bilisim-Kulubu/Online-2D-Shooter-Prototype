using Photon.Bolt;
using UnityEngine;

public class ProjectileNormal : Photon.Bolt.EntityBehaviour<IMermi>
{
    [SerializeField] private GameObject debugObject;
    [SerializeField] private float hiz,hasar,yokolmaSuresi;
    [SerializeField] private string silahAd;
    private int tak�m;
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
    public void Tak�m()
    {
        tak�m = s.tak�m;
    }
    public override void SimulateOwner()
    {
        state.Position = t.position += t.right * hiz;
          
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
            t.position = state.Position;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (entity.IsOwner && !other.isTrigger && !hasarVerildi)
        {
            if (s.gameObject.GetComponent<DebugPlayer>().debug)
            {
                Instantiate(debugObject, other.ClosestPoint(this.transform.position), Quaternion.identity);
            }
            hasarVerildi = true;

            Senkranizasyon e = other.GetComponent<Senkranizasyon>();
            e = other.gameObject.GetComponent<Senkranizasyon>();

#if UNITY_EDITOR
            Debug.Log(e != null ? "Projectile Collision Player" : "Projectile Collision NOT Player");
#endif

            if(e != null)
            {
                send(e);
            }
            entity.DestroyDelayed(0f);
        } 
    }
    public void send(Senkranizasyon e)
    {
        ProjectileDamage p = ProjectileDamage.Create();
        p.Damage = hasar;
        p.Rotation = t.right;

        p.EffectedID = e.state.ID;
        p.EffectiveID = s.state.ID;

        p.EffectedNick = e.state.Nick;
        p.EffectiveNick = s.state.Nick;
        p.Team = tak�m;
        p.Gun = silahAd;
        p.AreaDamage = false;

#if UNITY_EDITOR
        Debug.Log("================PROJECTILE LOG\nEFFECTED ID/NICK:" + p.EffectedID + " " + p.EffectedNick + " \nEFFFECTIVE ID/NICK: " + p.EffectiveID + " " + p.EffectiveNick + "KILLER TEAM NUM:" + tak�m);
#endif

        p.Send();
    }
}