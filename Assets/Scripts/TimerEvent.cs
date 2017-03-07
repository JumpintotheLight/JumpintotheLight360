using UnityEngine;
using System.Collections;

public class TimerEvent : MonoBehaviour {
	public float timerduration = 0.0f;
    public string level = "menu";
//	public string PublicFunctionCall = "";
//	public GameObject LoadScreen;
//	public GameObject LoginScreen;

	public void Start(){
		StartCoroutine(timer());//timer ();
	}

		public IEnumerator timer() {
        //		print(Time.time);

        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(level); //Application.LoadLevelAdditiveAsync(level);
        yield return async;
        Debug.Log("Loading complete");
        Destroy(gameObject);

        // Application.LoadLevelAdditiveAsync(level);
        // yield return new WaitForSeconds(timerduration);

        //		LoginScreen.SetActive (true);
        //		LoadScreen.SetActive (false);

        //		BroadcastMessage (PublicFunctionCall);
        //		print(Time.time);
    }

}