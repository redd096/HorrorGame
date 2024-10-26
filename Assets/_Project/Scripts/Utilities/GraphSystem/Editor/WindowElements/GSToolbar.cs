using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.IO;

namespace GraphSystem.Editor
{
    public class GSToolbar : VisualElement
    {
        private GSGraphView graphView;
        private Toolbar toolbar;

        private TextField fileNameTextfield;
        private Button minimapButton;

        private string fileName;

        public GSToolbar(GSGraphView graphView) : base()
        {
            this.graphView = graphView;
            fileName = GSConstUtility.DefaultFileName;

            //we have to use a VisualElement with inside a Toolbar, because for some reason if we inherit from toolbar it loses all the graphic styles
            toolbar = new Toolbar();
            Add(toolbar);

            //add label 
            fileNameTextfield = GSElementUtility.CreatetextField(fileName, "File Name:", callback => fileName = callback.newValue);

            //add buttons
            Button saveButton = GSElementUtility.CreateButton("Save", () => ClickSave());
            Button loadButton = GSElementUtility.CreateButton("Load", () => ClickLoad());
            Button clearButton = GSElementUtility.CreateButton("Clear", () => ClickClear());
            Button resetButton = GSElementUtility.CreateButton("Reset", () => ClickReset());
            minimapButton = GSElementUtility.CreateButton("Minimap", () => ClickToggleMinimap());

            //check if minimapButton is toggled or not (minimap visible or not)
            if (graphView.IsMinimapVisible())
                minimapButton.ToggleInClassesList("ds-toolbar__button__selected");

            AddInToolbar(fileNameTextfield);
            AddInToolbar(saveButton);
            AddInToolbar(loadButton);
            AddInToolbar(clearButton);
            AddInToolbar(resetButton);
            AddInToolbar(minimapButton);
        }

        private void AddInToolbar(VisualElement element)
        {
            toolbar.Add(element);
        }

        #region on click buttons

        void ClickSave()
        {
            //save all assets in the project
            if (string.IsNullOrEmpty(fileNameTextfield.value))
            {
                EditorUtility.DisplayDialog(
                    "Invalid file name",
                    "Please ensure the file name you have typed in is valid.",
                    "OK");

                return;
            }

            GSSaveUtility.Initialize(graphView, fileName);
            GSSaveUtility.Save();
        }

        void ClickLoad()
        {
            //load from asset
            string fullPath = EditorUtility.OpenFilePanel("Load Graph", GSConstUtility.SavedFolderPath, "asset");

            if (string.IsNullOrEmpty(fullPath) == false)
            {
                //clear graph view
                graphView.ClearGraph();

                //save file path and file name
                fileName = Path.GetFileNameWithoutExtension(fullPath);

                //update label
                fileNameTextfield.SetValueWithoutNotify(fileName);

                GSSaveUtility.Initialize(graphView, fileName);
                GSSaveUtility.Load();
            }
        }

        void ClickClear()
        {
            //clear graph view
            graphView.ClearGraph();
        }

        void ClickReset()
        {
            //clear graph view, reset file path and file name
            ClickClear();
            fileName = GSConstUtility.DefaultFileName;
            fileNameTextfield.SetValueWithoutNotify(fileName);
        }

        void ClickToggleMinimap()
        {
            //toggle graph view minimap
            graphView.ToggleMinimap();

            //toggle minimap button styles
            minimapButton.ToggleInClassesList("ds-toolbar__button__selected");
        }

        #endregion
    }
}