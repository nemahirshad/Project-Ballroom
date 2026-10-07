using UnityEngine;
public class EnemyInfo : MonoBehaviour {
 public GameObject key; public Enemies parent;public int maxHealth=2,damage=1,chasers;public bool hasKey,isChaser,allDead;
 public int Health { get; private set; } public bool IsDead { get; private set; }
 public bool Protected { get { return isChaser && parent!=null && parent.TurretsRemaining>0; } }
 BallroomFeedback feedback;
 void Awake(){Health=Mathf.Max(1,maxHealth);feedback=gameObject.AddComponent<BallroomFeedback>();}
 void Update(){if(feedback!=null)feedback.SetShield(Protected);}
 public void TakeDamage(int value){if(value<=0 || IsDead || BallroomGame.Blocked)return;if(Protected){feedback.Flash(new Color(.35f,.8f,1));BallroomAudio.Cue(BallroomAudio.Sound.Shield);return;}Health=Mathf.Max(0,Health-value);feedback.Flash(Color.white);BallroomAudio.Cue(BallroomAudio.Sound.Hit);if(Health==0)Die();}
 void Die(){if(IsDead)return;IsDead=true;if(parent!=null)parent.RemoveEnemy(this,transform.position);else if(hasKey && key!=null){key.transform.position=transform.position;key.SetActive(true);}foreach(var c in GetComponentsInChildren<Collider>())c.enabled=false;BallroomFeedback.Burst(transform.position,new Color(1,.3f,.55f));Destroy(gameObject);}
 void OnCollisionEnter(Collision collision){if(IsDead || BallroomGame.Blocked)return;var p=collision.collider.GetComponentInParent<PlayerInfo>();if(p!=null)p.TakeDamage(damage);}
}
