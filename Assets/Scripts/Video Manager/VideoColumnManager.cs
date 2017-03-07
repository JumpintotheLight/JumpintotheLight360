using UnityEngine;
using System.Collections;

public class VideoColumnManager : MonoBehaviour 
{
	public int amountOfVideosPerColumn = 3;
	public Vector2 currentVideoDisplayRange;
	public Transform verticalLayoutManager;

	Vector2 rangeDisplay;

	void Awake()
	{
		//TODO Initialize Vertical Layout Manager
		InitializeVideoLayoutManager();
		//TODO Ensure there are enough thumbnail display objects
		InitializeThumbnails();

	}

	public void InitializeVideoLayoutManager()
	{
		//The first VerticalLayoutGroup is the transform itself the second is the video thumbnail manager
		if (verticalLayoutManager == null)
			verticalLayoutManager = transform.GetComponentsInChildren<UnityEngine.UI.VerticalLayoutGroup>()[1].transform;
	}

	public void InitializeThumbnails()
	{
		InitializeVideoLayoutManager ();
		int iCountVideoColumn = verticalLayoutManager.transform.GetComponentsInChildren<VideoThumbnail>(true).Length;
		//Make sure all children are enabled before continuing - any padding objects should not have the VideoColumnManagerClass
		foreach(VideoThumbnail tChild in verticalLayoutManager.transform.GetComponentsInChildren<VideoThumbnail>(true))
			tChild.gameObject.SetActive (true);
		if (iCountVideoColumn != amountOfVideosPerColumn)
		if (iCountVideoColumn > amountOfVideosPerColumn) {
			int iColumnDisabled = 0;
			for (int i = verticalLayoutManager.transform.childCount-1; i > 0; i--) {
				if (iCountVideoColumn - iColumnDisabled != amountOfVideosPerColumn)
				if (verticalLayoutManager.transform.GetChild (i).GetComponent<VideoThumbnail> ()) {
					DestroyImmediate (verticalLayoutManager.transform.GetChild (i).gameObject);
					iColumnDisabled++;
				}
			}
		} else {
			int iColumnEnabled = 0;
			for (int i = 0; iColumnEnabled < amountOfVideosPerColumn; i++) {
				if (iColumnEnabled == amountOfVideosPerColumn)
					break;
				try {
					if (verticalLayoutManager.transform.GetChild (i).GetComponent<VideoThumbnail> ()) {					
						verticalLayoutManager.transform.GetChild (i).gameObject.SetActive (true);
					}
				} catch {
					//Instantiate a column
					InstantiateThumbnail();
				}
				iColumnEnabled++;
			}
		}
		//This is here because of a wierd scaling issue that occurs if you dont refresh the new column
		foreach(VideoThumbnail tChild in verticalLayoutManager.GetComponentsInChildren<VideoThumbnail>(true))
			tChild.gameObject.SetActive (false);
		foreach(VideoThumbnail tChild in verticalLayoutManager.GetComponentsInChildren<VideoThumbnail>(true))
			tChild.gameObject.SetActive (true);
	}

	void InstantiateThumbnail()
	{
		GameObject newObject;
		newObject = (GameObject)Instantiate (Resources.Load("Prefabs/VideoThumbnail") as GameObject, verticalLayoutManager);
		RectTransform newRect;
		newRect = newObject.GetComponent<RectTransform>();
		newRect.localScale = Vector3.one;
		newRect.localPosition = new Vector3 (newRect.localPosition.x,newRect.localPosition.y,0);
	}

	public void SetVideoRange()
	{
		int iVideoToSet = (int)rangeDisplay.x;
		for (int i = 0; i < verticalLayoutManager.childCount; i++) 
		{
			if (iVideoToSet + 1 < (int)rangeDisplay.y) {
				verticalLayoutManager.GetChild (i).GetComponent<VideoThumbnail> ().SetVideo (iVideoToSet);
				iVideoToSet++;
			} else {
				verticalLayoutManager.GetChild (i).GetComponent<VideoThumbnail> ().ToggleThumbnail (true);
			}
		}
	}

	public void SetVideoRange(Vector2 videoRange)
	{
		currentVideoDisplayRange = videoRange;

	}

	void ReturnVideoInformationFromContentManager()
	{
		
	}
}
