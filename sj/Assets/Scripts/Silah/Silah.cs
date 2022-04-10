using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Bolt;

public class Silah : MonoBehaviour
{
    [SerializeField] private SilahDebug sd;
    public GameObject[] silahObjeleri;
    public SilahAyarlarý ayar;
    [SerializeField] private Senkranizasyon s;
    [SerializeField] private TMP_Text t;
    [SerializeField] private float aim;
    private float shootT,reloadT;
    private bool shoot,tutunma,reload;
    public Camera kamera;
    public GameObject projectile,projectileSpawn,aimParent,gunObject;
    [SerializeField] private float shootTime,maxAmmo,ammo,reloadSuresi,currentAmmo;
    private int mermiTipi = 1;
    public int silahID, defSilah = 0;

    public void Boss()
    {
        currentAmmo = 999;
    }

    private void Awake()
    {
        if (PlayerPrefs.HasKey("GUN"))
            defSilah = PlayerPrefs.GetInt("GUN");
        ayar = sd.ayar[defSilah];
        Ayarla();
        currentAmmo = 999;
        reload = false;
        ammo = maxAmmo;
        kamera = FindObjectOfType<Camera>();
    }
    void Update()
    {
        shootT+= Time.deltaTime;
        if (reload)
        {
            t.text = "Þarjör Deðiþiyor...";
            reloadT += Time.deltaTime;
            if (reloadT > reloadSuresi)
            {
                currentAmmo += ammo;
                ammo = currentAmmo >= maxAmmo ? maxAmmo : currentAmmo;
                currentAmmo -= ammo;
                if (ayar.silahSayi == defSilah)
                    currentAmmo = 999;
                reload = false;
                reloadT = 0;
            }
        }
        else
        {
            t.text = "Mermi : " + ammo + " / " + currentAmmo;
        }


        if (shootT > shootTime && shoot && !s.spawnProtection && ammo > 0 && !reload)
        {
            shootT = 0;
            GameObject a =
            BoltNetwork.Instantiate(projectile, projectileSpawn.transform.position, projectileSpawn.transform.rotation);
            switch (mermiTipi)
            {
                case 1:
                    a.GetComponent<ProjectileNormal>().s = s;
                    a.GetComponent<ProjectileNormal>().Takým();
                    ammo--;
                    break;
                case 2:
                    a.GetComponent<ProjectileBomb>().s = s;
                    a.GetComponent<ProjectileBomb>().Takým();
                    ammo--;
                    break;
                case 4:
                    a.GetComponent<ProjectileCluster>().s = s;
                    ammo--;
                    break;
            }
        }
        else if(ammo <= 0 && currentAmmo > 0)
        {
            Reload();
        }
        else if (ammo <= 0 && currentAmmo <= 0)
        {
            ayar = sd.ayar[defSilah];
            Ayarla();
            s.SilahSenkranizasyon();
        }
    }

    public void Ayarla()
    {
        projectile = ayar.mermi;
        shootTime = ayar.AteþSüresi;
        maxAmmo = ayar.MaksimumMermi;
        ammo = maxAmmo;
        reload = ayar.silahSayi == defSilah ? true : false;
        currentAmmo = maxAmmo * 2;
        reloadSuresi = ayar.ÞarjörYenilemeSüresi;
        projectileSpawn.transform.localPosition = ayar.MermiSpawn;
        mermiTipi = ayar.mermiTipi;
        silahID = ayar.silahSayi;

        foreach (GameObject s in silahObjeleri)
            s.SetActive(false);
        silahObjeleri[silahID].SetActive(true);
    }

    public void Sýfýrla()
    {
        ammo = 0;
    }

    public void Reload()
    {
        if (!reload && ammo != maxAmmo && currentAmmo > 0)
        {
            reload = true;
        }
    }

    #region girdi

    public void Shoot(InputAction.CallbackContext value)
    {
        shoot = value.ReadValueAsButton();
    }
    public void Reload(InputAction.CallbackContext value)
    {
        Reload();
    }

    #endregion
}
