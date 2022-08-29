using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private Vector3 cameraDistance;
    [SerializeField] private Vector3 cameraRotation;
    [SerializeField] private int numberOfTries = 3;
    [SerializeField] private float freeModeCameraMovement = .2f;

    [Header("Objects References")]
    [SerializeField] private Canvas canvas;
    private UIBehaviour uiBehaviour;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject ball;

    [Header("DEBUG ONLY")]
    [SerializeField] private GameState gameState;

    public enum GameState
    {
        FreeMode,
        Aiming,
        Moving,
        Stopping,
        GameOver
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public void SetGameState(GameState value)
    {
        gameState = value;
    }

    private void Awake()
    {
        uiBehaviour = canvas.GetComponent<UIBehaviour>();

        mainCamera.transform.rotation = Quaternion.Euler(cameraRotation);
    }

    void Start()
    {
        gameState = GameState.Aiming;
        uiBehaviour.SetTriesValue(numberOfTries);
    }

    private void LateUpdate()
    {
        if(GetGameState() != GameState.FreeMode)
            mainCamera.transform.position = ball.transform.position + cameraDistance;
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                mainCamera.transform.position += new Vector3(freeModeCameraMovement, 0f, 0f);
                Debug.Log("Left pressed");
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                mainCamera.transform.position += new Vector3(-freeModeCameraMovement, 0f, 0f);
                Debug.Log("Right pressed");
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                mainCamera.transform.position += new Vector3(0f, 0f, -freeModeCameraMovement);
                Debug.Log("Up pressed");
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                mainCamera.transform.position += new Vector3(0f, 0f, freeModeCameraMovement);
                Debug.Log("Down pressed");
            }
        }        
    }

    public void UpdateTries()
    {
        uiBehaviour.SetTriesValue(--numberOfTries);
        if (numberOfTries >= 1)
        {            
            SetGameState(GameState.Aiming);
        }            
        else
        {
            SetGameState(GameState.GameOver);
            uiBehaviour.ShowGameOverScreen(false);
        }
    }

    public void LevelCompleted()
    {
        uiBehaviour.ShowGameOverScreen(true);
    }
}
