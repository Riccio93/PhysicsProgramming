using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
    [SerializeField] private GameObject[] guidePages;
    private int currentGuidePage;
    [SerializeField] private TextMeshProUGUI pageNumberText;

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
        {
            level2Lock.SetActive(true);
        }
        else
        {
            level2Lock.SetActive(false);
        }

        //Guide section pages initialization
        foreach(GameObject obj in guidePages)
        {
            obj.SetActive(false);
        }
        guidePages[0].SetActive(true);
        currentGuidePage = 0;
        pageNumberText.text = "Page " + (currentGuidePage + 1) + " / " + guidePages.Length;
    }

    public void GoToLevel(int level)
    {
        if(!(level == 2 && PlayerPrefs.GetInt("Level2Unlocked", 0) == 0))
        {
            SceneManager.LoadScene(level);
        }        
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    //Unsure if this is working, check later when audio is implemented (is shown like a string in the
    //playerprefs plugin window)
    public void OnMusicVolumeChanged(System.Single Value) => PlayerPrefs.SetFloat("MusicVolume", Value);
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
