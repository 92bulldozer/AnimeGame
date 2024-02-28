using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using EJ;
using UnityEngine;

public class PhysicsSound : MonoBehaviour
{
    private Rigidbody _rb;
    
    public string physicsSound;
    public float startVelocityMagnitude = 10;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        
        if (_rb.velocity.magnitude > startVelocityMagnitude)
            MasterAudio.PlaySound3DAtTransform(physicsSound, transform);
    }
}
