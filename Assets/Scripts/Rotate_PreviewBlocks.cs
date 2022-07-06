using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_PreviewBlocks : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, speed * Time.deltaTime);
    }
}
