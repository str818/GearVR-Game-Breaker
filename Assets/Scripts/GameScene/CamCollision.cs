using UnityEngine;
using System.Collections;

// 检测摄像机碰撞的类
public class CamCollision : MonoBehaviour {

	public AudioSource audio_alert;		// 警告音效
	Fire fire = null;					// 获取Fire
	CameraShake cs = null;				// 获取CameraShake
	bool isToGlass = false;				// 是否撞到玻璃的标志位
	bool isToDoor= false;				// 是否撞到门的标志位

	void Start () {
		cs = GameObject.Find ("Main Camera").GetComponent<CameraShake> ();		// 初始化CameraShake
		fire = GameObject.Find ("Main Camera").GetComponent<Fire> ();			// Fire
	}

	// 检测摄像机撞击到障碍物的方法
	void OnTriggerEnter(Collider collider) {
        Debug.Log(collider.name);
		if (!UIdata.isDead) {
			string name = collider.gameObject.name;		// 得到碰撞物体的名字
			if (name.Equals("Glass") && !isToGlass) {				// 如果撞到障碍玻璃
				cs.Shake ();					// 屏幕抖动一次
				audio_alert.Play ();			// 播放警告音效
				checkScore();				// 更改分数
				isToGlass = true;			// 是否撞到玻璃的标志位为true
				UIdata.isInjured = true; 	// 受伤效果标志位置为true
				fire.hertFire();			// 受伤射击效果
				StartCoroutine (s (2.5f));	// 2.5秒后才可以再次与障碍玻璃发生碰撞
			} else if (name.Substring (0, 4).Equals ("Cube") && !isToDoor) {
				cs.Shake ();					// 屏幕抖动一次
				audio_alert.Play ();			// 播放警告音效
				checkScore();				// 更改分数
				isToDoor = true;			// 是否撞到玻璃的标志位为true
				UIdata.isInjured = true; 	// 受伤效果标志位置为true
				fire.hertFire();			// 受伤射击效果
				StartCoroutine (s (2.5f));	// 2.5秒后才可以再次与障碍玻璃发生碰撞
			}
		}
	}

	// 更改分数的方法
	private void checkScore () {
		if (UIdata.sco >= 10) {
			UIdata.sco -= 10;
		} else {
			UIdata.sco = 0;
		}
	}

	// 等待几秒后恢复可以再次碰撞
	IEnumerator s (float seconds) {
		yield return new WaitForSeconds (seconds);
		isToGlass = false;				// 是否撞到玻璃的标志位为false
		isToDoor = false;				// 是否撞到门的标志位为false
	}

}
