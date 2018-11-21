using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/**
 * 发射小球
 */
public class Fire : MonoBehaviour {

	public GameObject ballPre;					// 小球预制体
	public GameObject MainCamera;				// 主摄像机
	public Transform centerEye;					// 摄像机中心位置
	private Vector3 target;						// 触摸点的坐标
	public float speed = 18.0f;					// 小球的初始速度
	public AudioSource audio_fire;				// 发射小球的音效
	public AudioSource audio_noball_fire;		// 没有小球时发射小球的音效

	private Vector3 ballPos;					// 克隆体小球初始位置

	// 发射小球的方法
	public void fire () {
		if (UIdata.isGamePlaying) {
			if (UIdata.sco > 0) {														// 如果当前球数大于零
                Ray ray = new Ray(centerEye.position, centerEye.forward);				// 实现正前方射线
				ballPos = MainCamera.transform.position + centerEye.forward * 2.0f;		// 克隆体小球初始位置为主摄像机位置
				Rigidbody ballRi = ((GameObject)(Instantiate(ballPre, ballPos , ballPre.transform.rotation))).GetComponent<Rigidbody>();	// 克隆出一个小球（刚体）
                initBallAttribute(ballRi, ray);											// 更改参数
				ballRi.gameObject.AddComponent<DestoryBall>();                              //  不在摄像机可视范围内就干掉
                checkScore();															// 更新分数
				audio_fire.Play ();														// 播放发射音效
			} else if(UIdata.sco == 0) {												// 如果当前球数等于零
				audio_noball_fire.Play ();												// 播放发射音效
			}
		}
	}

	// 受伤时掉球的方法
	public void hertFire () {
		for (int i = 0; i < 5; i++) {
			Rigidbody ballRi = ((GameObject)(Instantiate(ballPre, new Vector3(transform.position.x - 4.0f, transform.position.y, transform.position.z), ballPre.transform.rotation))).GetComponent<Rigidbody>();	//克隆出一个小球（刚体）
			initBallAttribute2(ballRi);											// 更改参数
		}
		for (int i = 0; i < 5; i++) {
			Rigidbody ballRi = ((GameObject)(Instantiate(ballPre, ballPre.transform.position , ballPre.transform.rotation))).GetComponent<Rigidbody>();	//克隆出一个小球（刚体）
			initBallAttribute2(ballRi);											// 更改参数
		}
	}

	// 更改克隆体小球各个属性的有无得方法(为了上面的Fire())
	private void initBallAttribute (Rigidbody ballRi, Ray ray) {
		ballRi.GetComponent<Rigidbody>().useGravity = true;
		ballRi.GetComponent<SphereCollider> ().enabled = true;
		ballRi.GetComponent<MeshRenderer> ().enabled = true;
		ballRi.GetComponent<FollowNode> ().enabled = false;
		ballRi.GetComponent<CollisionEffect> ().enabled = true;
		ballRi.AddForce (new Vector3(ray.direction.x, ray.direction.y/* + 0.08f*/, ray.direction.z) * speed * 10000/*,ForceMode.Impulse*/);
	}

	// 更改克隆体小球各个属性的有无得方法(为了上面的hertFire())
	private void initBallAttribute2 (Rigidbody ballRi) {
		ballRi.GetComponent<Rigidbody>().useGravity = true;
		ballRi.GetComponent<SphereCollider> ().enabled = true;
		ballRi.GetComponent<MeshRenderer> ().enabled = true;
		ballRi.GetComponent<FollowNode> ().enabled = false;
		ballRi.GetComponent<CollisionEffect> ().enabled = true;
		ballRi.AddForce (new Vector3(Random.Range(0.5f, 0.98f), Random.Range(-0.2f, 0.3f), Random.Range(-0.5f, 0.7f)) * speed * 5000);
	}

	// 更新得分的方法
	private void checkScore () {
		UIdata.sco -= 1;
	}

	// 得到当前小球克隆体的初始位置的方法
	/*private  Vector3 getPoint(){
		Vector3 point = new Vector3 (target.x - MainCamera.transform.position.x, target.y - MainCamera.transform.position.y, target.z - MainCamera.transform.position.z) * 0.2f;
		point = new Vector3 (point.x + MainCamera.transform.position.x, MainCamera.transform.position.y, point.z + MainCamera.transform.position.z);
		return point;
	}*/



}
