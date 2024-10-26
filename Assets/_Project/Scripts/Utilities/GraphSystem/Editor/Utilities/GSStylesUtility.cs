using UnityEditor;
using UnityEngine.UIElements;

namespace GraphSystem.Editor
{
    public static class GSStylesUtility
    {
        /// <summary>
        /// Call VisualElement.AddToClassList(className)
        /// </summary>
        /// <param name="element"></param>
        /// <param name="classNames"></param>
        public static void AddToClassesList(this VisualElement element, params string[] classNames)
        {
            foreach (string className in classNames)
            {
                element.AddToClassList(className);
            }
        }

        /// <summary>
        /// Call VisualElement.AddToClassList(className)
        /// </summary>
        /// <param name="element"></param>
        /// <param name="classNames"></param>
        public static void RemoveFromClassesList(this VisualElement element, params string[] classNames)
        {
            foreach (string className in classNames)
            {
                element.RemoveFromClassList(className);
            }
        }

        /// <summary>
        /// Call VisualElement.ToggleInClassList(className)
        /// </summary>
        /// <param name="element"></param>
        /// <param name="classNames"></param>
        public static void ToggleInClassesList(this VisualElement element, params string[] classNames)
        {
            foreach (string className in classNames)
            {
                element.ToggleInClassList(className);
            }
        }

        /// <summary>
        /// If styleSheet is null load it with LoadAssetAtPath(styleSheetPath), then call VisualElement.styleSheets.Add(styleSheet)
        /// </summary>
        /// <param name="element"></param>
        /// <param name="styleSheet"></param>
        /// <param name="styleSheetPath"></param>
        public static void AddStyleSheets(this VisualElementStyleSheetSet element, StyleSheet styleSheet, string styleSheetPath)
        {
            if (styleSheet == null)
                styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(styleSheetPath);

            element.Add(styleSheet);
        }
    }
}