using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using GraphSystem.Enumerations;
using GraphSystem.Data;

namespace GraphSystem.Editor
{
    public class GSNodeMultiple : GSNode
    {
        public override void Initialize(string nodeName, GSGraphView graphView, Vector2 position)
        {
            base.Initialize(nodeName, graphView, position);
            
            //set multiple and add first output to the list
            NodeType = GSNodeType.Multiple;
            OutputsData.Add(new GSNodeOutputData("Output"));
        }

        public override void Draw()
        {
            base.Draw();

            //output port
            foreach (GSNodeOutputData choice in OutputsData)
            {
                Port outputPort = CreateOutputPort(choice);
                outputContainer.Add(outputPort);
            }

            //button to add new port
            Button addPortButton = GSElementUtility.CreateButton("Add Transition", () =>
            {
                GSNodeOutputData data = new GSNodeOutputData("Output");
                Port outputPort = CreateOutputPort(data);
                outputContainer.Insert(outputContainer.childCount - 1, outputPort); //insert always before the Add button
                OutputsData.Add(data);
            });
            outputContainer.Add(addPortButton);
            addPortButton.AddToClassesList("ds-node__button");
        }

        private Port CreateOutputPort(object userData)
        {
            //create port (no name, so we can use TextField as name)
            Port outputPort = this.CreatePort("", Orientation.Horizontal, Direction.Output, Port.Capacity.Single);
            outputPort.userData = userData;     //save data to have this class as variable inside the port

            GSNodeOutputData data = (GSNodeOutputData)userData;

            //create a TextField to change port's text
            TextField portTextField = GSElementUtility.CreatetextField(data.Text, null, callback => data.Text = callback.newValue);
            portTextField.AddToClassesList("ds-node__textfield", "ds-node__choice-textfield", "ds-node__textfield__hidden");

            //create an X button to delete the port
            Button deletePortButton = GSElementUtility.CreateButton("X", () =>
            {
                if (OutputsData.Count == 1)
                    return;

                //if connected, remove connections
                if (outputPort.connected)
                    graphView.DeleteElements(outputPort.connections);

                //remove port
                OutputsData.Remove(data);
                graphView.RemoveElement(outputPort);
            });
            deletePortButton.AddToClassesList("ds-node__button");

            //on output port, elements will be added from right to left, so to have the X button to the left we have to add it as the last element
            outputPort.Add(portTextField);
            outputPort.Add(deletePortButton);

            return outputPort;
        }
    }
}