using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_Block : MonoBehaviour
{
    GameObject parent;
    Block parentBlock;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RepositionBlock());
    }
    public void SetParent(GameObject _parent)
    {
        parent = _parent;
        parentBlock = parent.GetComponent<Block>();
    }
    void PositionGhost()
    {
        transform.position = parent.transform.position;
        transform.rotation = parent.transform.rotation;
    }

    IEnumerator RepositionBlock()
    {
        while (parentBlock.enabled)
        {
            PositionGhost();
            MoveDown();
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
        yield return null;
    }

    void MoveDown()
    {
        while (CheckValidate())
        {
            transform.position += Vector3.down;
        }
        if (!CheckValidate())
        {
            transform.position += Vector3.up;
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
            if(t!= null && t.parent == parent.transform)
            {
                return true;
            }
            if (t != null && t.parent != transform)
            {
                return false;
            }
        }
        return true;
    }
}
