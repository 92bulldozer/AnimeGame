using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTouchShieldImpact : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           
            if (Physics.Raycast(ray, out hit, 1000))
            {
                MeshRenderer mesh = hit.collider.gameObject.GetComponent<MeshRenderer>();
                if (mesh != null)
                {
                    Debug.Log(hit.transform.name);
                    Debug.DrawRay(ray.origin, hit.point-ray.origin, Color.red,5);
                    Material mat = mesh.material;
                    Vector3 hitPosition = mesh.transform.InverseTransformPoint(hit.point);
                    mat.SetVector("_HitPosition",hitPosition);
                }
            }
        }
    }
}
