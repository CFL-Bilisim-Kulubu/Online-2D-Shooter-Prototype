using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;


public class GrafikAyarları : MonoBehaviour
{
    
    public string High, Low, On, Off;
    [SerializeField] private int[] fpsLimitleri;
    [SerializeField] private TMPro.TMP_Text settingText,fpsLmitText,bloomText,screenText;
    [SerializeField] private UnityEngine.UI.Slider fpsLimitSlider;
    private int setting = 0, fpsLimit = 2;
    private bool bloom = true,screen = true;
    [SerializeField] private VolumeProfile[] assets;
    public void ChangeSetting()
    {
        setting++;
        setting = setting > 1 ? 0 : setting;
        QualitySettings.SetQualityLevel(setting);
        settingText.text = (int)QualitySettings.GetQualityLevel() == 0 ? High : Low;
        PlayerPrefs.SetInt("SETTINGS_QUALITY", setting);
    }
    public void ChangeFPSLimit()
    {
        fpsLimit = (int)fpsLimitSlider.value;
        Application.targetFrameRate = fpsLimitleri[fpsLimit];
        fpsLmitText.text = fpsLimitleri[fpsLimit].ToString();
        PlayerPrefs.SetInt("FPS_LIMIT", fpsLimit);
    }
    private void Awake()
    {
        try
        {
            fpsLimit = PlayerPrefs.GetInt("FPS_LIMIT");
            setting = PlayerPrefs.GetInt("SETTINGS_QUALITY");
            bloom = PlayerPrefs.GetInt("BLOOM") == 1 ? true : false;
            screen = PlayerPrefs.GetInt("SCREEN") == 1 ? true : false;
        }
        catch (PlayerPrefsException playerPrefsException)
        {
            Debug.Log(" DIKKAT AYARLAR OKUNAMADI, OKUNABİLENLER KULLANCAK; OKUNAMAYANLARDAN VARSAYILAN AYARLAR KULLANILACAK \n==========\nHATA: " + playerPrefsException + "\n==========\n");
        } //ayarlar çekiliyor 
        QualitySettings.SetQualityLevel(setting);
        Application.targetFrameRate = fpsLimitleri[fpsLimit];
        if (screen)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.Windowed);
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        foreach (VolumeProfile v in assets)
        {
            if (v.TryGet(out Bloom bl))
            {
                bl.active = bloom;
            }
        }
        //
        screenText.text = screen == true ? On : Off;
        bloomText.text = bloom == true ? On : Off;
        fpsLmitText.text = fpsLimitleri[fpsLimit].ToString();
        settingText.text = (int)QualitySettings.GetQualityLevel() == 0 ? High : Low;
    }
    public void ChangeBloom()
    {
        bloom = !bloom;
        bloomText.text = bloom == true ? On : Off;
        foreach(VolumeProfile v in assets)
                {
            if (v.TryGet(out Bloom bl))
            {
                bl.active = bloom;
            }
        }
        PlayerPrefs.SetInt("BLOOM", bloom == true ? 1 : 0);
    }
    public void ChangeFullscreen()
    {
        screen = !screen;
        screenText.text = screen == true ? On : Off;
        if (screen)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height,FullScreenMode.Windowed);
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        PlayerPrefs.SetInt("SCREEN", screen == true ? 1 : 0);
    }

}
