using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBoundaries : MonoBehaviour {
    [SerializeField] private Camera _mainCamera;

    private Vector2 _screenBounds;
    private float _playerSpriteWidth;
    private float _playerSpriteHeight;

    private void Start () {
        _screenBounds = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _mainCamera.transform.position.z));
        _playerSpriteWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x; // extents = width / 2
        _playerSpriteHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y; // extents = height / 2
    }

    private void LateUpdate() {
        Vector3 viewPos = transform.position;

        // Multiply with -1 because Camera coordinates are (-x, -y)
        viewPos.x = Mathf.Clamp(viewPos.x, (_screenBounds.x * -1) + _playerSpriteWidth, _screenBounds.x - _playerSpriteWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, (_screenBounds.y * -1) + _playerSpriteHeight, _screenBounds.y - _playerSpriteHeight);

        transform.position = viewPos;
    }
}