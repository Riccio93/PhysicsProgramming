using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuBehaviour : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float cameraTurnTime;
    [SerializeField] private Quaternion mainRotation;
    [SerializeField] private Quaternion levelRotation;
    [SerializeField] private Quaternion optionsRotation;
    [SerializeField] private Quaternion guideRotation;

    [Header("Objects references")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject eventSystem;
    [SerializeField] private GameObject level2Lock;
    [SerializeField] private GameObject level3Lock;
    [SerializeField] private GameObject level4Lock;
    [SerializeField] private GameObject level5Lock;
    [SerializeField] private GameObject[] guidePages;
    private int currentGuidePage;
    [SerializeField] private TextMeshProUGUI pageNumberText;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private AudioSource ostSource;

    public void GoToLevelSelection() => StartCoroutine(RotateCameraCoroutine(mainRotation, levelRotation));
    public void GoToMainMenuFromLevelSelection() => StartCoroutine(RotateCameraCoroutine(levelRotation, mainRotation));
    public void GoToOptions() => StartCoroutine(RotateCameraCoroutine(mainRotation, optionsRotation));
    public void GoToMainMenuFromOptions() => StartCoroutine(RotateCameraCoroutine(optionsRotation, mainRotation));
    public void GoToGuide() => StartCoroutine(RotateCameraCoroutine(mainRotation, guideRotation));
    public void GoToMainMenuFromGuide() => StartCoroutine(RotateCameraCoroutine(guideRotation, mainRotation));

    private IEnumerator RotateCameraCoroutine(Quaternion start, Quaternion end)
    {
        float time = 0f;
        eventSystem.SetActive(false);
        while(time < cameraTurnTime)
        {
            time += Time.deltaTime;
            mainCamera.transform.rotation = Quaternion.Slerp(start, end, time);
            yield return null;
        }
        eventSystem.SetActive(true);
    }

    public void Start()
    {
        //Shows locks if levels are locked in the selection screen
        if(PlayerPrefs.GetInt("Level2Unlocked", 0) == 0)
            level2Lock.SetActive(true);
        else
            level2Lock.SetActive(false);

        if (PlayerPrefs.GetInt("Level3Unlocked", 0) == 0)
            level3Lock.SetActive(true);
        else
            level3Lock.SetActive(false);

        if (PlayerPrefs.GetInt("Level4Unlocked", 0) == 0)
            level4Lock.SetActive(true);
        else
            level4Lock.SetActive(false);

        if (PlayerPrefs.GetInt("Level5Unlocked", 0) == 0)
            level5Lock.SetActive(true);
        else
            level5Lock.SetActive(false);

        //Guide section pages initialization
        foreach (GameObject obj in guidePages)
        {
            obj.SetActive(false);
        }
        guidePages[0].SetActive(true);
        currentGuidePage = 0;
        pageNumberText.text = "Page " + (currentGuidePage + 1) + " / " + guidePages.Length;

        //Sound sliders initialization
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1);
    }

    public void GoToLevel(int level)
    {
        switch(level)
        {
            case 2:
                if(PlayerPrefs.HasKey("Level2Unlocked"))
                    SceneManager.LoadScene(2);
                break;
            case 3:
                if (PlayerPrefs.HasKey("Level3Unlocked"))
                    SceneManager.LoadScene(3);
                break;
            case 4:
                if (PlayerPrefs.HasKey("Level4Unlocked"))
                    SceneManager.LoadScene(4);
                break;
            case 5:
                if (PlayerPrefs.HasKey("Level5Unlocked"))
                    SceneManager.LoadScene(5);
                break;
            default:
                SceneManager.LoadScene(level);
                break;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void OnMusicVolumeChanged(System.Single Value)
    {
        PlayerPrefs.SetFloat("MusicVolume", Value);
        ostSource.volume = Value;
    }
        
    public void OnSFXVolumeChanged(System.Single Value) => PlayerPrefs.SetFloat("SFXVolume", Value);

    public void ChangePage(int offset)
    {
        if (currentGuidePage + offset >= 0 && currentGuidePage + offset < guidePages.Length)
        {
            guidePages[currentGuidePage].SetActive(false);
            guidePages[currentGuidePage += offset].SetActive(true);
            pageNumberText.text = "Page " + (currentGuidePage + 1) + " / " + guidePages.Length;
        }
    }
}
