using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    [SerializeField] private float _shakeIntensity = 0.05f;
    [SerializeField] private float _shakeDuration = 0.3f;
    [SerializeField] private float _shakeFadeTime = 0.1f;

    private Camera _mainCamera;
    private Vector3 _initialPosition;

    private void Start() {
        _mainCamera = Camera.main;

        _initialPosition = _mainCamera.transform.position;
    }

    // Shake the screen.
    public void Shake() {
        StartCoroutine(ShakeCoroutine());
    }

    // The shake coroutine.
    private IEnumerator ShakeCoroutine() {
        float startTime = Time.time;
        float shakeEndTime = startTime + _shakeDuration;

        while (Time.time < shakeEndTime) {
            float shakeTime = Time.time - startTime;

            // Calculate the shake intensity.
            float intensity = 0f;

            if (shakeTime < _shakeFadeTime) {
                intensity = Mathf.Lerp(0f, _shakeIntensity, shakeTime / _shakeFadeTime);
            } else if (Time.time > shakeEndTime - _shakeFadeTime) {
                intensity = Mathf.Lerp(0f, _shakeIntensity, (shakeEndTime - Time.time) / _shakeFadeTime);
            } else {
                intensity = _shakeIntensity;
            }

            // Calculate the shake position.
            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;
            Vector3 shakePosition = _initialPosition + new Vector3(x, y, 0f);

            // Set the camera position.
            _mainCamera.transform.position = shakePosition;

            // Apparently waits for the next frame.
            yield return null;
        }

        _mainCamera.transform.position = _initialPosition;
    }
}
