using UnityEngine;
using UnityEngine.SceneManagement;
public class BallroomMenuActions : MonoBehaviour {
 public GameObject main,settings,help;
 public void Play(){Time.timeScale=1;SceneManager.LoadScene("Level 1");}
 public void Quit(){Application.Quit();}
 public void MainMenu(){Time.timeScale=1;SceneManager.LoadScene("Menu");}
 void Page(GameObject page){if(main!=null)main.SetActive(page==main);if(settings!=null)settings.SetActive(page==settings);if(help!=null)help.SetActive(page==help);if(Application.isPlaying)BallroomAudio.Cue(BallroomAudio.Sound.Click);}
 public void OpenSettings(){Page(settings);}
 public void OpenHelp(){Page(help);}
 public void Back(){Page(main);}
 public void SetVolume(float value){if(Application.isPlaying){BallroomAudio.Ensure();BallroomAudio.Instance.SetVolume(value);}}
 public void ShowResults(bool value){if(Application.isPlaying)PlayerPrefs.SetInt("Ballroom.ShowResults",value?1:0);}
}
