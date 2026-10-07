using UnityEngine;
using UnityEngine.Rendering;
public static class BallroomLighting {
 public static void Apply(){
  QualitySettings.shadows=ShadowQuality.All;QualitySettings.shadowDistance=65;QualitySettings.shadowResolution=ShadowResolution.Medium;QualitySettings.shadowCascades=2;QualitySettings.antiAliasing=4;
  RenderSettings.ambientMode=AmbientMode.Trilight;RenderSettings.ambientSkyColor=new Color(.48f,.52f,.65f);RenderSettings.ambientEquatorColor=new Color(.32f,.35f,.47f);RenderSettings.ambientGroundColor=new Color(.15f,.13f,.2f);RenderSettings.ambientIntensity=1;RenderSettings.fog=false;RenderSettings.skybox=null;
  foreach(var camera in Object.FindObjectsOfType<Camera>()){camera.clearFlags=CameraClearFlags.SolidColor;camera.backgroundColor=new Color(.018f,.023f,.045f);}
  foreach(var light in Object.FindObjectsOfType<Light>())if(light.type==LightType.Directional && light.name!="Ballroom cyan fill"){
   light.color=new Color(1,.96f,.98f);light.intensity=1.15f;light.transform.rotation=Quaternion.Euler(48,-35,0);light.shadows=LightShadows.Soft;light.shadowStrength=.45f;light.shadowBias=.035f;
  }
  var fillObject=GameObject.Find("Ballroom cyan fill");if(fillObject==null)fillObject=new GameObject("Ballroom cyan fill");var fill=fillObject.GetComponent<Light>();if(fill==null)fill=fillObject.AddComponent<Light>();fill.type=LightType.Directional;fill.color=new Color(.4f,.72f,1);fill.intensity=.28f;fill.shadows=LightShadows.None;fill.transform.rotation=Quaternion.Euler(32,140,0);
 }
}
