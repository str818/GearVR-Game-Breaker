using UnityEngine;
using System.Collections;

public class UIdata {

	public static int sco = 25;					// 储存成绩
	public static int whitchScene = 0;
    public static int targetScene = 1;         //目标关卡（默认为第一关）
	public static bool isGamePlaying = true;	// 游戏是否正在运行的标志位
	public static bool isInjured = false;		// 是否受伤
	public static bool isDead = false;			// 是否死亡
	public static bool isTouchButton = false;	// 是否点击在Button上
	public static bool isHaveCamera = false;
    public static bool isHitedZhui = false;		// 是否打碎过棱锥	
    public static int soundIndex=3;           //音效等级
    public static int musicIndex=3;           //音乐等级
	public static float musicVolume = 1.0f;		// 音量大小因数
	public static float soundVolume = 1.0f;		// 音效大小因数
    public static float[] volumes = new float[]{  //音量数组
                0.01f,//击打cube        
                0.15f,//背景音乐，按钮音效，发射小球，无小球，击打开门机关
                0.3f//警告碰撞，击打棱锥，击打玻璃，击打开门机关，死亡音效
                };           

	public static void init () {
		sco = 25;					// 储存成绩
		isGamePlaying = true;		// 游戏是否正在运行的标志位
		isInjured = false;			// 是否受伤
		isDead = false;				// 是否死亡
		isTouchButton = false;		// 是否点击在Button上
        isHitedZhui = false;		    // 是否打碎过棱锥	
	}

    //设置关卡为第一关
    public static void setFirst()
    {
        targetScene = 1;
    }

    //设置关卡为第二关
    public static void setSecond()
    {
        targetScene = 2;
    }
}
