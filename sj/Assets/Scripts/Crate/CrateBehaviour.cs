using Photon.Bolt;
using UnityEngine;

public class CrateBehaviour : Photon.Bolt.EntityBehaviour<ICrate>
{
    [SerializeField] private Transform t;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private int destroyTime,item;
    [SerializeField] private int[] itemVariety;
    [SerializeField] private float teleportTime = 5f,positionLerpSpeed = 2f;
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
            state.Rotation = transform.rotation;
            state.Position = new Vector3(t.position.x, t.position.y, 0);
        }
    }
    private void Update()
    {
        if (entity.IsOwner)
            return;


        rb.velocity = state.Velocity;
        t.rotation = state.Rotation;
        if (Vector3.Distance(state.Position, rb.position) > teleportTime)
            rb.MovePosition(state.Position);
        else
            rb.MovePosition(Vector3.Slerp(rb.position, state.Position, positionLerpSpeed * Time.fixedDeltaTime));
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player");
            Senkranizasyon s = other.gameObject.GetComponent<Senkranizasyon>();

            if (s.entity.GetState<IMain>().IsBoss)      //boss ise silahýný deðiþtirme
                return;

            GetCrate c = GetCrate.Create();
            c.PlayerID = s.state.ID;
            c.ItemID = item;
            c.Send();

            BoltNetwork.Destroy(this.gameObject);
        }
    }
}
