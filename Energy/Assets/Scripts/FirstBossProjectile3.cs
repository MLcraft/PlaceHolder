using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossProjectile3 : MonoBehaviour {
    public float velocityX, velocityY;
    private Rigidbody2D _rb;
    private float _firstPart;
    private float _secondPart;
    private float _count;
    private float _rotationSpeed;
    private float _rotationTime;
    private float _angleOffset;
    private float _speed;
    private bool stopRotation;

    // Use this for initialization
    void Start()
    {
        _count = 0;
        _firstPart = 1.0f;
        _secondPart = _firstPart + 0.1f;
        _rb = GetComponent<Rigidbody2D>();
        _rotationSpeed = 10.0f;
        _angleOffset = 90.0f;
        stopRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_count < _firstPart)
        {
            _count += Time.deltaTime;
            _rb.velocity = new Vector2(velocityX, velocityY);
        }
        else if (_count < _secondPart)
        {
            velocityX = 0;
            velocityY = 0;
            _rb.velocity = new Vector2(velocityX, velocityY);
            _count += Time.deltaTime;
        }
        else if (!stopRotation)
        {
            Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            Vector3 direction = playerPos - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * _rotationSpeed);

            _rotationTime += Time.deltaTime;
            if (_rotationTime >= 0.5f)
            {
                stopRotation = true;
            }
        }
        else
        {

            transform.position += transform.up * _speed * Time.deltaTime;
        }
    }

    public void setVelocity(float x, float y)
    {
        velocityX = x;
        velocityY = y;
    }

    public void setup(float x, float y, float angle, float speed, bool isRightSide)
    {
        velocityX = x;
        velocityY = y;
        _speed = isRightSide ? speed + 0.1f : speed;
        
        transform.rotation = Quaternion.Euler(0, 0, _angleOffset + angle);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
