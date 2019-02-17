using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrazingColliderController : MonoBehaviour {

	public PlayerController player;

	private Rigidbody2D _rb;
	// Use this for initialization
	void Start () {
		_rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Projectile")
		{
			player.gainFuel();
		}
	}

	public void setVelocity(Vector2 velocity) {
		_rb.velocity = velocity;
	}
}
