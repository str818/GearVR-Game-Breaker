using UnityEngine;
using System.Collections;

public class UIControl : MonoBehaviour {

    private Transform myTransform;                              //自身坐标系引用
	public Transform centerEye;                                 //摄像机中心位置
	public float fixedDepth;

	void Start () {
        myTransform = transform;                           //获取自身坐标
	}
		
	void Update () {
		myTransform.forward = centerEye.forward;            // 设置随摄像机转动而转动
		myTransform.position = centerEye.position + (centerEye.forward * fixedDepth);//设置位置跟随摄像机转动
	}

}
