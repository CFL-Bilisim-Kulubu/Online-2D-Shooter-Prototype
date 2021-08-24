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
        if (evnt.EffectedID == s.state.ID && !s.spawnProtection && s.tak�m != evnt.Team)
        {
            if(!evnt.AreaDamage)
            {
                s.Vurul(evnt.Rot * evnt.Damage);
            }
            else
            {
                s.rb.AddExplosionForce(evnt.Damage, evnt.Position, evnt.Area);
            }
            s.IDEffecter = evnt.EffectiveID;
            s.NickEffecter = evnt.EffectiveNick;
            s.GunEffecter = evnt.Silah;
        }
    }
    public override void OnEvent(Died evnt)
    {
        if (evnt.EffectiveID == s.state.ID)
        {
            �l.OnKilled(evnt.EffectiveNick, evnt.EffectedNick, evnt.Gun);
            �l.tak�m = evnt.Team;
        }
        else
        {
            �l.OnKilled(evnt.EffectiveNick, evnt.EffectedNick, evnt.Gun);
            �l.tak�m = evnt.Team;
        }
    }
    public override void OnEvent(VisualisePlayer evnt)
    {
        Senkranizasyon[] senk = FindObjectsOfType<Senkranizasyon>();
        foreach(Senkranizasyon sj in senk)
        {
            sj.Ayarla(sj.state.Color);
        }
        FindObjectOfType<Callbacks>().renk();// tak�ml�ysa rengimiz kendimizde d�zelsin diye
    }
    public override void OnEvent(ChangeWeapon evnt)
    {
        Senkranizasyon[] senk = FindObjectsOfType<Senkranizasyon>();
        foreach (Senkranizasyon sj in senk)
        {
            if(sj.state.ID == evnt.ID)
            {
                sj.SilahModelDe�i�(evnt.weapon);
            }
        }
    }
}
