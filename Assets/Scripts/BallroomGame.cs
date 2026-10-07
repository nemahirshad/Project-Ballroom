using UnityEngine;
using UnityEngine.SceneManagement;
public class BallroomGame : MonoBehaviour {
 public enum Phase { Menu,Running,Paused,Defeat,Victory }
 public static BallroomGame Instance { get; private set; }
 public static bool Blocked { get { return Instance!=null && Instance.State!=Phase.Running; } }
 public Phase State { get; private set; }
 public PlayerInfo Player { get; private set; } public Enemies Enemies { get; private set; }
 public BallroomUI UI { get; private set; }public int Level { get; private set; }
 public float LevelSeconds {get;private set;} public int DamageTaken {get;private set;} public void RecordDamage(int amount){DamageTaken+=Mathf.Max(0,amount);} public void FinishLevel(){BallroomRun.Finish(Level,LevelSeconds,DamageTaken);}
 public string Objective { get { if(Player!=null && Player.HasKey)return "KEY COLLECTED / RETURN TO THE HOUSE";if(Enemies==null)return "CLEAR THE ARENA";if(Enemies.Remaining==0)return "ARENA CLEAR / COLLECT THE KEY";if(Enemies.TurretsRemaining>0)return "DEFEAT THE TURRETS / CHASERS ARE SHIELDED";return "SHIELDS DOWN / DEFEAT THE CHASERS"; } }
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Bootstrap(){Instance=null;Time.timeScale=1;SceneManager.sceneLoaded-=Loaded;SceneManager.sceneLoaded+=Loaded;}
 static void Loaded(Scene scene,LoadSceneMode mode){if(mode==LoadSceneMode.Additive)return;if(!scene.name.StartsWith("Level ") && scene.name!="Menu" && scene.name!="Victory")return;new GameObject("Ballroom game").AddComponent<BallroomGame>();}
 void Awake(){Instance=this;Time.timeScale=1;string scene=SceneManager.GetActiveScene().name;int n;Level=scene.StartsWith("Level ") && int.TryParse(scene.Substring(6),out n)?n:0;State=Level>0?Phase.Running:scene=="Victory"?Phase.Victory:Phase.Menu;if(Level>0)BallroomRun.Begin(Level);BallroomAudio.Ensure();}
 void Start(){BallroomLighting.Apply();Player=FindObjectOfType<PlayerInfo>();Enemies=FindObjectOfType<Enemies>();foreach(var source in FindObjectsOfType<AudioSource>())if(source.clip!=null && source.loop && source.gameObject!=BallroomAudio.Instance.gameObject)BallroomAudio.AdoptMusic(source);
 if(Level>0){gameObject.AddComponent<BallroomArena>().Initialize();BallroomDifficulty.Apply(this);}UI=gameObject.AddComponent<BallroomUI>();UI.Initialize(this);if(Level>0)gameObject.AddComponent<BallroomReticle>().Initialize(UI.Canvas);if(State==Phase.Victory)BallroomAudio.Cue(BallroomAudio.Sound.Win);}
 void Update(){if(State==Phase.Running)LevelSeconds+=Time.deltaTime;if(Input.GetKeyDown(KeyCode.Escape)){if(UI!=null && UI.HandleEscape())return;if(Level>0)TogglePause();}}
 public void TogglePause(){if(State!=Phase.Running && State!=Phase.Paused)return;State=State==Phase.Running?Phase.Paused:Phase.Running;Time.timeScale=State==Phase.Paused?0:1;if(UI!=null)UI.ShowState();}
 public void Defeat(){if(State!=Phase.Running)return;State=Phase.Defeat;Time.timeScale=0;BallroomAudio.Cue(BallroomAudio.Sound.Defeat);if(UI!=null)UI.ShowState();}
 public void Load(string name){Time.timeScale=1;BallroomAudio.Cue(BallroomAudio.Sound.Click);SceneManager.LoadScene(name);}
 public void Retry(){Load(SceneManager.GetActiveScene().name);}
 public void ShowMessage(string text,float duration){if(UI!=null)UI.ShowMessage(text,duration);}
 void OnDestroy(){if(Instance==this){Instance=null;Time.timeScale=1;}}
}
