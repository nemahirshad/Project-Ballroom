using UnityEngine;
using UnityEngine.SceneManagement;
public class Victory : MonoBehaviour {
 public GameObject heart;public int sceneNumber;public bool isHouse;bool triggered;
 public bool Enter(PlayerInfo player){if(player==null || player.IsDead || BallroomGame.Blocked || triggered)return false;if(isHouse){if(!player.HasKey)return false;triggered=true;if(BallroomGame.Instance!=null)BallroomGame.Instance.FinishLevel();BallroomAudio.Cue(BallroomAudio.Sound.Win);Time.timeScale=1;if(sceneNumber>=0 && sceneNumber<SceneManager.sceneCountInBuildSettings)SceneManager.LoadScene(sceneNumber);else Debug.LogError("Ballroom exit needs a valid scene number.");}else{triggered=true;if(heart!=null){var display=heart.GetComponent<Heart>();var collider=GetComponent<Collider>();if(display!=null)display.ShowAbove(collider!=null?collider.bounds.center:transform.position);else heart.SetActive(true);}}return true;}
 void OnTriggerEnter(Collider other){Enter(other.GetComponentInParent<PlayerInfo>());}
}
