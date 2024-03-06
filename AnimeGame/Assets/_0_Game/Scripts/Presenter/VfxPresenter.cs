using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using DG.Tweening;
using EJ;
using UnityEngine;

public class VfxPresenter : MonoBehaviour
{
    public static VfxPresenter Instance;
    
    [Header("Field")] [Space(10)] 
    public Light DirLight;
    public GameObject bloodDecal;
    public GameObject[] BloodFX;
    public Vector3 direction;
    int effectIdx;
    public int layerMask;

    public MeshRenderer mesh;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy((gameObject));
        }

        Init();
    }
    
    Transform GetNearestObject(Transform hit, Vector3 hitPos)
    {
        var closestPos = 100f;
        Transform closestBone = null;
        var childs = hit.GetComponentsInChildren<Transform>();

        foreach (var child in childs)
        {
            var dist = Vector3.Distance(child.position, hitPos);
            if (dist < closestPos)
            {
                closestPos = dist;
                closestBone = child;
            }
        }

        var distRoot = Vector3.Distance(hit.position, hitPos);
        if (distRoot < closestPos)
        {
            closestPos = distRoot;
            closestBone = hit;
        }
        return closestBone;
    }

  


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,1000,layerMask))
            {
                
                
                
                MasterAudio.PlaySound3DAtTransform("FemaleScream", transform);
                MasterAudio.PlaySound3DAtVector3("Flesh", hit.point);
                //hit.transform.name.Log();
                // var randRotation = new Vector3(0, Random.value * 360f, 0);
                // var dir = CalculateAngle(Vector3.forward, hit.normal);
                float angle = Mathf.Atan2(hit.normal.x, hit.normal.z) * Mathf.Rad2Deg + 180;
                //var effectIdx = Random.Range(0, BloodFX.Length);
                if (effectIdx == BloodFX.Length) 
                    effectIdx = 0;

                var instance = Instantiate(BloodFX[effectIdx], hit.point, Quaternion.Euler(0, angle + 90, 0));
                effectIdx++;
                var settings = instance.GetComponent<BFX_BloodSettings>();
                //settings.FreezeDecalDisappearance = InfiniteDecal;
                settings.LightIntensityMultiplier = DirLight.intensity;


                var nearestBone = GetNearestObject(hit.transform.root, hit.point);
                if(nearestBone != null)
                {
                    var attachBloodInstance = Instantiate(bloodDecal);
                    var bloodT = attachBloodInstance.transform;
                    bloodT.position = hit.point;
                    bloodT.localRotation = Quaternion.identity;
                    bloodT.localScale = Vector3.one * Random.Range(0.75f, 1.2f);
                    bloodT.LookAt(hit.point + hit.normal, direction);
                    bloodT.Rotate(90, 0, 0);
                    bloodT.transform.parent = nearestBone;
                    //Destroy(attachBloodInstance, 20);
                }
                  
                // if (!InfiniteDecal) Destroy(instance, 20);

            }

        }
    }

    public void PlayBloodVfx(Transform targetTransform)
    {
        MasterAudio.PlaySound3DAtVector3("Flesh", targetTransform.position);
        MasterAudio.PlaySound3DAtVector3("Gore", targetTransform.position);
        //hit.transform.name.Log();
        // var randRotation = new Vector3(0, Random.value * 360f, 0);
        // var dir = CalculateAngle(Vector3.forward, hit.normal);
        //var effectIdx = Random.Range(0, BloodFX.Length);
        if (effectIdx == BloodFX.Length) 
            effectIdx = 0;
        
        var instance = Instantiate(BloodFX[effectIdx], targetTransform.position, Quaternion.identity);
        effectIdx++;
        var settings = instance.GetComponent<BFX_BloodSettings>();
        //settings.FreezeDecalDisappearance = InfiniteDecal;
        settings.LightIntensityMultiplier = DirLight.intensity;
        
        
        var nearestBone = GetNearestObject(targetTransform.root.transform.GetChild(1), targetTransform.position);
        if(nearestBone != null)
        {
            var attachBloodInstance = Instantiate(bloodDecal);
            var bloodT = attachBloodInstance.transform;
            bloodT.position = targetTransform.position;
            bloodT.localRotation = Quaternion.identity;
            bloodT.localScale = Vector3.one * Random.Range(0.75f, 1.2f);
            bloodT.LookAt(targetTransform.position, direction);
            bloodT.Rotate(90, 0, 0);
            bloodT.transform.parent = nearestBone;
            //Destroy(attachBloodInstance, 20);
        }
    }

    public void DelayEJ()
    {
        "OhMyGod".Log();
    }
    
    public void Init()
    {
        layerMask = (-1) - (1 << LayerMask.NameToLayer("Interaction"));
    }
}
