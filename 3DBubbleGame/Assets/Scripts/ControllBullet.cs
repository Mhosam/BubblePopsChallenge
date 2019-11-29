using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ControllBullet : MonoBehaviour
{
    bool fired = false;
    public int bulletSpeed = 6;
    public GameObject bulletPrefab;
    Vector3 mousePos;
    public Sprite[] spritesBubbles;
    public GridHex gh;
    int rand;
    // Start is called before the first frame update
    void Start()
    {
        rand = Random.Range(0, 5);
        GetComponent<SpriteRenderer>().sprite = spritesBubbles[rand];
        gh = FindObjectOfType<GridHex>();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(mousePos.x, mousePos.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (fired)
        {
            Move();
        }

        else if (Input.GetMouseButtonUp(0))
        {
            fired = true;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = new Vector3(mousePos.x, mousePos.y, 0);

            Vector3 diff = mousePos - transform.position;
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        }

        
            for (int i = 0; i < gh.listBubbles.Count; i++)
            {
                if (gh.listBubbles[i] != null)
                {


                    string name = gh.listBubbles[i].gameObject.name;
                    string[] splitArray = name.Split(char.Parse("|"));
                    string nameA = splitArray[1];

                    int x = -1;
                    int y = -1;
                    foreach (char c in nameA)
                    {
                        if (char.IsNumber(c) && x == -1)
                        {
                            x = int.Parse(c.ToString());
                        }
                        else if (char.IsNumber(c))
                        {
                            y = int.Parse(c.ToString());
                        }

                    }
                if (x != 0 && y != 0)
                {
                    if (NearBubblesEmpty(x, y))
                    {
                        gh.listBubbles[i].GetComponent<Rigidbody>().useGravity = true;
                        gh.listBubbles.Remove(gh.listBubbles[i]);
                    }
                }
                }
            }
        

        if (gh.listBubbles.Count == 0)
            Time.timeScale = 0;
    }

    void Move()
    {
        transform.position += transform.up * bulletSpeed * Time.deltaTime;
    }



    void OnTriggerEnter(Collider other)
    {

        if (other.tag.Equals("Bubble"))
        {

            string name = other.gameObject.name;
            string[] splitArray = name.Split(char.Parse("|"));
            string nameA = splitArray[1];

            int x = -1;
            int y = -1;
            foreach (char c in nameA)
            {
                if (char.IsNumber(c) && x == -1)
                {    
                    x = int.Parse(c.ToString());
                }
                else if (char.IsNumber(c))
                {
                    y = int.Parse(c.ToString());
                }

            }
            if (fired) 
            {
                if(other.gameObject.GetComponent<SpriteRenderer>().sprite == GetComponent<SpriteRenderer>().sprite)
                {
                    NearCircle(other.gameObject,x,y, true, 1);
                }
                else
                {
                    gh.AddBulletToGrid(x, y, GetComponent<SpriteRenderer>().sprite);
                }
                rand = Random.Range(0, 5);
                GameObject newBullet = Instantiate(bulletPrefab, new Vector3(0, -6.92f, 0), Quaternion.identity);
                newBullet.GetComponent<SpriteRenderer>().sprite = spritesBubbles[rand];
                GetComponentInChildren<ParticleSystem>().Play();
                GetComponent<SpriteRenderer>().enabled = false;
                ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
                Destroy(gameObject, ps.main.duration);

                fired = false;
            }

        }
        if (other.tag.Equals("Wall"))
        {
            Vector3 v = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(v.x, v.y, -v.z);
        }

    }


    void SwitchNumberCircle(Collider other, float numCircle)
    {
        

        switch (numCircle)
        {
            case 2:
                other.gameObject.GetComponent<SpriteRenderer>().sprite = spritesBubbles[1];
                break;
            case 4:
                other.gameObject.GetComponent<SpriteRenderer>().sprite = spritesBubbles[2];
                break;
            case 8:
                other.gameObject.GetComponent<SpriteRenderer>().sprite = spritesBubbles[3];
                break;
            case 16:
                other.gameObject.GetComponent<SpriteRenderer>().sprite = spritesBubbles[4];
                break;
            case 32:
                other.gameObject.GetComponent<SpriteRenderer>().sprite = spritesBubbles[5];
                break;
            case 64:
                other.gameObject.GetComponent<SpriteRenderer>().sprite = spritesBubbles[6];
                break;
            case 128:
                other.gameObject.GetComponent<SpriteRenderer>().sprite = spritesBubbles[7];
                break;
            case 256:
                other.gameObject.GetComponent<SpriteRenderer>().sprite = spritesBubbles[8];
                break;
            case 512:
                other.gameObject.GetComponent<SpriteRenderer>().sprite = spritesBubbles[9];
                break;
            case 1024:
                other.gameObject.GetComponent<SpriteRenderer>().sprite = spritesBubbles[10];
                break;
            

            default:
                break;
        }

    }

    void NearCircle(GameObject other, int x, int y, bool stop, float num)
    {
        Sprite sprite = other.gameObject.GetComponent<SpriteRenderer>().sprite;
        List<Transform> index = SameColorNear(x, y, sprite);
        int startCount = 0;

        while (startCount != index.Count)
        {
            startCount = index.Count;
            for(int i = 0; i  < index.Count; i++)
            {
                string name2 = index[i].gameObject.name;
                string[] splitArray2 = name2.Split(char.Parse("|"));
                string nameA2 = splitArray2[1];

                int a2 = -1;
                int b2 = -1;
                foreach (char c in nameA2)
                {
                    if (char.IsNumber(c) && a2 == -1)
                    {
                        a2 = int.Parse(c.ToString());
                    }
                    else if (char.IsNumber(c))
                    {
                        b2 = int.Parse(c.ToString());
                    }

                }

                index = index.Union<Transform>(SameColorNear(a2, b2, sprite)).Distinct().ToList<Transform>();
            }
        }


        float n = float.Parse(other.gameObject.GetComponent<SpriteRenderer>().sprite.name);
        float power = Mathf.Log(n, 2) + index.Count-2;
        float numCircle = Mathf.Pow(2, power);
        if (numCircle >= 1024)
            numCircle = 1024;

        for (int i = 0; i < index.Count - 1; i++)
        {
            gh.listBubbles.Remove(index[i]);
            Destroy(index[i].gameObject);
        }


      

        if (index.Count >= 1)
        {
            string name = index[index.Count - 1].gameObject.name;
            string[] splitArray = name.Split(char.Parse("|"));
            string nameA = splitArray[1];

            int a = -1;
            int b = -1;
            foreach (char c in nameA)
            {
                if (char.IsNumber(c) && a == -1)
                {
                    a = int.Parse(c.ToString());
                }
                else if (char.IsNumber(c))
                {
                    b = int.Parse(c.ToString());
                }

            }
            SwitchNumberCircle(other.gameObject.GetComponent<SphereCollider>(), numCircle);

            NearCircle(index[index.Count - 1].gameObject, a, b, false, 2);
        }

        if(index.Count == 0 && stop)
        {
            numCircle = System.Convert.ToInt32(GetComponent<SpriteRenderer>().sprite.name);
            SwitchNumberCircle(other.gameObject.GetComponent<SphereCollider>(), numCircle);

            NearCircle(other, x,y, false, 2);

        }

    }


    List<Transform> SameColorNear(int x, int y, Sprite s)
    {
        List<Transform> index = new List<Transform>();
        int upLeft = 0;
        int upRight = 0;
        int left = 0;
        int right = 0;
        int downLeft = 0;
        int downRight = 0;
        int myNum = x + y * 4;

        int xplus1 = x + 1;
        int xminus1 = x - 1;
        int yplus1 = y + 1;
        int yminus1 = y - 1;

        if (x == 0)
            xminus1 = 0;
        if (x == 3)
            xplus1 = 3;
        if (y == 0)
            yminus1 = 0;


        if (y % 2 == 0)
        {
            upLeft = xminus1 + yminus1 * 4;
            upRight = x + yminus1 * 4;
            left = xminus1 + (y) * 4;
            right = xplus1 + (y) * 4;
            downLeft = xminus1 + yplus1 * 4;
            downRight = x + yplus1 * 4;
        }
        else
        {
            upLeft = x + yminus1 * 4;
            upRight = xplus1 + yminus1 * 4;
            left = xminus1 + (y) * 4;
            right = xplus1 + (y) * 4;
            downLeft = x + yplus1 * 4;
            downRight = xplus1 + yplus1 * 4;
        }

        for (int i = 0; i < gh.listBubbles.Count; i++)
        {
            if (gh.listBubbles[i] != null && gh.listBubbles[i].gameObject.GetComponent<SpriteRenderer>().sprite == s)
            {
                if (gh.listBubbles[i].name.Contains(myNum.ToString() + "|"))
                {
                }
                else if (gh.listBubbles[i].name.Contains(downLeft.ToString() + "|"))
                {
                    index.Add(gh.listBubbles[i]);

                }
                else if (gh.listBubbles[i].name.Contains(downRight.ToString() + "|"))
                {
                    index.Add(gh.listBubbles[i]);

                }
                else if (gh.listBubbles[i].name.Contains(left.ToString() + "|"))
                {
                    index.Add(gh.listBubbles[i]);

                }
                else if (gh.listBubbles[i].name.Contains(right.ToString() + "|"))
                {
                    index.Add(gh.listBubbles[i]);

                }
                else if (gh.listBubbles[i].name.Contains(upLeft.ToString() + "|"))
                {
                    index.Add(gh.listBubbles[i]);
                }

                else if (gh.listBubbles[i].name.Contains(upRight.ToString() + "|"))
                {
                    index.Add(gh.listBubbles[i]);
                }
               
            }
        }

        return index;
    }

    bool NearBubblesEmpty(int x, int y)
    {
        List<Transform> index = new List<Transform>();
        int upLeft = 0;
        int upRight = 0;
        int left = 0;
        int right = 0;
        int downLeft = 0;
        int downRight = 0;
        int myNum = x + y * 4;

        int xplus1 = x + 1;
        int xminus1 = x - 1;
        int yplus1 = y + 1;
        int yminus1 = y - 1;

        if (x == 0)
            xminus1 = 0;
        if (x == 3)
            xplus1 = 3;
        if (y == 0)
            yminus1 = 0;


        if (y % 2 == 0)
        {
            upLeft = xminus1 + yminus1 * 4;
            upRight = x + yminus1 * 4;
            left = xminus1 + (y) * 4;
            right = xplus1 + (y) * 4;
            downLeft = xminus1 + yplus1 * 4;
            downRight = x + yplus1 * 4;
        }
        else
        {
            upLeft = x + yminus1 * 4;
            upRight = xplus1 + yminus1 * 4;
            left = xminus1 + (y) * 4;
            right = xplus1 + (y) * 4;
            downLeft = x + yplus1 * 4;
            downRight = xplus1 + yplus1 * 4;
        }

        for (int i = 0; i < gh.listBubbles.Count; i++)
        {
            if (gh.listBubbles[i] != null)
            {
                if (gh.listBubbles[i].name.Contains(myNum.ToString() + "|"))
                {
                }
                else if (gh.listBubbles[i].name.Contains(upLeft.ToString() + "|"))
                {
                    index.Add(gh.listBubbles[i]);
                }

                else if (gh.listBubbles[i].name.Contains(upRight.ToString() + "|"))
                {
                    index.Add(gh.listBubbles[i]);
                }

            }
        }
        if (index.Count == 0)
            return true;
        else
            return false;
    }
}
