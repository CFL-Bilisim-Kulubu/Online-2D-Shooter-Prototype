using Photon.Bolt;
using UnityEngine;

public class ProjectileCluster : Photon.Bolt.EntityBehaviour<IMermi>
{
    [SerializeField] private Collider self;
    [SerializeField] private Transform[] clusters;
    [SerializeField] private GameObject bombaInstance;
    [SerializeField] private float hiz, yokolmaSuresi = 3, yercekimiKatsayisi;
    [SerializeField] private string silahAd;
    public Senkranizasyon s;
    private float yc,zamanlayici;
    private int takým;
    [SerializeField] private Transform t;
    public override void Initialized()
    {
        base.Initialized();
    }
    public override void SimulateOwner()
    {

#if UNITY_EDITOR
        Debug.DrawRay(transform.position, t.right * hiz * 2, Color.red);
#endif
    }
    private void Update()
    {
        if (!entity.IsOwner)
        {
            t.position = state.Position;
        }
        else
        {
            yc += yercekimiKatsayisi;
            t.position += new Vector3(0, -yc, 0);
            state.Position = t.position += t.right * hiz;
            zamanlayici += Time.deltaTime;
        }
        if (yokolmaSuresi < zamanlayici)
            Patlat();
    }
    private void Patlat()
    {
        self.enabled = false;
        foreach (Transform t in clusters)
        {
            GameObject g = BoltNetwork.Instantiate(bombaInstance, t.position, t.rotation);
            ProjectileBomb b = g.GetComponent<ProjectileBomb>();
            b.s = s;
            b.Takým();
        }
        entity.DestroyDelayed(0);
    }

    private void OnTriggerEnter(Collider other) => Patlat();
}
