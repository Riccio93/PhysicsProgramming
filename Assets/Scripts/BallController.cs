using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameState = GameManager.GameState;

public class BallController : MonoBehaviour
{
    [Header("Parameters")]
    //Stop if the ball's velocity is lower than this value
    [SerializeField] private float stopVelocity = .1f;
    //Value to multiply for the line length to obtain the resulting force
    [SerializeField] private float shotPower = 150f;

    [Header("Objects References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject raycastPlane;

    //Components
    private Rigidbody rbody;
    private LineRenderer lineRenderer;

    private Vector3 ballPosBeforeAiming;
    public Vector3 GetBallPosBeforeAiming()
    {
        return ballPosBeforeAiming;
    }

    //[Header("DEBUG ONLY")]
    /*[SerializeField]*/
    private bool bIsIdle;
    /*[SerializeField]*/ private bool bIsAiming;

    #region Unity Functions

    private void Awake()
    {
        rbody = GetComponent<Rigidbody>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
        lineRenderer.enabled = false;
        bIsIdle = true;
        bIsAiming = false;
    }

    private void FixedUpdate()
    {
        // if > 0 is removed bIsIdle is set to true again the first non-physics frame after the fixedupdate
        if (rbody.velocity.magnitude < stopVelocity && rbody.velocity.magnitude > 0)
        {
            Stop();
        }
        ComputeAim();
    }

    #endregion

    #region Aiming

    private void ComputeAim()
    {
        if (gameManager.GetGameState()!= GameState.Aiming || !bIsAiming || !bIsIdle)
            return;

        Vector3? worldPoint = CastMouseClickRay();
        if (!worldPoint.HasValue)
        {
            return;
        }
        DrawLine(worldPoint.Value);
    }

    private Vector3? CastMouseClickRay()
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        if (Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out RaycastHit hit, float.PositiveInfinity))
        {
            return hit.point;
        }
        else
        {
            return null;
        }
    }

    private void DrawLine(Vector3 worldPoint)
    {
        Vector3[] positions = { transform.position, worldPoint };
        lineRenderer.SetPositions(positions);
        lineRenderer.enabled = true;
    }

    private void OnMouseDown()
    {
        if (gameManager.GetGameState() == GameState.Aiming && bIsIdle)
        {
            bIsAiming = true;
            ballPosBeforeAiming = transform.position;
        }
    }

    private void OnMouseUp()
    {
        if (gameManager.GetGameState() == GameState.Aiming && bIsAiming && bIsIdle)
        {
            Vector3? worldPoint = CastMouseClickRay();
            Shoot(worldPoint.Value);
        }
    }

    #endregion

    #region Shot

    private void Shoot(Vector3 worldPoint)
    {
        bIsAiming = false;
        lineRenderer.enabled = false;
        bIsIdle = false;

        Vector3 horizontalWorldPoint = new Vector3(worldPoint.x, transform.position.y, worldPoint.z);
        Vector3 direction = (horizontalWorldPoint - transform.position).normalized;
        float strength = Vector3.Distance(transform.position, horizontalWorldPoint);
        //the minus reverses the direction of the force
        rbody.AddForce(-shotPower * strength * direction);
        //rbody.AddTorque(-shotPower * strength * direction);

        gameManager.SetGameState(GameState.Moving);
    }

    public void Stop()
    {
        rbody.velocity = Vector3.zero;
        rbody.angularVelocity = Vector3.zero;
        bIsIdle = true;

        if (gameManager.GetGameState() == GameState.Moving)
        {
            gameManager.SetGameState(GameState.Stopping);
            gameManager.UpdateTries();
        }

        UpdateRaycastPlanePosition();
    }

    private void UpdateRaycastPlanePosition()
    {
        //Moves the raycast plane under the ball, so that the raycast is always possible.
        raycastPlane.transform.position = transform.position + new Vector3(0, -1, 0);
    }

    #endregion
}
