using UnityEngine;
using UnityEngine.AI;

public class UnitAnimationController : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _animator;
    private static readonly int Dead1 = Animator.StringToHash("Dead");
    private static readonly int Speed = Animator.StringToHash("Speed");

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        _animator.SetFloat(Speed,_agent.velocity.magnitude/ _agent.speed);
    }

 

    public void SetRootMotion(bool value)
    {
        _animator.applyRootMotion = value;
    }

    public void Dead()
    {
        _animator.SetTrigger(Dead1);
    }

}