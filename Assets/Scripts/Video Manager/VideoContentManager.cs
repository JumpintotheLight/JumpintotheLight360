using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class VideoContentManager : MonoBehaviour
{
    public VideoColumnManager[] columnsForVideo; //needs to be set beforehand
	public string filePath;
	public bool usefileprefix = false; //This needs to be true when you build for android to point to the corret filepath variable
	public string[] videosInPath;
	public string[] thumbnailsInPath;
	public Vector2 rangeBeingShown;
	public int iAmountOfColumns = 3;


	// Use this for initialization
	void Awake()
    {		
        //TODO Read assets from streaming folder
        ReadFilesFromPath();
        //TODO Load all video from path into a list - map out range currently on display in video list
		InitializeVideoColumns();
        //TODO Initialize all videos in the video layout manager
		SetVideosPerColumn();
	}

	public void ReadFilesFromPath()
    {
		//TODO Read Videos From Path
		//TODO Read Thumbnails From Path
        string filepath = "";
#if UNITY_IPHONE
		//TODO Add File Viewer to pick folder with videos
        string fileNameBase = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')); 
        fileName = fileNameBase.Substring(0, fileNameBase.LastIndexOf('/')) + "/Documents/JITLVideo/"; 
#elif UNITY_ANDROID
		//TODO Add File Viewer to pick folder with videos
        filepath = Application.persistentDataPath + "/JITLVideo/";
#else
		//TODO Add File Viewer to pick folder with videos
		filepath = Application.dataPath + "/JITLVideo/"; 
#endif
		filePath = filepath;
        //TODO Read all files at path using WWW and cache each video file.
        DirectoryInfo directoryInfo = new DirectoryInfo(filepath);
        List<string> videoPathList = new List<string>();
		List<string> thumbnailPathList = new List<string>();
        foreach(FileInfo file in directoryInfo.GetFiles())
        {
            //WWW www = new WWW("file://" + file.FullName);
			Debug.Log(file.FullName);
			string filePathURL = usefileprefix ? "file:///" + file.FullName.Replace('\\','/') : file.FullName.Replace('\\','/');
			if (file.Extension.ToUpper().Contains("MP4"))
            {
				videoPathList.Add(filePathURL);
            }
			else if(file.Extension.ToUpper().Contains("PNG") || file.Extension.ToUpper().Contains("JPG"))
            {
				thumbnailPathList.Add(filePathURL);
            }
		}
		videosInPath = videoPathList.ToArray();
		thumbnailsInPath = thumbnailPathList.ToArray();
    }

	public void InitializeVideoColumns()
	{
		int iCountVideoColumn = transform.GetComponentsInChildren<VideoColumnManager>(true).Length;
		//Make sure all children are enabled before continuing - any padding objects should not have the VideoColumnManagerClass
		foreach(VideoColumnManager tChild in transform.GetComponentsInChildren<VideoColumnManager>(true))
			tChild.gameObject.SetActive (true);
		if (iCountVideoColumn != iAmountOfColumns)
		if (iCountVideoColumn > iAmountOfColumns) {
			int iColumnDisabled = 0;
			for (int i = transform.childCount-1; i > 0; i--) {
				if (iCountVideoColumn - iColumnDisabled != iAmountOfColumns)
					if (transform.GetChild (i).GetComponent<VideoColumnManager> ()) {
						DestroyImmediate (transform.GetChild (i).gameObject);
						iColumnDisabled++;
				}
			}
		} else {
			int iColumnEnabled = 0;
			for (int i = 0; iColumnEnabled < iAmountOfColumns; i++) {
				if (iColumnEnabled == iAmountOfColumns)
					break;
				try {
					if (transform.GetChild (i).GetComponent<VideoColumnManager> ()) {					
						transform.GetChild (i).gameObject.SetActive (true);

						iColumnEnabled++;
					}
				} catch {
					//Instantiate a column
					InstantiateVideoColumn ();
					iColumnEnabled++;
				}
				if (transform.GetChild (i).GetComponent<VideoColumnManager> ())
					transform.GetChild (i).GetComponent<VideoColumnManager>().InitializeThumbnails ();
			}
		}
		//This is here because of a wierd scaling issue that occurs if you dont refresh the new column
		foreach(VideoColumnManager tChild in transform.GetComponentsInChildren<VideoColumnManager>(true))
			tChild.gameObject.SetActive (false);
		foreach(VideoColumnManager tChild in transform.GetComponentsInChildren<VideoColumnManager>(true))
			tChild.gameObject.SetActive (true);
		//Get all column that can display video
		SetDisplayColumnsVariable();
	}


	void InstantiateVideoColumn()
	{
		GameObject newObject;
		newObject = (GameObject)Instantiate (Resources.Load("Prefabs/VideoColumn") as GameObject, transform);
		RectTransform newRect;
		newRect = newObject.GetComponent<RectTransform>();
		newRect.localScale = Vector3.one;
		newRect.localPosition = new Vector3 (newRect.localPosition.x,newRect.localPosition.y,0);
	}

	public void InitializeVideoColumn()
	{
		for (int i = 0; i < columnsForVideo.Length; i++) {
			columnsForVideo [i].InitializeThumbnails ();
		}
	}

	public void SetDisplayColumnsVariable()
	{
		int iCountVideoColumn = transform.GetComponentsInChildren<VideoColumnManager> (false).Length;
		if (columnsForVideo.Length != iCountVideoColumn) {
			columnsForVideo = new VideoColumnManager[transform.GetComponentsInChildren<VideoColumnManager> ().Length];
			int iColumnVideo = 0;
			for (int i = 0; i < transform.childCount; i++) {
				if (transform.GetChild (i).GetComponent<VideoColumnManager> ()) {
					columnsForVideo [iColumnVideo] = transform.GetChild (i).GetComponent<VideoColumnManager> ();
					iColumnVideo++;
				}
			}
		}
	}

	public void SetVideosPerColumn(int iVideoStartRange = 0)
	{
		//Need to do videosInPath.Length-1 due to array starting first index at 0
		int iVideosInPathIndexLength = videosInPath.Length-1;
		if (iVideoStartRange > iVideosInPathIndexLength)
		{
			for (int i = 0; i < columnsForVideo.Length; i++) {
				if (iVideoStartRange <= iVideosInPathIndexLength) {
					if (!columnsForVideo [i].gameObject.activeSelf)
						columnsForVideo [i].gameObject.SetActive (true);
					if (iVideoStartRange + columnsForVideo [i].amountOfVideosPerColumn <= iVideosInPathIndexLength)
						columnsForVideo [i].SetVideoRange (
							new Vector2 (iVideoStartRange, iVideoStartRange + columnsForVideo [i].amountOfVideosPerColumn)
						);
					else {
						Debug.Log ("We've reached the end of the videosInPath.Length and setting the range for Column " + i + "to " + new Vector2 (iVideoStartRange, iVideosInPathIndexLength));
						columnsForVideo [i].SetVideoRange (
							new Vector2 (iVideoStartRange, iVideosInPathIndexLength)
						);
					}
				} else {
					Debug.Log ("We've reached the end of the list and are disabling the rest of the columns");
					columnsForVideo [i].gameObject.SetActive (false);
				}					
			}
		}
		else
			Debug.Log("Trying to set video range for any column failed since iVideoStartRange >= videosInPath.Length");
	}

	public string[] ReturnVideoInformation(int iVideo)
	{
		if (iVideo > -1 && iVideo < videosInPath.Length) {
			string[] returnValue = new string[2];
			returnValue [0] = videosInPath [iVideo];
			returnValue [1] = thumbnailsInPath [iVideo];
			return returnValue;
		} else
			Debug.Log ("Tried to return an invalid video. iVideo = " + iVideo);
		return null;
	}
}
