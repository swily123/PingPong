using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    [Tooltip("0 - EnemySide, 1 - PlayerSide")]
    [SerializeField] private Zone[] _zones;
    
    [Header("Ball")]
    [SerializeField] private float _height;
    [SerializeField] private Ball _ball;
    [SerializeField] private Transform _startPoint;
    
    [Header("Racket")]
    [SerializeField] private Transform _playerRacket;
    [SerializeField] private Vector2 _racketAreaSize;
    [SerializeField] private Transform _racketArea;
    [SerializeField] private float _sensitivity;
    [SerializeField] private BallCollision _ballCollision;
    
    [Header("Predictor")]
    [SerializeField] private Predictor _predictor;

    [Header("Enemy")]
    [SerializeField] private Transform _enemyRacket;
    
    private readonly Thrower _thrower = new();
    private Vector3 _playerRacketPosition;
    private float _collisionCooldown;
    
    private void Awake()
    {
        _predictor.Prepare();
    }

    private void OnEnable()
    {
        _ballCollision.CollisionEntered += ThrowPlayerToEnemy;
    }
    
    private void OnDisable()
    {
        _ballCollision.CollisionEntered -= ThrowPlayerToEnemy;
    }

    private void Update()
    {
        if (_collisionCooldown > 0)
            _collisionCooldown -= Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
            StartGame();

        _playerRacket.localPosition = _playerRacketPosition;
        _playerRacketPosition += (Vector3.left * Input.GetAxis("Mouse X") + Vector3.up * Input.GetAxis("Mouse Y")) * _sensitivity;
        _playerRacketPosition.x = Mathf.Clamp(_playerRacketPosition.x, -_racketAreaSize.x, _racketAreaSize.x);
        _playerRacketPosition.y = Mathf.Clamp(_playerRacketPosition.y, -_racketAreaSize.y, _racketAreaSize.y);
    }

    private void StartGame()
    {
        _ball.SetPosition(_startPoint.position);
        ThrowPlayerToEnemy();
    }

    private void ThrowPlayerToEnemy( )
    {
        if (_collisionCooldown > 0)
            return;

        _collisionCooldown = 0.1f;
        
        Vector3 endPoint = _zones[0].GetRandomPointInZone();
        _ball.SetVelocity(_thrower.CalculateVelocityByHeight(_ball.transform.position, endPoint, _height));

        var endPosition = _predictor.Predict(true, out float time);
        StartCoroutine(EnemyRacketRoutine(time, endPosition));
    }

    private void ThrowEnemyToPlayer()
    {
        Vector3 endPoint = _zones[1].GetRandomPointInZone();
        _ball.SetVelocity(_thrower.CalculateVelocityByHeight(_ball.transform.position, endPoint, _height));
    }
    
    private IEnumerator EnemyRacketRoutine(float duration, Vector3 enemyEndPoint)
    {
        float timer = duration;
        var enemyStartPosition = _enemyRacket.transform.position;
        
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
            
            float time = Mathf.Clamp01(1f - timer / duration);
            time = Mathf.SmoothStep(0, 1, time);
            _enemyRacket.transform.position = Vector3.Lerp(enemyStartPosition, enemyEndPoint, time);
        }

        ThrowEnemyToPlayer();
    }
}