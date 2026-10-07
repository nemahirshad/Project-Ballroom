using UnityEngine;
public class BallroomHouseReveal : MonoBehaviour {
 const float Duration=.85f;Renderer[] originals;GameObject display;Transform visual;LineRenderer ring;float age;Vector3 basePosition,fullScale;bool started;
 public bool Finished {get;private set;}
 public static void Play(GameObject house){var reveal=house.GetComponent<BallroomHouseReveal>();if(reveal==null)reveal=house.AddComponent<BallroomHouseReveal>();reveal.Begin();}
 void Begin(){if(started)return;started=true;originals=GetComponentsInChildren<MeshRenderer>();Bounds bounds=originals.Length>0?originals[0].bounds:new Bounds(transform.position,Vector3.one);foreach(var renderer in originals)bounds.Encapsulate(renderer.bounds);
  basePosition=new Vector3(bounds.center.x,bounds.min.y,bounds.center.z);display=new GameObject("House reveal visuals");display.transform.SetParent(transform,true);display.transform.position=basePosition;visual=display.transform;fullScale=visual.localScale;
  foreach(var renderer in originals){var filter=renderer.GetComponent<MeshFilter>();if(filter==null || filter.sharedMesh==null)continue;var copy=new GameObject("Reveal "+renderer.name,typeof(MeshFilter),typeof(MeshRenderer));copy.transform.position=renderer.transform.position;copy.transform.rotation=renderer.transform.rotation;copy.transform.localScale=renderer.transform.lossyScale;copy.transform.SetParent(visual,true);copy.GetComponent<MeshFilter>().sharedMesh=filter.sharedMesh;copy.GetComponent<MeshRenderer>().sharedMaterials=renderer.sharedMaterials;renderer.enabled=false;}
  var halo=new GameObject("House welcome halo");halo.transform.SetParent(transform,true);ring=halo.AddComponent<LineRenderer>();ring.useWorldSpace=true;ring.loop=true;ring.positionCount=48;ring.widthMultiplier=.07f;ring.sharedMaterial=BallroomFeedback.GlowMaterial;ring.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;visual.localScale=Vector3.one*.15f;visual.position=basePosition-Vector3.up*1.5f;UpdateVisuals();
 }
 void Update(){if(Finished || !started)return;age+=Time.deltaTime;UpdateVisuals();if(age>=Duration){Finished=true;Restore();}}
 void UpdateVisuals(){float t=Mathf.Clamp01(age/Duration),ease=1-Mathf.Pow(1-t,3);visual.position=basePosition-Vector3.up*(1.5f*(1-ease));visual.localScale=fullScale*Mathf.Lerp(.15f,1,ease);float radius=Mathf.Lerp(.7f,3.3f,ease);Color color=Color.Lerp(new Color(1,.8f,.3f),new Color(1,.25f,.6f),t);color.a=1-t;ring.startColor=ring.endColor=color;for(int i=0;i<48;i++){float a=i*Mathf.PI*2/48;ring.SetPosition(i,basePosition+new Vector3(Mathf.Cos(a)*radius,.06f,Mathf.Sin(a)*radius));}}
 void Restore(){if(originals!=null)foreach(var renderer in originals)if(renderer!=null)renderer.enabled=true;if(display!=null){display.SetActive(false);Destroy(display);}if(ring!=null){ring.gameObject.SetActive(false);Destroy(ring.gameObject);}}
 void OnDisable(){if(started && !Finished){Finished=true;Restore();}}
}
