using UnityEngine;
using System.Collections.Generic;

public class GridHex : MonoBehaviour
{
    public Transform hexPrefab;

    public int gridWidth = 11;
    public int gridHeight = 11;

    float hexWidth = 0.55f;
    float hexHeight = 0.55f;
    public float gap = 0.0f;
    public List<Transform> listBubbles;
    public Sprite[] spritesBubbles;

    int rand;

    Vector3 startPos;

    void Start()
    {
        rand = Random.Range(0, 5);
        listBubbles = new List<Transform>();
        AddGap();
        CalcStartPos();
        CreateGrid();
    }

    

    void AddGap()
    {
        hexWidth += hexWidth * gap;
        hexHeight += hexHeight * gap;
    }

    void CalcStartPos()
    {
        float offset = 0;
        if (gridHeight / 2 % 2 != 0)
            offset = hexWidth / 2;

        float x = -hexWidth * (gridWidth / 2) - offset;
        float z = hexHeight * 0.75f * (gridHeight / 2);

        startPos = new Vector3(x, z, 0);
    }

    Vector3 CalcWorldPos(Vector2 gridPos)
    {
        float offset = 0;
        if (gridPos.y % 2 != 0)
            offset = hexWidth / 2;

        float x = startPos.x + gridPos.x * hexWidth + offset;
        float z = startPos.z - gridPos.y * hexHeight * 0.75f;

        return new Vector3(x, z, 0);
    }

    void CreateGrid()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                rand = Random.Range(0, 5);
                AddCircleToGrid(x, y);
            }
        }
    }

    public void AddCircleToGrid(int x, int y)
    {
        Transform hex = Instantiate(hexPrefab) as Transform;
        hex.GetComponent<SpriteRenderer>().sprite = spritesBubbles[rand];
        Vector2 gridPos = new Vector2(x, y);
        hex.position = CalcWorldPos(gridPos);
        hex.parent = this.transform;
        int z = x + (y * 4);
        hex.name = z.ToString() + "|" + x +","+ y;
        listBubbles.Add(hex);
    }

    public void AddBulletToGrid(int x, int y, Sprite s)
    {
    restart:
        y++;
        for (int i = 0; i < listBubbles.Count; i++)
        {
            if (y % 2 == 0)
            {

                if (listBubbles[i].name.Contains("|" + x + "," + y))
                {
                    x++;
                    if (listBubbles[i].name.Contains("|" + x + "," + y))
                    {
                        
                        goto restart;
                    }
                }
            }
            else
            {
               
                if (listBubbles[i].name.Contains("|" + x + "," + y))
                {
                    x--;
                    if (listBubbles[i].name.Contains("|" + x + "," + y))
                    {
                        goto restart;
                    }
                }
            }
        }
        Transform hex = Instantiate(hexPrefab) as Transform;
        Vector2 gridPos = new Vector2(x, y);
        hex.position = CalcWorldPos(gridPos);
        hex.parent = this.transform;
        hex.GetComponent<SpriteRenderer>().sprite = s;
        int z = x + (y * 4);
        hex.name = z.ToString() + "|" + x + "," + y;
        listBubbles.Add(hex);
    }

    void PositionOfBullet(int x, int y)
    {
        int xNew;
        int yNew;
        restart:
        for (int i = 0; i < listBubbles.Count; i++)
        {
            if (y % 2 == 0)
            {
                if (listBubbles[i].name.Contains("|" + x + "," + y))
                {
                    y++;
                    goto restart;

                }
            }
            else
            {
                if (listBubbles[i].name.Contains("|" + x + "," + y))
                {
                    y++;
                    goto restart;
                }
            }
        }
    }
}