using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Input : MonoBehaviour
{
    public static Button_Input instance;
    public GameObject[] rotateCanvas;
    public GameObject moveCanvas;

    GameObject activeBlock;
    Block active;

    bool moveIsOn = true;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    void RepositionToActiveBlock()
    {
        if (activeBlock != null)
        {
            transform.position = activeBlock.transform.position;
        }
    }

    public void SetActiveBlock(GameObject block, Block block1)
    {
        activeBlock = block;
        active = block1; //Script
    }
    // Update is called once per frame
    void Update()
    {
        RepositionToActiveBlock();
        if (Input.GetKeyDown(KeyCode.Return)|| Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SetHighSpeed();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchInput();
        }
    }
    public void MoveBlock(string direction)
    {
        if(activeBlock != null)
        {
            if(direction == "left")
            {
                active.SetInput(Vector3.left);
            }
            if (direction == "right")
            {
                active.SetInput(Vector3.right);
            }
            if (direction == "forward")
            {
                active.SetInput(Vector3.forward);
            }
            if (direction == "back")
            {
                active.SetInput(Vector3.back);
            }
        }
    }

    public void RotateBlock(string rotation)
    {
        if(activeBlock != null)
        {
            //X rotation
            if(rotation == "X+")
            {
                active.SetRotation(new Vector3(90,0,0));
            }
            if (rotation == "X-")
            {
                active.SetRotation(new Vector3(-90, 0, 0));
            }
            //Y rotation
            if (rotation == "Y+")
            {
                active.SetRotation(new Vector3(0,90, 0));
            }
            if (rotation == "Y-")
            {
                active.SetRotation(new Vector3(0, -90, 0));
            }
            //Z rotation
            if (rotation == "Z+")
            {
                active.SetRotation(new Vector3(0, 0, 90));
            }
            if (rotation == "Z-")
            {
                active.SetRotation(new Vector3(0, 0, -90));
            }
        }
    }

    public void SwitchInput()
    {
        moveIsOn = !moveIsOn;
        moveCanvas.SetActive(moveIsOn);
        foreach (GameObject c in rotateCanvas)
        {
            c.SetActive(!moveIsOn);
        }
    }

    public void SetHighSpeed()
    {
        if(activeBlock!= null)
        {
            active.SetSpeed();
        }
    }
}
