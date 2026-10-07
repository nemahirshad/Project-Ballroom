using UnityEngine;
using UnityEngine.AI;

public static class BallroomDifficulty {
 public static void Apply(BallroomGame game) {
  if(game.Level<2 || game.Player==null || game.Enemies==null)return;
  bool final=game.Level==3;
  var weapon=game.Player.GetComponent<Shooting>();
  if(weapon!=null)weapon.fireRate=final?2f:1.5f;
  var turrets=Object.FindObjectsOfType<TurretShoot>();
  System.Array.Sort(turrets,(a,b)=>string.CompareOrdinal(a.name,b.name));
  for(int i=0;i<turrets.Length;i++) {
   turrets[i].fireRate=final?.65f:.85f;
   turrets[i].projectileSpeed=final?11f:12f;
   turrets[i].ScheduleFirstShot(1f+i/(float)Mathf.Max(1,turrets.Length)/turrets[i].fireRate);
  }
  foreach(var enemy in game.Enemies.enemies) {
   if(!enemy.isChaser)continue;
   enemy.damage=final?3:2;
   var agent=enemy.GetComponent<NavMeshAgent>();
   if(agent!=null)agent.speed=final?5f:4.5f;
  }
 }
}
