using System;
using System.Linq;
using CodeBase.Logic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(UniqueId))]
    public class UniqueIdEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            var uniqueId = (UniqueId) target;

            if (IsPrefab(uniqueId))
            {
                uniqueId.Reset();
                return;
            }

            if (string.IsNullOrEmpty(uniqueId.Id))
            {
                GenerateUniqueIdFor(uniqueId);
            }
            else
            {
                var uniqueIds = FindObjectsOfType<UniqueId>();

                if (uniqueIds.Any(other => other != uniqueId && other.Id == uniqueId.Id))
                {
                    GenerateUniqueIdFor(uniqueId);
                }
            }
        }

        private void GenerateUniqueIdFor(UniqueId uniqueId)
        {
            uniqueId.Generate();
            uniqueId.Generated += MarkAsEdited;
        }


        private bool IsPrefab(UniqueId uniqueId)
        {
            var uniqueIdGameObject = uniqueId.gameObject;
            return uniqueIdGameObject.scene.rootCount == 0 || uniqueIdGameObject.name == uniqueIdGameObject.scene.name;
        }

        private void MarkAsEdited(string sceneName)
        {
            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(this);
                EditorSceneManager.MarkSceneDirty(SceneManager.GetSceneByName(sceneName));
            }
        }
    }
}