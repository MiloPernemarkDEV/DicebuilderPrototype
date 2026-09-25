using UnityEngine;
using Tweens;

public class TweenTester : MonoBehaviour
{
    void Start()
    {
        TestTween();
    }

    private void TestTween()
    {
        float testFloat = 0f;
        Tween testTween = TweenService.GetFloatTween(gameObject, 0f, 4f, 5f, EnumTweenEase.QUART, EnumTweenDirection.IN);
        testTween.OnValueUpdated += (value) =>
        {
            testFloat = value.x;
            transform.localScale = new Vector3(testFloat, testFloat, testFloat);
            Debug.Log(testFloat);
        };
        testTween.StartTween();
    }
}
