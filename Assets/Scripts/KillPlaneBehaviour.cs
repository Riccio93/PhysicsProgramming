using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlaneBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    private BallController ballController;
    private Rigidbody ballRB;

    void Start()
    {
        ballController = ball.GetComponent<BallController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        ballController.Stop();
        ball.transform.position = ballController.GetBallPosBeforeAiming();
    }
}
