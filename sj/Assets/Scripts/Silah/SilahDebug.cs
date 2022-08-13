using UnityEngine.InputSystem;
using UnityEngine;

public class SilahDebug : MonoBehaviour
{
    [SerializeField] private Senkranizasyon senkronizasyon;
    [SerializeField] private bool aktif;
    public SilahAyarlar�[] ayar;
    [SerializeField] private Silah silah;
    [SerializeField] private int a;
    public void AktiveEt()
    {
        aktif = true;
    }
    public void DeAktiveEt()
    {
        aktif = false;
    }
    public void Degisme(InputAction.CallbackContext ctx)
    {
        if(ctx.performed && aktif)
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
