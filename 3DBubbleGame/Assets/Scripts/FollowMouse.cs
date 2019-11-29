using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{

    public Transform cursorObject;
    public float distance = 1.5f;
 


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        ObjectFollowCursor();

    }

    void ObjectFollowCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 point = ray.origin + (ray.direction * distance);
        //Debug.Log( "World point " + point );
        Vector3 pos = new Vector3(point.x, point.y, 0f);
        cursorObject.position = pos;
        //if()
    }

}
