using UnityEngine;
public class BallroomFeedback : MonoBehaviour {
 static Material glow;public static Material GlowMaterial { get { if(glow==null){glow=new Material(Shader.Find("Sprites/Default"));glow.name="Ballroom shared glow";}return glow; } }
 Renderer[] meshes;MaterialPropertyBlock block;LineRenderer ring;float until;Color flash;bool shield;
 void Awake(){meshes=GetComponentsInChildren<Renderer>();block=new MaterialPropertyBlock();var obj=new GameObject("Protection and hit ring");obj.transform.SetParent(transform,false);ring=obj.AddComponent<LineRenderer>();ring.useWorldSpace=false;ring.loop=true;ring.positionCount=32;ring.widthMultiplier=.065f;ring.sharedMaterial=GlowMaterial;ring.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;for(int i=0;i<32;i++){float a=i*Mathf.PI*2/32;ring.SetPosition(i,new Vector3(Mathf.Cos(a)*.75f,.05f,Mathf.Sin(a)*.75f));}ring.enabled=false;}
 public void SetShield(bool value){shield=value;}
 public void Flash(Color color){flash=color;until=Time.time+.15f;}
 void LateUpdate(){bool flashing=Time.time<until;ring.enabled=shield || flashing;Color c=flashing?flash:new Color(.3f,.75f,1,.55f+.2f*Mathf.Sin(Time.time*4));ring.startColor=ring.endColor=c;
 foreach(var r in meshes){if(r==null)continue;r.GetPropertyBlock(block);if(flashing)block.SetColor("_Color",Color.Lerp(Color.white,flash,.55f));else block.Clear();r.SetPropertyBlock(block);}}
 public static void Burst(Vector3 position,Color color,float duration=.2f,float size=.4f){var obj=new GameObject("Ballroom impact");obj.transform.position=position;var fx=obj.AddComponent<BallroomBurst>();fx.Initialize(color,duration,size);}
}
public class BallroomBurst : MonoBehaviour {
 LineRenderer lines;float started,duration,size;
 public void Initialize(Color color,float life,float radius){duration=life;size=radius;started=Time.time;lines=gameObject.AddComponent<LineRenderer>();lines.useWorldSpace=false;lines.loop=true;lines.positionCount=8;lines.sharedMaterial=BallroomFeedback.GlowMaterial;lines.startColor=color;lines.endColor=color;lines.widthMultiplier=.06f;lines.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;Destroy(gameObject,life);}
 void Update(){if(lines==null)return;float t=Mathf.Clamp01((Time.time-started)/duration);for(int i=0;i<8;i++){float a=i*Mathf.PI/4;lines.SetPosition(i,new Vector3(Mathf.Cos(a),.04f,Mathf.Sin(a))*size*(.3f+t));}Color c=lines.startColor;c.a=1-t;lines.startColor=lines.endColor=c;}
}
