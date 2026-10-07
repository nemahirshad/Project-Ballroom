using UnityEngine;
public class BallroomArenaDecor : MonoBehaviour {
 Material dark;public void Initialize(Bounds floor){dark=new Material(Shader.Find("Standard"));dark.color=new Color(.045f,.055f,.1f);dark.SetFloat("_Glossiness",.3f);
  // All geometry sits outside the arena and has no collider.
  foreach(float x in new[]{floor.min.x-1.25f,floor.max.x+1.25f})foreach(float z in new[]{floor.min.z+2.5f,floor.max.z-2.5f}){
   Color accent=x<floor.center.x?new Color(1,.25f,.6f):new Color(.35f,.78f,1);Vector3 foot=new Vector3(x,floor.max.y,z);
   Shape("Arena speaker",PrimitiveType.Cube,foot+Vector3.up*.7f,new Vector3(.85f,1.4f,.8f),dark,Quaternion.identity);
   for(int i=0;i<2;i++)Shape("Speaker cone",PrimitiveType.Cylinder,foot+new Vector3(0,.4f+i*.55f,-.42f),new Vector3(.5f,.025f,.5f),dark,Quaternion.Euler(90,0,0));
   Glow("Speaker trim",foot+new Vector3(0,1.23f,-.43f),new Vector3(.65f,.045f,.03f),accent);
   Glow("Neon pedestal",foot+Vector3.down*.1f,new Vector3(1.1f,.08f,1.05f),accent);
  }
  for(int i=0;i<5;i++){float x=Mathf.Lerp(floor.min.x+2,floor.max.x-2,i/4f);Vector3 foot=new Vector3(x,floor.max.y,floor.max.z+1.2f);Shape("Ballroom light column",PrimitiveType.Cube,foot+Vector3.up*.75f,new Vector3(.18f,1.5f,.18f),dark,Quaternion.identity);Glow("Light column glow",foot+Vector3.up*.85f,new Vector3(.08f,1.35f,.2f),i%2==0?new Color(1,.25f,.6f):new Color(.35f,.78f,1));}
  // A restrained back rail frames the stage without crossing the foreground.
  Glow("Rear ballroom rail",new Vector3(floor.center.x,floor.max.y+.12f,floor.max.z+1.2f),new Vector3(floor.size.x-3,.05f,.06f),new Color(.48f,.35f,.7f));
 }
 GameObject Shape(string name,PrimitiveType type,Vector3 position,Vector3 scale,Material material,Quaternion rotation){var o=GameObject.CreatePrimitive(type);o.name=name;o.transform.SetParent(transform);o.transform.position=position;o.transform.rotation=rotation;o.transform.localScale=scale;var collider=o.GetComponent<Collider>();collider.enabled=false;Destroy(collider);var renderer=o.GetComponent<MeshRenderer>();renderer.sharedMaterial=material;renderer.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;return o;}
 void Glow(string name,Vector3 position,Vector3 scale,Color color){var o=Shape(name,PrimitiveType.Cube,position,scale,BallroomFeedback.GlowMaterial,Quaternion.identity);var block=new MaterialPropertyBlock();block.SetColor("_Color",color);o.GetComponent<MeshRenderer>().SetPropertyBlock(block);}
 void OnDestroy(){if(dark!=null)Destroy(dark);}
}
