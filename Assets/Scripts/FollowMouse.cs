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

    }
}
