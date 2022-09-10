using UnityEngine;

public class BumperBehaviour : MonoBehaviour
{
    [Header("Objects References")]
    [SerializeField] private GameObject ball;
    private Rigidbody ballRB;

    [Header("Parameters")]
    [SerializeField] private float bumperForce = 125f;

    private void Start()
    {
        ballRB = ball.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == ball)
        {
            //ballRB.AddForce(collision.contacts[0].normal * bumperForce, ForceMode.Impulse);
            ballRB.AddExplosionForce(bumperForce, transform.position, 1);
            //ballRB.AddForce(Vector3.Reflect(ballRB.velocity, collision.contacts[0].normal) * bumperForce, 
                //ForceMode.Impulse);
            Debug.Log("Ho bumpato la palla");
        }
    }
}
