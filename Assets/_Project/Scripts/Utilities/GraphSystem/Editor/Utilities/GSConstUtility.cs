namespace GraphSystem.Editor
{
    public static class GSConstUtility
    {
        //styles paths
        public const string VariablesStylesPath = "Packages/com.mollica.graph-system/Editor/Styles/GSVariables.uss";
        public const string GraphViewStylesPath = "Packages/com.mollica.graph-system/Editor/Styles/GSGraphViewStyles.uss";
        public const string NodeStylesPath = "Packages/com.mollica.graph-system/Editor/Styles/GSNodeStyles.uss";
        public const string MinimapStylesPath = "Packages/com.mollica.graph-system/Editor/Styles/GSMinimapStyles.uss";
        public const string ToolbarStylesPath = "Packages/com.mollica.graph-system/Editor/Styles/GSToolbarStyles.uss";

        //save folder
        public static string SavedFolderPath => System.IO.Path.Combine("Assets", SavedFolderName);
        public const string SavedFolderName = "SavedGraphs";

        //file names
        public const string DefaultFileName = "GraphFileName";
    }
}