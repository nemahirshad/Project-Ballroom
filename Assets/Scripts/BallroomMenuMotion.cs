using UnityEngine;
public class BallroomMenuMotion : MonoBehaviour {
 public bool spin;public float amplitude=.05f;Vector3 position,scale;
 void Start(){position=transform.localPosition;scale=transform.localScale;}
 void Update(){float t=Time.unscaledTime;if(spin)transform.Rotate(Vector3.up,Time.unscaledDeltaTime*12,Space.Self);else{transform.localPosition=position+Vector3.up*Mathf.Sin(t*1.8f)*amplitude;transform.localScale=scale*(1+.035f*Mathf.Sin(t*2.4f));}}
}
