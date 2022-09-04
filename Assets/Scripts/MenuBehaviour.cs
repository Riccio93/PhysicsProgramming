using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        if(PlayerPrefs.GetInt("Level2Unlocked", 0) == 0)
        {
            level2Lock.SetActive(true);
        }
        else
        {
            level2Lock.SetActive(false);
        }
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
}
