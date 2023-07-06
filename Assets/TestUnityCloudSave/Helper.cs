using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public static class Helper
{
    public static void LookAt2D(this Transform _tf, Vector2 _forward)
    {
        _tf.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(_forward.y, _forward.x) * Mathf.Rad2Deg);
    }

    public static async UniTask RotateLerp(this Transform _tf, Vector3 _targetPosition, float _duration)
    {
        float time = 0;
        Vector3 directionStart = _tf.right;
        Vector3 direction = Vector3.zero;
        while (time < _duration)
        {
            direction = Vector3.Lerp(directionStart, _targetPosition, time / _duration);
            _tf.LookAt2D(direction);
            time += Time.deltaTime;
            await UniTask.Yield();
        }
        _tf.LookAt2D(_targetPosition);
    }

    public static async UniTask RotateLerp(this Transform _tf, Transform _targetPosition, float _duration)
    {
        float time = 0;
        Vector3 directionStart = _tf.right;
        Vector3 directionOrigin = Vector3.zero;
        Vector3 directionStartDelta = _targetPosition.position - _tf.position;
        Vector3 directionStartDeltaCheck = directionStartDelta;
        while (time < _duration)
        {
            if (directionStartDeltaCheck != directionStartDelta)
            {
                directionStartDeltaCheck = directionStartDelta;
                time = 0f;
                directionStart = _tf.right;
            }
            directionOrigin = Vector3.Lerp(directionStart, directionStartDelta, time / _duration);
            _tf.LookAt2D(directionOrigin);
            time += Time.deltaTime;
            await UniTask.Yield();
            directionStartDelta = _targetPosition.position - _tf.position;
        }
        _tf.LookAt2D(directionStartDelta);
    }
}
