using UnityEngine;
using System.Collections;

// 朝目标点上下移动的脚本
public class SXMoving : MonoBehaviour {

	private float _pointA;				// 物体当前坐标（y值）
	public Transform PointB;			// 目标坐标
	private int _direction = 1;			// 方向

	IEnumerator Start () {
		_pointA = transform.position.y;	// 初始化物体坐标（y值）
		while (true) {		// 无限循环执行
			if (transform.position.y < _pointA) {
				_direction = 1;
			}
			if (transform.position.y > PointB.position.y) {
				_direction = -1;
			}
			transform.Translate(0,_direction * 5 * Time.deltaTime, 0);
			yield return 0;
		}
	}

}