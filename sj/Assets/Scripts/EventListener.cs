using Photon.Bolt;
using UnityEngine;
using System;

public class EventListener : Photon.Bolt.GlobalEventListener
{
    [SerializeField] private SilahDebug sd;
    [SerializeField] private Silah silah;
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
                s.Vurul(evnt.Rotation * evnt.Damage);
            }
            else
            {
                s.rb.AddExplosionForce(evnt.Damage, evnt.Position, evnt.Area);
            }
            s.IDEffecter = evnt.EffectiveID;
            s.NickEffecter = evnt.EffectiveNick;
            s.GunEffecter = evnt.Gun;
        }
    }
    public override void OnEvent(Died evnt)
    {
        if (evnt.EffectiveID == s.state.ID)
        {
            Öl.OnKilled(evnt.EffectiveNick, evnt.EffectedNick, evnt.Gun);
            Öl.takým = evnt.Team;
            s.entity.GetState<IMain>().Kill++;
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
                sj.SilahModelDeðiþ(evnt.Weapon);
            }
        }
    }
    public override void OnEvent(GetCrate evnt)
    {
        if(evnt.PlayerID == s.state.ID)
        {
            silah.ayar = sd.ayar[evnt.ItemID];
            silah.Ayarla();
            //silah.Sýfýrla();
            s.SilahSenkranizasyon();
        }
    }
}
