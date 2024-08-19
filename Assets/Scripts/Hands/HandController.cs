using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hands
{
    public class HandController : MonoBehaviour
    {
        public bool CanMove { get; set; } = true;

        [SerializeField] private float _maxSpeed = 10f;
        [SerializeField] private float _acceleration = 10f;
        [SerializeField] private float _deceleration = 10f;
        [SerializeField] private float _turnSpeed = 10f;

        [SerializeField] private HandPositionConstraints _positionConstraints;

        [SerializeField] private EdgeCollider2D _edgeCollider;
        [SerializeField] private BoxCollider2D _boxCollider;

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
            if (CanMove is false) return;
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

            if (CanMove)
            {
                CalculateCurrentVelocity();
                ClampCurrentVelocity();
                _rigidbody.velocity = _velocity;
            }
            else
            {
                _rigidbody.velocity = Vector2.zero;
                _direction.Value = Vector2.zero;
                _velocity = Vector2.zero;
            }
        }
    
        private void CalculateCurrentVelocity()
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange.x);
            _velocity.y = Mathf.MoveTowards(_velocity.y, _desiredVelocity.y, _maxSpeedChange.y);
        }

        private void ClampCurrentVelocity()
        {
            var positionAtEndOfStep = _rigidbody.position + _velocity * Time.deltaTime;
            positionAtEndOfStep.x = Mathf.Clamp(
                positionAtEndOfStep.x, 
                _positionConstraints.MinPosition.x,
                _positionConstraints.MaxPosition.x);
            positionAtEndOfStep.y = Mathf.Clamp(
                positionAtEndOfStep.y, 
                _positionConstraints.MinPosition.y,
                _positionConstraints.MaxPosition.y);
            _velocity = (positionAtEndOfStep - _rigidbody.position) / Time.deltaTime;
        }

        public void EnableColliders()
        {
            _edgeCollider.enabled = true;
            _boxCollider.enabled = true;
        }

        public void DisableColliders()
        {
            _edgeCollider.enabled = false;
            _boxCollider.enabled = false;
        }
    }
}
