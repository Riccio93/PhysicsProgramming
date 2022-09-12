using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

using GameState = GameManager.GameState;

public class UIBehaviour : MonoBehaviour
{
    [Header("Objects References")]
    [SerializeField] private TextMeshProUGUI triesValue;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject aimingModeImage;
    [SerializeField] private GameObject freeModeImage;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI cubeCount;
    [SerializeField] private GameObject unlockAssets;
    

    private void Start()
    {
        if (gameManager.IsCubeModeActive())
        {
            cubeCount.gameObject.SetActive(true);
            cubeCount.text = gameManager.GetCubeCount().ToString() + " / " + gameManager.GetCubeGoal().ToString();
        }
        else
        {
            cubeCount.gameObject.SetActive(false);
        }
    }

    public void UpdateCubeCountText()
    {
        cubeCount.text = gameManager.GetCubeCount().ToString() + " / " + gameManager.GetCubeGoal().ToString();
    }

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

            //Next level unlocking
            if(SceneManager.GetActiveScene().name == "Level1Scene" && PlayerPrefs.GetInt("Level2Unlocked", 0) == 0)
            {
                unlockAssets.GetComponentInChildren<TextMeshProUGUI>().text = "Level 2 Unlocked!";
                unlockAssets.SetActive(true);
                PlayerPrefs.SetInt("Level2Unlocked", 1);
            }
            else if(SceneManager.GetActiveScene().name == "Level2Scene" && PlayerPrefs.GetInt("Level3Unlocked", 0) == 0)
            {
                unlockAssets.GetComponentInChildren<TextMeshProUGUI>().text = "Level 3 Unlocked!";
                unlockAssets.SetActive(true);
                PlayerPrefs.SetInt("Level3Unlocked", 1);
            }
            else if (SceneManager.GetActiveScene().name == "Level3Scene" && PlayerPrefs.GetInt("Level4Unlocked", 0) == 0)
            {
                unlockAssets.GetComponentInChildren<TextMeshProUGUI>().text = "Level 4 Unlocked!";
                unlockAssets.SetActive(true);
                PlayerPrefs.SetInt("Level4Unlocked", 1);
            }
            else if (SceneManager.GetActiveScene().name == "Level4Scene" && PlayerPrefs.GetInt("Level5Unlocked", 0) == 0)
            {
                unlockAssets.GetComponentInChildren<TextMeshProUGUI>().text = "Level 5 Unlocked!";
                unlockAssets.SetActive(true);
                PlayerPrefs.SetInt("Level5Unlocked", 1);
            }
            else
            {
                unlockAssets.SetActive(false);
            }
        }
        else
        {
            resultText.text = "YOU LOSE...";
            unlockAssets.SetActive(false);
        }
        gameOverScreen.SetActive(true);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
