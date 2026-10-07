using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BallroomReticle : MonoBehaviour {
 Canvas canvas;RectTransform root;bool previousCursor;bool ownsCursor;
 public RectTransform Root { get { return root; } }
 public void Initialize(Canvas target) {
  canvas=target;previousCursor=Cursor.visible;
  var obj=new GameObject("Aim reticle",typeof(RectTransform));obj.layer=5;
  root=obj.GetComponent<RectTransform>();root.SetParent(canvas.transform,false);
  root.anchorMin=root.anchorMax=new Vector2(.5f,.5f);root.sizeDelta=new Vector2(40,40);
  Bar(new Vector2(-12,0),new Vector2(8,2));Bar(new Vector2(12,0),new Vector2(8,2));
  Bar(new Vector2(0,-12),new Vector2(2,8));Bar(new Vector2(0,12),new Vector2(2,8));
  Image("Center outline",root,Vector2.zero,new Vector2(6,6),new Color(.025f,.035f,.065f));
  Image("Center",root,Vector2.zero,new Vector2(3,3),new Color(1,.25f,.6f));
  root.gameObject.SetActive(false);
 }
 void Bar(Vector2 position,Vector2 size) {
  Image("Outline",root,position,size+Vector2.one*3,new Color(.025f,.035f,.065f));
  Image("Crosshair",root,position,size,new Color(.35f,.85f,1));
 }
 static void Image(string name,Transform parent,Vector2 position,Vector2 size,Color color) {
  var obj=new GameObject(name,typeof(RectTransform),typeof(Image));obj.layer=5;
  var rect=obj.GetComponent<RectTransform>();rect.SetParent(parent,false);
  rect.anchorMin=rect.anchorMax=new Vector2(.5f,.5f);rect.anchoredPosition=position;rect.sizeDelta=size;
  var graphic=obj.GetComponent<Image>();graphic.color=color;graphic.raycastTarget=false;
 }
 void LateUpdate(){Refresh(Input.mousePosition);}
 public void Refresh(Vector2 screen) {
  if(root==null || canvas==null)return;
  bool active=!BallroomGame.Blocked && screen.x>=0 && screen.y>=0 && screen.x<=Screen.width && screen.y<=Screen.height &&
   (EventSystem.current==null || !EventSystem.current.IsPointerOverGameObject());
  root.gameObject.SetActive(active);
  if(active) {
   Vector2 local;RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform,screen,canvas.renderMode==RenderMode.ScreenSpaceOverlay?null:canvas.worldCamera,out local);
   root.anchoredPosition=local;root.SetAsLastSibling();Cursor.visible=false;ownsCursor=true;
  } else if(ownsCursor){Cursor.visible=previousCursor;ownsCursor=false;}
 }
 void OnDisable(){if(root!=null)root.gameObject.SetActive(false);if(ownsCursor)Cursor.visible=previousCursor;ownsCursor=false;}
 void OnDestroy(){if(ownsCursor)Cursor.visible=previousCursor;}
}
