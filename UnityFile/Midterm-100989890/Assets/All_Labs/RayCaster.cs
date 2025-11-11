using UnityEngine;

public class RayCaster : MonoBehaviour
{

 
    private Camera cam;
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.point);  //point is a position x,y,z  e.g. this.transform.position = hit.point
            }       
        }
    }
}
