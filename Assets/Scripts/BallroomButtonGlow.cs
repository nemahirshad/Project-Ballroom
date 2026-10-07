using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class BallroomButtonGlow : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,ISelectHandler,IDeselectHandler {
 bool hover;Outline glow;
 void Awake(){glow=GetComponent<Outline>();if(glow==null)glow=gameObject.AddComponent<Outline>();glow.effectDistance=new Vector2(2,-2);glow.effectColor=new Color(.35f,.78f,1,.25f);}
 void Update(){transform.localScale=Vector3.Lerp(transform.localScale,Vector3.one*(hover?1.025f:1),1-Mathf.Exp(-12*Time.unscaledDeltaTime));if(glow!=null)glow.effectColor=new Color(.35f,.78f,1,hover?.85f:.18f);}
 public void OnPointerEnter(PointerEventData e){hover=true;}public void OnPointerExit(PointerEventData e){hover=false;}public void OnSelect(BaseEventData e){hover=true;}public void OnDeselect(BaseEventData e){hover=false;}
}
