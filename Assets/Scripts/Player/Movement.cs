using UnityEngine;
[RequireComponent(typeof(Rigidbody))] public class Movement : MonoBehaviour {
 public Camera cam; public LayerMask groundLayer;public float speed=6;
 Rigidbody rb;Vector3 move;Quaternion aim;
 void Awake(){rb=GetComponent<Rigidbody>();rb.interpolation=RigidbodyInterpolation.Interpolate;rb.constraints|=RigidbodyConstraints.FreezeRotationX|RigidbodyConstraints.FreezeRotationZ;aim=rb.rotation;}
 public static Vector3 PlanarInput(float x,float z){return Vector3.ClampMagnitude(new Vector3(x,0,z),1);}
 void Update(){if(BallroomGame.Blocked){move=Vector3.zero;return;}move=PlanarInput(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));if(cam==null)cam=Camera.main;if(cam==null)return;Ray ray=cam.ScreenPointToRay(Input.mousePosition);RaycastHit hit;Vector3 target;
 float distance;if(new Plane(Vector3.up,transform.position).Raycast(ray,out distance))target=ray.GetPoint(distance);else if(Physics.Raycast(ray,out hit,1000,groundLayer,QueryTriggerInteraction.Ignore))target=hit.point;else return;Vector3 direction=target-transform.position;direction.y=0;if(direction.sqrMagnitude>.001f)aim=Quaternion.LookRotation(direction);}
 void FixedUpdate(){if(rb==null)return;Vector3 horizontal=BallroomGame.Blocked?Vector3.zero:move*Mathf.Max(0,speed);rb.velocity=new Vector3(horizontal.x,rb.velocity.y,horizontal.z);if(!BallroomGame.Blocked)rb.MoveRotation(aim);}
}
