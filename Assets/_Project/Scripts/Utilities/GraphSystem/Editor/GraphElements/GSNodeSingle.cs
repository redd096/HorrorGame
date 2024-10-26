using GraphSystem.Data;
using GraphSystem.Enumerations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GraphSystem.Editor
{
    public class GSNodeSingle : GSNode
    {
        public override void Initialize(string nodeName, GSGraphView graphView, Vector2 position)
        {
            base.Initialize(nodeName, graphView, position);

            //set single and add the only output to the list
            NodeType = GSNodeType.Single;
            OutputsData.Add(new GSNodeOutputData("Output"));
        }

        public override void Draw()
        {
            base.Draw();

            //output port
            foreach (GSNodeOutputData data in OutputsData)
            {
                Port outputPort = this.CreatePort(data.Text, Orientation.Horizontal, Direction.Output, Port.Capacity.Single);
                outputPort.userData = data;       //save data to have this class as variable inside the port
                outputContainer.Add(outputPort);
            }
        }
    }
}