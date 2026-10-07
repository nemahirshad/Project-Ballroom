using UnityEngine;
using UnityEngine.UI;
public class BallroomConfetti : MonoBehaviour {
 RectTransform[] pieces;Vector2[] velocities;float age;
 void Start(){pieces=new RectTransform[36];velocities=new Vector2[36];var random=new System.Random(44);for(int i=0;i<pieces.Length;i++){var o=new GameObject("Celebration",typeof(RectTransform),typeof(Image));o.layer=5;o.transform.SetParent(transform,false);var r=o.GetComponent<RectTransform>();r.anchorMin=r.anchorMax=new Vector2(.5f,.5f);r.sizeDelta=new Vector2(7,12);r.anchoredPosition=new Vector2((i%2==0?-600:600),-50);r.localRotation=Quaternion.Euler(0,0,random.Next(360));var image=o.GetComponent<Image>();image.raycastTarget=false;image.color=i%3==0?new Color(1,.25f,.6f):i%3==1?new Color(.35f,.78f,1):new Color(1,.83f,.35f);pieces[i]=r;velocities[i]=new Vector2((i%2==0?1:-1)*random.Next(80,350),random.Next(240,480));}}
 void Update(){age+=Time.unscaledDeltaTime;for(int i=0;i<pieces.Length;i++){velocities[i]+=Vector2.down*Time.unscaledDeltaTime*300;pieces[i].anchoredPosition+=velocities[i]*Time.unscaledDeltaTime;pieces[i].Rotate(0,0,Time.unscaledDeltaTime*95);var image=pieces[i].GetComponent<Image>();var c=image.color;c.a=Mathf.Clamp01(3-age);image.color=c;}if(age>3)Destroy(gameObject);}
}
