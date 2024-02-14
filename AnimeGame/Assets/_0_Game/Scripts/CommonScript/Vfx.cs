using System;
using System.Collections;
using PathologicalGames;
using UnityEngine;

[RequireComponent(typeof(VfxSound))]
public class Vfx : MonoBehaviour
{
    private ParticleSystem particleSystem;

    public float lifeTime;
    public string poolName;
    private IEnumerator DelayDestroyIenumerator;
    private WaitForSeconds ws;

    private VfxSound _vfxSound;

    private void Awake()
    {
        ws = new WaitForSeconds(lifeTime);
        _vfxSound = GetComponent<VfxSound>();
        particleSystem = GetComponent<ParticleSystem>();
    }


    private void OnSpawned(SpawnPool pool)
    {
        DelayDestroyIenumerator = DelayDestroy();
        StartCoroutine(DelayDestroyIenumerator);
        particleSystem.Clear();
        particleSystem.Play();
        _vfxSound.PlaySound();
    }
	
    private void OnDespawned(SpawnPool pool)
    {
        if(DelayDestroyIenumerator != null);
            StopCoroutine(DelayDestroyIenumerator);
            
        DelayDestroyIenumerator = null;
    }

    IEnumerator DelayDestroy()
    {
        yield return ws;
       
        PoolManager.Pools[poolName].Despawn(transform);
        
        
    }


    
   
}
