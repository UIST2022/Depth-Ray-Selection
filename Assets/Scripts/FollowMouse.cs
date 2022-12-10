using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    void Update()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = 1;

        transform.position = Camera.main.ScreenToWorldPoint(pos);

        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

    }
}
