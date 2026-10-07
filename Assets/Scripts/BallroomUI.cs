using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class BallroomUI : MonoBehaviour {
 BallroomGame game;Canvas canvas;TMP_FontAsset font;TextMeshProUGUI objective,count,message,healthText;Image[] hearts;GameObject modal;float messageUntil;string notice;bool confirming;BallroomMenuActions menuActions;
 readonly Color navy=new Color(.035f,.045f,.09f,.96f),pink=new Color(1,.25f,.6f),blue=new Color(.35f,.78f,1),white=new Color(.94f,.96f,1);
 public Canvas Canvas { get { return canvas; } }
 public void Initialize(BallroomGame owner){game=owner;font=Resources.Load<TMP_FontAsset>("Ballroom HD SDF");foreach(var old in FindObjectsOfType<Canvas>()){old.enabled=false;var ray=old.GetComponent<GraphicRaycaster>();if(ray!=null)ray.enabled=false;}
 if(FindObjectOfType<EventSystem>()==null){var e=new GameObject("Ballroom event system");e.AddComponent<EventSystem>();e.AddComponent<StandaloneInputModule>();}
 var root=new GameObject("Ballroom HUD",typeof(RectTransform),typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster));root.layer=5;root.transform.SetParent(transform,false);canvas=root.GetComponent<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceOverlay;canvas.sortingOrder=100;var scale=root.GetComponent<CanvasScaler>();scale.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;scale.referenceResolution=new Vector2(1920,1080);scale.matchWidthOrHeight=.5f;
 if(game.Level>0){var bar=Panel("Status bar",root.transform,navy,.04f,.935f,.96f,.985f);Label("Level",bar,"<color=#FF4099>BALL</color><color=#59C7FF>ROOM</color> / LEVEL "+game.Level,24,white,.02f,.10f,.34f,.90f);count=Label("Enemy count",bar,"",22,blue,.35f,.10f,.65f,.90f);count.alignment=TextAlignmentOptions.Center;
 healthText=Label("Health value",bar,"",20,white,.65f,.15f,.76f,.85f);hearts=new Image[Mathf.Max(1,game.Player!=null?game.Player.maxHealth:5)];float step=.22f/hearts.Length;for(int i=0;i<hearts.Length;i++){var h=Panel("Health "+i,bar,pink,.77f+i*step,.24f,.77f+(i+.8f)*step,.77f);hearts[i]=h.GetComponent<Image>();hearts[i].sprite=HeartSprite;hearts[i].preserveAspect=true;}
 var obj=Panel("Objective bar",root.transform,navy,.12f,.025f,.88f,.08f);objective=Label("Objective",obj,"",22,white,.025f,.10f,.975f,.90f);objective.alignment=TextAlignmentOptions.Center;
 var foot=Panel("Action banner",root.transform,navy,.12f,.025f,.88f,.08f);message=Label("Action",foot,"",22,blue,.02f,.1f,.98f,.9f);message.alignment=TextAlignmentOptions.Center;foot.gameObject.SetActive(false);}
 ShowState();}
 RectTransform Panel(string name,Transform parent,Color color,float x1,float y1,float x2,float y2){var o=new GameObject(name,typeof(RectTransform),typeof(Image));o.layer=5;o.transform.SetParent(parent,false);var r=o.GetComponent<RectTransform>();r.anchorMin=new Vector2(x1,y1);r.anchorMax=new Vector2(x2,y2);r.offsetMin=r.offsetMax=Vector2.zero;o.GetComponent<Image>().color=color;return r;}
 TextMeshProUGUI Label(string name,Transform parent,string text,float size,Color color,float x1,float y1,float x2,float y2){var o=new GameObject(name,typeof(RectTransform),typeof(TextMeshProUGUI));o.layer=5;o.transform.SetParent(parent,false);var r=o.GetComponent<RectTransform>();r.anchorMin=new Vector2(x1,y1);r.anchorMax=new Vector2(x2,y2);r.offsetMin=r.offsetMax=Vector2.zero;var label=o.GetComponent<TextMeshProUGUI>();label.font=font;label.fontSize=size;label.color=color;label.text=text;label.alignment=TextAlignmentOptions.MidlineLeft;label.raycastTarget=false;label.enableWordWrapping=false;label.overflowMode=TextOverflowModes.Ellipsis;return label;}
 void Center(string name,Transform parent,string text,float size,Color color,float x1,float y1,float x2,float y2){Label(name,parent,text,size,color,x1,y1,x2,y2).alignment=TextAlignmentOptions.Center;}
 void Button(string name,Transform parent,string text,Color color,float y1,float y2,UnityEngine.Events.UnityAction clicked){var r=Panel(name,parent,color,.18f,y1,.82f,y2);var b=r.gameObject.AddComponent<Button>();var colors=b.colors;colors.normalColor=Color.white;colors.highlightedColor=new Color(.85f,.9f,1);colors.pressedColor=new Color(.65f,.75f,.85f);colors.selectedColor=Color.white;b.colors=colors;b.onClick.AddListener(clicked);r.gameObject.AddComponent<BallroomButtonGlow>();Center(name+" label",r,text,28,navy,.025f,.1f,.975f,.9f);}
 void Volume(Transform parent,float y){Center("Volume label",parent,"VOLUME",17,blue,.18f,y,.39f,y+.055f);var track=Panel("Volume slider",parent,new Color(.18f,.23f,.36f),.42f,y+.017f,.82f,y+.036f);var slider=track.gameObject.AddComponent<Slider>();slider.minValue=0;slider.maxValue=1;var fill=Panel("Fill",track,blue,0,0,1,1);var handle=Panel("Handle",track,pink,0,-.7f,0,1.7f);handle.sizeDelta=new Vector2(15,0);slider.fillRect=fill;slider.handleRect=handle;slider.targetGraphic=handle.GetComponent<Image>();slider.value=BallroomAudio.Instance!=null?BallroomAudio.Instance.Volume:.7f;slider.onValueChanged.AddListener(value=>{if(Application.isPlaying){BallroomAudio.Ensure();BallroomAudio.Instance.SetVolume(value);}});}
 public bool ConfirmationOpen {get{return confirming;}}
 public bool HandleEscape(){if(confirming){ShowState();return true;}if(menuActions!=null && menuActions.main!=null && !menuActions.main.activeSelf){menuActions.Back();return true;}return false;}
 public void RequestLeave(string destination){
  if(game.State!=BallroomGame.Phase.Paused && game.State!=BallroomGame.Phase.Running){game.Load(destination);return;}
  if(game.State==BallroomGame.Phase.Running)game.TogglePause();
  if(modal!=null){modal.SetActive(false);Destroy(modal);}confirming=true;
  var shade=Panel("Leave confirmation",canvas.transform,new Color(.01f,.02f,.05f,.8f),0,0,1,1);modal=shade.gameObject;var card=Card(shade,.32f,.27f,.68f,.73f);
  Center("Leave title",card,destination=="Menu"?"LEAVE THIS DANCE?":"RESTART THIS LEVEL?",36,white,.04f,.72f,.96f,.91f);
  Center("Leave detail",card,destination=="Menu"?"Progress in this level will be lost.\nYou can start a new dance from the menu.":"Progress in this level will be lost.\nCompleted levels remain in this run.",23,blue,.04f,.48f,.96f,.68f);
  Button("Confirm leave",card,destination=="Menu"?"LEAVE TO MAIN MENU":"RESTART LEVEL",pink,.26f,.40f,()=>game.Load(destination));Button("Cancel leave",card,"KEEP PLAYING",blue,.07f,.21f,ShowState);
 }
 RectTransform Card(Transform parent,float x1,float y1,float x2,float y2){var card=Panel("Ballroom menu",parent,navy,x1,y1,x2,y2);Panel("Pink accent",card,pink,0,.984f,1,1);Panel("Blue accent",card,blue,0,0,1,.016f);return card;}
 public void ShowState(){confirming=false;menuActions=null;if(modal!=null){modal.SetActive(false);if(Application.isPlaying)Destroy(modal);else DestroyImmediate(modal);modal=null;}var state=game.State;if(!Application.isPlaying && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name=="Victory")state=BallroomGame.Phase.Victory;if(state==BallroomGame.Phase.Running)return;
 var shade=Panel("Modal shade",canvas.transform,new Color(.025f,.03f,.065f,game.Level>0?.7f:0),0,0,1,1);modal=shade.gameObject;
 if(state==BallroomGame.Phase.Menu){
  Center("Menu title",shade,"<color=#FF4099>BALL</color><color=#59C7FF>ROOM</color>",78,white,.06f,.86f,.94f,.97f);
  Center("Tagline",shade,"Clear the floor. Find the key. Bring them together.",25,white,.06f,.79f,.94f,.86f);
  Center("Brie name",shade,"BRIE  <color=#59C7FF>+</color>  JACK",28,pink,.07f,.15f,.51f,.22f);
  Center("Menu goal",shade,"A LITTLE BRAVERY. A LITTLE HEART.",21,blue,.035f,.07f,.54f,.14f);
  var main=Card(shade,.58f,.16f,.94f,.77f);var settings=Card(shade,.58f,.16f,.94f,.77f);var help=Card(shade,.58f,.16f,.94f,.77f);
  menuActions=shade.gameObject.AddComponent<BallroomMenuActions>();menuActions.main=main.gameObject;menuActions.settings=settings.gameObject;menuActions.help=help.gameObject;settings.gameObject.SetActive(false);help.gameObject.SetActive(false);
  Center("Menu invitation",main,"YOUR NEXT DANCE",29,blue,.06f,.84f,.94f,.95f);
  Button("Start",main,"PLAY",pink,.65f,.78f,menuActions.Play);Button("Settings",main,"SETTINGS",blue,.46f,.59f,menuActions.OpenSettings);Button("How to play",main,"HOW TO PLAY",blue,.27f,.40f,menuActions.OpenHelp);Button("Quit",main,"QUIT",blue,.08f,.21f,menuActions.Quit);
  Center("Settings title",settings,"MAKE YOURSELF AT HOME",30,white,.05f,.78f,.95f,.94f);Volume(settings,.57f);
  var toggleRoot=Panel("Show results",settings,new Color(.15f,.2f,.3f),.18f,.38f,.245f,.45f);var toggle=toggleRoot.gameObject.AddComponent<Toggle>();var check=Panel("Results check",toggleRoot,pink,.18f,.18f,.82f,.82f);toggle.targetGraphic=toggleRoot.GetComponent<Image>();toggle.graphic=check.GetComponent<Image>();toggle.isOn=PlayerPrefs.GetInt("Ballroom.ShowResults",1)!=0;toggle.onValueChanged.AddListener(menuActions.ShowResults);Center("Results option",settings,"SHOW RUN RESULTS",23,white,.27f,.36f,.85f,.47f);Center("Settings note",settings,"Time and damage taken on the victory screen.",18,blue,.05f,.27f,.95f,.35f);Button("Back settings",settings,"BACK",pink,.08f,.21f,menuActions.Back);
  Center("Help title",help,"HOW TO PLAY",35,white,.06f,.79f,.94f,.94f);Center("Controls",help,"WASD / MOVE\nMOUSE / AIM\nHOLD LEFT CLICK / FIRE\nESC / PAUSE",25,white,.05f,.43f,.95f,.76f);Center("Help goal",help,"Clear red turrets to break green shields.\nDefeat the last enemy and collect the key.\nReach the house to reunite Brie and Jack.",21,blue,.025f,.25f,.975f,.43f);Button("Back help",help,"BACK",pink,.08f,.21f,menuActions.Back);
 }
 else if(state==BallroomGame.Phase.Victory){
  Center("Menu title",shade,"TOGETHER AT LAST",65,white,.06f,.86f,.94f,.97f);Center("Victory description",shade,"ALL THREE ARENAS CLEARED",25,blue,.06f,.79f,.94f,.86f);
  var card=Card(shade,.58f,.16f,.94f,.77f);Center("Victory celebration",card,"YOU BROUGHT THEM HOME",29,pink,.05f,.81f,.95f,.93f);
  var h=Panel("Victory heart",card,pink,.43f,.61f,.57f,.77f);h.GetComponent<Image>().sprite=HeartSprite;h.GetComponent<Image>().preserveAspect=true;h.GetComponent<Image>().raycastTarget=false;if(Application.isPlaying)h.gameObject.AddComponent<BallroomMenuMotion>();
  string result=PlayerPrefs.GetInt("Ballroom.ShowResults",1)==0?"A LITTLE BRAVERY. A LITTLE HEART.":BallroomRun.Complete?"TIME  "+BallroomRun.TimeLabel+"    /    DAMAGE TAKEN  "+BallroomRun.Damage:"Your next completed run's results appear here.";
  Center("Run results",card,result,22,white,.04f,.47f,.96f,.59f);Button("Replay",card,"PLAY AGAIN",pink,.28f,.41f,()=>game.Load("Level 1"));Button("Main menu",card,"MAIN MENU",blue,.09f,.22f,()=>game.Load("Menu"));
  if(Application.isPlaying){var confetti=new GameObject("Victory confetti",typeof(RectTransform),typeof(BallroomConfetti));confetti.layer=5;confetti.transform.SetParent(shade,false);var r=confetti.GetComponent<RectTransform>();r.anchorMin=Vector2.zero;r.anchorMax=Vector2.one;r.offsetMin=r.offsetMax=Vector2.zero;confetti.transform.SetAsFirstSibling();}
 }
 else{
  var card=Card(shade,.32f,.18f,.68f,.82f);Center("Menu title",card,state==BallroomGame.Phase.Defeat?"TRY AGAIN":"PAUSED",44,white,.04f,.79f,.96f,.95f);
  if(state==BallroomGame.Phase.Defeat){Center("Defeat description",card,"LEVEL "+game.Level+" / YOU CAN DO THIS",25,blue,.05f,.65f,.95f,.77f);Center("Defeat tip",card,"Clear turrets to break chaser shields.\nKeep moving and avoid contact.",22,white,.025f,.45f,.975f,.64f);Button("Retry",card,"RETRY LEVEL",pink,.26f,.39f,game.Retry);Button("Main menu",card,"MAIN MENU",blue,.08f,.21f,()=>game.Load("Menu"));}
  else{Center("Pause description",card,"TAKE A BREATH",24,blue,.05f,.70f,.95f,.78f);Button("Resume",card,"RESUME",pink,.53f,.66f,game.TogglePause);Button("Retry",card,"RESTART LEVEL",blue,.35f,.48f,()=>RequestLeave(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name));Button("Main menu",card,"MAIN MENU",blue,.17f,.30f,()=>RequestLeave("Menu"));Volume(card,.045f);}
 }
 }

 public void ShowMessage(string text,float seconds){notice=text;messageUntil=Time.unscaledTime+seconds;}
 void Update(){if(game==null || game.Level==0)return;if(objective!=null)objective.text=game.Objective;if(count!=null)count.text="ENEMIES / "+(game.Enemies!=null?game.Enemies.Remaining:0);if(healthText!=null && game.Player!=null)healthText.text="HP "+game.Player.Health+" / "+game.Player.maxHealth;if(hearts!=null)for(int i=0;i<hearts.Length;i++)hearts[i].color=game.Player!=null && i<game.Player.Health?pink:new Color(.25f,.27f,.34f);if(message!=null){bool announcing=Time.unscaledTime<messageUntil;message.transform.parent.gameObject.SetActive(announcing);if(objective!=null)objective.transform.parent.gameObject.SetActive(!announcing);message.text=announcing?notice:"";}}
 static Sprite heart;public static Sprite HeartSprite { get { if(heart!=null)return heart;int n=32;var texture=new Texture2D(n,n,TextureFormat.RGBA32,false);texture.filterMode=FilterMode.Point;var data=new Color[n*n];for(int y=0;y<n;y++)for(int x=0;x<n;x++){float a=(x/(n-1f)*2-1)*1.2f,b=(y/(n-1f)*2-1)*1.2f;float v=a*a+b*b-1;data[y*n+x]=v*v*v-a*a*b*b*b<=0?Color.white:Color.clear;}texture.SetPixels(data);texture.Apply();heart=Sprite.Create(texture,new Rect(0,0,n,n),new Vector2(.5f,.5f),32);heart.name="Ballroom heart";return heart; } }
}
