using UnityEngine;
using System.Collections;

// UI监听的类
public class BaseListener : MonoBehaviour {

	public GameObject CenterPanel;						// 中心菜单
	public GameObject[] buttons = new GameObject[4];	// 按钮数组
    public GameObject startPanel;                     //提示面板（轻触抛球）
	public Transform centerEye;						// 摄像机中心位置
	public int Bit = -1;								// 哪一个按钮的标志

    void Start()
    {
        if(UIdata.targetScene==1)
        {
            startPanel.SetActive(true); //激活提示面板
        }
    }

	void Update () {
		Ray ray = new Ray (centerEye.position, centerEye.forward);				// 摄像机视野正前方的射线
		RaycastHit hit = new RaycastHit ();										// 实例化RaycastHit
		Bit = -1;
		for (int i = 0; i < buttons.Length; i++) {
			if (buttons [i].GetComponent<BoxCollider> ().Raycast (ray, out hit, 20.0f)) {	// 如果射线与按钮发生了碰撞
				buttons [i].GetComponent<Renderer> ().GetComponent<MeshRenderer> ().material.SetColor ("_Color", new Color (0.55f, 0.3f, 0.3f, 0.5f));	// 改变颜色
				Bit = i;														// 更新标志
				UIdata.isTouchButton = true;									// 已经点击在按钮上设为true
			} else {
				buttons [i].GetComponent<Renderer> ().GetComponent<MeshRenderer> ().material.SetColor ("_Color", new Color (1.0f, 1.0f, 1.0f, 0.8f));	// 改变颜色
			}
		}
		if (Bit == -1) {
			UIdata.isTouchButton = false;										// 已经点击在按钮上设为true
		}
	}

	// 控制游戏暂停的方法
	public void gameStop () {
		Debug.Log ("Stop");					// 打印调试信息
		if (UIdata.isGamePlaying) {			// 如果游戏正在进行
			CenterPanel.SetActive (true);	// 打开中央画布
			UIdata.isGamePlaying = false;	// 游戏进行标志位设置成false
			Time.timeScale = 0;				// 时间流速为0
		} else {							// 如果游戏没有进行
			CenterPanel.SetActive (false);	// 关闭中央画布
			UIdata.isGamePlaying = true;	// 游戏进行标志位设置成true
			Time.timeScale = 1;				// 时间流速为1
		}
	}

	// 控制游戏继续的方法
	public void gameGoon () {
		Debug.Log ("Goon");					// 打印调试信息
		CenterPanel.SetActive (false);		// 关闭中央画布
		UIdata.isGamePlaying = true;		// 游戏进行标志位设置成true
		Time.timeScale = 1;					// 时间流速为1
	}

	// 控制游戏重玩的方法
	public void gameRestart () {	
		Debug.Log ("Restart");				// 打印调试信息
		checkScore ();						// 更新得分
		UIdata.isGamePlaying = true;		// 游戏进行标志位设置成true
        UIdata.isHitedZhui = false;         // 是否打碎过棱锥
        UIdata.isDead = false;				// 游戏结束标志位设置成false
		Time.timeScale = 1;					// 时间流速为1
		CenterPanel.SetActive (false);		// 关闭中央画布
		UIdata.isTouchButton = false;		// 没点击在UI上
		System.GC.Collect ();				// 主动清理垃圾
		Application.LoadLevel(1);			// 重新加载当前场景
	}

	public void gameMenu () {
		Debug.Log ("Menu");					// 打印调试信息
		Application.LoadLevel(0);			// 进入菜单界面
		Time.timeScale = 1;					// 时间流速为1
	}

	// 更新分数的方法
	private void checkScore () {
        if(UIdata.targetScene==1)
        {
            UIdata.sco = 25;					// 设置初始分数为25
        }else
        {
            UIdata.sco = 10;                 //第二关初始分数为10
        }
		
	}

}
