using System.Collections.Generic;
using UnityEngine;
public class BallroomAudio : MonoBehaviour {
 public enum Sound { Shot,Hit,Shield,Hurt,Key,Win,Defeat,Click }
 public static BallroomAudio Instance { get; private set; }
 AudioSource music,effects;readonly Dictionary<Sound,AudioClip> clips=new Dictionary<Sound,AudioClip>();
 public float Volume { get; private set; }
 public static void Ensure(){if(Instance==null)new GameObject("Ballroom audio").AddComponent<BallroomAudio>();}
 void Awake(){if(Instance!=null && Instance!=this){Destroy(gameObject);return;}Instance=this;DontDestroyOnLoad(gameObject);Volume=Mathf.Clamp01(PlayerPrefs.GetFloat("Ballroom.Volume",.7f));AudioListener.volume=Volume;music=gameObject.AddComponent<AudioSource>();music.loop=true;music.volume=.213f;effects=gameObject.AddComponent<AudioSource>();effects.volume=.35f;
 var settings=Resources.Load<BallroomAudioSettings>("Ballroom Audio");if(settings!=null && settings.music!=null){music.clip=settings.music;music.Play();}
 clips[Sound.Shot]=Tone(new[]{850f,550f},.035f);clips[Sound.Hit]=Tone(new[]{400f,650f},.05f);clips[Sound.Shield]=Tone(new[]{220f,180f},.06f);clips[Sound.Hurt]=Tone(new[]{180f,100f},.12f);clips[Sound.Key]=Tone(new[]{523f,784f,1046f},.1f);clips[Sound.Win]=Tone(new[]{523f,659f,784f,1046f},.12f);clips[Sound.Defeat]=Tone(new[]{330f,220f,110f},.16f);clips[Sound.Click]=Tone(new[]{700f},.035f);}
 AudioClip Tone(float[] notes,float length){int n=Mathf.CeilToInt(44100*length*notes.Length);var samples=new float[n];for(int i=0;i<n;i++){float t=i/44100f;int note=Mathf.Min(notes.Length-1,(int)(t/length));float local=t%length;float envelope=Mathf.Sin(Mathf.PI*local/length);samples[i]=Mathf.Sin(2*Mathf.PI*notes[note]*t)*envelope*.18f;}var clip=AudioClip.Create("Ballroom cue",n,1,44100,false);clip.SetData(samples,0);return clip;}
 public static void AdoptMusic(AudioSource source){Ensure();if(source==null)return;if(Instance.music.clip==null && source.clip!=null){Instance.music.clip=source.clip;Instance.music.Play();}source.Stop();source.enabled=false;}
 public static void Cue(Sound cue){Ensure();AudioClip clip;if(Instance.clips.TryGetValue(cue,out clip))Instance.effects.PlayOneShot(clip);}
 public void SetVolume(float value){Volume=Mathf.Clamp01(value);AudioListener.volume=Volume;PlayerPrefs.SetFloat("Ballroom.Volume",Volume);}
 void OnApplicationQuit(){PlayerPrefs.Save();}
 void OnDestroy(){if(Instance!=this)return;foreach(var c in clips.Values)if(c!=null)Destroy(c);Instance=null;}
}
