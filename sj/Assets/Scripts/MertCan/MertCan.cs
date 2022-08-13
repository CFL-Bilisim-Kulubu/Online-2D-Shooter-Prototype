using UnityEngine;
public class MertCan
{
    // Input Field i�in ideal
    // Girilen stringin sonunda �ift bo�luk varsa birini silerek geri d�nd�r�r.
    // Girilen string sadece bo�luksa bo� string geri d�nd�r�r.
    public static string Anti�iftBo�luk(string value)
    {
        if (value.Length > 1)
        {
            if (value[value.Length - 1] == ' ' && value[value.Length - 2] == ' ') // son iki karakter bo�luksa
            {
                return value.Substring(0, value.Length - 1);
            }
        }
        else if (value == " ")
            return "";

        return value;
    }
    public static class Rect
    {
        public static RectTransform De�i�tir(RectTransform rt, float yatay, float dikey)
        {
            rt = SolDe�i�tir(rt, yatay); rt = Sa�De�i�tir(rt, yatay); rt = �stDe�i�tir(rt, dikey); rt = AltDe�i�tir(rt, dikey);
            return rt;
        }
        public static RectTransform De�i�tir(RectTransform rt, float sol, float sa�, float �st, float alt)
        {
            rt = SolDe�i�tir(rt, sol); rt = Sa�De�i�tir(rt, sa�); rt = �stDe�i�tir(rt, �st); rt = AltDe�i�tir(rt, alt);
            return rt;
        }
        public static RectTransform SolDe�i�tir(RectTransform rt, float sol)
        {
            rt.offsetMin = new Vector2(sol, rt.offsetMin.y);
            return rt;
        }
        public static RectTransform Sa�De�i�tir(RectTransform rt, float sa�)
        {
            rt.offsetMax = new Vector2(-sa�, rt.offsetMax.y);
            return rt;
        }
        public static RectTransform �stDe�i�tir(RectTransform rt, float �st)
        {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -�st);
            return rt;
        }
        public static RectTransform AltDe�i�tir(RectTransform rt, float alt)
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, alt);
            return rt;
        }
    }
}