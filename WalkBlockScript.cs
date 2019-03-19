using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkBlockScript : MonoBehaviour {

    public Vector2 Dir = Vector2.right;

    private Rigidbody2D RB;
	// Use this for initialization
	void Start ()
    {
        RB = GetComponent<Rigidbody2D>();
        RB.velocity = Dir;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Box")
        {
            Dir = Dir * -1;
            RB.velocity = Vector2.zero;
            RB.velocity = Dir;
        }
    }
}
