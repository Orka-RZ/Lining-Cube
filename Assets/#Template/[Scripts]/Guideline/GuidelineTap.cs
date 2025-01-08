using System.Collections;
using System.Collections.Generic;
using DancingLineFanmade.Level;
using UnityEngine;

namespace DancingLineFanmade.Guideline
{
    [DisallowMultipleComponent]
    public class GuidelineTap : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Material material;
        [SerializeField] private Sprite sprite;
        [SerializeField] internal float displayTime = -100f;
        [SerializeField] internal float triggerTime;
        [SerializeField] internal float triggerDistance = 1f;
        [SerializeField] internal int colorIndex;
        [SerializeField] internal bool haveLine = true;
        [SerializeField] internal bool triggered;

        private GameObject triggerEffect;
        private BoxCollider autoplayCollider;
        private readonly List<SpriteRenderer> sprites = new();
        private const float timeOffset = 0.25f;

        internal bool autoplay;
        internal bool noEffect;

        private float Distance => (transform.position - Player.Instance.transform.position).sqrMagnitude;

        public void SetColor(List<Color> colors)
        {
            spriteRenderer.color = colors[colorIndex];
        }

        private void Update()
        {
            if (AudioManager.Time > displayTime && !spriteRenderer.enabled && !triggered &&
                LevelManager.GameState == GameStatus.Playing)
                SetDisplay(true);
        }

        public void InitBox(bool auto)
        {
            if (!autoplay)
                Player.Instance.OnTurn.AddListener(Trigger);
            sprites.AddRange(GetComponentsInChildren<SpriteRenderer>());
            triggerEffect = Resources.Load<GameObject>("Prefabs/GuidelineTapEffect");
            spriteRenderer.material = material;
            spriteRenderer.sprite = sprite;
            autoplay = auto;
            noEffect = auto;
            if (displayTime < 0)
            {
                displayTime = 0f;
                SetDisplay(true);
            }
            else SetDisplay(false);
        }

        public void AddBoxCollider(Vector3 size)
        {
            autoplayCollider = gameObject.AddComponent<BoxCollider>();
            autoplayCollider.isTrigger = true;
            autoplayCollider.size = size;
        }

        private void Trigger()
        {
            if (!(Distance <= triggerDistance) || !(Mathf.Abs(AudioManager.Time - triggerTime) <= timeOffset) ||
                triggered)
                return;
            triggered = true;
            if (noEffect)
                return;
            SetDisplay(false);
            StartCoroutine(DisplayEffect());
            Player.Instance.OnTurn.RemoveListener(Trigger);
        }

        public void SetDisplay(bool active)
        {
            foreach (var VARIABLE in sprites)
            {
                VARIABLE.enabled = active;
            }
        }

        public IEnumerator DisplayEffect()
        {
            var color = Color.white;
            var scale = transform.localScale;
            var scaleVector = Vector3.one * 1.05f;
            var effect = Instantiate(triggerEffect, transform.position, Quaternion.Euler(-90, 0, 0)).transform;
            var component = effect.GetComponent<SpriteRenderer>();
            while (color.a > 0f)
            {
                yield return new WaitForSeconds(0.016f);
                color.a -= 0.03f;
                scale.Scale(scaleVector);
                component.color = color;
                effect.localScale = scale;
            }

            Destroy(effect.gameObject);
            yield return null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!autoplay || autoplayCollider == null)
                return;
            if (!other.CompareTag("Player") || triggered)
                return;
            var time = Mathf.Abs((Player.Instance.transform.position - transform.position).magnitude) /
                       Player.Instance.Speed;
            if (time > 0)
                Invoke(nameof(TurnPlayer), time);
            else TurnPlayer();
        }

        private void TurnPlayer()
        {
            Player.Instance.Turn();
        }
    }
}