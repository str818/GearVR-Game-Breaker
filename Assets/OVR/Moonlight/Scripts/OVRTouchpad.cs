using UnityEngine;
using System;

// 接口类触摸板
public static class OVRTouchpad {

	// 触摸类型
	public enum TouchEvent {
		SingleTap,
	   	Left,
	   	Right,
	   	Up,
	   	Down,
	};

	public class TouchArgs : EventArgs {
		public TouchEvent TouchType;
	}

	// 当触碰时发生
	public static event EventHandler TouchHandler;

	// 本地触摸状态。
	enum TouchState {
		Init,
	   	Down,
	   	Stationary,
	   	Move,
	   	Up
   	};

	static TouchState touchState = TouchState.Init;
	static Vector2 moveAmount;
	static float minMovMagnitude = 100.0f; // 调整到测量点之间的点击和刷卡
	
	// 鼠标
	static Vector3 moveAmountMouse;
	static float minMovMagnitudeMouse = 25.0f;

	// 禁用未使用的变量警告
#pragma warning disable 0414
	// 确保touchpadhelper在现场开始会自动创建
	static private OVRTouchpadHelper touchpadHelper =
		(new GameObject("OVRTouchpadHelper")).AddComponent<OVRTouchpadHelper>();
#pragma warning restore 0414

	// 添加触控游戏对象到场景
	static public void Create() {
		// 只调用构造函数将游戏对象添加到场景中
	}
		
	static public void Update() {

		// 触摸板输入
		/*if (Input.touchCount > 0) {
			switch(Input.GetTouch(0).phase) {
				case(TouchPhase.Began):
					touchState = TouchState.Down;
					// Get absolute location of touch
					moveAmount = Input.GetTouch(0).position;
					break;
	
				case(TouchPhase.Moved):
					touchState = TouchState.Move;
					break;
				
				case(TouchPhase.Stationary):
					touchState = TouchState.Stationary;
					break;
				
				case(TouchPhase.Ended):
					moveAmount -= Input.GetTouch(0).position;
					HandleInput(touchState, ref moveAmount);
					touchState = TouchState.Init;
					break;
				
				case(TouchPhase.Canceled):
					Debug.Log( "CANCELLED\n" );
					touchState = TouchState.Init;
					break;				
			}
		}*/

		// 鼠标输入
		if (Input.GetMouseButtonDown(0)) {
			moveAmountMouse = Input.mousePosition;
			touchState = TouchState.Down;
		}
		else if (Input.GetMouseButtonUp(0)) {
			moveAmountMouse -= Input.mousePosition;
			HandleInputMouse(ref moveAmountMouse);
			touchState = TouchState.Init;
		}
	}

	static public void OnDisable() {
	}

	// 确定是否输入点击或刷卡并发送消息给所有的医生
	static void HandleInput(TouchState state, ref Vector2 move) {
		if ((move.magnitude < minMovMagnitude) || (touchState == TouchState.Stationary)) {
			//Debug.Log( "CLICK" );
		} else if (touchState == TouchState.Move) {
			move.Normalize();
			// 左
			if(Mathf.Abs(move.x) > Mathf.Abs (move.y)) {
				if(move.x > 0.0f) {
					//Debug.Log( "SWIPE: LEFT" );
				} else {
					//Debug.Log( "SWIPE: RIGHT" );
				}
			}
			// 右
			else {
				if(move.y > 0.0f) {
					//Debug.Log( "SWIPE: DOWN" );
				} else {
					//Debug.Log( "SWIPE: UP" );
				}
			}
		}
	}

	static void HandleInputMouse(ref Vector3 move) {
		if (move.magnitude < minMovMagnitudeMouse) {
			if (TouchHandler != null) {
				TouchHandler(null, new TouchArgs() { TouchType = TouchEvent.SingleTap });
			}
		} else {
			move.Normalize();
			// 左/右
			if (Mathf.Abs(move.x) > Mathf.Abs(move.y)) {
				if (move.x > 0.0f) {
					if (TouchHandler != null) {
						TouchHandler(null, new TouchArgs () { TouchType = TouchEvent.Left });
					}
				} else {
					if (TouchHandler != null) {
						TouchHandler(null, new TouchArgs () { TouchType = TouchEvent.Right });
					}
				}
			}
			// 上/下
			else {
				if (move.y > 0.0f) {
					if (TouchHandler != null) {
						TouchHandler(null, new TouchArgs () { TouchType = TouchEvent.Down });
					}
				} else {
					if(TouchHandler != null) {
						TouchHandler(null, new TouchArgs () { TouchType = TouchEvent.Up });
					}
				}
			}
		}
	}
}
	
// 这个单例类被创建并驻留在应用程序中。它是用来捕捉触摸板的值，使广播在“触摸板”通道任何听众。
public sealed class OVRTouchpadHelper : MonoBehaviour {
	
    private GameObject camera;
	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	void Start(){
		OVRTouchpad.TouchHandler += LocalTouchEventCallback;
	}

	void Update(){
		OVRTouchpad.Update();
		if (!UIdata.isHaveCamera) {
			camera = GameObject.Find ("Main Camera");
			UIdata.isHaveCamera = true;
		}
	}

	public void OnDisable(){
		OVRTouchpad.OnDisable();
	}

	void LocalTouchEventCallback(object sender, EventArgs args){
		var touchArgs = (OVRTouchpad.TouchArgs)args;
		OVRTouchpad.TouchEvent touchEvent = touchArgs.TouchType;

		switch(touchEvent){
		case OVRTouchpad.TouchEvent.SingleTap:									// 单机左侧按钮
			if (UIdata.whitchScene == 0) {
				if (UIdata.isTouchButton) {
                    camera.GetComponent<ButtonSound>().playSound(); //播放点选按钮音效
					switch (camera.GetComponent<MenuListener> ().Bit) {
					case 0://开始按钮
						camera.GetComponent<MenuListener> ().start ();
						break;
					case 1://第一关
						camera.GetComponent<MenuListener> ().showGuankaOne();
						break;
                    case 2://第二关
                        camera.GetComponent<MenuListener>().showGuankaTwo();
                        break;
                    case 3://选项按钮
                        camera.GetComponent<MenuListener>().soundSet();
                        break;
                    case 4://帮助
                        camera.GetComponent<MenuListener>().showTip();
                        break; 
                    case 5://音乐
                        camera.GetComponent<MenuListener>().musicHit();
                        break;
                    case 6://音效n
                        camera.GetComponent<MenuListener>().soundHit();
                        break;
                    case 7://开始第一关
                        camera.GetComponent<MenuListener> ().start ();
						break;
                    case 8://开始第二关
                        camera.GetComponent<MenuListener>().startTwo();
                        break;
					}
                }
                else{
                    camera.GetComponent<MenuListener>().soundHide();//关闭选项（声音控制）界面
                    camera.GetComponent<MenuListener>().hideTip();//隐藏提示面板
                    camera.GetComponent<MenuListener>().hideGuankaOne();//隐藏关卡一面板
                    camera.GetComponent<MenuListener>().hideGuankaTwo();//隐藏关卡二面板
                }
			}
			if (UIdata.whitchScene == 1) {
				if (UIdata.isTouchButton) {
					camera.GetComponent<ButtonSound> ().playSound ();
					switch (camera.GetComponent<BaseListener> ().Bit) {
					case 0:
						camera.GetComponent<BaseListener> ().gameStop ();
						break;
					case 1:
						camera.GetComponent<BaseListener> ().gameMenu ();
						break;
					case 2:
						camera.GetComponent<BaseListener> ().gameRestart ();
						break;
					case 3:
						camera.GetComponent<BaseListener> ().gameGoon ();
						break;
					}
				} else {
					camera.transform.GetComponent<Fire> ().fire ();
				}
			}
			break;
			
		case OVRTouchpad.TouchEvent.Left:
			//Debug.Log("LEFT SWIPE\n");
			break;

		case OVRTouchpad.TouchEvent.Right:
			//Debug.Log("RIGHT SWIPE\n");
			break;

		case OVRTouchpad.TouchEvent.Up:
			//Debug.Log("UP SWIPE\n");
			break;

		case OVRTouchpad.TouchEvent.Down:
			//Debug.Log("DOWN SWIPE\n");
			break;
		}
	}

}
