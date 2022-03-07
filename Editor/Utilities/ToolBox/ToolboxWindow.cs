using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EditorUtilities.Editor.Utilities.ToolBox
{
    [Serializable]
    public abstract class ToolboxWindow<ToolType> : EditorWindow
    where ToolType : ATool
    {
        public abstract int ToolBoxRowSize
        {
            get;
        }

        private void OnValidate()
        {
            InitToolsIfNeeded();
        }

        #region Inspector Drawing

        private void OnGUI()
        {
            PreToolGUI();
            ToolSelectionGUI();
            DrawToolGUI();
            PostToolGUI();
        }

        protected virtual void PostToolGUI() {}

        protected virtual void PreToolGUI() {}

        
        private void DrawToolGUI()
        {
            CurrentTool?.OnGUI();
        }

        private void ToolSelectionGUI()
        {
            m_CurrentToolIndex = GUILayout.SelectionGrid(
                m_CurrentToolIndex, m_ToolsTypes.TypeNames, 
                ToolBoxRowSize
            );
        }

        #endregion

        #region Scene Drawing
        private void OnSceneGUI(SceneView sceneView)
        {
            SceneGUI();
            CurrentTool?.OnSceneGUI(sceneView);
        }

        protected virtual void SceneGUI() { }
        
        #endregion

        #region Tool Handling

        [SerializeReference] private List<ToolType> m_Instances = new List<ToolType>();
        [SerializeField] private int m_CurrentToolIndex = -1;

        protected ToolType CurrentTool
        {
            get
            {
                return m_CurrentToolIndex == -1 ? null : m_Instances[m_CurrentToolIndex];
            }
        }

        private DisplayableTypes m_ToolsTypes;

        private void InitToolsIfNeeded()
        {
            m_ToolsTypes = DisplayableTypes.CreateFromSubClass<ToolType>();
            if (m_ToolsTypes.Types.Length != m_Instances.Count)
            {
                var types = m_ToolsTypes.Types
                    .Where(type => m_Instances.All(instance => instance.GetType() != type));
                m_Instances.AddRange(
                    types
                            .Select(Activator.CreateInstance).Cast<ToolType>()
                    );
            }
        }

        #endregion

        #region Delegates Handling

        private void OnEnable()
        {
            InitToolsIfNeeded();
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDestroy()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        #endregion
    }
}