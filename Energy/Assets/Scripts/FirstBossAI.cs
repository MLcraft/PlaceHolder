using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstBossAI : MonoBehaviour
{
    enum AIStage { Stage1, Stage2, Stage3 };

    public float fireRate;         // Rate of fire
    public float health;
    public float speed;
	public Text victoryText;
	public AudioSource stage1Music;
	public AudioSource stage2Music;
	public AudioSource stage3Music;
    public GameObject projectile;   // Projectile to be fired

    private float _delay;           // Delay until the next shot
    private float _xVelocity;
    private float _stage2Health;
    private float _stage2Speed;
    private float _stage2FireRate;
    private float _stage3Health;
    private float _stage3FireRate;

    private AIStage _AIStage = AIStage.Stage1;
    private Rigidbody2D _rb;

    // Use this for initialization
    void Start()
    {
		victoryText.gameObject.SetActive(false);
        _rb = GetComponent<Rigidbody2D>();
        _xVelocity = speed;
        _stage2Health = 20;
        _stage2Speed = speed * 2;
        _stage2FireRate = fireRate / 2;
        _stage3Health = 10;
        _stage3FireRate = _stage2FireRate / 2;
    }

    // Update is called once per frame
    void Update()
    {
		if (health <= 0) {
			victoryText.gameObject.SetActive(true);
			DeleteAll ();
		}
        if (_delay <= 0)
        {
            _delay += fireRate;
            // Create Projectile
            Vector3 projectilePosition = new Vector3(transform.position.x, transform.position.y, -1f);

            Instantiate(projectile, projectilePosition, Quaternion.identity);
        }
        else
            _delay -= Time.deltaTime;

        AIPattern();
        _rb.velocity = new Vector2(_xVelocity * Time.deltaTime, 0);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Projectile")
        {
            Destroy(coll.gameObject);
            health -= 1;

            if (health < _stage3Health)
            {
                _AIStage = AIStage.Stage3;
                fireRate = _stage3FireRate;
            }
            else if (health < _stage2Health)
            {
                _AIStage = AIStage.Stage2;
                fireRate = _stage2FireRate;
            }
        }
    }

    void AIPattern()
    {
        if (_AIStage == AIStage.Stage1)
        {
			if (!stage1Music.isPlaying) {
				stage2Music.Stop();
				stage3Music.Stop();
				stage1Music.loop = true;
				stage1Music.Play();
			}
            if (Camera.main.WorldToViewportPoint(transform.position).x < 0 && _xVelocity < 0)
                _xVelocity = speed;
            if (Camera.main.WorldToViewportPoint(transform.position).x > 1 && _xVelocity > 0)
                _xVelocity = -speed;
        }
        else if (_AIStage == AIStage.Stage2)
        {
			if (!stage2Music.isPlaying) {
				stage1Music.Stop();
				stage3Music.Stop();
				stage2Music.loop = true;
				stage2Music.Play();
			}
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            float xPos = transform.position.x;
            float offset = 4.0f;
            float playerXPos = player.transform.position.x;
            float playerXPosLeft = Camera.main.WorldToViewportPoint(new Vector3(playerXPos - offset, 0, 0)).x < 0 ? Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x : playerXPos - offset;
            float playerXPosRight = Camera.main.WorldToViewportPoint(new Vector3(playerXPos + offset, 0, 0)).x > 1 ? Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x : playerXPos + offset;

            if (xPos < playerXPosLeft && _xVelocity < 0)
                _xVelocity = _stage2Speed;
            if (xPos > playerXPosRight && _xVelocity > 0)
                _xVelocity = -_stage2Speed;
        }
        else if (_AIStage == AIStage.Stage3)
		{
			if (!stage3Music.isPlaying) {
				stage1Music.Stop();
				stage2Music.Stop();
				stage3Music.loop = true;
				stage3Music.Play();
			}
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            float xPos = transform.position.x;
            float offset = 2.0f;
            float playerXPos = player.transform.position.x;
            float playerXPosLeft = Camera.main.WorldToViewportPoint(new Vector3(playerXPos - offset, 0, 0)).x < 0 ? Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x : playerXPos - offset;
            float playerXPosRight = Camera.main.WorldToViewportPoint(new Vector3(playerXPos + offset, 0, 0)).x > 1 ? Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x : playerXPos + offset;

            if (xPos < playerXPosLeft && _xVelocity < 0)
                _xVelocity = _stage2Speed;
            if (xPos > playerXPosRight && _xVelocity > 0)
                _xVelocity = -_stage2Speed;
        }
    }

	void DeleteAll(){
		foreach (GameObject o in Object.FindObjectsOfType<GameObject>()) {
			if (o.tag != "MainCamera")
				Destroy(o);
		}
	}
}
