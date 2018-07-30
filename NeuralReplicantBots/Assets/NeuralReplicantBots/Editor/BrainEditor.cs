using UnityEngine;

namespace NeuralReplicantBot.PerceptronHandler.Editor
{
    using UnityEditor;
	using NeuralReplicantBot.PerceptronHandler;
    using System.IO;

    [CanEditMultipleObjects]
	[CustomEditor(typeof(Brain))]
	public class BrainEditor : Editor {

                Brain b ;
            
                void OnEnable()
                {
                    b = (Brain) target;
                }
                public override void OnInspectorGUI()
                {       
                    if(b == null)
                        b = (Brain) target;

                    serializedObject.Update();
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("epoch"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("topology"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("lossShowRate"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("batchSize"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("activationFunction"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("learningRate"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("save"));                  

                    if(b.save)
                    {
                        var dropArea = GUILayoutUtility.GetRect(Screen.width, 35, GUILayout.MaxWidth(Screen.width - 40));

                        if (string.IsNullOrEmpty(b.dataPath))
                            GUI.Box(dropArea, "Drag an neural network");
                        else
                            GUI.Box(dropArea, Path.GetFileName(b.dataPath));

                        DragAndDropFile(dropArea);

                        EditorGUILayout.Space();

                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Clear", GUILayout.MaxWidth(Screen.width/2 - 20)))
                        {
                            b.dataPath = "";
                        }
                        if (GUILayout.Button("Create", GUILayout.MaxWidth(Screen.width/2 - 20)))
                        {
                            CreateFile();
                        }
                        GUILayout.EndHorizontal();

                        //EditorGUILayout.Space();
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("If you do not save your training, it will be lost", MessageType.Warning);
                    }

                    serializedObject.ApplyModifiedProperties();
                }

                private void CreateFile()
                {
                    string filepath = EditorUtility.SaveFilePanel("Create neural network file", "Assets", this.name, "nn");

                    if (filepath != string.Empty)
                    {
                        File.WriteAllText(filepath, "");
                        b.dataPath = filepath;
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
                                    if(DragAndDrop.paths[0].EndsWith(".nn"))
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

                                    b.dataPath = DragAndDrop.paths[0];
                                    DragAndDrop.AcceptDrag();
                                    current.Use();
                                }
                            }
                            break;
                    }
        }


    }
}