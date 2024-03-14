using System;
using System.Collections;
using System.Collections.Generic;
using EJ;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Space(20)] [Header("Field")] [Space(10)]
    public float interactRange = 1;

    public InteractableObject currentInteractableObject;

    public bool canDrawGizmo;
    public bool canInteract = false;
    private Collider[] interactColliderArray;
    public int layerMask;

    public void Init()
    {
        canInteract = true;
        interactColliderArray = new Collider[10];
        layerMask = 1 << LayerMask.NameToLayer("InteractableObject");
    }

    void OnDrawGizmos()
    {
        if (!canDrawGizmo)
            return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }

    public void FindInteractObject()
    {
        Array.Clear(interactColliderArray, 0, 10);
        var size = Physics.OverlapSphereNonAlloc(transform.position, interactRange, interactColliderArray,layerMask);
        GameObject shortDistanceObject = null;
        //currentInteractableObject = null;
        float shortDistance = 1000000000;

        for (int i = 0; i < size; i++)
        {
            float distance = Vector3.Distance(gameObject.transform.position, interactColliderArray[i].transform.position);

            if (distance < shortDistance)
            {
                shortDistanceObject = interactColliderArray[i].gameObject;
                shortDistance = distance;
                
                
            }
        }

        if (size == 0)
        {
            if (currentInteractableObject != null)
            {
                currentInteractableObject.DisableOutline();
                currentInteractableObject.HideInteractPanel();
            }
            
            currentInteractableObject = null;
        }
        else
        {
            InteractableObject io = shortDistanceObject.GetComponent<InteractableObject>();
            
            if (currentInteractableObject == null)
            {
                currentInteractableObject = io;
                currentInteractableObject.EnableOutline();
                currentInteractableObject.ShowInteractPanel();
            }
            else
            {
                if (shortDistanceObject != null && shortDistanceObject != currentInteractableObject.gameObject)
                {
                    if (io != currentInteractableObject)
                    {
                        if (currentInteractableObject != null)
                        {
                            currentInteractableObject.DisableOutline();
                        }
                    }
                    currentInteractableObject = io;
                    currentInteractableObject.EnableOutline();
                    currentInteractableObject.ShowInteractPanel();
                }
            }
            
            
        }

        

       


        // if(size != 0)
        //     $"ShortDistance = {shortDistance} ShortDistanceObject = {shortDistanceObject.name}".Log();
    }
}