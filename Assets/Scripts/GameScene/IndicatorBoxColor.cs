using UnityEngine;
using System.Collections;

public class IndicatorBoxColor : MonoBehaviour {
	
    public Material material;

    float minAlpha = 0.5f;
    float maxAlpha = 1.0f;
    float varifySpeed = 0.4f;
    public float curAlpha = 0.5f;

	void Awake () {
	     material = gameObject.GetComponent<Renderer>().material;
        if (material == null) print("托盘位置提示box颜色控制脚本无法进行，找不到托盘指示box的Material");
	}
	

	void Update () {
        if (material == null) return;
		/*if (curAlpha < minAlpha)
			Destroy (this.gameObject, 0.5f);*/
        curAlpha += Time.deltaTime * varifySpeed;
        if (curAlpha > maxAlpha) varifySpeed *= -1;
		curAlpha = Mathf.Clamp(curAlpha, 0.0f, maxAlpha);
        Color c = material.color;
        c.a = curAlpha;
        material.color= c;
	}

}
