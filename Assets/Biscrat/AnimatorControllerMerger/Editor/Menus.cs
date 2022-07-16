using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace com.biscrat.AnimatorControllerMerger.Editor
{
    public static class Menus
    {
        [MenuItem("Tools/Biscrat/Run All Merge Settings")]
        public static void MergeAll()
        {
            var guids = AssetDatabase.FindAssets("t:AnimatorControllerMergeSetting");

            var settingPaths = guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .OrderBy(Path.GetFileName, StringComparer.InvariantCulture)
                .ToArray();


            foreach (var path in settingPaths)
            {
                var setting = AssetDatabase.LoadAssetAtPath<AnimatorControllerMergeSetting>(path);
                try
                {
                    Debug.Log($"Merging {path}");
                    setting.Merge();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to merge animator controllers. SettingPath:{path}, exception:{e}", setting);
                }
            }
        }
    }
}