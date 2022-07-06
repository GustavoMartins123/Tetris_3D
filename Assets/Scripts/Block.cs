using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    float prevTime;
    float fallTime = 3f;
    // Start is called before the first frame update
    void Start()
    {
        Button_Input.instance.SetActiveBlock(gameObject, this);
        fallTime = GameManager.instance.ReadFallSpeed();
        if (!CheckValidate())
        {
            GameManager.instance.SetGameOver();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - prevTime > fallTime)
        {
            
            transform.position += Vector3.down;
            if (!CheckValidate())
            {
                transform.position += Vector3.up;
                //Destruir layer se estiver completa
                Grid_PlayField.instance.DeleteLayer();
                enabled = false;
                //Criar o proximo Bloco
                if (!GameManager.instance.returnGameOver())
                {
                    Grid_PlayField.instance.Spawn_Block();
                }
            }
            else
            {
                //Atualizar Grid
                Grid_PlayField.instance.GridUpdate(this);
            }
            prevTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)|| Input.GetKeyDown(KeyCode.A))
        {
            SetInput(Vector3.left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            SetInput(Vector3.right);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            SetInput(Vector3.forward);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            SetInput(Vector3.back);
        }

    }
    public void SetInput(Vector3 direction)
    {
        transform.position += direction;
        if (!CheckValidate())
        {
            transform.position -= direction;
        }
        else
        {
            Grid_PlayField.instance.GridUpdate(this);
        }
    }
    public void SetRotation(Vector3 rot)
    {
        transform.Rotate(rot,Space.World);
        if (!CheckValidate())
        {
            transform.Rotate(-rot, Space.World);
        }
        else
        {
            Grid_PlayField.instance.GridUpdate(this);
        }
    }
    bool CheckValidate()
    {
        foreach (Transform child in transform)
        {
            Vector3 pos = Grid_PlayField.instance.Round(child.position);
            if (!Grid_PlayField.instance.CheckGrid(pos))
            {
                return false;
            }
        }
        foreach (Transform child in transform)
        {
            Vector3 pos = Grid_PlayField.instance.Round(child.position);
            Transform t = Grid_PlayField.instance.GetTransformOnGridPos(pos);
            if(t != null && t.parent !=transform)
            {
                return false;
            }
        }
        return true;
    }

    public void SetSpeed()
    {
        fallTime = 0.1f;    
    }
}
