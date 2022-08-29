using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTriggerBehaviour : MonoBehaviour
{
    [Header("Objects References")]
    [SerializeField] private GameObject ball;
    [SerializeField] private GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other == ball.GetComponent<SphereCollider>())
        {
            gameManager.LevelCompleted();
        }
    }
}
