using UnityEngine;
using System.Collections;

public class CameraEffects : MonoBehaviour
{
    public AnimationCurve curve;
    public float duration;
    public float distance;

    public bool inEffect;

    private Vector3 startPosition;
    private float startTime;

    public void PlayImpact(Vector3 direction)
    {
        startPosition = transform.localPosition;
        startTime = Time.time;
        inEffect = true;

        StartCoroutine(MoveCamera(direction));
    }

    private IEnumerator MoveCamera(Vector3 direction)
    {
        while (true)
        {
            float timePassed = Time.time - startTime;
            float curveValue = curve.Evaluate(timePassed / duration);
            Vector3 offset = direction * curveValue * distance;
            transform.localPosition = startPosition + offset;

            if (timePassed >= duration)
            {
                break;
            }

            yield return null;
        }

        yield return StartCoroutine(MoveCameraBack());
    }

    private IEnumerator MoveCameraBack()
    {
        Vector3 endPosition = startPosition;
        startPosition = transform.localPosition;
        startTime = Time.time;

        while (true)
        {
            float timePassed = Time.time - startTime;
            float curveValue = curve.Evaluate(timePassed / duration);
            Vector3 offset = (endPosition - startPosition) * curveValue;
            transform.localPosition = startPosition + offset;

            if (timePassed >= duration)
            {
                transform.localPosition = endPosition;
                inEffect = false;
                break;
            }

            yield return null;
        }
    }
}
