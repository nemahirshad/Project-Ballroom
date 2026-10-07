using System.Collections.Generic;
using UnityEngine;
public class Enemies : MonoBehaviour {
 public List<EnemyInfo> enemies=new List<EnemyInfo>();
 public GameObject key;
 public int Remaining { get { Clean(); return enemies.Count; } }
 public int TurretsRemaining { get { Clean(); return enemies.FindAll(e=>!e.isChaser).Count; } }
 public bool KeyDropped { get; private set; }
 void Awake(){if(enemies==null)enemies=new List<EnemyInfo>();Clean();foreach(var e in enemies){e.parent=this;e.key=key;}}
 void Start(){RefreshProtection();if(Remaining==0)DropKey(transform.position);}
 void Clean(){enemies.RemoveAll(e=>e==null || e.IsDead);}
 public void RefreshProtection(){bool vulnerable=TurretsRemaining==0;foreach(var e in enemies){e.allDead=vulnerable;e.hasKey=enemies.Count==1;}}
 public void RemoveEnemy(EnemyInfo enemy,Vector3 position){enemies.Remove(enemy);Clean();if(enemies.Count==0)DropKey(position);RefreshProtection();}
 void DropKey(Vector3 position){if(KeyDropped)return;if(key==null){Debug.LogError("Ballroom needs a key reference on the enemy manager.");return;}var pickup=key.GetComponent<Key>();if(pickup==null){Debug.LogError("Ballroom key needs its Key component.");return;}pickup.DropAt(position);KeyDropped=true;if(BallroomGame.Instance!=null)BallroomGame.Instance.ShowMessage("ARENA CLEAR / COLLECT THE KEY",3);}
}
