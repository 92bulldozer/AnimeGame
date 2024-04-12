
using AnimeGame;
using EJ;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement.AstarPathfindingProject
{
    [TaskCategory("AnimeGame")]
    public class EnemyPatrol : IAstarAIMovement
    {

        [Tooltip("The length of time that the agent should pause when arriving at a waypoint")]
        public SharedFloat waypointPauseDuration = 0;

        public EnemyPresenter enemyPresenter;

        public float animationSpeed;
        public bool isRoomEnter;


        [Tooltip("The waypoints to move to")] public SharedGameObjectList waypoints;

        // The current index that we are heading towards within the waypoints array
        private int waypointIndex;
        private float waypointReachedTime;

        public override void OnAwake()
        {
            base.OnAwake();
            enemyPresenter = gameObject.GetComponent<EnemyPresenter>();
            enemyPresenter.isPatrolling = true;
        }


        public override void OnStart()
        {
            base.OnStart();

            Animator animator = transform.GetComponentInChildren<Animator>();
            animator.SetFloat("AnimationSpeed", animationSpeed);
            // initially move towards the closest waypoint
            float distance = Mathf.Infinity;
            float localDistance;
            for (int i = 0; i < waypoints.Value.Count; ++i)
            {
                if ((localDistance =
                        Vector3.Magnitude(transform.position - waypoints.Value[i].transform.position)) < distance)
                {
                    distance = localDistance;
                    waypointIndex = i;
                }
            }

            waypointIndex = 0;
            waypointReachedTime = -waypointPauseDuration.Value;
            SetDestination(Target());
        }

        // Patrol around the different waypoints specified in the waypoint array. Always return a task status of running. 
        public override TaskStatus OnUpdate()
        {
            if (waypoints.Value.Count == 0)
            {
                return TaskStatus.Failure;
            }


            $"{waypointIndex}".Log(EColor.RED);

            if (HasArrived())
            {
                waypoints.Value[0].GetComponent<EnemyRoomTrigger>().EnterRoom(enemyPresenter);
                return TaskStatus.Success;
            }

            // if (HasArrived())
            // {
            //     if (waypointReachedTime == -waypointPauseDuration.Value)
            //     {
            //         waypointReachedTime = Time.time;
            //     }
            //
            //     // wait the required duration before switching waypoints.
            //     if (waypointReachedTime + waypointPauseDuration.Value <= Time.time)
            //     {
            //         if (randomPatrol.Value)
            //         {
            //             if (waypoints.Value.Count == 1)
            //             {
            //                 "EnterRoom".Log(EColor.RED);
            //                 waypoints.Value[0].GetComponent<EnemyRoomTrigger>().EnterRoom(enemyPresenter);
            //                 enemyPresenter.isRandomPatrol = false;
            //
            //                 waypointIndex = 0;
            //             }
            //             else
            //             {
            //                 // // prevent the same waypoint from being selected
            //                 // var newWaypointIndex = waypointIndex;
            //                 // while (newWaypointIndex == waypointIndex)
            //                 // {
            //                 //     newWaypointIndex = Random.Range(0, waypoints.Value.Count);
            //                 // }
            //                 //
            //                 // waypointIndex = newWaypointIndex;
            //                 // $"waypointIndex RandomPatrol".Log();
            //             }
            //         }
            //         else
            //         {
            //             if (waypoints.Value.Count == 1)
            //             {
            //             }
            //             else
            //             {
            //             }
            //
            //             waypointIndex.Log(EColor.YELLOW);
            //             //마지막 DoorTrigger 떄문에 -1
            //             if (waypointIndex == waypoints.Value.Count - 1)
            //             {
            //                 $"waypointIndex={waypointIndex} waypoints.Value.Count={waypoints.Value.Count}".Log(
            //                     EColor.YELLOW);
            //                 waypointIndex = 0;
            //                 // waypoints.Value.Clear();
            //                 // waypoints.Value.Add(WayPointManager.Instance.SelectRandomWayPoint());
            //                 return TaskStatus.Success;
            //             }
            //             else
            //             {
            //                 "WayPointIndex Add".Log(EColor.GREEN);
            //                 waypointIndex = (waypointIndex + 1) % waypoints.Value.Count;
            //             }
            //         }
            //
            //         SetDestination(Target());
            //         waypointReachedTime = -waypointPauseDuration.Value;
            //     }
            // }


            return TaskStatus.Running;
        }


        // Return the current waypoint index position
        private Vector3 Target()
        {
            if (waypointIndex >= waypoints.Value.Count)
            {
                return transform.position;
            }

            return waypoints.Value[waypointIndex].transform.position;
        }

        // Reset the public variables
        public override void OnReset()
        {
            base.OnReset();

            waypointPauseDuration = 0;
            waypoints = null;
        }

        // Draw a gizmo indicating a patrol 
        public override void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (waypoints == null || waypoints.Value == null)
            {
                return;
            }

            var oldColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = Color.yellow;
            for (int i = 0; i < waypoints.Value.Count; ++i)
            {
                if (waypoints.Value[i] != null)
                {
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5
                    UnityEditor.Handles.SphereCap(0, waypoints.Value[i].transform.position, waypoints.Value[i].transform.rotation, 1);
#else
                    UnityEditor.Handles.SphereHandleCap(0, waypoints.Value[i].transform.position,
                        waypoints.Value[i].transform.rotation, 1, EventType.Repaint);
#endif
                }
            }

            UnityEditor.Handles.color = oldColor;
#endif
        }
    }
}