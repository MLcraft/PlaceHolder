using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    enum States { Idle, Moving, Shooting };         // States of the player

    public float speed;                             // Movement Speed of the player
    public float focusSpeed;
    public int health;
    public int maxHealth;
    public float fuel;
	public float invincibilityFrame;
    public Text fuelText;
    public float fireRate;         // Rate of fire
    public float energyRate;
    public GameObject projectile;
    public GameObject healthicon;
    public PlayerHitboxController hitbox;
    public GrazingColliderController grazing;
	public InvincibleController invincible;
    public Image fuelBar;
	public bool isInvincible;

    private States _state = States.Idle;            // Current State of the Player
    private float _xVelocity;                       // X Velocity of the player
    private float _yVelocity;                       // Y Velocity of the player
    private bool _isFocused;                        // Focused Mechanic
    private float _delay;           // Delay until the next shot
    private float _energyDelay;
    private Rigidbody2D _rb;
    private GameObject[] hearts;
	private float _lastDamage;

    // Use this for initialization
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        hearts = new GameObject[maxHealth];
        for (int i = 0; i < maxHealth; i++)
        {
            float iconX = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 1)).x - i * healthicon.GetComponent<Renderer>().bounds.size.x - healthicon.GetComponent<Renderer>().bounds.size.x / 2 - 0.025f * i;
            float iconY = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 1)).y - 0.1f;
            Vector3 healthIconPosition = new Vector3(iconX, iconY, -2);
            hearts[i] = Instantiate(healthicon, healthIconPosition, Quaternion.identity);
        }

		fuelBar.type = Image.Type.Filled;
		_lastDamage = 0;
		isInvincible = false;
		invincible.gameObject.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
		if (Time.time - _lastDamage <= invincibilityFrame) {
			isInvincible = true;
		} else {
			isInvincible = false;
		}

        CheckPlayerInput();

        UpdateHealth();

        UpdateFuel();

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
            if (Input.GetAxis("Shoot") != 0)
            {
                Vector2 projectilePosition = transform.position;

                Instantiate(projectile, projectilePosition, Quaternion.identity);
            }
        }
        else
            _delay -= Time.deltaTime;

        if (_energyDelay <= 0)
        {
            _energyDelay += energyRate;
            // Create Projectile
        }
        else
            _energyDelay -= Time.deltaTime;

        if (Camera.main.WorldToViewportPoint(transform.position).x < 0 && _xVelocity < 0)
            _xVelocity = 0;
        if (Camera.main.WorldToViewportPoint(transform.position).x > 1 && _xVelocity > 0)
            _xVelocity = 0;
        if (Camera.main.WorldToViewportPoint(transform.position).y < 0 && _yVelocity < 0)
            _yVelocity = 0;
        if (Camera.main.WorldToViewportPoint(transform.position).y > 1 && _yVelocity > 0)
            _yVelocity = 0;
        _rb.velocity = new Vector2(_xVelocity, _yVelocity);
        hitbox.setVelocity(new Vector2(_xVelocity, _yVelocity));
        grazing.setVelocity(new Vector2(_xVelocity, _yVelocity));
		invincible.setVelocity(new Vector2 (_xVelocity, _yVelocity));
    }

    void CheckPlayerInput()
    {

        if (_state == States.Idle)
        {
            _xVelocity = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            _yVelocity = Input.GetAxis("Vertical") * speed * Time.deltaTime;

            if (_xVelocity != 0 || _yVelocity != 0)
                _state = States.Moving;

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
			if (!isInvincible){
				_lastDamage = Time.time;
				Destroy (coll.gameObject);
				health -= 1;
			}
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

    void UpdateHealth()
    {
        // Update health bar visuals
        if (health <= 0)
        {
            hearts[0].GetComponent<Renderer>().enabled = false;
        }
        else
        {
            for (int i = 0; i < health; i++)
            {
                hearts[i].GetComponent<Renderer>().enabled = true;
            }
            for (int i = health; i < maxHealth; i++)
            {
                hearts[i].GetComponent<Renderer>().enabled = false;
            }
        }

    }

    void UpdateFuel()
    {

        if (_state == States.Moving)
            fuel -= 0.1f;

        fuelText.text = fuel.ToString();
        fuelBar.fillAmount = fuel / 100.0f;

    }

    public void gainFuel()
    {
        fuel += 1;
    }

    void DeleteAll()
    {
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            if (o.tag != "MainCamera")
                Destroy(o);
        }
    }
}
