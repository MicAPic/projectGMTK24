using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hands
{
    public class HandController : MonoBehaviour
    {
        public float _maxSpeed = 10f;
        public float _acceleration = 10f;
        public float _deceleration = 10f;
        public float _turnSpeed = 10f;
            
        public IReadOnlyReactiveProperty<Vector2> Direction => _direction;
        private ReactiveProperty<Vector2> _direction;
        private Vector2 _desiredVelocity;
        private Vector2 _velocity;
        private Vector2 _maxSpeedChange;
    
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
    
            _direction = new ReactiveProperty<Vector2>(Vector2.zero);
        }
            
        public void OnMoveHand(InputValue value)
        {
            _direction.Value = value.Get<Vector2>();
        }

        // Update is called once per frame
        private void Update()
        {
            _desiredVelocity = _direction.Value * Mathf.Max(_maxSpeed, 0f);
        }
    
        private void FixedUpdate()
        {
            var directionX = _direction.Value.x;
            var directionY = _direction.Value.y;
            
            if (directionX != 0)
            {
                if (Mathf.Sign(directionX) != Mathf.Sign(_velocity.x))
                {
                    _maxSpeedChange.x = _turnSpeed * Time.deltaTime;
                }
                else
                {
                    _maxSpeedChange.x = _acceleration * Time.deltaTime;
                }
            }
            else
            {
                _maxSpeedChange.x = _deceleration * Time.deltaTime;
            }
            
            if (directionY != 0)
            {
                if (Mathf.Sign(directionY) != Mathf.Sign(_velocity.y))
                {
                    _maxSpeedChange.y = _turnSpeed * Time.deltaTime;
                }
                else
                {
                    _maxSpeedChange.y = _acceleration * Time.deltaTime;
                }
            }
            else
            {
                _maxSpeedChange.y = _deceleration * Time.deltaTime;
            }
            
            Move();
        }
    
        private void Move()
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange.x);
            _velocity.y = Mathf.MoveTowards(_velocity.y, _desiredVelocity.y, _maxSpeedChange.y);
            _rigidbody.velocity = _velocity;
        }
    }
}
