using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NineNumbers : MonoBehaviour {

	int bit = -1;					// 标志位

	void Update () {
		if (UIdata.sco > 9) {
			bit = -1;
		}
		switch (UIdata.sco) {
		case 9:
			if (bit == 9) {
				return;
			}
			transform.FindChild ("Image9").gameObject.SetActive (true);
			bit = 9;
			break;
		case 8:
			if (bit == 8) {
				return;
			}
			transform.FindChild ("Image8").gameObject.SetActive (true);
			bit = 8;
			break;
		case 7:
			if (bit == 7) {
				return;
			}
			transform.FindChild ("Image7").gameObject.SetActive (true);
			bit = 7;
			break;
		case 6:
			if (bit == 6) {
				return;
			}
			transform.FindChild ("Image6").gameObject.SetActive (true);
			bit = 6;
			break;
		case 5:
			if (bit == 5) {
				return;
			}
			transform.FindChild ("Image5").gameObject.SetActive (true);
			bit = 5;
			break;
		case 4:
			if (bit == 4) {
				return;
			}
			transform.FindChild ("Image4").gameObject.SetActive (true);
			bit = 4;
			break;
		case 3:
			if (bit == 3) {
				return;
			}
			transform.FindChild ("Image3").gameObject.SetActive (true);
			bit = 3;
			break;
		case 2:
			if (bit == 2) {
				return;
			}
			transform.FindChild ("Image2").gameObject.SetActive (true);
			bit = 2;
			break;
		case 1:
			if (bit == 1) {
				return;
			}
			transform.FindChild ("Image1").gameObject.SetActive (true);
			bit = 1;
			break;
		case 0:
			if (bit == 0) {
				return;
			}
			transform.FindChild ("Image0").gameObject.SetActive (true);
			bit = 0;
			break;
		}
	}

}
