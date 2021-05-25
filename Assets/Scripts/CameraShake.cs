using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    private Vector3 _originalPos;
    [SerializeField]
    private float _shakeDuration = .7f;
    private float _currentShakeDuration;
    [SerializeField]
    private float _shakeStrength = 2f;
    private bool _isShakeCameraTime = false;

    void Start()
    {
        _originalPos = transform.position;
        _currentShakeDuration = _shakeDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isShakeCameraTime)
        {
            if (_currentShakeDuration > 0)
            {
                transform.position = _originalPos + (Vector3) Random.insideUnitCircle * _shakeStrength * Time.deltaTime;
                _currentShakeDuration -= Time.deltaTime;
            }
            else
            {
                transform.position = _originalPos;
                _isShakeCameraTime = false;
                _currentShakeDuration = _shakeDuration;
            }
        }
    }

    public void ShakeCamera()
    {
        _isShakeCameraTime = true;
    }

}
