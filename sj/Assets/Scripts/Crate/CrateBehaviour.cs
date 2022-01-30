using Photon.Bolt;
using UnityEngine;

public class CrateBehaviour : Photon.Bolt.EntityBehaviour<ICrate>
{
    [SerializeField] private Transform t;
    [SerializeField] private BoltEntity _entity;
    [SerializeField] private Rigidbody rb;
    public override void SimulateOwner()
    {
        if(_entity.IsOwner)
        {
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
}
