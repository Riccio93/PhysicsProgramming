using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private Vector3 cameraDistance;
    [SerializeField] private Vector3 cameraRotation;
    private int numberOfTries = 3;

    [Header("Objects references")]
    [SerializeField] private Canvas canvas;
    private UIBehaviour uiBehaviour;
    [SerializeField] private Camera mainCamera;

    [Header("DEBUG ONLY")]
    [SerializeField] private GameState gameState;

    public enum GameState
    {
        Panoramic,
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
        gameState = GameState.Panoramic;
        //ShowLevel();
        uiBehaviour.SetTriesValue(numberOfTries);
    }

    private void LateUpdate()
    {
        mainCamera.transform.position = transform.position + cameraDistance;
    }

    void ShowLevel()
    {

    }

    public void UpdateTries()
    {
        uiBehaviour.SetTriesValue(--numberOfTries);
        if (numberOfTries >= 2)
        {            
            SetGameState(GameState.Aiming);
        }            
        else
        {
            SetGameState(GameState.GameOver);
        }
    }
}
