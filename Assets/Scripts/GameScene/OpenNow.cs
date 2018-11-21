using UnityEngine;
using System.Collections;

public class OpenNow : MonoBehaviour {

	public string Bit;					// 标志
	public Transform Camera;            // 摄像机的位置
	private Vector3 CameraPosition;     // 摄像机的位置

	void Update () {
		CameraPosition = Camera.position;		// 初始化摄像机的坐标
		if (transform.position.x > CameraPosition.x) {			// 如果符合要求
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
