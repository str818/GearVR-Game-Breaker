using UnityEngine;
using System.Runtime.InteropServices;		// DllImport要求

public class OVRChromaticAberration : MonoBehaviour {
	
	public OVRGamepadController.Button toggleButton = OVRGamepadController.Button.X;
	private bool chromatic = false;

#if (UNITY_ANDROID && !UNITY_EDITOR)
	[DllImport("OculusPlugin")]
	private static extern void OVR_TW_EnableChromaticAberration( bool enable );
#endif

	// 启动此实例
	void Start () {
#if (UNITY_ANDROID && !UNITY_EDITOR)
		// Enable/Disable Chromatic Aberration Correction.
		// NOTE: Enabling Chromatic Aberration for mobile has a large performance cost.
		OVR_TW_EnableChromaticAberration(chromatic);
#endif
	}

	/// <summary>
	/// Check input and toggle chromatic aberration correction if necessary.
	/// See the input mapping setup in the Unity Integration guide.
	/// </summary>
	void Update()
	{
		// NOTE: some of the buttons defined in OVRGamepadController.Button are not available on the Android game pad controller
		if (Input.GetButtonDown(OVRGamepadController.ButtonNames[(int)toggleButton]))
		{
			//*************************
			// toggle chromatic aberration correction
			//*************************
			chromatic = !chromatic;
#if (UNITY_ANDROID && !UNITY_EDITOR)
			OVR_TW_EnableChromaticAberration(chromatic);
#endif
		}
	}

}
