using UnityEngine;

public class Game : MonoBehaviour
{
    [Tooltip("0 - EnemySide, 1 - PlayerSide")]
    [SerializeField] private Zone[] _zones;
    
    [SerializeField] private float _height;
    [SerializeField] private Ball _ball;
    [SerializeField] private Transform _startPoint;
    
    private readonly Thrower _thrower = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 endPoint = _zones[0].GetRandomPointInZone();
            _ball.SetPosition(_startPoint.position);
            _ball.SetVelocity(_thrower.CalculateVelocityByHeight(_startPoint.position, endPoint, _height));
        }
    }
}