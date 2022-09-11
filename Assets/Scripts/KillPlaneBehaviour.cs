using UnityEngine;

public class KillPlaneBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    private BallController ballController;

    void Start()
    {
        ballController = ball.GetComponent<BallController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ball"))
        {
            ball.transform.position = ballController.GetBallPosBeforeAiming();
            ballController.Stop();   
        }        
    }
}
