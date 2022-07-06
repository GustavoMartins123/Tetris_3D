using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Preview : MonoBehaviour
{
    public static Spawner_Preview instance;
    public GameObject[] preview_Blocks;
    GameObject currentActive;
    private void Awake()
    {
        instance = this;
    }

    public void ShowPreview(int index)
    {
        if (currentActive != null)
        {
            Destroy(currentActive.gameObject);
        }
        currentActive = Instantiate(preview_Blocks[index], transform.position, Quaternion.identity) as GameObject;
    }
}
