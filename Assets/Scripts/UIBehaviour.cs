using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using GameState = GameManager.GameState;

public class UIBehaviour : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private TextMeshProUGUI triesValue;
    [Header("Objects References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject aimingModeImage;
    [SerializeField] private GameObject freeModeImage;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI unlockText;


    public void SetTriesValue(int value)
    {
        triesValue.text = value.ToString();
    }

    public void ToggleMode()
    {
        GameState newGameState = gameManager.GetGameState() == GameState.Aiming ? GameState.FreeMode : GameState.Aiming;
        gameManager.SetGameState(newGameState);
        switch(newGameState)
        {
            case GameState.Aiming:
                aimingModeImage.SetActive(true);
                freeModeImage.SetActive(false);
                break;
            case GameState.FreeMode:
                aimingModeImage.SetActive(false);
                freeModeImage.SetActive(true);
                break;
            default:
                Debug.Log("ERROR: INVALID GAME STATE");
                aimingModeImage.SetActive(false);
                freeModeImage.SetActive(false);
                break;
        }        
    }

    public void ShowGameOverScreen(bool result) //result = has the player won?
    {
        Time.timeScale = 0f;
        if (result)
        {
            resultText.text = "YOU WIN!";

            //Next levels unlocking
            if(SceneManager.GetActiveScene().name == "Level1Scene" && PlayerPrefs.GetInt("Level2Unlocked", 0) == 0)
            {
                unlockText.text = "Level 2 Unlocked!";
                unlockText.gameObject.SetActive(true);
                PlayerPrefs.SetInt("Level2Unlocked", 1);
            }
            else if(SceneManager.GetActiveScene().name == "Level12Scene" && PlayerPrefs.GetInt("Level3Unlocked", 0) == 0)
            {
                unlockText.text = "Level 3 Unlocked!";
                unlockText.gameObject.SetActive(true);
                PlayerPrefs.SetInt("Level3Unlocked", 1);
            }
        }
        else
        {
            resultText.text = "YOU LOSE...";
            unlockText.gameObject.SetActive(false);
        }
        gameOverScreen.SetActive(true);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
