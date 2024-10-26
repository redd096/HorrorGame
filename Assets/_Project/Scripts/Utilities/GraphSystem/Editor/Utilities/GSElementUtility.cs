using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace GraphSystem.Editor
{
    public static class GSElementUtility
    {
        public static Button CreateButton(string text, System.Action onClick = null)
        {
            Button button = new Button(onClick)
            {
                text = text,
            };

            return button;
        }

        public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            Foldout foldout = new Foldout()
            {
                text = title,
                value = !collapsed,
            };

            return foldout;
        }

        public static Port CreatePort(this Node node, string portName = "", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));
            port.portName = portName;

            return port;
        }

        public static TextField CreatetextField(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            //create textfield
            TextField textField = new TextField()
            {
                value = value,
                label = label,
            };

            //register event if not null
            if (onValueChanged != null)
            {
                textField.RegisterValueChangedCallback(onValueChanged);
            }

            return textField;
        }

        public static TextField CreateTextArea(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textArea = CreatetextField(value, label, onValueChanged);
            textArea.multiline = true;
            return textArea;
        }

        public static Label CreateLabel(string value)
        {
            Label label = new Label(value);
            return label;
        }
    }
}