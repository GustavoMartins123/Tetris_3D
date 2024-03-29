using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_Controll : MonoBehaviour
{
    Transform target;
    Transform rotTarget;
    Vector3 lastPosition;
    float sensitivity = 0.25f;

    // Start is called before the first frame update
    void Awake()
    {
        rotTarget = transform.parent;
        target = rotTarget.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
        if (Input.GetMouseButtonDown(0))
        {
            lastPosition = Input.mousePosition;
        }
        Orbit();
    }

    void Orbit()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastPosition;
            float angleY = -delta.y * sensitivity;
            float angleX = delta.x * sensitivity;
            //X Axis
            Vector3 angles = rotTarget.transform.eulerAngles;
            angles.x += angleY;
            angles.x = ClampAngle(angles.x, -85f, 85f);
            rotTarget.transform.eulerAngles = angles;
            //Y Axis
            target.RotateAround(target.position, Vector3.up, angleX);
            lastPosition = Input.mousePosition;
        }
    }

    float ClampAngle(float angle, float from, float to)
    {
        if(angle < 0)
        {
            angle = 360 + angle;
        }
        if(angle > 180f)
        {
            return Mathf.Max(angle, 360 + from);
        }
        return Mathf.Min(angle, to);

    }
}
