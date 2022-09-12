using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private Vector3 cameraDistance;
    [SerializeField] private Vector3 cameraRotation;
    [SerializeField] private int numberOfTries = 3;
    [SerializeField] private float freeModeCameraMovement = .2f;
    [SerializeField] private bool cubeMode = false;
    [SerializeField] private int cubeGoal = 0;
    private int cubeCount = 0;
    private GameState gameState;
    public int GetCubeGoal()
    {
        return cubeGoal;
    }
    public int GetCubeCount()
    {
        return cubeCount;
    }
    public void IncrementCubeCount()
    {
        cubeCount++;
        uiBehaviour.UpdateCubeCountText();
    }
    public bool IsCubeModeActive()
    {
        return cubeMode;
    }

    [Header("Objects References")]
    [SerializeField] private Canvas canvas;
    private UIBehaviour uiBehaviour;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject ball;    

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

    private void OnMouseDown()
    {
        Debug.Log("LOOOOOOOOOOOOOOOOL");
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
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                mainCamera.transform.position += new Vector3(-freeModeCameraMovement, 0f, 0f);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                mainCamera.transform.position += new Vector3(0f, 0f, -freeModeCameraMovement);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                mainCamera.transform.position += new Vector3(0f, 0f, freeModeCameraMovement);
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

    public void CheckCubeModeWin()
    {
        if(cubeMode && cubeCount >= cubeGoal)
        {
            LevelCompleted();
        }
    }

    public void LevelCompleted()
    {
        uiBehaviour.ShowGameOverScreen(true);
    }
}
