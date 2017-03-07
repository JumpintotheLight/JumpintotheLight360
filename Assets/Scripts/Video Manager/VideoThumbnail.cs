using UnityEngine;
using System.Collections;

public class VideoThumbnail : MonoBehaviour {

	public string path = "";
	public int iVideoIndex;
	public VideoContentManager contentManaager
	{
		get
		{
			if (_contentManaager == null)
				_contentManaager = ReturnVideoContentManager ();
			return _contentManaager;
		}
	}
	VideoContentManager _contentManaager;

	public Texture2D thumbnail;
	public UnityEngine.UI.RawImage thumbnailImage;
	public UnityEngine.UI.Text thumbnailTitle;

	public void SetVideo(int iVideo = 0)
	{
		Debug.Log("Setting Video to " + iVideo);
		iVideoIndex = iVideo;
		ToggleThumbnail (true);
		SetVideoThumbnailProperties();
	}

	void SetVideoThumbnailProperties()
	{			
		Debug.Log ("Setting Video Properties");
		string[] videoInformation = contentManaager.ReturnVideoInformation(iVideoIndex);
		path = videoInformation [0];
		StartCoroutine(SetThumbnail(videoInformation [0]));
	}

	IEnumerator SetThumbnail(string path)
	{
		Debug.Log ("Setting Thumbnail WWW");
		if (thumbnailImage == null)
			thumbnailImage = transform.GetComponentInChildren<UnityEngine.UI.RawImage> ();
		WWW newTexture = new WWW(path);
		yield return newTexture;
		thumbnail = newTexture.texture;

		Debug.Log ("Setting Thumbnail Texture");
		//newTexture.LoadImageIntoTexture (thumbnail);
		SetThumbnailTitle ();
		//thumbnailImage.texture = thumbnail as Texture;
	}

	void SetThumbnailTitle()
	{
		if(thumbnailTitle == null)
			thumbnailTitle = transform.FindChild("Image").GetComponentInChildren<UnityEngine.UI.Text>(); //TODO Create a return text component class to scan through all children and get the component
		thumbnailTitle.text = ParseTitleFromPath ();
	}

	string ParseTitleFromPath ()
	{
		string[] splitpath = path.Split ('/'); //TODO change to \ when building out
		return splitpath[splitpath.Length-1].Split('.')[0];
	}

	VideoContentManager ReturnVideoContentManager()
	{
		return ReturnVideoContentManager(transform);
			
	}

	VideoContentManager ReturnVideoContentManager(Transform objectToCheck)
	{
		if (!objectToCheck.GetComponent<VideoContentManager> ()) {
			if (transform.parent != null)
				return ReturnVideoContentManager (objectToCheck.parent);
			else
				try {
					return GetComponentInChildren<VideoContentManager> ();
				} catch {
					Debug.Log (gameObject.name + " Could not find the video content manager");
					return null;
				}
		} else
			return objectToCheck.GetComponent<VideoContentManager> ();
			
	}

	void OnTouch()
	{
		//TODO Get Media Controler and set Video to path
	}

	public void ToggleThumbnail(bool onOff)
	{
		gameObject.SetActive (onOff);
	}
}
