using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FollowMouse : MonoBehaviour
{
    private Vector3 pos;
    private Ray ray;
    private float depth = 0;
    private float scrollScale = 0.1f;
    private float rayLength = 50;
    private Vector3 spherePos;
    private bool isColliding = false;
    private GameObject selectedObject;
    private bool isDragging = false;
    private float rotatationScale = 0.1f;
    private float scalingScale = 0.001f;

    /*
    NOTE: PLEASE MAKE SURE TO TURN ON THE GIZMOS. OTHERWISE, THE RAY AND THE SPHERE WILL NOT APPEAR.
    */

    void Update()
    {
        pos = Input.mousePosition;
        pos.z = 1;

        transform.position = Camera.main.ScreenToWorldPoint(pos);
        // this.ray = new Ray(transform.position, transform.forward);

        ray = Camera.main.ScreenPointToRay(pos);
        ray.direction = (ray.origin - Camera.main.transform.position).normalized;

        // make ray start from slightly below the camera, so that it's not a single point
        ray.direction = ray.direction + new Vector3(0, 0.5f, 0);

        Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);

        // add mouse scroll delta to depth
        depth += Input.mouseScrollDelta.y * scrollScale;
        depth = Mathf.Clamp(depth, 0.1f, 20f);
        spherePos = ray.origin + ray.direction * depth;

        Debug.Log("Depth: " + depth);

        // check whether the spherePos collides with any cube
        Collider[] colliders = Physics.OverlapSphere(spherePos, 0.05f);
        if (colliders.Length > 0)
        {
            Debug.Log("Collided with: " + colliders[0]);
            isColliding = !(colliders[0].name == "Obstruction");
        }
        else
        {
            isColliding = false;
        }

        // select the collided object by holding down the left mouse button
        // deselect the object by releasing the left mouse button
        // object changes color to red when selected
        if (Input.GetMouseButtonDown(0))
        {
            if (isColliding)
            {
                selectedObject = colliders[0].gameObject;
                selectedObject.GetComponent<Renderer>().material.color = Color.red;
                isDragging = true;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (selectedObject != null)
            {
                selectedObject.GetComponent<Renderer>().material.color = Color.white;
                selectedObject = null;
                isDragging = false;
            }
        }
        // while dragging, the object will follow the mouse and can be rotated
        // using WASD keys around the camera axes
        if (isDragging)
        {
            selectedObject.transform.position = spherePos;
            if (Input.GetKey(KeyCode.W))
            {
                selectedObject.transform.RotateAround(Camera.main.transform.position, Camera.main.transform.right, rotatationScale);
            }
            if (Input.GetKey(KeyCode.S))
            {
                selectedObject.transform.RotateAround(Camera.main.transform.position, -Camera.main.transform.right, rotatationScale);
            }
            if (Input.GetKey(KeyCode.A))
            {
                selectedObject.transform.RotateAround(Camera.main.transform.position, -Camera.main.transform.up, rotatationScale);
            }
            if (Input.GetKey(KeyCode.D))
            {
                selectedObject.transform.RotateAround(Camera.main.transform.position, Camera.main.transform.up, rotatationScale);
            }
            // uniformly scale the object using left control and left shift
            if (Input.GetKey(KeyCode.LeftControl))
            {
                selectedObject.transform.localScale += scalingScale * Vector3.one;
                // limit the scale to 0.1 to 10
                selectedObject.transform.localScale = Vector3.Max(selectedObject.transform.localScale, new Vector3(0.1f, 0.1f, 0.1f));
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                selectedObject.transform.localScale -= scalingScale * Vector3.one;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (isColliding)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(spherePos, 0.05f);
    }
}
