using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField, Min(0)] private float speed = 1.5f;

    public void Rotate(float angle)
    {
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private IEnumerator ResetRotate()
    {
        float t = 0;
        while (t < 1 / speed)
        {
            t += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
        transform.localRotation = Quaternion.identity;
    }

    public Coroutine ResetRotation() => StartCoroutine(ResetRotate());


    private IEnumerator Animate(float xVal, float yVal)
    {
        Vector3 endScale = new (xVal, yVal, transform.localScale.z);

        float t = 0;
        while (t < 1/speed)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Slerp(transform.localScale, endScale, t);

            yield return new WaitForEndOfFrame();
        }
        transform.localScale = endScale;
    }

    private IEnumerator AnimateLoop(float xVal, float yVal)
    {
        transform.localScale = Vector3.one;

        yield return StartCoroutine(Animate(xVal, yVal));
        StartCoroutine(Animate(1, 1));
    }

    public Coroutine Stretch(float xVal, float yVal) => StartCoroutine(Animate(xVal, yVal));
    public Coroutine Flatten() => StartCoroutine(Animate(1, 1));
    public Coroutine StretchLoop(float xVal, float yVal) => StartCoroutine(AnimateLoop(xVal, yVal));
}
