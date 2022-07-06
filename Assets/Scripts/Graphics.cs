using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graphics : MonoBehaviour
{
    int graphics_Num, resolution_Num, exhibition_Num = 1;
    [Space]
    [SerializeField] Text graficsButton, resolutionButton, exhibitionButton;
    [Space]
    [SerializeField] string[] graphics, resolution;
    [Space]
    [SerializeField] int[] resolution_Widht, resolution_Height;
    [SerializeField] GameObject graphics_GameObject;
    bool open;
    bool isReady;
    // Start is called before the first frame update
    void Start()
    {
        graphics_Num = PlayerPrefs.GetInt("graphics", 0);
        resolution_Num = PlayerPrefs.GetInt("resolution", 0);
        exhibition_Num =  PlayerPrefs.GetInt("exhibition", 1);
        QualitySettings.SetQualityLevel(graphics_Num, true);
    }
    public void GraphicsNext()
    {
        graphics_Num++;
        if (graphics_Num > 5)
        {
            graphics_Num = 0;
        }
        QualitySettings.SetQualityLevel(graphics_Num, true);
        graficsButton.text = graphics[graphics_Num];
        PlayerPrefs.SetInt("graphics", graphics_Num);
    }
    private void Update()
    {
        SetExibition();
    }
    public void GraphicsBack()
    {
        graphics_Num--;
        if (graphics_Num < 0)
        {
            graphics_Num = 5;
        }
        QualitySettings.SetQualityLevel(graphics_Num, true);
        graficsButton.text = graphics[graphics_Num];
        PlayerPrefs.SetInt("graphics", graphics_Num);
    }
    public void ResolutionNext()
    {
        resolution_Num++;
        if (resolution_Num > 5)
        {
            resolution_Num = 0;
        }
        for (int i = 0; i < resolution_Height.Length; i++)
        {
            Screen.SetResolution(resolution_Widht[i], resolution_Height[i], true);
        }
        resolutionButton.text = resolution[resolution_Num];
        PlayerPrefs.SetInt("resolution", resolution_Num);
    }
    public void ResolutionBack()
    {
        resolution_Num--;
        if (resolution_Num < 0)
        {
            resolution_Num = 5;
        }
        for (int i = 0; i < resolution_Height.Length; i++)
        {
            Screen.SetResolution(resolution_Widht[i], resolution_Height[i], true);
        }
        resolutionButton.text = resolution[resolution_Num];
        PlayerPrefs.SetInt("resolution", resolution_Num);
    }

    public void exhibitionNext()
    {
        exhibition_Num++;
        PlayerPrefs.SetInt("exhibition", exhibition_Num);
    }
    public void exhibitionBack()
    {
        exhibition_Num++;
        PlayerPrefs.SetInt("exhibition", exhibition_Num);
    }
    void SetExibition()
    {
        switch (exhibition_Num)
        {
            case -1:
                exhibition_Num = 1;
                break;
            case 0:
                isReady = true;
                exhibitionButton.text = "FullScreen";
                break;
            case 1:
                isReady = false;
                exhibitionButton.text = "Windowed";
                break;
            default:
                exhibition_Num = 0;
                break;
        }
        Screen.fullScreen = isReady;
    }
    public void Open_Graphics()
    {
        open = !open;
        resolutionButton.text = resolution[resolution_Num];
        graficsButton.text = graphics[graphics_Num];
        graphics_GameObject.SetActive(open);
    }
}
