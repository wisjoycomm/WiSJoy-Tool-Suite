// using System;
// using UnityEditor;
// using UnityEngine;
// using WiSJoy.DesignPattern;

// namespace WiSdom.DesignPattern.Editor
// {
//     public class MessageIODashboard : EditorWindow
//     {
//         private string filterText = "";
//         private Vector2 scrollPosition;

//         [MenuItem("WiSdom/MessageIO Dashboard")]
//         public static void ShowWindow()
//         {
//             var window = GetWindow<MessageIODashboard>("MessageIO Dashboard");
//             window.minSize = new Vector2(300, 200); // Set minimum size of the window
//         }

//         void OnGUI()
//         {
//             GUILayout.Space(10);
//             EditorGUILayout.LabelField("MessageIO Dashboard", EditorStyles.boldLabel);

//             // Add a refresh button at the top of the window
//             if (GUILayout.Button("Refresh", GUILayout.Height(30)))
//             {
//                 Repaint();
//             }

//             GUILayout.Space(10);
//             GUILayout.Label("Filter Messages:", EditorStyles.boldLabel);
//             filterText = EditorGUILayout.TextField(filterText);

//             scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

//             DisplayData();

//             EditorGUILayout.EndScrollView();
//         }

//         void DisplayData()
//         {
//             GUILayout.Space(10); // Add some spacing for better layout

//             if (MessageBus.I != null)
//             {
//                 // Section for Active Messages
//                 EditorGUILayout.LabelField("Active Messages:", EditorStyles.boldLabel);
//                 foreach (var messageType in MessageBus.I.GetMessageCounts())
//                 {
//                     if (string.IsNullOrEmpty(filterText) || messageType.Key.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0)
//                     {
//                         EditorGUILayout.HelpBox($"{messageType.Key}: {messageType.Value}", MessageType.Info);
//                     }
//                 }

//                 GUILayout.Space(10); // Add spacing between sections

//                 // Section for Subscriber Counts
//                 EditorGUILayout.LabelField("Subscribers Count:", EditorStyles.boldLabel);
//                 foreach (var subscriberType in MessageBus.I.GetSubscriberCounts())
//                 {
//                     if (string.IsNullOrEmpty(filterText) || subscriberType.Key.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0)
//                     {
//                         EditorGUILayout.HelpBox($"{subscriberType.Key}: {subscriberType.Value}", MessageType.Info);
//                     }
//                 }
//             }
//             else
//             {
//                 EditorGUILayout.HelpBox("MessageIO instance is not active.", MessageType.Warning);
//             }
//         }
//     }
// }