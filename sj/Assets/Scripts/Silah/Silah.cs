using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Bolt;

public class Silah : MonoBehaviour
{
    [SerializeField] private SilahDebug sd;
    public GameObject[] silahObjeleri;
    public SilahAyarlar� ayar;
    [SerializeField] private Senkranizasyon s;
    [SerializeField] private TMP_Text t;
    public Conttrollrer c;
    [SerializeField] private float aim;
    private float shootT,reloadT;
    private Vector2 gamepadAim, mouseAim, playerLoc;
    private bool gamepad,shoot,tutunma,reload;
    public Camera kamera;
    public GameObject projectile,projectileSpawn,aimParent,gunObject;
    [SerializeField] private float shootTime,maxAmmo,ammo,reloadSuresi,currentAmmo;
    private int mermiTipi = 1,defSilah = 0;
    public int silahID;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("GUN"))
            defSilah = PlayerPrefs.GetInt("GUN");
        ayar = sd.ayar[defSilah];
        Ayarla();
        currentAmmo = 999;
        reload = false;
        kamera = FindObjectOfType<Camera>();
    }
    void Update()
    {
        shootT+= Time.deltaTime;
        if (reload)
        {
            t.text = "�arj�r De�i�iyor...";
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


        if (shootT > shootTime && shoot && !c.tutunma && !s.spawnProtection && ammo > 0 && !reload)
        {
            shootT = 0;
            GameObject a =
            BoltNetwork.Instantiate(projectile, projectileSpawn.transform.position, projectileSpawn.transform.rotation);
            switch (mermiTipi)
            {
                case 1:
                    a.GetComponent<ProjectileNormal>().s = s;
                    a.GetComponent<ProjectileNormal>().Tak�m();
                    ammo--;
                    break;
                case 2:
                    a.GetComponent<ProjectileBomb>().s = s;
                    a.GetComponent<ProjectileBomb>().Tak�m();
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
        else if (currentAmmo <= 0)
        {
            ayar = sd.ayar[defSilah];
            Ayarla();
            s.SilahSenkranizasyon();
        }

        #region aim alma

        playerLoc = kamera.WorldToScreenPoint(transform.position);

        if (gamepad)
        {
            aim = Mathf.Rad2Deg * Mathf.Atan2(gamepadAim.y, gamepadAim.x);
        }
        else
        {
            aim = Mathf.Rad2Deg * Mathf.Atan2(
                mouseAim.y - playerLoc.y,
                mouseAim.x - playerLoc.x);
        }
        float sayi = aim > 90 || aim < -90 ? -1f : 1f;


        gunObject.transform.localScale = new Vector3(1,sayi,1);

        aimParent.transform.rotation = Quaternion.Euler(0, 0, aim);

        #endregion
    }

    public void Ayarla()
    {
        projectile = ayar.mermi;
        shootTime = ayar.Ate�S�resi;
        maxAmmo = ayar.MaksimumMermi;
        ammo = maxAmmo;
        currentAmmo = maxAmmo * 2;
        reloadSuresi = ayar.�arj�rYenilemeS�resi;
        projectileSpawn.transform.localPosition = ayar.MermiSpawn;
        mermiTipi = ayar.mermiTipi;
        silahID = ayar.silahSayi;

        foreach (GameObject s in silahObjeleri)
            s.SetActive(false);
        silahObjeleri[silahID].SetActive(true);
    }

    public void S�f�rla()
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

    public void GamepadAim(InputAction.CallbackContext value)
    {
        gamepadAim = value.ReadValue<Vector2>();
        gamepad = true;
    }
    public void MouseAim(InputAction.CallbackContext value)
    {
        mouseAim = value.ReadValue<Vector2>();
        //mouseAim += new Vector2(-screenX, -screenY);
        gamepad = false;
    }
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
