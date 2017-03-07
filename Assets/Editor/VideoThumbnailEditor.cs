using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VideoThumbnail))]
public class VideoThumbnailEditor : Editor {

    public override void OnInspectorGUI()
    {
		DrawDefaultInspector();
		VideoThumbnail targetManager = (VideoThumbnail)target;
		if(GUILayout.Button("Set Video"))
		{
			targetManager.SetVideo();
		}
    }

}
