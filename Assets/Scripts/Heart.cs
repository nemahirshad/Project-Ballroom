using UnityEngine;
public class Heart : MonoBehaviour {
 public float lifeTime=5;
 public void ShowAbove(Vector3 position){
  bool found=false;Bounds bounds=new Bounds();
  foreach(var mesh in GetComponentsInChildren<MeshFilter>(true)){
   if(mesh.sharedMesh==null)continue;var b=mesh.sharedMesh.bounds;
   for(int x=-1;x<=1;x+=2)for(int y=-1;y<=1;y+=2)for(int z=-1;z<=1;z+=2){var p=mesh.transform.TransformPoint(b.center+Vector3.Scale(b.extents,new Vector3(x,y,z)));if(!found){bounds=new Bounds(p,Vector3.zero);found=true;}else bounds.Encapsulate(p);}
  }
  if(found)transform.position+=position+Vector3.up*(bounds.extents.y+.7f)-bounds.center;
  gameObject.SetActive(true);
 }
 void OnEnable(){if(lifeTime>0)Destroy(gameObject,lifeTime);}
}
