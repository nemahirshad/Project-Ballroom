using UnityEngine;
public class BallroomPresentation : MonoBehaviour {
 public Transform disco,heart;public Light leftLight,rightLight;
 void Update(){if(leftLight!=null)leftLight.intensity=.65f+.08f*Mathf.Sin(Time.unscaledTime*.65f);if(rightLight!=null)rightLight.intensity=.65f+.08f*Mathf.Sin(Time.unscaledTime*.65f+Mathf.PI);}
 void Start(){if(disco!=null && disco.GetComponent<BallroomMenuMotion>()==null){var spin=disco.gameObject.AddComponent<BallroomMenuMotion>();spin.spin=true;}if(heart!=null && heart.GetComponent<BallroomMenuMotion>()==null)heart.gameObject.AddComponent<BallroomMenuMotion>();}
}
