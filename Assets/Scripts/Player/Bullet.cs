using UnityEngine;
public class Bullet : MonoBehaviour {
 public float lifeTime=2;public bool isEnemy,isPlayer;
 bool spent;GameObject owner;
 public bool Spent { get { return spent; } }
 void Start(){Destroy(gameObject,Mathf.Max(.1f,lifeTime));}
 public void Launch(Vector3 direction,float speed,GameObject source,bool enemy){owner=source;isEnemy=enemy;isPlayer=!enemy;var rb=GetComponent<Rigidbody>();if(rb!=null){rb.useGravity=false;rb.collisionDetectionMode=CollisionDetectionMode.ContinuousDynamic;rb.velocity=direction.normalized*speed;}
 if(owner!=null)foreach(var a in GetComponentsInChildren<Collider>())foreach(var b in owner.GetComponentsInChildren<Collider>())Physics.IgnoreCollision(a,b);
 var trail=gameObject.AddComponent<TrailRenderer>();trail.time=.10f;trail.startWidth=.12f;trail.endWidth=0;trail.material=BallroomFeedback.GlowMaterial;trail.startColor=enemy?new Color(1,.3f,.45f):new Color(.3f,.85f,1);trail.endColor=new Color(1,1,1,0);trail.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;}
 public bool Hit(Collider other){if(spent || other==null || BallroomGame.Blocked || (owner!=null && other.transform.IsChildOf(owner.transform)))return false;
 var enemy=other.GetComponentInParent<EnemyInfo>();var player=other.GetComponentInParent<PlayerInfo>();
 if(enemy!=null && isEnemy || player!=null && isPlayer)return false;
 if(enemy!=null && !isEnemy){spent=true;enemy.TakeDamage(1);}else if(player!=null && !isPlayer){spent=true;player.TakeDamage(1);}else if(!other.isTrigger){spent=true;}else return false;
 foreach(var c in GetComponentsInChildren<Collider>())c.enabled=false;BallroomFeedback.Burst(transform.position,isEnemy?new Color(1,.3f,.5f):new Color(.3f,.85f,1),.12f,.2f);Destroy(gameObject);return true;}
 void OnTriggerEnter(Collider other){Hit(other);}
 // Trigger callbacks alone can miss small targets crossed in a single physics step.
 void FixedUpdate(){if(spent || BallroomGame.Blocked)return;var body=GetComponent<Rigidbody>();if(body==null || body.velocity.sqrMagnitude<.001f)return;
  var sphere=GetComponent<SphereCollider>();float radius=sphere!=null?sphere.radius*Mathf.Max(Mathf.Abs(transform.lossyScale.x),Mathf.Abs(transform.lossyScale.y),Mathf.Abs(transform.lossyScale.z)):.1f;
  var hits=Physics.SphereCastAll(body.position,radius,body.velocity.normalized,body.velocity.magnitude*Time.fixedDeltaTime,~0,QueryTriggerInteraction.Collide);
  System.Array.Sort(hits,(a,b)=>a.distance.CompareTo(b.distance));foreach(var hit in hits)if(Hit(hit.collider))break;
 }
 void OnCollisionEnter(Collision other){Hit(other.collider);}
}
