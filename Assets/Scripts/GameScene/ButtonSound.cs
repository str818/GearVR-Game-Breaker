using UnityEngine;
using System.Collections;

public class ButtonSound : MonoBehaviour {

	public AudioSource audio_button;		// 按按钮的音效

	// 播放音效的方法
	public void playSound () {
		audio_button.Play();				// 播放音效
	}

}
