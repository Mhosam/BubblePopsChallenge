using UnityEngine;
using System.Collections;
public class TargetLine : MonoBehaviour
{
    public GameObject gun; // up to you to initialize this to the gun
    LineRenderer lineRenderer;
    Vector3 mousePos;
    public Material newMat;
   /* private Ray ray;
    private RaycastHit hit;
    private Vector3 direction;
    public int nReflections = 2;
    public float maxLength = 100f;
    private int numPoints;
    private Transform goTransform;
    */
    void Start()
    {

        //goTransform = this.GetComponent<Transform>();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lineRenderer = gameObject.AddComponent<LineRenderer>() as LineRenderer;
        //lineRenderer.material = new Material(Shader.Find("Particles/Standard Surface"));
        lineRenderer.material = newMat;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.SetVertexCount(2);
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lineRenderer.SetPosition(0, new Vector3(0f, -6.92f, 0f));

        lineRenderer.SetPosition(1, new Vector3(mousePos.x, 0.5f, 0f));
        RaycastHit hit;
        Vector3 poss = new Vector3(mousePos.x, 0.5f, 0f);
        lineRenderer.SetVertexCount(2);
        if (Physics.Raycast(mousePos, Vector3.forward, out hit, Mathf.Infinity))
        {
            //lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, new Vector3(0f, -6.92f, 0f));
            lineRenderer.SetPosition(1, hit.point);
            lineRenderer.SetVertexCount(2);

            if (hit.collider.tag == "Wall")
            {
                lineRenderer.SetVertexCount(3);
                Vector3 pos = Vector3.Reflect(hit.point, hit.normal);
                lineRenderer.SetPosition(2, pos);
                //lineRenderer.SetPosition(3, pos);
            }

        }
        /*nReflections = Mathf.Clamp(nReflections, 1, nReflections);
        ray = new Ray(goTransform.position, goTransform.forward);

        float remainingLength = maxLength;

        for (int i = 0; i < nReflections; i++)
        {
            // ray cast
            if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength))
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                remainingLength -= Vector3.Distance(ray.origin, hit.point);
                ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                if (hit.collider.tag != "Wall")
                    break;
            }
            else
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);
                break;
            }
        }*/
    }

    
}

   
   

