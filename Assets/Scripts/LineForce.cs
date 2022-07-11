using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineForce : MonoBehaviour
{
    [Header("Objects refs")]
    [SerializeField] private Camera mainCamera;
    //Components
    private Rigidbody rbody;
    private LineRenderer lineRenderer;
    

    [Header("Parameters")]
    [SerializeField] private float stopVelocity = .05f;
    [SerializeField] private float shotPower = 150;
    [SerializeField] private Vector3 cameraDistance;

    [Header("Variables (READ ONLY - FOR DEBUGGING)")]
    [SerializeField] private bool bIsIdle;
    [SerializeField] private bool bIsAiming;

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
        // if > 0 is removed bIsIdle is set to true again the first "normal" frame after the fixedupdate
        if (rbody.velocity.magnitude < stopVelocity && rbody.velocity.magnitude > 0)
        {
            Stop();
        }
        ComputeAim();
    }

    //private void LateUpdate()
    //{
    //    mainCamera.transform.position = transform.position + cameraDistance;
    //}

    private void ComputeAim()
    {
        if (!bIsAiming || !bIsIdle)
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
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
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

    private void Stop()
    {
        rbody.velocity = Vector3.zero;
        rbody.angularVelocity = Vector3.zero;
        bIsIdle = true;
    }    

    private void Shoot(Vector3 worldPoint)
    {
        bIsAiming = false;
        lineRenderer.enabled = false;

        bIsIdle = false;

        Vector3 horizontalWorldPoint = new Vector3(worldPoint.x, transform.position.y, worldPoint.z);

        Vector3 direction = (horizontalWorldPoint - transform.position).normalized;
        float strength = Vector3.Distance(transform.position, horizontalWorldPoint);

        rbody.AddForce(shotPower * strength * direction);
        
    }

    private void DrawLine(Vector3 worldPoint)
    {
        Vector3[] positions = {
            transform.position,
            worldPoint
        };
        lineRenderer.SetPositions(positions);
        lineRenderer.enabled = true;
    }

    private void OnMouseDown()
    {
        if (bIsIdle)
        {
            bIsAiming = true;
        }
    }

    private void OnMouseUp()
    {
        if (bIsAiming && bIsIdle)
        {
            Vector3? worldPoint = CastMouseClickRay();

            Shoot(worldPoint.Value);
        }
    }
}
