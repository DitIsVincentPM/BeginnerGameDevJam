using UnityEngine;

public class LichtFlasher : MonoBehaviour
{
    private Light _light;
    private float _elapsedTime;
    private bool _isRed;
    private Color _startColor;
    private Color _endColor;

    public float FlashDuration = 2f;

    void Start()
    {
        _light = GetComponent<Light>();
        _elapsedTime = 0f;
        _isRed = false;
        _startColor = new Color(0.78f,0.1945087f,0.1993889f);
        _endColor = Color.red;
    }

    void Update()
    {
        _elapsedTime += Time.deltaTime;
        float t = _elapsedTime / FlashDuration;

        if (t > 1f)
        {
            _isRed = !_isRed;
            _elapsedTime = 0f;
            t = 0f;
        }

        if (_isRed)
        {
            _light.color = Color.Lerp(_startColor, _endColor, t);
        }
        else
        {
            _light.color = Color.Lerp(_endColor, _startColor, t);
        }
    }
}