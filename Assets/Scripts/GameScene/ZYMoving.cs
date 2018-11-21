using UnityEngine;
using System.Collections;

// 朝目标点左右移动的脚本
public class ZYMoving : MonoBehaviour {

	private float _pointA;				// 物体当前坐标（y值）
	public Transform PointB;			// 目标坐标
	private int _direction = 1;			// 方向

	IEnumerator Start () {
		_pointA = transform.position.z;	// 初始化物体坐标（z值）
		while (true) {		// 无限循环执行
			if (transform.position.z < _pointA) {
				_direction = 1;
			}
			if (transform.position.z > PointB.position.z) {
				_direction = -1;
			}
			transform.Translate(0,0,_direction * 5 * Time.deltaTime);
			yield return 0;
		}
	}

}