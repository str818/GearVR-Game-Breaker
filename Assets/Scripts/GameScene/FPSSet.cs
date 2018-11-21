using UnityEngine;
using System.Collections;

// 用来设置FPS的脚本
public class FPSSet : MonoBehaviour {

	public int fps;							// 帧数变量

	// 初始化函数，在游戏开始时系统自动调用。一般用来创建变量之类的东西。 
    void Awake() {
		Application.targetFrameRate = fps;			// 设置帧数大小为为fps
	}

	// 初始化函数，在所有Awake函数运行完之后（一般是这样，但不一定），在所有Update函数前系统自动条用。一般用来给变量赋值。
	void Start () {
		UIdata.isHaveCamera = false;
		UIdata.whitchScene = 1;
	}

}
