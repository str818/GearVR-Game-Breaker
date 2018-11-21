using UnityEngine;
using System.Collections;

// 完成屏幕自适应的方法
public class InitUI : MonoBehaviour {

	void Start () {
		InitUi ();				// 初始化屏幕UI，完成屏幕自适应
	}

	private void InitUi() {                                             //UI按钮屏幕自适应方法
		Vector2 editScreen = new Vector2(800, 480);
		Transform canvas = GameObject.Find("Canvas").transform;         //在Canvas下的对象将进行位置和大小的调整
		Vector2 scaleExchange = new Vector2(Screen.width / editScreen.x, Screen.height / editScreen.y);
		for (int i = 0; i < canvas.childCount; i++) {
			RectTransform canvasChildRT = canvas.GetChild(i).GetComponent<RectTransform>();
			canvasChildRT.position = new Vector3(scaleExchange.x * canvasChildRT.position.x,    //调整其位置
				scaleExchange.y * canvasChildRT.position.y, 0);
			canvasChildRT.sizeDelta = new Vector3(scaleExchange.x * canvasChildRT.sizeDelta.x,    //调整其大小
				scaleExchange.y * canvasChildRT.sizeDelta.y, 1);
		}
	}

}
