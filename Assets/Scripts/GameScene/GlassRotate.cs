using UnityEngine;
using System.Collections;

public class GlassRotate : MonoBehaviour {

	public float rotateAim_z;			// 目标的旋转度（z轴）
	public float distanseToCam;			// 出发旋转的距离摄像机的距离
	public Transform Camera;            // 摄像机的位置
	float distance;						// 当前距离摄像机的距离
	Quaternion q;						// 用于计算的四元数

	void Start () {
		q = Quaternion.Euler (0, 0, rotateAim_z);												// 初始化四元数
	}

	void Update () {
		distance = Vector3.Distance (Camera.position, transform.position);						// 计算物体和摄像机之间的距离
		if (distance < distanseToCam && transform.position.x > Camera.position.x) {				// 如果满足条件
			transform.rotation = Quaternion.RotateTowards (transform.rotation, q, 2.0f);		// 平缓旋转至目标状态
		}
	}

}
