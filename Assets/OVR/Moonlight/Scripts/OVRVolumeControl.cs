using UnityEngine;
using System.Collections;

public class OVRVolumeControl : MonoBehaviour {
	
	private const float showPopupTime = 3;						// 显示弹出时间
	private const float	popupOffsetY = 64.0f / 500.0f;			// 弹出的Y值
	private const float	popupDepth = 1.8f;						// 弹出的远度
	private const int maxVolume = 15;							// 最大深度
	private const int numVolumeImages = maxVolume + 1;			// 图片索引值
	private Transform myTransform = null;						// 预制体自身transform
	
	void Start () {
		DontDestroyOnLoad( gameObject );						// 更换场景不删除这个音量组件
		myTransform = transform;								// 给myTransform赋值
		GetComponent<Renderer>().enabled = false;				// 开始时不显示此组件
	}

	public virtual void UpdatePosition (Transform cameraTransform) {
		// 如果听者没有初始化gettimesincelastvolumechange()将返回-1
		// 有时发生在一个框架运行在统一
		double timeSinceLastVolumeChange = OVRManager.timeSinceLastVolumeChange;					// 最后一次音量改变的时间
		if ((timeSinceLastVolumeChange != -1) && (timeSinceLastVolumeChange < showPopupTime)) {		// 如果满足弹出条件
			GetComponent<Renderer>().enabled = true;												// Renderer显示
			GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.0f, (float)(maxVolume - OVRManager.volumeLevel) / (float)numVolumeImages);		// 根据上面数据显示正确的贴图信息
			if (myTransform != null && cameraTransform != null) {
				// 在相机前的位置
				myTransform.rotation = cameraTransform.rotation;
				myTransform.position = cameraTransform.position + (myTransform.forward * popupDepth) + (myTransform.up * popupOffsetY);
			}
		} else {
			GetComponent<Renderer>().enabled = false;		// 消失
		}
	}

}
