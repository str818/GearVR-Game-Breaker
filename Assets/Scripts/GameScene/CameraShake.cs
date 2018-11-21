using UnityEngine;
using System.Collections;

// 摄像机抖动的类
public class CameraShake : MonoBehaviour {
	
	private Vector3 originPosition;
	private Quaternion originRotation;
	public float shake_decay;
	public float shake_intensity; 			// 抖动强度(此处暂时定为0.12f)
 
   	void Update (){
      	if (shake_intensity > 0){
			transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
			transform.rotation = new Quaternion (
				originRotation.x + Random.Range (-shake_intensity, shake_intensity) * 0.2f,
				originRotation.y + Random.Range (-shake_intensity, shake_intensity) * 0.2f,
				originRotation.z + Random.Range (-shake_intensity, shake_intensity) * 0.2f,
				originRotation.w + Random.Range (-shake_intensity, shake_intensity) * 0.2f);
			shake_intensity -= shake_decay;
      	}
   	}
 
	// 抖动的方法
   	public void Shake () {
      	originPosition = transform.position;
      	originRotation = transform.rotation;
      	shake_intensity = 0.12f;
		shake_decay = 0.005f;
   	}

}
