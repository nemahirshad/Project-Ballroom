using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class PlayerInfo : MonoBehaviour {
 public UnityEvent hasKey=new UnityEvent();public Slider slider;public int maxHealth=5;
 public int Health { get; private set; } public bool HasKey { get; private set; } public bool IsDead { get; private set; }
 BallroomFeedback feedback;float safeUntil;
 void Awake(){Health=Mathf.Max(1,maxHealth);feedback=gameObject.AddComponent<BallroomFeedback>();}
 void Start(){if(slider!=null){slider.maxValue=maxHealth;slider.value=Health;}}
 public void TakeDamage(int value){if(value<=0 || IsDead || BallroomGame.Blocked || Time.time<safeUntil)return;int previous=Health;Health=Mathf.Max(0,Health-value);if(BallroomGame.Instance!=null)BallroomGame.Instance.RecordDamage(previous-Health);safeUntil=Time.time+.55f;if(slider!=null)slider.value=Health;feedback.Flash(new Color(1,.25f,.5f));BallroomAudio.Cue(BallroomAudio.Sound.Hurt);if(Health==0){IsDead=true;var rb=GetComponent<Rigidbody>();if(rb!=null)rb.velocity=Vector3.zero;if(BallroomGame.Instance!=null)BallroomGame.Instance.Defeat();}}
 public bool CollectKey(GameObject item){if(HasKey || IsDead || BallroomGame.Blocked || item==null)return false;var pickup=item.GetComponentInParent<Key>();if(pickup==null || !pickup.CanCollect)return false;pickup.MarkCollected();HasKey=true;hasKey.Invoke();pickup.Spawn();pickup.gameObject.SetActive(false);Destroy(pickup.gameObject);BallroomAudio.Cue(BallroomAudio.Sound.Key);if(BallroomGame.Instance!=null)BallroomGame.Instance.ShowMessage("KEY COLLECTED / RETURN TO THE HOUSE",3);return true;}
 void OnTriggerEnter(Collider other){if(other.CompareTag("Key"))CollectKey(other.gameObject);}
}
