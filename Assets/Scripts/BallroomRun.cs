public static class BallroomRun {
 static readonly float[] times=new float[3];static readonly int[] damage=new int[3];static readonly bool[] finished=new bool[3];
 public static bool Complete { get {return finished[0] && finished[1] && finished[2];} }
 public static float Seconds {get{return times[0]+times[1]+times[2];}}
 public static int Damage {get{return damage[0]+damage[1]+damage[2];}}
 public static void Begin(int level){if(level==1)for(int i=0;i<3;i++){times[i]=0;damage[i]=0;finished[i]=false;}}
 public static void Finish(int level,float seconds,int hits){if(level<1 || level>3)return;times[level-1]=seconds;damage[level-1]=hits;finished[level-1]=true;}
 public static string TimeLabel {get{int seconds=(int)Seconds;return (seconds/60).ToString("00")+":"+(seconds%60).ToString("00");}}
}
