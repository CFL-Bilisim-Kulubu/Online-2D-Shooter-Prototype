using UnityEngine;

[CreateAssetMenu(fileName = "YeniSilah", menuName = "AyarObjeleri/SilahAyarları", order = 1)]
public class SilahAyarları : ScriptableObject
{

    public GameObject mermi;
    public Vector3 MermiSpawn = new Vector3(0.3f,0.34f,0);
    public float MaksimumMermi, AteşSüresi, ŞarjörYenilemeSüresi;
    
    [Tooltip("Mermi 1 normal mermi \nMermi 2 patlayıcı mermi \nMermi 3 lazer mermi\nMermi 4 içinden patlayıcı çıkan mermi")]
    public int mermiTipi = 1,mermiSayısı = 1;

    public int silahSayi;
}