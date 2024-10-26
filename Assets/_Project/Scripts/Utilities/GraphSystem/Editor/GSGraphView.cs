using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using GraphSystem.Enumerations;
using System.Collections.Generic;
using UnityEditor;
using GraphSystem.Data;

namespace GraphSystem.Editor
{
    public class GSGraphView : GraphView
    {
        public GSNode StartingNode;

        private EditorWindow EditorWindow;

        private MiniMap minimap;
        private GSSearchWindow searchWindow;

        public GSGraphView(EditorWindow editorWindow) : base()
        {
            EditorWindow = editorWindow;

            //add callbacks
            OnElementsDeleted();
            OnGroupElementsAdded();
            OnGroupElementsRemoved();
            OnGraphViewChanged();

            //add manipulators (to do things when events occurs) - contains also contextual menu with right click
            AddManipulators();

            //add minimap
            AddMinimap();

            //when created, add background (GraphView doesn't have a background by default)
            AddGridBackground();

            //add search window (like AddComponent on inspector - event occurs with space bar)
            AddSearchWindow();
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();
            ports.ForEach(port =>
            {
                //not on same node
                if (startPort.node == port.node)
                    return;

                //not input with input, or output with output
                if (startPort.direction == port.direction)
                    return;

                //input can connect with every output on another node, and output can connect with every input on another node
                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        #region callbacks

        void OnElementsDeleted()
        {
            //deleteSelection is the call back when delete an element from graph view.
            //name of operation to undo/redo, and wheter or not ask user
            deleteSelection += (operationName, askUser) =>
            {
                foreach (GraphElement element in selection)
                {
                    //if delete a group, remove all its nodes to not delete buttons that are not selected
                    if (element is Group group)
                    {
                        foreach (GraphElement groupElement in new List<GraphElement>(group.containedElements))
                        {
                            if (groupElement is Node)
                            {
                                group.RemoveElement(groupElement);
                            }
                        }

                        continue;
                    }
                }

                //call this, because in GraphView we can see that DeleteSelectionOperation doesn't call DeleteSelection when the callback is overrided
                DeleteSelection();
            };
        }

        void OnGroupElementsAdded()
        {
            //this is the call back when something is added to a group
            elementsAddedToGroup += (group, elements) =>
            {
                //save group in node
                foreach (GraphElement element in elements)
                {
                    if (element is GSNode node)
                    {
                        node.Group = group as GSGroup;
                    }
                }
            };
        }

        void OnGroupElementsRemoved()
        {
            //this is the call back when something is removed from a group
            elementsRemovedFromGroup += (group, elements) =>
            {
                //clear group in node
                foreach (GraphElement element in elements)
                {
                    if (element is GSNode node)
                    {
                        node.Group = null;
                    }
                }
            };
        }

        void OnGraphViewChanged()
        {
            //callback when something is changed in the graphview
            graphViewChanged += (changes) =>
            {
                //when create connections between nodes
                if (changes.edgesToCreate != null)
                {
                    foreach (Edge edge in changes.edgesToCreate)
                    {
                        //save on outputPort, the node it reach
                        GSNode nextNode = edge.input.node as GSNode;
                        GSNodeOutputData data = edge.output.userData as GSNodeOutputData;
                        if (data != null) data.NextNodeID = nextNode != null? nextNode.ID : "";
                    }
                }

                //when delete elements
                if (changes.elementsToRemove != null)
                {
                    foreach (GraphElement element in changes.elementsToRemove)
                    {
                        //if delete connections between nodes
                        if (element is Edge edge)
                        {
                            //remove node from port
                            GSNodeOutputData data = edge.output.userData as GSNodeOutputData;
                            if (data != null) data.NextNodeID = "";
                        }
                    }
                }

                return changes;
            };
        }

        #endregion

        #region add elements on constructor

        void AddManipulators()
        {
            //add drag manipulator when click middle mouse button, and zoom when move the wheel
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentZoomer());
            //another method to add content zoomer
            //SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            //drag selected nodes, and rectangle to select more nodes at time
            //for some reason, SelectionDragger must be added before RectangleSelector, otherwise the nodes selected with the rectangle can't be dragged
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            //manipulator to show new methods on the contextual menu (right click of the mouse)
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single)", GSNodeType.Single));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple)", GSNodeType.Multiple));
            this.AddManipulator(CreateGroupContextualMenu());
        }

        void AddMinimap()
        {
            //create minimap and set position
            minimap = new MiniMap()
            {
                //anchored or can user drag it? - with right click on the minimap is possible to toggle this option
                anchored = true,
            };
            minimap.SetPosition(new Rect(15, 50, 200, 180));

            //add instead of AddElement, because AddElement is used when create something on the Graph like nodes, that could also increase its size
            Add(minimap);

            //by default is not visible
            minimap.visible = false;
        }

        void AddGridBackground()
        {
            //create a GridBackground and set same size as the graph view
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();

            //add to graphView, with index 0 to be sure is behind everything
            Insert(0, gridBackground);
        }

        void AddSearchWindow()
        {
            //create Search Window scriptable object instance
            if (searchWindow == null)
            {
                searchWindow = ScriptableObject.CreateInstance<GSSearchWindow>();
                searchWindow.Initialize(this);
            }

            //node creation request is called when press Space
            //when press Space, call Unity SearchWindow.Open passing the mouse position to know where open it, and our scriptable object to know what visualize
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
            //we do not need to call GetLocalMousePosition, because we don't need to instantiate on the grap, but just to show a window on the screen
        }

        #endregion

        #region contextual menu

        private IManipulator CreateNodeContextualMenu(string actionTitle, GSNodeType nodeType)
        {
            //update the contextual menu to show when right click on the graph
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator((menuEvent) =>          //menuEvent is the event to trigger when the menu is showed
            {
                //only when click on the graph view (not on one node, group or other things)
                if (menuEvent.target is GraphView)
                {
                    menuEvent.menu.AppendAction(actionTitle, (actionEvent) =>                                           //we take the menu already showed (menuEvent.menu) and add an action "Add Node"
                    CreateNode("NodeName", nodeType,                                                                    //and Create Node will create a node to the mouse position,
                        GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)));                              //using this function to find the correct position even when the graph is zoomed
                }
            });

            return contextualMenuManipulator;
        }

        private IManipulator CreateGroupContextualMenu()
        {
            //update the contextual menu to show when right click on the graph
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator((menuEvent) =>
            {
                //only when click on the graph view or node to add in the group (not on group or other things)
                if (menuEvent.target is GraphView || menuEvent.target is Node)
                {
                    menuEvent.menu.AppendAction("Add Group", (actionEvent) =>                                           //Add Group action
                    CreateGroup("Group", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)));             //call CreateGroup event
                }
            });

            return contextualMenuManipulator;
        }

        #endregion

        #region public API

        /// <summary>
        /// Create a node inside this GraphView
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="nodeType"></param>
        /// <param name="position"></param>
        /// <param name="shouldDraw"></param>
        /// <returns></returns>
        public GSNode CreateNode(string nodeName, GSNodeType nodeType, Vector2 position, bool shouldDraw = true)
        {
            //get node type
            System.Type type = System.Type.GetType($"GraphSystem.Editor.GSNode{nodeType}");

            //create node and use AddElement instead of Add, to add to the GraphView
            GSNode node = System.Activator.CreateInstance(type) as GSNode;
            node.Initialize(nodeName, this, position);

            if (shouldDraw)
            {
                node.Draw();
                node.FinishDraw();
            }

            //add to graph view
            AddElement(node);

            //if first node in the graph, set as Starting Node
            if (StartingNode == null)
                SetStartingNode(node);

            return node;
        }

        /// <summary>
        /// Create a group inside this GraphView
        /// </summary>
        /// <param name="title"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public GSGroup CreateGroup(string title, Vector2 position)
        {
            //add group where drag nodes.
            //Shift + drag to remove
            GSGroup group = new GSGroup(title, position);

            //add to graph view
            AddElement(group);

            //if selecting nodes when create group, add them to it
            foreach (GraphElement element in selection)
            {
                if (element is Node node)
                {
                    group.AddElement(node);
                }
            }

            //if add a node already inside a menu, by default the node is removed from previous group and added to new one

            return group;
        }

        /// <summary>
        /// Mouse position from editor position to graph position.
        /// So it works also if we zoom or move the graph view
        /// </summary>
        /// <param name="mousePosition"></param>
        /// <param name="isScreenPosition">If we pass screen position instead of mousePoint in editor position</param>
        /// <returns></returns>
        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isScreenPosition = false)
        {
            //if screen position instead of editor position, remove the window editor position
            if (isScreenPosition)
                mousePosition -= EditorWindow.position.position;

            return contentViewContainer.WorldToLocal(mousePosition);
        }

        public void SetStartingNode(GSNode node)
        {
            if (node == null || StartingNode == node)
                return;

            //remove color from previous starting node
            if (StartingNode != null)
            {
                StartingNode.mainContainer.RemoveFromClassesList("ds-node__main-container__starting-button");
            }

            //and add to new start node
            StartingNode = node;
            StartingNode.mainContainer.AddToClassesList("ds-node__main-container__starting-button");
        }

        public void ClearGraph()
        {
            graphElements.ForEach(graphElement => RemoveElement(graphElement));
        }

        public void ToggleMinimap()
        {
            minimap.visible = !minimap.visible;
        }

        public bool IsMinimapVisible()
        {
            return minimap != null && minimap.visible;
        }

        #endregion
    }
}