using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VideoColumnManager))]
public class VideoColumnManagerEditor : Editor {

    public override void OnInspectorGUI()
    {
		DrawDefaultInspector();
		VideoColumnManager targetManager = (VideoColumnManager)target;
		if(GUILayout.Button("Initialize Video Layout Manager"))
		{
			targetManager.InitializeVideoLayoutManager();
		}
		if(GUILayout.Button("Initialize Thumbnails"))
		{
			targetManager.InitializeThumbnails();
		}
    }

}
