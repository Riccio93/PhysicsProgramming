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
        ballRB = ball.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        ballRB.velocity = Vector3.zero;
        ball.transform.position = ballController.GetBallPosBeforeAiming();
    }
}
