using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeRaycast : MonoBehaviour
{
    [SerializeField] private GameObject ball;

    // Update is called once per frame
    private void OnMouseDown()
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        RaycastHit[] hits = Physics.RaycastAll(worldMousePosNear, worldMousePosFar - worldMousePosNear,
            float.PositiveInfinity);

        foreach (RaycastHit h in hits)
        {
            if (h.collider.name == "Ball")
            {
                h.collider.gameObject.GetComponent<BallController>().OnMouseDown();
            }
        }
    }

    private void OnMouseUp()
    {
        ball.GetComponent<BallController>().OnMouseUp();
    }
}
