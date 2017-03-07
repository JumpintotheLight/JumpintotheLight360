using UnityEngine;
using System.Collections;

public class screenManager : MonoBehaviour {
	public int myScreen;

	// Use this for initialization
	void Awake () {

		myScreen = PlayerPrefs.GetInt ("ScreenSetting");
		if (myScreen == 1) {
			Screen.orientation = ScreenOrientation.LandscapeLeft;
		} else {
			Screen.orientation = ScreenOrientation.Portrait;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}