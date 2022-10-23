using System.Collections;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField, Min(0)] private float speed = 1.5f;
    [SerializeField, Min(0)] private float fastSpeed = 4f;

    public void Rotate(float angle)
    {
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private IEnumerator ResetRotate()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * speed;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
        transform.localRotation = Quaternion.identity;
    }

    public Coroutine ResetRotation() => StartCoroutine(ResetRotate());


    private IEnumerator Animate(float xVal, float yVal, bool fast)
    {
        Vector3 endScale = new(xVal, yVal, transform.localScale.z);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * (fast ? fastSpeed : speed);
            transform.localScale = Vector3.Lerp(transform.localScale, endScale, t);

            yield return new WaitForEndOfFrame();
        }
        transform.localScale = endScale;
    }

    private IEnumerator AnimateLoop(float xVal, float yVal)
    {
        transform.localScale = Vector3.one;

        yield return StartCoroutine(Animate(xVal, yVal, false));
        StartCoroutine(Animate(1, 1, false));
    }

    public Coroutine Stretch(float xVal, float yVal) => StartCoroutine(Animate(xVal, yVal, false));
    public Coroutine FastStretch(float xVal, float yVal) => StartCoroutine(Animate(xVal, yVal, true));
    public Coroutine Flatten() => StartCoroutine(Animate(1, 1, false));
    public Coroutine StretchLoop(float xVal, float yVal) => StartCoroutine(AnimateLoop(xVal, yVal));
}
