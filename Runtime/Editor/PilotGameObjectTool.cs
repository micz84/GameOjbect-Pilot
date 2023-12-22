﻿using System;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

namespace pl.micz84.gameobjectpilot.Editor
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    [Icon("Packages/pl.micz84.gameobjectpilot/Runtime/Icons/camera.png")]
    public class PilotGameObjectTool : EditorToolbarToggle
    {
        public const string ID = "pilot-game-object-tool";

        public PilotGameObjectTool()
        {
            icon = AssetDatabase.LoadAssetAtPath<Texture2D>(
                "Packages/pl.micz84.gameobjectpilot/Runtime/Icons/camera.png");
            tooltip = "Pilot GameObject";
            GameObjectPilotOverlay.OnDisplayedChanged += OnDisplayedChangedHandler;
            RegisterCallback<ChangeEvent<bool>>(ChangeEventCallback);
        }

        private void OnDisplayedChangedHandler(bool isShown)
        {
            if(isShown)
                return;
            
            UnregisterCallback<ChangeEvent<bool>>(ChangeEventCallback);
            SceneView.duringSceneGui -= SceneViewOnDuringSceneGui;
            GameObjectPilotOverlay.OnDisplayedChanged -= OnDisplayedChangedHandler;
        }

        private void ChangeEventCallback(ChangeEvent<bool> evt)
        {
            if (evt.newValue)
                SceneView.duringSceneGui += SceneViewOnDuringSceneGui;
            else
                SceneView.duringSceneGui -= SceneViewOnDuringSceneGui;
        }

        private void SceneViewOnDuringSceneGui(SceneView sceneView)
        {
            var sceneViewCameraTransform = sceneView.camera.transform;
            var position = sceneViewCameraTransform.position;
            var rotation = sceneViewCameraTransform.rotation;
            var selectedGameObjects = Selection.gameObjects;

            for (var i = 0; i < selectedGameObjects.Length; i++)
            {
                var t = selectedGameObjects[i].transform;
                t.position = position;
                t.rotation = rotation;
            }
        }
    }
}