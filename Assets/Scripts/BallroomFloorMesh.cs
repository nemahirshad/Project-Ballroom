using System.Collections.Generic;
using UnityEngine;
public static class BallroomFloorMesh {
 public static Mesh Create(Vector3 scale){
  float insetX=.25f/Mathf.Max(.01f,Mathf.Abs(scale.x)),insetZ=.25f/Mathf.Max(.01f,Mathf.Abs(scale.z)),drop=.12f/Mathf.Max(.01f,Mathf.Abs(scale.y));
  var top=new[]{new Vector3(-.5f+insetX,.5f,-.5f+insetZ),new Vector3(-.5f+insetX,.5f,.5f-insetZ),new Vector3(.5f-insetX,.5f,.5f-insetZ),new Vector3(.5f-insetX,.5f,-.5f+insetZ)};
  var rim=new[]{new Vector3(-.5f,.5f-drop,-.5f),new Vector3(-.5f,.5f-drop,.5f),new Vector3(.5f,.5f-drop,.5f),new Vector3(.5f,.5f-drop,-.5f)};
  var bottom=new[]{new Vector3(-.5f,-.5f,-.5f),new Vector3(-.5f,-.5f,.5f),new Vector3(.5f,-.5f,.5f),new Vector3(.5f,-.5f,-.5f)};
  var vertices=new List<Vector3>();var triangles=new List<int>();Quad(vertices,triangles,top[0],top[1],top[2],top[3]);
  for(int i=0;i<4;i++){int j=(i+1)%4;Quad(vertices,triangles,top[j],top[i],rim[i],rim[j]);Quad(vertices,triangles,rim[j],rim[i],bottom[i],bottom[j]);}Quad(vertices,triangles,bottom[3],bottom[2],bottom[1],bottom[0]);
  var mesh=new Mesh();mesh.name="Ballroom beveled stage";mesh.SetVertices(vertices);mesh.SetTriangles(triangles,0);mesh.RecalculateNormals();mesh.RecalculateBounds();return mesh;
 }
 static void Quad(List<Vector3> v,List<int> t,Vector3 a,Vector3 b,Vector3 c,Vector3 d){int n=v.Count;v.AddRange(new[]{a,b,c,d});t.AddRange(new[]{n,n+1,n+2,n,n+2,n+3});}
}
