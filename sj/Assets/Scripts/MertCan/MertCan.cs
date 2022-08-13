using UnityEngine;
public class MertCan
{
    // Input Field için ideal
    // Girilen stringin sonunda çift boþluk varsa birini silerek geri döndürür.
    // Girilen string sadece boþluksa boþ string geri döndürür.
    public static string AntiÇiftBoþluk(string value)
    {
        if (value.Length > 1)
        {
            if (value[value.Length - 1] == ' ' && value[value.Length - 2] == ' ') // son iki karakter boþluksa
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
        public static RectTransform Deðiþtir(RectTransform rt, float yatay, float dikey)
        {
            rt = SolDeðiþtir(rt, yatay); rt = SaðDeðiþtir(rt, yatay); rt = ÜstDeðiþtir(rt, dikey); rt = AltDeðiþtir(rt, dikey);
            return rt;
        }
        public static RectTransform Deðiþtir(RectTransform rt, float sol, float sað, float üst, float alt)
        {
            rt = SolDeðiþtir(rt, sol); rt = SaðDeðiþtir(rt, sað); rt = ÜstDeðiþtir(rt, üst); rt = AltDeðiþtir(rt, alt);
            return rt;
        }
        public static RectTransform SolDeðiþtir(RectTransform rt, float sol)
        {
            rt.offsetMin = new Vector2(sol, rt.offsetMin.y);
            return rt;
        }
        public static RectTransform SaðDeðiþtir(RectTransform rt, float sað)
        {
            rt.offsetMax = new Vector2(-sað, rt.offsetMax.y);
            return rt;
        }
        public static RectTransform ÜstDeðiþtir(RectTransform rt, float üst)
        {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -üst);
            return rt;
        }
        public static RectTransform AltDeðiþtir(RectTransform rt, float alt)
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, alt);
            return rt;
        }
    }
}