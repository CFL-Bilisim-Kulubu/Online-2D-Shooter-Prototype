using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Bolt;

public class Silah : MonoBehaviour
{
    [SerializeField] private Senkranizasyon s;
    [SerializeField] private TMP_Text t;
    public Conttrollrer c;
    private float aim,shootT,reloadT;
    private Vector2 gamepadAim, mouseAim;
    private bool gamepad,shoot,tutunma,reload;
    public Camera kamera;
    public GameObject projectile,projectileSpawn,aimParent;
    [Header("Silah Ayaarlarý")]
    [Space]
    public float shootTime,maxAmmo,ammo,reloadSuresi;

    private void Awake()
    {
        reload = false;
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
                ammo = maxAmmo;
                reload = false;
                reloadT = 0;
            }
        }
        else
        {
            t.text = "Mermi : " + ammo + " / " + maxAmmo;
        }
        if (shootT > shootTime && shoot && !c.tutunma && !s.spawnProtection && ammo > 0 && !reload)
        {
            shootT = shootT - shootTime > shootTime ? 0 : shootT - shootTime;
            GameObject a =
            BoltNetwork.Instantiate(projectile, projectileSpawn.transform.position, projectileSpawn.transform.rotation);
            a.GetComponent<ProjectileNormal>().s = s;
            a.GetComponent<ProjectileNormal>().Takým();
            ammo--;
        }
        else if(ammo <= 0)
        {
            Reload();
        }

        

        if (gamepad)
        {
            aim = Mathf.Rad2Deg * Mathf.Atan2(gamepadAim.y, gamepadAim.x);
        }
        else
        {
            aim = Mathf.Rad2Deg * Mathf.Atan2(
                mouseAim.y - kamera.WorldToScreenPoint(transform.position).y,
                mouseAim.x - kamera.WorldToScreenPoint(transform.position).x);
        }
        aimParent.transform.rotation = Quaternion.Euler(0, 0, aim);
    }
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
    public void Reload()
    {
        if (!reload)
        {
            reload = true;
        }
    }
}
