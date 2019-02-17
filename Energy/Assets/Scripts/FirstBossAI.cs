using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstBossAI : MonoBehaviour
{
    enum AIStage { Stage1, Stage2, Stage3 };
    enum AIPos { TL, TR, BL, BR, TIRED };

    public float fireRate;         // Rate of fire
    public float health;
    public float maxHealth;
    public float speed;
    public Text victoryText;
    public AudioSource stage1Music;
    public AudioSource stage2Music;
    public AudioSource stage3Music;
    public GameObject projectile;   // Projectile to be fired
    public GameObject projectile2;
    public GameObject projectile3;
    public Text bossText;
    public Image bossBar;


    private float _delay;           // Delay until the next shot
    private Vector3 _originalPosition;
    private float _counter;
    public float stage1XOffset;
    public float stage1YOffset;
    public float stage1Speed;
    public float stage1Width;
    public float stage1Height;
    public float stage1FireRate;
    public float stage2Health;
    public float stage2Speed;
    public float stage2FireRate;
    public float projectile2Speed;
    public float stage3Health;
    public float stage3FireRate;
    public float stage3CoolDown;
    public float projectile3Speed;
    public float stage3BuildUp;

    private short _stage2ProjectileCount;
    private short _stage2ProjectileCountMax;
    

    private AIPos _AIPos;
    private Vector3 _stage3TL, _stage3TR, _stage3BL, _stage3BR, _stage3Target;

    private AIStage _AIStage = AIStage.Stage1;

    // Use this for initialization
    void Start()
    {        
        _delay = 0;
        _originalPosition = transform.position;
        _stage2ProjectileCount = 0;
        _stage2ProjectileCountMax = 11;
        _stage3TL = Camera.main.ViewportToWorldPoint(new Vector3(0, 1f, 1f));
        _stage3TR = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 1f));
        _stage3BL = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 1f));
        _stage3BR = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0, 1f));
    }

    // Update is called once per frame
    void Update()
    {
     
        if (_delay <= 0)
        {
            // Create Projectile

            AIAttackPattern();
        }
        else
            _delay -= Time.deltaTime;

        AIPattern();
        updateHP();
        //_rb.velocity = new Vector2(_xVelocity * Time.deltaTime, 0);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Projectile")
        {
            Destroy(coll.gameObject);
            health -= 1;

            if (health < stage3Health && _AIStage == AIStage.Stage2)
            {
                _AIStage = AIStage.Stage3;
                fireRate = stage3FireRate;
                _AIPos = AIPos.TL;
                _stage3Target = _stage3TL;
            }
            else if (health < stage2Health && _AIStage == AIStage.Stage1)
            {
                _AIStage = AIStage.Stage2;
                _delay = 1.0f;
            }
        }
    }

    void AIPattern()
    {
        if (_AIStage == AIStage.Stage1)
        {
            if (!stage1Music.isPlaying)
            {
                stage2Music.Stop();
                stage3Music.Stop();
                stage1Music.loop = true;
                stage1Music.Play();
            }

            _counter += Time.deltaTime * stage1Speed;
            float x = Mathf.Cos(_counter) * stage1Width;
            float y = Mathf.Sin(_counter) * stage1Height;

            transform.position = new Vector3(x + stage1XOffset, y + stage1YOffset, 0);

        }
        else if (_AIStage == AIStage.Stage2)
        {
            if (!stage2Music.isPlaying)
            {
                stage1Music.Stop();
                stage3Music.Stop();
                stage2Music.loop = true;
                stage2Music.Play();
            }

            if (transform.position != _originalPosition)
                transform.position = Vector3.MoveTowards(transform.position, _originalPosition, speed * Time.deltaTime);
        }
        else if (_AIStage == AIStage.Stage3)
        {
            if (!stage3Music.isPlaying)
            {
                stage1Music.Stop();
                stage2Music.Stop();
                stage3Music.loop = true;
                stage3Music.Play();
            }

            if (transform.position != _stage3Target)
            {
                transform.position = Vector3.MoveTowards(transform.position, _stage3Target, speed * Time.deltaTime);
            }
            else
            {
                switch (_AIPos)
                {
                    case AIPos.TL:
                        _AIPos = AIPos.BL;
                        _stage3Target = _stage3BL;

                        for (int i = 0; i < 6; i++)
                        {
                            float angle = i * -15.0f;
                            float xSpeed = projectile3Speed * Mathf.Cos(Mathf.Deg2Rad * angle);
                            float ySpeed = projectile3Speed * Mathf.Sin(Mathf.Deg2Rad * angle);

                            Vector3 projectilePosition = new Vector3(transform.position.x, transform.position.y, -1f);

                            Instantiate(projectile3, projectilePosition, Quaternion.identity).GetComponent<FirstBossProjectile3>().setup(xSpeed, ySpeed, angle, projectile3Speed, false);
                        }
                        break;
                    case AIPos.BL:
                        _AIPos = AIPos.BR;
                        _stage3Target = _stage3BR;

                        for (int i = 0; i < 6; i++)
                        {
                            float angle = i * 15.0f;
                            float xSpeed = projectile3Speed * Mathf.Cos(Mathf.Deg2Rad * angle);
                            float ySpeed = projectile3Speed * Mathf.Sin(Mathf.Deg2Rad * angle);

                            Vector3 projectilePosition = new Vector3(transform.position.x, transform.position.y, -1f);

                            Instantiate(projectile3, projectilePosition, Quaternion.identity).GetComponent<FirstBossProjectile3>().setup(xSpeed, ySpeed, angle, projectile3Speed, false);
                        }

                        break;
                    case AIPos.BR:
                        _AIPos = AIPos.TR;
                        _stage3Target = _stage3TR;

                        for (int i = 0; i < 6; i++)
                        {
                            float angle = i * 15.0f + 90.0f;
                            float xSpeed = projectile3Speed * Mathf.Cos(Mathf.Deg2Rad * angle);
                            float ySpeed = projectile3Speed * Mathf.Sin(Mathf.Deg2Rad * angle);

                            Vector3 projectilePosition = new Vector3(transform.position.x, transform.position.y, -1f);

                            Instantiate(projectile3, projectilePosition, Quaternion.identity).GetComponent<FirstBossProjectile3>().setup(xSpeed, ySpeed, angle, projectile3Speed, false);
                        }

                        break;
                    case AIPos.TR:
                        _AIPos = AIPos.TL;
                        _stage3Target = _stage3TL;

                        for (int i = 0; i < 6; i++)
                        {
                            float angle = i * -15.0f + 270.0f;
                            float xSpeed = projectile3Speed * Mathf.Cos(Mathf.Deg2Rad * angle);
                            float ySpeed = projectile3Speed * Mathf.Sin(Mathf.Deg2Rad * angle);

                            Vector3 projectilePosition = new Vector3(transform.position.x, transform.position.y, -1f);

                            Instantiate(projectile3, projectilePosition, Quaternion.identity).GetComponent<FirstBossProjectile3>().setup(xSpeed, ySpeed, angle, projectile3Speed, false);
                        }

                        break;
                    case AIPos.TIRED:
                        if (stage3BuildUp < 0)
                        {
                            _AIPos = AIPos.TL;
                            _stage3Target = _stage3TL;
                        }
                        else
                            stage3BuildUp -= Time.deltaTime;

                        break;
                }
            }
        }
    }

    void AIAttackPattern()
    {
        if (_AIStage == AIStage.Stage1)
        {
            _delay += stage1FireRate;
            Vector3 projectilePosition = new Vector3(transform.position.x, transform.position.y, -1f);

            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    if (!(x == 0 && y == 0))
                        Instantiate(projectile, projectilePosition, Quaternion.identity).GetComponent<PlaceHolderProjectileScript>().setVelocity(x, y);
        }
        else if (_AIStage == AIStage.Stage2)
        {
            _delay += stage2FireRate;
            Vector3 projectilePosition = new Vector3(transform.position.x, transform.position.y, -1f);
            float angle = 5.0f * _stage2ProjectileCount;

            if (_stage2ProjectileCount % 2 == 0)
            {
                for (int n = 0; n < 8; n++)
                {
                    float altAngle = n * 45.0f;
                    float nSpeed = projectile2Speed * Mathf.Cos(Mathf.Deg2Rad * altAngle);
                    float mSpeed = projectile2Speed * Mathf.Sin(Mathf.Deg2Rad * altAngle);

                    Instantiate(projectile, projectilePosition, Quaternion.identity).GetComponent<PlaceHolderProjectileScript>().setVelocity(nSpeed, mSpeed);
                }
            }
            else
            {
                for (int n = 0; n < 8; n++)
                {
                    float altAngle = 22.5f + n * 45.0f;
                    float nSpeed = projectile2Speed * Mathf.Cos(Mathf.Deg2Rad * altAngle);
                    float mSpeed = projectile2Speed * Mathf.Sin(Mathf.Deg2Rad * altAngle);

                    Instantiate(projectile, projectilePosition, Quaternion.identity).GetComponent<PlaceHolderProjectileScript>().setVelocity(nSpeed, mSpeed);
                }
            }

            for (int x = -3; x <= 3; x++)
                if (!(x == 0))
                {
                    float projAngle = x < 0 ? angle : -angle;
                    float xSpeed = (x * projectile2Speed) * Mathf.Cos(Mathf.Deg2Rad * angle);
                    float ySpeed = x > 0 ? -(x * projectile2Speed) * Mathf.Sin(Mathf.Deg2Rad * angle) : (x * projectile2Speed) * Mathf.Sin(Mathf.Deg2Rad * angle);

                    if (x < 0)
                        Instantiate(projectile2, projectilePosition, Quaternion.identity).GetComponent<FirstBossProjectile2>().setup(xSpeed, ySpeed, projAngle, projectile2Speed, false);
                    else
                        Instantiate(projectile2, projectilePosition, Quaternion.identity).GetComponent<FirstBossProjectile2>().setup(xSpeed, ySpeed, projAngle, projectile2Speed, true);

                }

            _stage2ProjectileCount++;
            if (_stage2ProjectileCount >= _stage2ProjectileCountMax)
            {
                _stage2ProjectileCount = 0;
            }
        }
        else if (_AIStage == AIStage.Stage3)
        {
            if (_AIPos != AIPos.TIRED)
            {
                _delay += stage3FireRate;

                for (int i = 0; i < 4; i++)
                {
                    float angle = i * 90.0f;
                    float xSpeed = projectile3Speed * Mathf.Cos(Mathf.Deg2Rad * angle);
                    float ySpeed = projectile3Speed * Mathf.Sin(Mathf.Deg2Rad * angle);

                    Vector3 projectilePosition = new Vector3(transform.position.x, transform.position.y, -1f);

                    Instantiate(projectile, projectilePosition, Quaternion.identity).GetComponent<PlaceHolderProjectileScript>().setVelocity(xSpeed, ySpeed);
                }

                if (stage3BuildUp >= stage3CoolDown)
                {
                    _AIPos = AIPos.TIRED;
                    _stage3Target = _originalPosition;
                }
                else
                {
                    stage3BuildUp += Time.deltaTime;
                }
            }
        }
    }

    void updateHP()
    {
        bossText.text = health.ToString() + "/" + maxHealth.ToString();
        bossBar.fillAmount = health / maxHealth;
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
