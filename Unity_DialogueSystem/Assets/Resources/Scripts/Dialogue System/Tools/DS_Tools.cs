using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using AGAC.DialogueSystem.Base;

namespace AGAC.DialogueSystem.Tools
{
    public static class DS_Tools
    {
        #region PUBLIC METHODS
        [MenuItem("Assets/Create/Dialogue System/Sequence", priority = 2)]
        public static void CreateNewSequence()
        {
            CreateNewAsset<DS_DialogueSequence>("Sequences/", "New Sequence", ".asset");
        }

        [MenuItem("Assets/Create/Dialogue System/Character", priority = 1)]
        public static void CreateNewCharacter()
        {
            CreateNewAsset<DS_Character>("Characters/", "New Character", ".asset");
        }

        [MenuItem("Assets/Create/Dialogue System/Conversation", priority = 0)]
        public static void CreateNewConversation()
        {
            CreateNewAsset<DS_Conversation>("Conversations/", "New Conversation", ".asset");
        }
        #endregion

        #region PRIVATE METHOSD
        private static void CreateNewAsset<T>(string assetPath, string assetName, string assetExtension) where T : ScriptableObject, new()
        {
            CreateAssetPath(assetPath);

            string fullPath = "Assets/Resources/" + assetPath + assetName + assetExtension;
            T newAsset = new T();

            ProjectWindowUtil.CreateAsset(newAsset, fullPath);
            //AssetDatabase.SaveAssets();

            //EditorUtility.FocusProjectWindow();
            //Selection.activeObject = newAsset;
        }
        private static void CreateAssetPath(string resourcePath)
        {
            string path = Application.dataPath + "/Resources/" + resourcePath;
            if (Directory.Exists(path)) return;
            Directory.CreateDirectory(path);
        }
        #endregion
    }
}