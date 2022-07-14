using System;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace com.biscrat.AnimatorControllerMerger.Editor
{
    [CreateAssetMenu(menuName = "Biscrat/" + nameof(AnimatorControllerMergeSetting))]
    public class AnimatorControllerMergeSetting : ScriptableObject
    {
        [SerializeField] private AnimatorController[] _src;
        [SerializeField] private AnimatorController _dest;

        internal static class FieldNames
        {
            public const string Src = nameof(_src);
            public const string Dest = nameof(_dest);
        }

        public void Merge()
        {
            if (_src is null) throw new InvalidOperationException($"{nameof(_src)} is null");
            if (_dest is null) throw new InvalidOperationException($"{nameof(_dest)} is null");

            ClearAnimatorController(_dest);

            foreach (var src in _src)
            {
                if (src is null) continue;
                AppendAnimatorController(src, _dest);
            }

            EditorUtility.SetDirty(_dest);
#if UNITY_2020_3_OR_NEWER
            AssetDatabase.SaveAssetIfDirty(_dest);
#else
            AssetDatabase.SaveAssets();
#endif
        }

        private static void ClearAnimatorController(AnimatorController animatorController)
        {
            for (var i = animatorController.layers.Length - 1; i >= 0; --i)
            {
                animatorController.RemoveLayer(i);
            }

            for (var i = animatorController.parameters.Length - 1; i >= 0; --i)
            {
                animatorController.RemoveParameter(i);
            }
        }

        private static void AppendAnimatorController(AnimatorController src, AnimatorController dest)
        {
            foreach (var parameter in src.parameters)
            {
                dest.AddParameter(parameter);
            }

            foreach (var layer in src.layers)
            {
                dest.AddLayer(layer);
            }
        }
    }
}