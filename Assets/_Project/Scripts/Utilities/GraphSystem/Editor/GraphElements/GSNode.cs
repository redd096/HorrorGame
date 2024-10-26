using GraphSystem.Enumerations;
using GraphSystem.Data;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphSystem.Editor
{
    public class GSNode : Node
    {
        public string NodeName;
        public string ID;
        public List<GSNodeOutputData> OutputsData;
        public string ContentText;
        public GSNodeType NodeType;
        public GSGroup Group;

        protected GSGraphView graphView;

        private TextField nodeNameText;
        private VisualElement customDataContainer;
        private TextField contentTextField;

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //add these buttons to the graph's contextual menu
            if (evt.target is Node)
            {
                evt.menu.AppendAction("Set as Starting Node", actionEvent => graphView.SetStartingNode(this));
                evt.menu.AppendSeparator();
                evt.menu.AppendAction("Disconnect inputs", actionEvent => DisconnectPorts(inputContainer), IsOnePortConnected(inputContainer) ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled);
                evt.menu.AppendAction("Disconnect outputs", actionEvent => DisconnectPorts(outputContainer), IsOnePortConnected(outputContainer) ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled);
                //disconnect all is already showed by default
            }

            base.BuildContextualMenu(evt);
        }

        #region public API

        public virtual void Initialize(string nodeName, GSGraphView graphView, Vector2 position)
        {
            this.graphView = graphView;

            //save default values
            NodeName = nodeName;
            ID = System.Guid.NewGuid().ToString();
            OutputsData = new List<GSNodeOutputData>();
            ContentText = "Content Text.";

            //set position
            SetPosition(new Rect(position, Vector2.zero));
        }

        public virtual void Draw()
        {
            //MainContainer contains everything
            //TitleContainer and TitleButtonContainer will be on top, also when collapsed                                               (index 0 inside main container)
            //on the top, under the title container: InputContainer on the left, TopContainer at center, OutputContainer on the right   (index 1 inside main container)
            //the body of the node is in ExtensionContainer (that want RefreshExpandedState() to show everything you added inside)      (index 2 inside main container)

            //set node name text, and add to title container
            nodeNameText = GSElementUtility.CreatetextField(NodeName, null, callback => NodeName = callback.newValue);
            titleContainer.Insert(0, nodeNameText);

            //input port - if portName is null it uses the type passed as last parameter as name
            Port inputPort = this.CreatePort("Input", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(inputPort);

            //create container for custom style
            customDataContainer = new VisualElement();

            //foldout and text field
            Foldout textFoldout = GSElementUtility.CreateFoldout("Content");
            contentTextField = GSElementUtility.CreateTextArea(ContentText, null, callback => ContentText = callback.newValue);
            textFoldout.Add(contentTextField);                      //text inside foldout (so we can open and close it)
            customDataContainer.Add(textFoldout);                   //foldout inside custom container (so we can give it a custom style)
            extensionContainer.Add(customDataContainer);            //container inside extension container (so under input and output)

            //audio clip
            //ObjectField audioClip = new ObjectField("Audio Clip");
            //audioClip.objectType = typeof(AudioClip);
            //customDataContainer.Add(audioClip);

            //for some reason, extension container need to be refreshed to show what's inside
            //refresh is called in FinishDraw to be sure to call it also after overrides of Draw function
        }

        public void FinishDraw()
        {
            //for some reason, extension container need to be refreshed to show what's inside
            //refresh is called in FinishDraw to be sure to call it also after overrides of Draw function
            RefreshExpandedState();

            //add styles
            AddStyles();
        }

        #endregion

        #region private API

        private bool IsOnePortConnected(VisualElement container)
        {
            //check if at least one port is connected
            foreach (VisualElement element in container.Children())
            {
                if (element is Port port)
                {
                    if (port.connected)
                        return true;
                }
            }

            return false;
        }

        private void DisconnectPorts(VisualElement container)
        {
            //foreach port, remove every connection
            foreach (VisualElement element in container.Children())
            {
                if (element is Port port)
                {
                    //if not connected, ignore it
                    if (port.connected == false)
                        continue;

                    graphView.DeleteElements(port.connections);
                }
            }
        }

        private void AddStyles()
        {
            //add styles
            mainContainer.AddToClassesList("ds-node__main-container");
            extensionContainer.AddToClassesList("ds-node__extension-container");

            //add styles to elements
            nodeNameText.AddToClassesList("ds-node__textfield", "ds-node__filename-textfield", "ds-node__textfield__hidden");
            customDataContainer.AddToClassesList("ds-node__custom-data-container");
            contentTextField.AddToClassesList("ds-node__textfield", "ds-node__quote-textfield");
        }

        #endregion
    }
}