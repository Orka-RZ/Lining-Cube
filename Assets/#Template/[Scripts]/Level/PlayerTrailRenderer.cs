#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Linq;
using DancingLineFanmade.Guideline;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DancingLineFanmade.Level
{
    [DisallowMultipleComponent]
    public class PlayerTrailRenderer : MonoBehaviour
    {
        [SerializeField] private ScoreReader reader;
        [SerializeField] private GuidelineManager controller;
        [SerializeField] private int maxDistance = 36000;
        [SerializeField] private Color trailColor = Color.blue;
        [SerializeField] private Vector3 trailOffset = new(0f, 0.4f, 0f);
        [SerializeField] private bool render;

        private List<Transform> trans = new();

#if UNITY_EDITOR
        [Button("Reload Trail Data", ButtonSizes.Large)]
        private void Reload()
        {
            trans.Clear();
            OnValidate();
        }

        private void OnValidate()
        {
            if (!controller.boxHolder)
                return;
            trans = controller.boxHolder.GetComponentsInChildren<Transform>().ToList();
            trans.RemoveRange(0, 2);
        }

        private void OnDrawGizmos()
        {
            if (reader == null || controller == null)
            {
                render = false;
                Debug.LogError("谱面读取器或引导线控制器未选择");
                return;
            }

            if (Application.isPlaying)
                return;
            if (!render)
                return;
            if (reader.hitTime.Count <= 0 || controller.boxHolder == null)
            {
                render = false;
                Debug.LogError("无法读取点击时间或引导线父物体未选择");
                return;
            }

            var textureBackground = new Texture2D(1, 1);
            textureBackground.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.6f));
            textureBackground.Apply();

            var style = new GUIStyle
            {
                normal =
                {
                    textColor = Color.white,
                    background = textureBackground
                },
                fontSize = 15
            };

            var rendererCamera = SceneView.lastActiveSceneView.camera;

            Gizmos.color = trailColor;
            Handles.color = trailColor;
            for (var i = 0; i < trans.Count; i++)
            {
                if (!((trans[i].position - rendererCamera.transform.position).sqrMagnitude <= maxDistance))
                    continue;
                if (i < trans.Count - 1)
                    Handles.DrawLine(trans[i].position + trailOffset, trans[i + 1].position + trailOffset, 3f);
                Gizmos.DrawCube(trans[i].position + trailOffset, Vector3.one * 0.3f);
                var text = $"[{i + 1}] {reader.hitTime[i]}";
                Handles.Label(trans[i].position + trailOffset, text, style);
            }
        }
#endif
    }
}