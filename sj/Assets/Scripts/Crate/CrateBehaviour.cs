using Photon.Bolt;
using UnityEngine;

public class CrateBehaviour : Photon.Bolt.EntityBehaviour<ICrate>
{
    [SerializeField] private Transform t;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private int destroyTime,item;
    [SerializeField] private int[] itemVariety;
    private void Awake()
    {
        item = itemVariety[Random.Range(0,itemVariety.Length)];
        entity.DestroyDelayed(destroyTime);
    }
    public override void SimulateOwner()
    {
        if(entity.IsOwner)
        {
            t.position = new Vector3(transform.position.x, transform.position.y, 0);
            state.Velocity = rb.velocity;
            state.Rotation = t.rotation;
            state.Position = t.position;
        }
        else
        {
            rb.velocity = state.Velocity;
            t.rotation = state.Rotation;
            t.position = state.Position;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player");
            GetCrate c = GetCrate.Create();
            Senkranizasyon s = other.gameObject.GetComponent<Senkranizasyon>();
            c.PlayerID = s.state.ID;
            c.ItemID = item;
            c.Send();
            BoltNetwork.Destroy(this.gameObject);
        }
    }
}
