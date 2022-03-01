using UnityEditor;
using UnityEngine;

namespace EditorUtilities.Editor.Utilities.ToolBox
{
    public abstract class ATool
    {
        public virtual void OnGUI() {}

        public virtual void OnSceneGUI(SceneView _view) {}
        
        protected void FilterSelectionEvents()
        {
            if (Event.current.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(0);
            }
        }
    }
}