using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeBase.CustomGizmos;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Logic.SaveLoad;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        private const string InitialPointTag = "InitialPoint";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LevelStaticData levelData = (LevelStaticData) target;

            if (GUILayout.Button("Collect"))
            {
                ChangeLevelData(ref levelData);
                EditorUtility.SetDirty(target);
            }
        }

        private void ChangeLevelData(ref LevelStaticData levelData)
        {
            levelData.Spawners = CollectAllSpawnMarkers();
            levelData.SceneKey = SceneManager.GetActiveScene().name;
            levelData.InitialPoint = InitialPoint();
            levelData.NextSceneKey = NextScene();
            InitializeLevelTransferTriggerInLevelData(levelData);
            InitializeSaveTriggerLevelData(levelData);
        }

        private void InitializeLevelTransferTriggerInLevelData(LevelStaticData levelData)
        {
            GameObject levelTransferTriggerObject = LevelTransferTrigger();
            
            if (levelTransferTriggerObject == null)
            {
                levelData.LevelTransferTrigger.Position = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                levelData.LevelTransferTrigger.ColliderSize = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                Debug.LogError("In current scene is not located object with LevelTransferTrigger tag");
                return;
            }
            levelData.LevelTransferTrigger.Position = levelTransferTriggerObject.transform.position;
            levelData.LevelTransferTrigger.ColliderSize = levelTransferTriggerObject.GetComponent<TriggerGizmo>().Size;
        }

        private static void InitializeSaveTriggerLevelData(LevelStaticData levelStaticData)
        {
            GameObject saveTrigger = SaveTrigger();

            if (saveTrigger == null)
            {
                levelStaticData.SaveTrigger.Position = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                levelStaticData.SaveTrigger.ColliderSize = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                Debug.LogError("In current scene is not located object with SaveTrigger tag");
                return;
            }
            
            levelStaticData.SaveTrigger.Position = saveTrigger.transform.position;
            levelStaticData.SaveTrigger.ColliderSize = saveTrigger.GetComponent<TriggerGizmo>().Size;
        }

        private static GameObject SaveTrigger() => 
            GameObject.FindGameObjectWithTag("SaveTrigger");

        private GameObject LevelTransferTrigger() =>
            GameObject.FindGameObjectWithTag("LevelTransferTrigger");
        
        private string NextScene()
        {
            int index = 0;
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if (IsCurrentScene(EditorBuildSettings.scenes[i]))
                {
                    i++;
                    if (i == EditorBuildSettings.scenes.Length)
                    {
                        Debug.LogError(
                            $"This is last scene in build settings, NextSceneKey field of {GetType()} set in empty ",
                            this);

                        return String.Empty;
                    }

                    index = i;
                }
            }

            return Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[index].path);


            bool IsCurrentScene(EditorBuildSettingsScene editorBuildSettingsScene)
            {
                return Path.GetFileNameWithoutExtension(editorBuildSettingsScene.path) ==
                       SceneManager.GetActiveScene().name;
            }
        }


        private List<EnemySpawnerData> CollectAllSpawnMarkers() =>
            FindObjectsOfType<SpawnMarker>()
                .Select(x =>
                    new EnemySpawnerData(
                        x.GetComponent<UniqueId>().Id,
                        x.Type(), x.transform.position
                    )
                ).ToList();

        private static Vector3 InitialPoint() =>
            GameObject.FindWithTag(InitialPointTag).transform.position;
    }
}