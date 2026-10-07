using UnityEngine;
public class BallroomArena : MonoBehaviour {
 Camera view;Bounds floor;bool ready;Material floorMaterial;Material[] tileMaterials;Mesh tiles,generatedFloor;Vector3 authoredPosition;Quaternion authoredRotation;float authoredFieldOfView;
 public float FloorTop { get { return floor.max.y; } }
 public void Initialize(){view=Camera.main;if(view==null)view=FindObjectOfType<Camera>();Renderer ground=null;foreach(var r in FindObjectsOfType<MeshRenderer>())if(r.name=="Ground" && r.bounds.size.x>10 && r.bounds.size.z>10){ground=r;break;}if(ground==null || view==null)return;floor=ground.bounds;ready=true;authoredPosition=view.transform.position;authoredRotation=view.transform.rotation;authoredFieldOfView=view.fieldOfView;
 var filter=ground.GetComponent<MeshFilter>();if(filter!=null && filter.sharedMesh.name!="Ballroom beveled stage"){generatedFloor=BallroomFloorMesh.Create(ground.transform.lossyScale);filter.sharedMesh=generatedFloor;}
 floorMaterial=new Material(Shader.Find("Standard"));floorMaterial.name="Ballroom stage fascia";floorMaterial.color=new Color(.065f,.075f,.13f);floorMaterial.SetFloat("_Glossiness",.3f);floorMaterial.SetFloat("_Metallic",.2f);ground.material=floorMaterial;DanceFloor();
 float y=floor.max.y+.015f;
 Strip(new Vector3(floor.min.x+.15f,y,floor.center.z),new Vector3(.12f,.025f,floor.size.z-.3f),new Color(1,.2f,.6f));Strip(new Vector3(floor.max.x-.15f,y,floor.center.z),new Vector3(.12f,.025f,floor.size.z-.3f),new Color(.2f,.7f,1));Strip(new Vector3(floor.center.x,y,floor.min.z+.15f),new Vector3(floor.size.x-.3f,.025f,.12f),new Color(.5f,.4f,.75f));Strip(new Vector3(floor.center.x,y,floor.max.z-.15f),new Vector3(floor.size.x-.3f,.025f,.12f),new Color(.5f,.4f,.75f));
 view.gameObject.tag="MainCamera";Fit();}
 void DanceFloor(){
  var vertices=new System.Collections.Generic.List<Vector3>();var triangles=new[]{new System.Collections.Generic.List<int>(),new System.Collections.Generic.List<int>()};int row=0;
  for(float x=floor.min.x+.3f;x<floor.max.x-.3f;x+=2,row++){int column=0;for(float z=floor.min.z+.3f;z<floor.max.z-.3f;z+=2,column++){float right=Mathf.Min(x+2,floor.max.x-.3f),far=Mathf.Min(z+2,floor.max.z-.3f),y=floor.max.y+.006f;int n=vertices.Count;vertices.AddRange(new[]{new Vector3(x,y,z),new Vector3(x,y,far),new Vector3(right,y,far),new Vector3(right,y,z)});triangles[(row+column)%2].AddRange(new[]{n,n+1,n+2,n,n+2,n+3});}}
  tiles=new Mesh();tiles.name="Ballroom dance tiles";tiles.SetVertices(vertices);tiles.subMeshCount=2;for(int i=0;i<2;i++)tiles.SetTriangles(triangles[i],i);tiles.RecalculateNormals();tiles.RecalculateBounds();
  tileMaterials=new Material[2];for(int i=0;i<2;i++){var m=new Material(Shader.Find("Standard"));m.color=i==0?new Color(.13f,.12f,.23f):new Color(.23f,.2f,.32f);m.SetFloat("_Glossiness",.24f);m.SetFloat("_Metallic",.08f);tileMaterials[i]=m;}
  var surface=new GameObject("Ballroom dance floor");surface.transform.SetParent(transform,false);surface.AddComponent<MeshFilter>().sharedMesh=tiles;surface.AddComponent<MeshRenderer>().sharedMaterials=tileMaterials;
 }
 void Strip(Vector3 position,Vector3 scale,Color color){var o=GameObject.CreatePrimitive(PrimitiveType.Cube);o.name="Arena marking";o.transform.SetParent(transform);o.transform.position=position;o.transform.localScale=scale;var collider=o.GetComponent<Collider>();collider.enabled=false;Destroy(collider);var renderer=o.GetComponent<Renderer>();renderer.sharedMaterial=BallroomFeedback.GlowMaterial;var props=new MaterialPropertyBlock();props.SetColor("_Color",color);renderer.SetPropertyBlock(props);renderer.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;}
 // Preserve the authored perspective and angle, with a small pullback for HUD clearance.
 public void Fit(){if(!ready)return;view.orthographic=false;view.fieldOfView=authoredFieldOfView;view.transform.rotation=authoredRotation;view.aspect=(float)Screen.width/Mathf.Max(1,Screen.height);
  for(float back=2.5f;back<=8.5f;back+=.5f){view.transform.position=authoredPosition-authoredRotation*Vector3.forward*back;bool fits=true;
   foreach(float x in new[]{floor.min.x+1,floor.max.x-1})foreach(float z in new[]{floor.min.z+1,floor.max.z-1})foreach(float y in new[]{floor.max.y,floor.max.y+1.5f}){Vector3 p=view.WorldToViewportPoint(new Vector3(x,y,z));if(p.z<=0 || p.x<.03f || p.x>.97f || p.y<.10f || p.y>.915f)fits=false;}if(fits)break;
  }
 }
 void OnDestroy(){if(floorMaterial!=null)Destroy(floorMaterial);if(tileMaterials!=null)foreach(var material in tileMaterials)Destroy(material);if(tiles!=null)Destroy(tiles);if(generatedFloor!=null)Destroy(generatedFloor);}
}
