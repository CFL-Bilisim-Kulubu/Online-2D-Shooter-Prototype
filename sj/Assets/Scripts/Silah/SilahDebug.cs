using UnityEngine.InputSystem;
using UnityEngine;

public class SilahDebug : MonoBehaviour
{
    [SerializeField] private Senkranizasyon senkronizasyon;
    [SerializeField] private bool aktif;
    [SerializeField] private SilahAyarlar�[] ayar;
    [SerializeField] private Silah silah;
    [SerializeField] private int a;

    public void Degisme(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            a++;
            if (a >= ayar.Length)
                a = 0;
            silah.ayar = ayar[a];
            silah.Ayarla();
            //silah.S�f�rla();
            senkronizasyon.SilahSenkranizasyon();
        }
    }

}
