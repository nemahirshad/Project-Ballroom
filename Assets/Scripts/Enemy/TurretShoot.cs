using UnityEngine;
public class TurretShoot : MonoBehaviour {
 public Transform pos;public GameObject bulletPrefab;public float bulletForce=2,fireRate=1;
 public float projectileSpeed=12;float nextTimeToFire;
 public void ScheduleFirstShot(float delay){nextTimeToFire=Time.time+Mathf.Max(0,delay);}
 void Update(){if(BallroomGame.Blocked || pos==null || bulletPrefab==null || fireRate<=0)return;var player=pos.GetComponent<PlayerInfo>();if(player!=null && player.IsDead)return;if(Time.time>=nextTimeToFire){Shoot();nextTimeToFire=Time.time+1f/fireRate;}}
 public void Shoot(){if(pos==null || bulletPrefab==null || BallroomGame.Blocked)return;var body=GetComponent<Collider>();Vector3 muzzle=body!=null?body.bounds.center:transform.position;var arena=BallroomGame.Instance!=null?BallroomGame.Instance.GetComponent<BallroomArena>():null;if(arena!=null)muzzle.y=Mathf.Max(muzzle.y,arena.FloorTop+.4f);Vector3 direction=pos.position-muzzle;direction.y=0;if(direction.sqrMagnitude<.001f)return;float distance=direction.magnitude;direction.Normalize();var projectile=Instantiate(bulletPrefab,muzzle+direction*.7f,Quaternion.LookRotation(direction));var b=projectile.GetComponent<Bullet>();if(b!=null){b.lifeTime=Mathf.Max(b.lifeTime,distance/Mathf.Max(1,projectileSpeed)+.5f);b.Launch(direction,Mathf.Max(1,projectileSpeed),gameObject,true);}BallroomFeedback.Burst(projectile.transform.position,new Color(1,.3f,.5f),.08f,.10f);}
}
