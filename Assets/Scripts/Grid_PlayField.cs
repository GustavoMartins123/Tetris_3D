using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_PlayField : MonoBehaviour
{
    public static Grid_PlayField instance;
    public int gridSizeX, gridSizeY, gridSizeZ;
    int rand;
    [Space]
    [Header("Blocks")]
    public GameObject[] blocks;
    [Space]
    public GameObject[] ghost_Blocks;
    [Space]
    [Header ("Grid Visual")]
    public GameObject plane_Ground;
    public GameObject[] other_Planes;//0 - N / 1 - S / 2 - O / 3 - L
    public Transform[,,] theGrid;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        theGrid = new Transform[gridSizeX, gridSizeY, gridSizeZ];
        CalculatePreview();
        Spawn_Block();
    }
    public Vector3 Round(Vector3 vec)
    {
        return new Vector3(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y), Mathf.RoundToInt(vec.z));
    }

    public bool CheckGrid(Vector3 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridSizeX && (int)pos.z >= 0 && (int)pos.z < gridSizeZ && (int) pos.y >= 0);
    }

    public void GridUpdate(Block blocks) 
    {
        //Deletando o "Pai"
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    if(theGrid[x,y,z] != null)
                    {
                        if (theGrid[x, y, z].parent == blocks.transform)
                        {
                            theGrid[x, y, z] = null;
                        }
                    }

                }
            }
        }
        //Enchendo todos os objetos filhos
        foreach (Transform child in blocks.transform)
        {
            Vector3 pos = Round(child.position);
            if(pos.y < gridSizeY)
            {
                theGrid[(int)pos.x, (int)pos.y, (int)pos.z] = child;
            }
        }
    }

    public Transform GetTransformOnGridPos(Vector3 pos)
    {
        if(pos.y > gridSizeY - 1)
        {
            return null;
        }
        else
        {
            return theGrid[(int)pos.x, (int)pos.y, (int)pos.z];
        }
    }

    public void Spawn_Block()
    {
        Vector3 spawn = new Vector3((int)(transform.position.x + (float)gridSizeX / 2), (int)transform.position.y + gridSizeY, (int)(transform.position.z + (float)gridSizeZ/2));
        GameObject block = Instantiate(blocks[rand], spawn, Quaternion.identity) as GameObject;
        GameObject ghost_Block = Instantiate(ghost_Blocks[rand], new Vector3(spawn.x,0,spawn.z), Quaternion.identity) as GameObject;
        ghost_Block.GetComponent<Ghost_Block>().SetParent(block);
        CalculatePreview();
        Spawner_Preview.instance.ShowPreview(rand);
    }

    public void CalculatePreview()
    {
        rand = Random.Range(0, blocks.Length);
    }
    public void DeleteLayer()
    {
        int layerCleared = 0;
        for (int y = gridSizeY - 1; y >= 0; y--)
        {
            //Checando layer
            if (CheckLayer(y))
            {
                layerCleared++;
                //Deletando todos blocos na layer
                DeleteLayerAt(y);
                //Mover todos acima em uma unidade para baixo
                MoveAllLayerDown(y);
            }
        }
        if (layerCleared > 0)
        {
            GameManager.instance.LayerCleared(layerCleared);
        }
    }

    bool CheckLayer(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                if(theGrid[x,y,z] == null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    void DeleteLayerAt(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Destroy(theGrid[x, y, z].gameObject);
                theGrid[x, y, z] = null;
            }
        }
    }

    void MoveAllLayerDown(int y)
    {
        for (int i = y; i < gridSizeY; i++)
        {
            MoveOneLayer(i);
        }
    }

    void MoveOneLayer(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                if(theGrid[x,y,z]!= null)
                {
                    theGrid[x, y - 1, z] = theGrid[x, y, z];
                    theGrid[x, y, z] = null;
                    theGrid[x, y - 1, z].position += Vector3.down;
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        if(plane_Ground != null)
        {
            //Resize plane_Ground
            Vector3 scaler = new Vector3((float)gridSizeX/10,1, (float)gridSizeZ/10);
            plane_Ground.transform.localScale = scaler;
            //Reposition
            plane_Ground.transform.position = new Vector3(transform.position.x + (float)gridSizeX / 2, transform.position.y, transform.position.z + (float)gridSizeZ/2 );
            //Retile Material
            plane_Ground.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeZ);
        }
        if (other_Planes[0] != null)
        {
            Vector3 scaler = new Vector3((float)gridSizeX / 10, 1, (float)gridSizeY / 10);
            other_Planes[0].transform.localScale = scaler;
            //Reposition
            other_Planes[0].transform.position = new Vector3(transform.position.x + (float)gridSizeX / 2, transform.position.y + (float) gridSizeY/2, transform.position.z + gridSizeZ);
            //Retile Material
            other_Planes[0].GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeY);
        }
        if (other_Planes[1] != null)
        {
            Vector3 scaler = new Vector3((float)gridSizeX / 10, 1, (float)gridSizeY / 10);
            other_Planes[1].transform.localScale = scaler;
            //Reposition
            other_Planes[1].transform.position = new Vector3(transform.position.x + (float)gridSizeX / 2, transform.position.y + (float)gridSizeY / 2, transform.position.z);
            //Retile Material
            //other_Planes[1].GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeY);
        }
        if (other_Planes[2] != null)
        {
            Vector3 scaler = new Vector3((float)gridSizeX / 10, 1, (float)gridSizeY / 10);
            other_Planes[2].transform.localScale = scaler;
            //Reposition
            other_Planes[2].transform.position = new Vector3(transform.position.x, transform.position.y + (float)gridSizeY / 2, transform.position.z + (float)gridSizeZ/2);
            //Retile Material
            //other_Planes[2].GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeY);
        }
        if (other_Planes[3] != null)
        {
            Vector3 scaler = new Vector3((float)gridSizeZ / 10, 1, (float)gridSizeY / 10);
            other_Planes[3].transform.localScale = scaler;
            //Reposition
            other_Planes[3].transform.position = new Vector3(transform.position.x + gridSizeX, transform.position.y + (float)gridSizeY / 2, transform.position.z + (float)gridSizeZ/2);
            //Retile Material
            other_Planes[3].GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeZ, gridSizeY);
        }
    }
}
