using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderEnemyAI : MonoBehaviour
{

	public float fireRate;         // Rate of fire
	public float health;
    public GameObject projectile;   // Projectile to be fired

    private float _delay;           // Delay until the next shot

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		if (health <= 0)
			Destroy(gameObject);
        if (_delay <= 0)
        {
            _delay += fireRate;
            // Create Projectile
			Vector3 projectilePosition = new Vector3(transform.position.x, transform.position.y, -1f);

            Instantiate(projectile, projectilePosition, Quaternion.identity);
        }
        else
            _delay -= Time.deltaTime;
    }

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Projectile")
		{
			Destroy(coll.gameObject);
			health -= 1;
		}
	}
}
