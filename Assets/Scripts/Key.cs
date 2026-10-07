using UnityEngine;
using TMPro;
public class Key : MonoBehaviour {
 public GameObject house,boy;
 bool spawned,collected,dropped;float collectAt,floorY;Renderer[] visuals;Transform pivot;Vector3 resting;LineRenderer beacon;TextMeshPro label;
 public bool CanCollect { get { return dropped && !collected && Time.time>=collectAt; } }
 public Bounds VisualBounds { get { var bounds=new Bounds(transform.position,Vector3.zero);if(visuals==null || visuals.Length==0)return bounds;bounds=visuals[0].bounds;for(int i=1;i<visuals.Length;i++)if(visuals[i]!=null)bounds.Encapsulate(visuals[i].bounds);return bounds; } }
 public void DropAt(Vector3 position){
  if(dropped)return;dropped=true;collectAt=Time.time+.25f;gameObject.SetActive(true);gameObject.tag="Key";
  visuals=GetComponentsInChildren<MeshRenderer>(true);foreach(var renderer in visuals)renderer.enabled=true;
  Bounds floor=new Bounds(position,new Vector3(30,1,30));foreach(var renderer in FindObjectsOfType<MeshRenderer>())if(renderer.name=="Ground" && renderer.bounds.size.x>10 && renderer.bounds.size.z>10){floor=renderer.bounds;break;}
  floorY=floor.max.y;Vector3 center=new Vector3(Mathf.Clamp(position.x,floor.min.x+1,floor.max.x-1),floorY+.7f,Mathf.Clamp(position.z,floor.min.z+1,floor.max.z-1));
  // Imported voxel models have offset pivots; position the visible mesh above the floor.
  transform.position+=center-VisualBounds.center;
  var v=new GameObject("Visible key");pivot=v.transform;pivot.SetParent(transform);pivot.position=center;
  foreach(var renderer in visuals)renderer.transform.SetParent(pivot,true);
  pivot.Rotate(Vector3.right,65,Space.World);resting=pivot.localPosition;
  var trigger=GetComponent<BoxCollider>() ?? gameObject.AddComponent<BoxCollider>();trigger.isTrigger=true;trigger.enabled=true;trigger.center=transform.InverseTransformPoint(center);Vector3 scale=transform.lossyScale;trigger.size=new Vector3(2/Mathf.Max(.01f,Mathf.Abs(scale.x)),1.8f/Mathf.Max(.01f,Mathf.Abs(scale.y)),2/Mathf.Max(.01f,Mathf.Abs(scale.z)));
  var ring=new GameObject("Key beacon");ring.transform.SetParent(transform);beacon=ring.AddComponent<LineRenderer>();beacon.useWorldSpace=true;beacon.loop=true;beacon.positionCount=40;beacon.widthMultiplier=.06f;beacon.sharedMaterial=BallroomFeedback.GlowMaterial;beacon.startColor=beacon.endColor=new Color(1,.8f,.25f);beacon.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;
  for(int i=0;i<40;i++){float angle=i*Mathf.PI*2/40;beacon.SetPosition(i,new Vector3(center.x+Mathf.Cos(angle)*.85f,floorY+.04f,center.z+Mathf.Sin(angle)*.85f));}
  var text=new GameObject("Key label");text.transform.SetParent(transform);label=text.AddComponent<TextMeshPro>();label.font=Resources.Load<TMP_FontAsset>("Ballroom HD SDF");label.fontSize=5;label.text="KEY";label.color=new Color(1,.85f,.3f);label.alignment=TextAlignmentOptions.Center;label.rectTransform.sizeDelta=new Vector2(3,1);label.transform.localScale=Vector3.one*.5f;
 }
 void Update(){if(!dropped || collected)return;if(pivot!=null){pivot.localPosition=resting+Vector3.up*(Mathf.Sin(Time.time*2.4f)*.045f);pivot.Rotate(Vector3.up,Time.deltaTime*35,Space.World);}if(label!=null){label.transform.position=VisualBounds.center+Vector3.up*.7f;var camera=Camera.main;if(camera!=null)label.transform.rotation=camera.transform.rotation;}}
 public void MarkCollected(){collected=true;}
 public void Spawn(){if(spawned)return;spawned=true;ClearSpawnSpace();if(house!=null)house.SetActive(true);if(boy!=null)boy.SetActive(true);}
 // A disabled house has no useful Collider.bounds. Calculate its future footprint
 // before activation so the physics solver never traps or ejects the player.
 static Bounds BoxBounds(BoxCollider box){Vector3 center=box.transform.TransformPoint(box.center);var matrix=box.transform.localToWorldMatrix;Vector3 half=box.size*.5f;Vector3 x=matrix.MultiplyVector(Vector3.right*half.x),y=matrix.MultiplyVector(Vector3.up*half.y),z=matrix.MultiplyVector(Vector3.forward*half.z);return new Bounds(center,new Vector3(Mathf.Abs(x.x)+Mathf.Abs(y.x)+Mathf.Abs(z.x),Mathf.Abs(x.y)+Mathf.Abs(y.y)+Mathf.Abs(z.y),Mathf.Abs(x.z)+Mathf.Abs(y.z)+Mathf.Abs(z.z))*2);}
 void ClearSpawnSpace(){
  var player=BallroomGame.Instance!=null?BallroomGame.Instance.Player:FindObjectOfType<PlayerInfo>();if(player==null)return;
  Physics.SyncTransforms();var body=player.GetComponent<Rigidbody>();var collider=player.GetComponent<Collider>();if(body==null || collider==null)return;
  var obstacles=new System.Collections.Generic.List<Bounds>();
  foreach(var root in new[]{house,boy})if(root!=null)foreach(var box in root.GetComponentsInChildren<BoxCollider>(true))if(box.enabled && !box.isTrigger)obstacles.Add(BoxBounds(box));
  Bounds current=collider.bounds;if(!obstacles.Exists(b=>b.Intersects(current)))return;
  var candidates=new System.Collections.Generic.List<Vector3>();Vector3 padding=current.extents+Vector3.one*.4f;
  foreach(var box in obstacles){Bounds expanded=box;expanded.Expand(padding*2);candidates.Add(new Vector3(expanded.min.x,current.center.y,current.center.z));candidates.Add(new Vector3(expanded.max.x,current.center.y,current.center.z));candidates.Add(new Vector3(current.center.x,current.center.y,expanded.min.z));candidates.Add(new Vector3(current.center.x,current.center.y,expanded.max.z));foreach(float x in new[]{expanded.min.x,expanded.max.x})foreach(float z in new[]{expanded.min.z,expanded.max.z})candidates.Add(new Vector3(x,current.center.y,z));}
  candidates.Sort((a,b)=>(a-current.center).sqrMagnitude.CompareTo((b-current.center).sqrMagnitude));
  foreach(var center in candidates){Bounds proposed=new Bounds(center,current.size+Vector3.one*.1f);if(obstacles.Exists(b=>b.Intersects(proposed)))continue;
   var camera=Camera.main;if(camera!=null){Vector3 sight=center-camera.transform.position;var ray=new Ray(camera.transform.position,sight.normalized);bool hidden=false;foreach(var obstacle in obstacles){float distance;if(obstacle.IntersectRay(ray,out distance) && distance<sight.magnitude-current.extents.magnitude){hidden=true;break;}}if(hidden)continue;}
   bool blocked=false;foreach(var other in Physics.OverlapBox(center,current.extents*.95f,Quaternion.identity,~0,QueryTriggerInteraction.Ignore)){if(other.transform.IsChildOf(player.transform) || other.name=="Ground" || other.name=="Raycast")continue;blocked=true;break;}if(blocked)continue;
   Vector3 position=body.position+center-current.center;body.position=position;body.transform.position=position;body.velocity=Vector3.zero;Physics.SyncTransforms();return;
  }
 }
 void OnTriggerEnter(Collider other){Pickup(other);}
 void OnTriggerStay(Collider other){Pickup(other);}
 void Pickup(Collider other){if(!CanCollect)return;var player=other.GetComponentInParent<PlayerInfo>();if(player!=null)player.CollectKey(gameObject);}
}
