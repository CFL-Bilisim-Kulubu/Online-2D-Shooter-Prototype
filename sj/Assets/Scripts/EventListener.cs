using Bolt;
using UnityEngine;
using System;

public class EventListener : Bolt.GlobalEventListener
{
    [SerializeField] private Senkranizasyon s;
    [SerializeField] private ÖlümListesi Öl;
    private void Awake()
    {
        Öl = FindObjectOfType<ÖlümListesi>();
    }
    public override void OnEvent(ProjectileDamage evnt)
    {
        if (evnt.EffectedID == s.state.ID && !s.spawnProtection && s.takým != evnt.Team)
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
            Öl.OnKilled(evnt.EffectiveNick, evnt.EffectedNick, evnt.Gun);
            Öl.takým = evnt.Team;
        }
        else
        {
            Öl.OnKilled(evnt.EffectiveNick, evnt.EffectedNick, evnt.Gun);
            Öl.takým = evnt.Team;
        }
    }
    public override void OnEvent(VisualisePlayer evnt)
    {
        Senkranizasyon[] senk = FindObjectsOfType<Senkranizasyon>();
        foreach(Senkranizasyon sj in senk)
        {
            sj.Ayarla(sj.state.Color);
        }
        FindObjectOfType<Callbacks>().renk();// takýmlýysa rengimiz kendimizde düzelsin diye
    }
    public override void OnEvent(ChangeWeapon evnt)
    {
        Senkranizasyon[] senk = FindObjectsOfType<Senkranizasyon>();
        foreach (Senkranizasyon sj in senk)
        {
            if(sj.state.ID == evnt.ID)
            {
                sj.SilahModelDeðiþ(evnt.weapon);
            }
        }
    }
}
