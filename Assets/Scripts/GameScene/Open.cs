using UnityEngine;
using System.Collections;

// 开门的方法
public class Open : MonoBehaviour {

	public string Bit;					// 标志
	public Transform Camera;            // 摄像机的位置
	private Vector3 CameraPosition;     // 摄像机的位置
	private Vector3 DoorPosition;       // 门的位置
	private bool isBool = false;

	void Start () {
		DoorPosition = transform.position;      // 初始化门本身的坐标
	}

	void Update () {
		CameraPosition = Camera.position;		// 初始化摄像机的坐标
		float distance = Vector3.Distance (CameraPosition, DoorPosition);		// 计算门和摄像机之间的距离
		if (distance < 20 && transform.position.x > CameraPosition.x) {			// 如果距离符合看门要求
			//Debug.Log ("yes");
			if (Bit.Equals ("L")) {
				transform.Translate (Vector3.forward * Time.deltaTime * 3);
			} else if (Bit.Equals ("R")) {
				transform.Translate (Vector3.forward * Time.deltaTime * -3);
			} else if (Bit.Equals ("LU")) {
				transform.Translate (0, 3 * Time.deltaTime, 3 * Time.deltaTime);
			} else if (Bit.Equals ("RU")) {
				transform.Translate (0, 3 * Time.deltaTime, -3 * Time.deltaTime);
			} else if (Bit.Equals ("LD")) {
				transform.Translate (0, -3 * Time.deltaTime, 3 * Time.deltaTime);
			} else if (Bit.Equals ("RD")) {
				transform.Translate (0, -3 * Time.deltaTime, -3 * Time.deltaTime);
			}
		}
	}

}
