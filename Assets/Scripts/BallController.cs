using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Vector3 cameraDistance;

    [Header("UI")]
    [SerializeField] private Slider horizontalSlider;
    [SerializeField] private Text horizontalSliderValueText;
    [SerializeField] private Slider verticalSlider;
    [SerializeField] private Text verticalSliderValueText;
    [SerializeField] private Slider windSlider;
    [SerializeField] private Text windSliderValueText;

    //[Header("Impulse")]
    //[SerializeField] private float horizontalMinValue;
    //[SerializeField] private float horizontalMaxValue;
    //[SerializeField] private float verticalMinValue;
    //[SerializeField] private float verticalMaxValue;
    //[SerializeField] private float windMinValue;
    //[SerializeField] private float windMaxValue;

    [Space]

    private float directionValue;
    private float impulseValue;
    private float windValue;

    private Rigidbody ballRB;
    public Vector3 impulseVersor;
    public bool bIsBallOnGround;

    private void Start()
    {
        ballRB = GetComponent<Rigidbody>();
        InitSliders();
    }

    private void InitSliders()
    {
        horizontalSlider.minValue = -Mathf.PI / 4;
        horizontalSlider.maxValue = Mathf.PI / 4;
        verticalSlider.minValue = 100;
        verticalSlider.maxValue = 1000;
        windSlider.minValue = -200;
        windSlider.maxValue = 200;

        UpdateHorizontalSlider();
        UpdateVerticalSlider();
        UpdateWindSlider();
    }

    public void AddImpulse()
    {
        impulseVersor = new Vector3(Mathf.Cos(directionValue + (Mathf.PI/2)), .5f, Mathf.Sin(directionValue + (Mathf.PI/2))).normalized;
        if (ballRB)
            Debug.Log("RB set");
        else
            Debug.Log("RB not set");
        ballRB.AddRelativeForce(impulseVersor * impulseValue, ForceMode.Impulse);
        ballRB.AddRelativeTorque((impulseVersor + new Vector3(90, 0, 0)) * impulseValue, ForceMode.Impulse);
        StartCoroutine(ApplyWindForceCoroutine());
    }

    public IEnumerator ApplyWindForceCoroutine()
    {
        do
        {
            ballRB.AddForce(windValue * Vector3.right, ForceMode.Impulse);
            ballRB.AddTorque(windValue * Vector3.right, ForceMode.Impulse);
            Debug.Log("Wind");
            yield return new WaitForSeconds(.2f);
        } while (!bIsBallOnGround);
        yield return null;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            bIsBallOnGround = true;
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            bIsBallOnGround = false;
        }
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Makes the camera follow the ball
    private void LateUpdate()
    {
        mainCamera.transform.position = transform.position + cameraDistance;
    }

    //Debug sliders update
    public void UpdateHorizontalSlider()
    {
        directionValue = horizontalSlider.value;
        horizontalSliderValueText.text = directionValue.ToString();
    }
    public void UpdateVerticalSlider()
    {
        impulseValue = verticalSlider.value;
        verticalSliderValueText.text = impulseValue.ToString();
    }
    public void UpdateWindSlider()
    {
        windValue = windSlider.value;
        windSliderValueText.text = windValue.ToString();
    }
}
