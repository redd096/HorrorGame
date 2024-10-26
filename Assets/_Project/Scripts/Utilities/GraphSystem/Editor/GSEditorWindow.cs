using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphSystem.Editor
{
    public class GSEditorWindow : EditorWindow
    {
        //styles are added in inspector -> click on the script in the project and in inspector you can set these variables
        //if not setted, will be used AssetDatabase.LoadAssetAtPath()
        [SerializeField] private StyleSheet variablesStyles = default;
        [SerializeField] private StyleSheet graphViewStyles = default;
        [SerializeField] private StyleSheet nodeStyles = default;
        [SerializeField] private StyleSheet minimapStyles = default;
        [SerializeField] private StyleSheet toolbarStyles = default;

        private GSGraphView graphView;
        private GSToolbar toolbar;

        [MenuItem("Window/GraphSystem")]
        public static void ShowWindow()
        {
            //show window
            GSEditorWindow wnd = GetWindow<GSEditorWindow>();
            wnd.titleContent = new GUIContent("GraphSystem");
        }

        public void CreateGUI()
        {
            //add graphview and toolbar
            graphView = new GSGraphView(this);
            graphView.StretchToParentSize();
            toolbar = new GSToolbar(graphView);

            rootVisualElement.Add(graphView);
            rootVisualElement.Add(toolbar);

            //add styles
            AddStyles();
        }

        void AddStyles()
        {
            //add styles
            rootVisualElement.styleSheets.AddStyleSheets(variablesStyles, GSConstUtility.VariablesStylesPath);
            graphView.styleSheets.AddStyleSheets(graphViewStyles, GSConstUtility.GraphViewStylesPath);
            graphView.styleSheets.AddStyleSheets(nodeStyles, GSConstUtility.NodeStylesPath);
            graphView.styleSheets.AddStyleSheets(minimapStyles, GSConstUtility.MinimapStylesPath);
            toolbar.styleSheets.AddStyleSheets(toolbarStyles, GSConstUtility.ToolbarStylesPath);
        }
    }
}