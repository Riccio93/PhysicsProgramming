using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCubeBehaviour : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float destoyCubeWaitTime = 1.5f;
    private bool bIsDestroyed = false;

    [Header("Objects References")]
    [SerializeField] private GameManager gameManager;    

    private void OnCollisionEnter(Collision collision)
    {
        //Floor and walls have "Floor" Tag
        if (collision.collider.gameObject.CompareTag("Floor") && !bIsDestroyed)
        {
            gameManager.IncrementCubeCount();
            bIsDestroyed = true;
            //Destroy the cube some time after it touches the floor
            StartCoroutine(DestroyCubeCoroutine());
        }
    }

    private IEnumerator DestroyCubeCoroutine()
    {
        yield return new WaitForSeconds(destoyCubeWaitTime);
        gameManager.CheckCubeModeWin();
        Destroy(gameObject);
    }

    //We have just one trigger, the kill plane
    private void OnTriggerEnter(Collider other)
    {
        gameManager.IncrementCubeCount();
        gameManager.CheckCubeModeWin();
        Destroy(gameObject);
    }

}
