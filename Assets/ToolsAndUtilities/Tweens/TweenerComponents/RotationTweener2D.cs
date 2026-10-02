using UnityEngine;

namespace Tweens
{
    public sealed class RotationTweener2D : MonoBehaviour
    {
        public event System.Action Rotation2DTweenFinished;

        public void TweenRotation2D(
            GameObject obj,
            float eulerAngles,
            float duration,
            EnumTweenEase easeType = EnumTweenEase.LINEAR,
            EnumTweenDirection easeDirection = EnumTweenDirection.IN_OUT,
            bool clockwise = true
            )
        {
            if (!clockwise) { eulerAngles *= -1; }
            Tween rotTween = TweenService.GetFloatTween(
                obj,
                obj.transform.rotation.eulerAngles.z,
                eulerAngles,
                duration,
                easeType,
                easeDirection
                );
            rotTween.OnFinished += () =>
            {
                Rotation2DTweenFinished?.Invoke();
            };
            rotTween.OnValueUpdated += (value) =>
            {
                Vector3 currentRot = obj.transform.rotation.eulerAngles;
                Vector3 rotVector = new Vector3(0f, 0, value.x);
                obj.transform.rotation = Quaternion.Euler(currentRot + rotVector);
            };
            rotTween.StartTween();
        }
    }
}