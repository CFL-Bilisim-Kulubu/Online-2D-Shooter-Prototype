using Photon.Bolt;
using UnityEngine;
using System.Collections;

public class EventListener : Photon.Bolt.GlobalEventListener
{
    [SerializeField] private SilahDebug sd;
    [SerializeField] private Silah silah;
    [SerializeField] private Senkranizasyon s;
    [SerializeField] private BossBattleGameRuler bbgr;
    private ÖlümListesi Öl;
    private void Awake()
    {
        Öl = FindObjectOfType<ÖlümListesi>();
        bbgr = FindObjectOfType<BossBattleGameRuler>();
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

        //Oyun Modu Boss Battle ise
        if(bbgr != null)
        {
#if UNITY_EDITOR
            Debug.Log("BossBattleLog BULUNDU");
#endif
            bbgr.OnSomeoneDied(evnt);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("BossBattleLog BULUNAMADI");
#endif
        }

    }
    public override void OnEvent(VisualisePlayer evnt)
    {
        Senkranizasyon[] senk = FindObjectsOfType<Senkranizasyon>();
        foreach(Senkranizasyon sj in senk)
        {
            sj.Ayarla(sj.state.Color);
        }

        if (FindObjectOfType<Callbacks>() == null) return;

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

            if (s.state.IsBoss)
                silah.Boss();
        }
    }
    public override void OnEvent(NewBoss evnt)
    {
        if(evnt.OldID == s.state.ID)
        {
            Debug.Log("Normal Oyuncu Olduk");
            s.state.IsBoss = false;
            bbgr.makeNormal(s, silah);
        }
        else if(evnt.NewID == s.state.ID)
        {
            Debug.Log("Boss Olduk");
            s.state.IsBoss = true;
            bbgr.makeBoss(s, silah);
        }
    }
    public override void OnEvent(ChangeColor evnt)
    {
        if (evnt.ID == s.state.ID)
        {
            s.state.Color = evnt.Color;

            StartCoroutine(visualisePlayerLate());
        }
    }
    private IEnumerator visualisePlayerLate()
    {
        yield return new WaitForSeconds(1.3f);
        
        VisualisePlayer p = VisualisePlayer.Create();
        p.Send();
    }
}
