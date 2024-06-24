using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

[Serializable]
public class EnemyMover : CharacterMover 
{
    [SerializeField] private Transform[] _patrolPoints;

    private int _currentPoint;

    public async UniTask PatrolAsync(CancellationToken token)
    {
        while (token.IsCancellationRequested == false)
        {
            _currentPoint = (_currentPoint + 1) % _patrolPoints.Length;
            await MoveToAsync(_patrolPoints[_currentPoint], token);
        }
    }

    public async UniTask MoveToAsync(Transform point, CancellationToken token)
    {
        const float RangePointDetecting = 0.5f;

        while (Vector2.Distance(Transform.position, point.position) > RangePointDetecting && token.IsCancellationRequested == false)
        {
            Transform.position = Vector2.MoveTowards(Transform.position, point.position, Speed * Time.deltaTime);
            await UniTask.Yield();
        }
    }
}