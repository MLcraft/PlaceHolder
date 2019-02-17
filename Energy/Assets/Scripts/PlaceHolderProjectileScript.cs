using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolderProjectileScript : MonoBehaviour {

    public float velocityX, velocityY;
    private Rigidbody2D _rb;

	// Use this for initialization
	void Start () {
        _rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        _rb.velocity = new Vector2(velocityX, velocityY);
	}

    public void setVelocity(float x, float y)
    {
        velocityX = x;
        velocityY = y;
    }

	void OnBecameInvisible() {
		Destroy(gameObject);
	}
}
