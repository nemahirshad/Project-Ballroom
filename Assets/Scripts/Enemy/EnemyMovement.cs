using UnityEngine;
using UnityEngine.AI;
public class EnemyMovement : MonoBehaviour {
 public Transform player;NavMeshAgent agent;float nextPathUpdate;bool wasStopped=true;
 void Awake(){agent=GetComponent<NavMeshAgent>();if(agent!=null){agent.acceleration=12;agent.angularSpeed=360;agent.stoppingDistance=.15f;agent.autoBraking=false;}}
 void Update(){if(agent==null || !agent.enabled || !agent.isOnNavMesh)return;
  bool stop=BallroomGame.Blocked || player==null;agent.isStopped=stop;
  if(stop){wasStopped=true;return;}
  if(wasStopped || Time.time>=nextPathUpdate){
   var body=player.GetComponent<Rigidbody>();Vector3 target=body!=null?body.position:player.position;
   NavMeshHit hit;if(NavMesh.SamplePosition(target,out hit,2,agent.areaMask))agent.SetDestination(hit.position);
   nextPathUpdate=Time.time+.12f;wasStopped=false;
  }
 }
}
