using UnityEngine;
using System.Collections;

/**
 * 跟随目标对象
 */

public class SmoothFollow : MonoBehaviour{
	
    public GameObject target;                               //所要跟随的目标对象
    public float distance = 10.0f;                          //与目标对象的距离
    public float height = 5.0f;                             //与目标对象的高度差
    public float heightDamping = 2.0f;                      //高度变化中的阻尼参数
    public float rotationDamping = 3.0f;                    //绕y轴的旋转中的阻尼参数

	private float wantedRotationAngle;						// 摄像机期望的的旋转角度
	private float wantedHeight;								// 摄像机期望的的旋转高度
	private float currentRotationAngle;						// 摄像机当前的旋转角度
	private float currentHeight;							// 摄像机当前的旋转高度

    void LateUpdate() { 	// 如果目标对象不存在将跳出方法
        if (!target)
            return;

        // 摄像机期望的的旋转角度和高度
        wantedRotationAngle = target.transform.eulerAngles.y;
        wantedHeight = target.transform.position.y + height;

        // 摄像机当前的旋转角度及高度
        currentRotationAngle = transform.eulerAngles.y;
        currentHeight = transform.position.y;

        // 计算摄像机绕y轴的旋转角度
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // 计算摄像机高度
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // 转换成旋转角度
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // 摄像机距离目标背后的距离
        transform.position = target.transform.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        // 设置摄像机的高度
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        // 摄像机一直注视目标
        transform.LookAt(target.transform);
    }
		
}
