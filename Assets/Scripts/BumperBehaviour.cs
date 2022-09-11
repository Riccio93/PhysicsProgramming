using System.Collections;
using UnityEngine;

public class BumperBehaviour : MonoBehaviour
{
    [Header("Objects References")]
    [SerializeField] private GameObject ball;
    private Rigidbody ballRB;

    [Header("Parameters")]
    [SerializeField] private float bumperForce = 100f;
    private bool bJustBumpered = false;

    private void Start()
    {
        ballRB = ball.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!bJustBumpered)
        {
            if(collision.gameObject == ball)
            {
                //ballRB.AddForce(collision.contacts[0].normal * bumperForce, ForceMode.Impulse);
                //ballRB.AddExplosionForce(bumperForce, transform.position, 1);
                ballRB.AddForce(Vector3.Reflect(new Vector3(ballRB.velocity.x, 0f, ballRB.velocity.z), collision.contacts[0].normal) * bumperForce, 
                    ForceMode.Impulse);
                bJustBumpered = true;
                StartCoroutine(ResetJustBumperedCoroutine());
            }
        }
        
    }

    private IEnumerator ResetJustBumperedCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        bJustBumpered = false;
    }
}
