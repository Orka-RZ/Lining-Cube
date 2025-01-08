using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace DancingLineFanmade.Level
{
    [DisallowMultipleComponent]
    public class CameraFollower : MonoBehaviour
    {
        private Transform selfTransform;

        public static CameraFollower Instance { get; private set; }
        public Camera thisCamera { get; set; }

        [SerializeField] private Transform target;

        [SerializeField] internal Vector3 followSpeed = new(1.5f, 1.5f, 1.5f);
        [SerializeField] internal bool follow = true;
        [SerializeField] internal bool smooth = true;

        internal Transform rotator;
        internal Transform scale;

        private Tween offset;
        private Tween rotation;
        private Tween zoom;
        private Tween shake;
        private Tween fov;

        private float shakePower { get; set; }

        private void Awake()
        {
            Instance = this;
            selfTransform = transform;
        }

        private void Start()
        {
            rotator = selfTransform.Find("Rotator");
            scale = rotator.Find("Scale");
            thisCamera = scale.Find("Camera").GetComponent<Camera>();
        }

        private void Update()
        {
            var translation = target.position - selfTransform.position;
            if (LevelManager.GameState != GameStatus.Playing || !follow)
                return;
            if (smooth)
                selfTransform.Translate(new Vector3(translation.x * followSpeed.x * Time.deltaTime,
                    translation.y * followSpeed.y * Time.deltaTime, translation.z * followSpeed.z * Time.deltaTime));
            else selfTransform.position = target.position;
        }

        internal void Trigger(Vector3 n_offset, Vector3 n_rotation, Vector3 n_scale, float n_fov, float duration,
            Ease ease, RotateMode mode, UnityEvent callback, bool use, AnimationCurve curve)
        {
            SetOffset(n_offset, duration, ease, use, curve);
            SetRotation(n_rotation, duration, mode, ease, use, curve);
            SetScale(n_scale, duration, ease, use, curve);
            SetFov(n_fov, duration, ease, use, curve);
            rotation.OnComplete(callback.Invoke);
        }

        internal void KillAll()
        {
            offset?.Kill();
            rotation?.Kill();
            zoom?.Kill();
            shake?.Kill();
            fov?.Kill();
        }

        private void SetOffset(Vector3 n_offset, float duration, Ease ease, bool use, AnimationCurve curve)
        {
            if (offset != null)
            {
                offset.Kill();
                offset = null;
            }

            offset = !use
                ? rotator.DOLocalMove(n_offset, duration).SetEase(ease)
                : rotator.DOLocalMove(n_offset, duration).SetEase(curve);
        }

        private void SetRotation(Vector3 n_rotation, float duration, RotateMode mode, Ease ease, bool use,
            AnimationCurve curve)
        {
            if (rotation != null)
            {
                rotation.Kill();
                rotation = null;
            }

            rotation = !use
                ? rotator.DOLocalRotate(n_rotation, duration, mode).SetEase(ease)
                : rotator.DOLocalRotate(n_rotation, duration, mode).SetEase(curve);
        }

        private void SetScale(Vector3 n_scale, float duration, Ease ease, bool use, AnimationCurve curve)
        {
            if (zoom != null)
            {
                zoom.Kill();
                zoom = null;
            }

            zoom = !use
                ? scale.DOScale(n_scale, duration).SetEase(ease)
                : scale.DOScale(n_scale, duration).SetEase(curve);
        }

        private void SetFov(float n_fov, float duration, Ease ease, bool use, AnimationCurve curve)
        {
            if (fov != null)
            {
                fov.Kill();
                fov = null;
            }

            fov = !use
                ? thisCamera.DOFieldOfView(n_fov, duration).SetEase(ease)
                : thisCamera.DOFieldOfView(n_fov, duration).SetEase(curve);
        }

        public void DoShake(float power = 1f, float duration = 3f)
        {
            if (shake != null)
            {
                shake.Kill();
                shake = null;
            }

            shake = DOTween.To(() => shakePower, x => shakePower = x, power, duration * 0.5f).SetEase(Ease.Linear);
            shake.SetLoops(2, LoopType.Yoyo);
            shake.OnUpdate(ShakeUpdate);
            shake.OnComplete(ShakeFinished);
        }

        private void ShakeUpdate()
        {
            scale.transform.localPosition = new Vector3(UnityEngine.Random.value * shakePower,
                UnityEngine.Random.value * shakePower, UnityEngine.Random.value * shakePower);
        }

        private void ShakeFinished()
        {
            scale.transform.localPosition = Vector3.zero;
        }
    }

    [Serializable]
    public class CameraSettings
    {
        public Vector3 offset;
        public Vector3 rotation;
        public Vector3 scale;
        public float fov;
        public bool follow;

        internal CameraSettings GetCamera()
        {
            var settings = new CameraSettings();
            var follower = CameraFollower.Instance;
            settings.offset = follower.rotator.localPosition;
            settings.rotation = follower.rotator.localEulerAngles;
            settings.scale = follower.scale.localScale;
            settings.fov = follower.thisCamera.fieldOfView;
            settings.follow = follower.follow;
            return settings;
        }

        internal void SetCamera()
        {
            var follower = CameraFollower.Instance;
            follower.rotator.localPosition = offset;
            follower.rotator.localEulerAngles = rotation;
            follower.scale.localScale = scale;
            follower.scale.localPosition = Vector3.zero;
            follower.thisCamera.fieldOfView = fov;
            follower.follow = follow;
        }
    }
}