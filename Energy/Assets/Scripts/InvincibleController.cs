using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleController : MonoBehaviour {

	public PlayerController player;

	private Rigidbody2D _rb;
	// Use this for initialization
	void Start () {
		_rb = GetComponent<Rigidbody2D>();
		gameObject.GetComponent<Renderer>().enabled = false;
	}

	// Update is called once per frame
	void Update () {
		if (player.isInvincible) {
			gameObject.GetComponent<Renderer> ().enabled = true;
		} else {
			gameObject.GetComponent<Renderer> ().enabled = false;
		}
	}

	public void setVelocity(Vector2 velocity) {
		_rb.velocity = velocity;
	}
}
