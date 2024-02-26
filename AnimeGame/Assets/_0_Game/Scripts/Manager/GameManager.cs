using System;
using System.Collections;
using System.Collections.Generic;
using AnimeGame;
using DG.Tweening;
using EJ;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Field")] [Space(20)] 
    public Transform target;
    public int layerMask;
    
    
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
        
        Init();
    }


    private void Init()
    {
        layerMask = (-1) - ((1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Interaction")));;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            target.DORewind();
            target.position = new Vector3(5, 1.5f, 6);
            foreach (var rigidbody in PlayerPresenter.Instance.ragDollRigidBodyList)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }

            for (int i = 0; i < PlayerPresenter.Instance.ragDollRigidBodyList.Count; i++)
            {
                if (i is 3 or 4 or 5)
                {
                    PlayerPresenter.Instance.ragDollRigidBodyList[i].mass = 50;
                    PlayerPresenter.Instance.ragDollRigidBodyList[i].solverIterations = 1;
                }
            }
            
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            target.DOMoveX(-15, 3).SetEase(Ease.Linear);
        }
        
        
        
        
        
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,1000,layerMask))
            {
                PlayerPresenter hitPlayer = hit.transform.root.GetComponent<PlayerPresenter>();
                hit.transform.name.Log();

                if (hitPlayer != null)
                {
                    hitPlayer.ActiveRagDoll();
                    Vector3 forceDirection = (hit.point - ray.origin).normalized;
                    float force = 500;
                    hit.transform.GetComponent<Rigidbody>().AddForce(forceDirection*force,ForceMode.Impulse);
                    
                    //StartCoroutine(DelayForceCor(ray,hit));
                }

            }

        }
        
    }

    IEnumerator DelayForceCor(Ray ray, RaycastHit hit)
    {
        yield return new WaitForSeconds(0.1f);
        Vector3 forceDirection = (hit.point - ray.origin).normalized;
        float force = 150;
        hit.transform.GetComponent<Rigidbody>().AddForce(forceDirection*force,ForceMode.Impulse);
    }
    
    
    
}
