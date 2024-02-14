using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
   

    // Update is called once per frame
    void LateUpdate()
    {
        if(target!=null)
            transform.position = target.position+offset;
    }
}
