using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using EJ;
using UnityEngine;

namespace AnimeGame
{
    [TaskCategory("AnimeGame")]
    public class SelectWayPoint : Action
    {
        public EnemyPresenter enemyPresenter;

    
    
        public override void OnAwake()
        {
            enemyPresenter = GetComponent<EnemyPresenter>();
        }

        public override TaskStatus OnUpdate()
        {
            enemyPresenter.wayPointList.Clear();
            enemyPresenter.wayPointList.Add(WayPointManager.Instance.SelectRandomWayPoint());
            return TaskStatus.Success;
        }
    }
}
