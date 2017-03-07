using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VideoContentManager))]
public class VideoContentManagerEditor : Editor {

    public override void OnInspectorGUI()
    {
		DrawDefaultInspector();
		VideoContentManager targetManager = (VideoContentManager)target;
		if(GUILayout.Button("Get Display Columns"))
		{
			targetManager.SetDisplayColumnsVariable();
		}
		if(GUILayout.Button("Read Files From Path"))
		{
			targetManager.ReadFilesFromPath();
		}
		if(GUILayout.Button("Initialize Video Columns"))
		{
			targetManager.InitializeVideoColumns();
		}

    }

	void OnValidate()
	{
		VideoContentManager targetManager = (VideoContentManager)target;
		targetManager.InitializeVideoColumns ();
	}

}
