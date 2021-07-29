using Bolt;
using UnityEngine;
using System;

public class EventListener : Bolt.GlobalEventListener
{
    [SerializeField] private Senkranizasyon s;
    [SerializeField] private �l�mListesi �l;
    private void Awake()
    {
        �l = FindObjectOfType<�l�mListesi>();
    }
    public override void OnEvent(ProjectileDamage evnt)
    {
        if (evnt.EffectedID == s.state.ID && !s.spawnProtection)
        {
            s.Vurul(evnt.Rot * evnt.Damage);
            s.IDEffecter = evnt.EffectiveID;
            s.NickEffecter = evnt.EffectiveNick;
        }
    }
    public override void OnEvent(Died evnt)
    {
        if (evnt.EffectiveID == s.state.ID)
        {
            �l.OnKilled(evnt.EffectiveNick, evnt.EffectedNick, "Normal Silah");
        }
        else
        {
            �l.OnKilled(evnt.EffectiveNick, evnt.EffectedNick, "Normal Silah");
        }
    }
}
