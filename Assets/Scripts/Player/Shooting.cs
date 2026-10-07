using UnityEngine;
public class Shooting : MonoBehaviour {
 public Transform firePoint;public GameObject bulletPrefab;public float bulletForce=40,fireRate=1;float nextTimeToFire;
 void Update(){if(Input.GetMouseButton(0) && (UnityEngine.EventSystems.EventSystem.current==null || !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()))TryShoot();}
 public bool TryShoot(){if(BallroomGame.Blocked || Time.time<nextTimeToFire || bulletPrefab==null || firePoint==null || fireRate<=0)return false;
  Vector3 direction=Vector3.ProjectOnPlane(firePoint.forward,Vector3.up).normalized;if(direction.sqrMagnitude<.001f)return false;
  nextTimeToFire=Time.time+1f/fireRate;
  var body=GetComponent<Rigidbody>();Vector3 muzzle=firePoint.position;
  // Render interpolation can trail the physical body while moving or landing.
  if(body!=null)muzzle+=body.position-transform.position;
  var arena=BallroomGame.Instance!=null?BallroomGame.Instance.GetComponent<BallroomArena>():null;
  var sphere=bulletPrefab.GetComponent<SphereCollider>();float radius=sphere!=null?sphere.radius*Mathf.Max(Mathf.Abs(bulletPrefab.transform.lossyScale.x),Mathf.Abs(bulletPrefab.transform.lossyScale.y),Mathf.Abs(bulletPrefab.transform.lossyScale.z)):.25f;
  if(arena!=null)muzzle.y=Mathf.Max(muzzle.y,arena.FloorTop+radius+.08f);
  var bullet=Instantiate(bulletPrefab,muzzle,Quaternion.LookRotation(direction));var b=bullet.GetComponent<Bullet>();if(b!=null)b.Launch(direction,Mathf.Max(1,bulletForce),gameObject,false);BallroomFeedback.Burst(muzzle,new Color(1,.8f,.3f),.09f,.13f);BallroomAudio.Cue(BallroomAudio.Sound.Shot);return true;}
}
