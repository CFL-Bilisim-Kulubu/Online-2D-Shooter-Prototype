using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSave : MonoBehaviour
{
    public void SaveGun(int gun)
    {
        PlayerPrefs.SetInt("GUN",gun);
    }
}
