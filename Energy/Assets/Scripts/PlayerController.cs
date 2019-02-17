using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum States { Idle, Moving, Shooting };         // States of the player

    public float speed;                             // Movement Speed of the player
    public float focusSpeed;
    public float health;
    public float maxHealth;
	public float fuel;
	public float fireRate;         // Rate of fire
	public GameObject projectile;
	public PlayerHitboxController hitbox;

    private States _state = States.Idle;            // Current State of the Player
    private float _xVelocity;                       // X Velocity of the player
    private float _yVelocity;                       // Y Velocity of the player
    private bool _isFocused;                        // Focused Mechanic
	private float _delay;           // Delay until the next shot
    private Rigidbody2D _rb;

    // Use this for initialization
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerInput();

        if (_isFocused)
        {
            if (_xVelocity != 0)
                _xVelocity = Input.GetAxis("Horizontal") * focusSpeed * Time.deltaTime;
            if (_yVelocity != 0)
                _yVelocity = Input.GetAxis("Vertical") * focusSpeed * Time.deltaTime;
        }

		if (_delay <= 0)
		{
			_delay += fireRate;
			// Create Projectile
			if (Input.GetAxis ("Shoot") != 0) {
				Vector2 projectilePosition = transform.position;

				Instantiate (projectile, projectilePosition, Quaternion.identity);
			}
		}
		else
			_delay -= Time.deltaTime;
		
		if (Camera.main.WorldToViewportPoint (transform.position).x < 0 && _xVelocity < 0)
			_xVelocity = 0;
		if (Camera.main.WorldToViewportPoint(transform.position).x > 1 && _xVelocity > 0) 
			_xVelocity = 0;
		if (Camera.main.WorldToViewportPoint(transform.position).y < 0 && _yVelocity < 0) 
			_yVelocity = 0;
		if (Camera.main.WorldToViewportPoint(transform.position).y > 1 && _yVelocity > 0) 
			_yVelocity = 0;
		_rb.velocity = new Vector2(_xVelocity, _yVelocity);
		hitbox.setVelocity(new Vector2 (_xVelocity, _yVelocity));
    }

    void CheckPlayerInput()
    {

        if (_state == States.Idle)
        {
            _xVelocity = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            _yVelocity = Input.GetAxis("Vertical") * speed * Time.deltaTime;


            CheckFocusInput();
        }
        else if (_state == States.Moving)
        {
            _xVelocity = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            _yVelocity = Input.GetAxis("Vertical") * speed * Time.deltaTime;

            if (_xVelocity == 0 && _yVelocity == 0)
                _state = States.Idle;

            CheckFocusInput();
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Projectile")
        {
            Destroy(coll.gameObject);
            health -= 1;
        }
    }

    // Check if the Player is Focusing (Shift Key)
    void CheckFocusInput()
    {
        // Focus Input
        if (Input.GetAxis("Focus") != 0)
            _isFocused = true;
        else
            _isFocused = false;
    }
}
