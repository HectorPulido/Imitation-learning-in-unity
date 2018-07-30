using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NeuralReplicantBot.PerceptronHandler.Editor
{
    using UnityEditor;
	using NeuralReplicantBot.PerceptronHandler;
    using System.IO;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(HumanMonitor), true)]
	public class HumanMonitorEditor : Editor 
	{
		HumanMonitor h ;

		void OnEnable()
		{
			h = (HumanMonitor) target;
		}
		public override void OnInspectorGUI()
		{       
			if(h == null)
				h = (HumanMonitor) target;

			serializedObject.Update();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("meditionPeriod"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("meditionCount"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("loadData"));

			if(h.loadData)
			{
				EditorGUILayout.HelpBox("If you load the data, a dataset will be load and you will not play", MessageType.Warning);
			}
			EditorGUILayout.PropertyField(serializedObject.FindProperty("saveData"));
			if(!h.saveData)
			{
				EditorGUILayout.HelpBox("If you do not save your data, it will be lost", MessageType.Warning);
			}
			
			if(h.saveData || h.loadData)
			{				
				var dropArea = GUILayoutUtility.GetRect(Screen.width, 35, GUILayout.MaxWidth(Screen.width - 40));

				if (string.IsNullOrEmpty(h.dataPath))
					GUI.Box(dropArea, "Drag a data file");
				else
					GUI.Box(dropArea, Path.GetFileName(h.dataPath));

				DragAndDropFile(dropArea);

				EditorGUILayout.Space();

				GUILayout.BeginHorizontal();
				if (GUILayout.Button("Clear", GUILayout.MaxWidth(Screen.width/2 - 20)))
				{
					h.dataPath = "";
				}
				if (GUILayout.Button("Create", GUILayout.MaxWidth(Screen.width/2 - 20)))
				{
					CreateFile();
				}
				GUILayout.EndHorizontal();
			}

			serializedObject.ApplyModifiedProperties();
		}

		private void CreateFile()
		{
			string filepath = EditorUtility.SaveFilePanel("Create neural network file", "Assets", this.name, "med");

			if (filepath != string.Empty)
			{
				File.WriteAllText(filepath, "");
				h.dataPath = filepath;
			}
		}
		private void DragAndDropFile(Rect DropArea)
		{
			Event current = Event.current;

			switch (current.type)
			{
				case EventType.DragUpdated:
				case EventType.DragPerform:

					if (DropArea.Contains(current.mousePosition))
					{
						if(DragAndDrop.paths.Length == 0)
						{
							DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
						}
						else
						{
							if(DragAndDrop.paths[0].EndsWith(".med"))
							{
								DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
							}
							else
							{
								DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
							}

						}
						if (current.type == EventType.DragPerform)
						{

							h.dataPath = DragAndDrop.paths[0];
							DragAndDrop.AcceptDrag();
							current.Use();
						}
					}
					break;
			}
        }
	}
}
