using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour {
 public bool isEnd;
 void Start(){var source=GetComponent<AudioSource>();if(source!=null)BallroomAudio.AdoptMusic(source);}
 public void StartGame(){Time.timeScale=1;SceneManager.LoadScene("Level 1");}
 public void SetVolume(float value){if(!Application.isPlaying)return;BallroomAudio.Ensure();BallroomAudio.Instance.SetVolume(value);}
 public void QuitGame(){Application.Quit();}
}
