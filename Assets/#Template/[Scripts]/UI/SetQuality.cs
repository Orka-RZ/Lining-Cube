using DancingLineFanmade.Level;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace DancingLineFanmade.UI
{
    [DisallowMultipleComponent]
    public class SetQuality : MonoBehaviour
    {
        [SerializeField] private Text text;
        PostProcessVolume post;
        public int id;

        private void Start()
        {
            id = 1;
            SetText();

        }

        public void SetLevel(bool add)
        {
            if (add)
                id = id++ >= 2 ? id = 0 : id++;
            else id = id-- <= 0 ? id = 2 : id--;
            QualitySettings.SetQualityLevel(id);
            SetText();
        }

        private void SetText()
        {
            post = FindObjectOfType<PostProcessVolume>();
            LevelManager.SetFPSLimit(int.MaxValue);
            switch (id)
            {
                case 0:
                    text.text = "中";
                    QualitySettings.shadows = ShadowQuality.Disable;
                    post.enabled = false;
                    break;
                case 1:
                    text.text = "高";
                    QualitySettings.shadows = ShadowQuality.All;
                    post.enabled = false;
                    break;
                case 2:
                    text.text = "好高";
                    QualitySettings.shadows = ShadowQuality.All;
                    post.enabled = true;
                    break;
            }
        }
    }
}