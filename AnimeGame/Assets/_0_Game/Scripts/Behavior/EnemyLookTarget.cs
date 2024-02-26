using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class EnemyLookTarget : Action
{
    public SharedGameObject targetObject;
    
    public override TaskStatus OnUpdate()
    {
        if (targetObject.Value == null)
            return TaskStatus.Failure;

        transform.rotation =
            Quaternion.LookRotation((targetObject.Value.transform.position - transform.position).normalized);
        
        return base.OnUpdate();
    }
}
