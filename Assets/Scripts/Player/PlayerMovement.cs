using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    public float MoveSpeed;
    public Rigidbody2D rb;
    private Vector2 PlayerInput;
	public bool freezePlayer = false;
   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
		if (freezePlayer)
		{
			PlayerInput.x = 0;
			PlayerInput.y = 0;
			return;
		}
        PlayerInput.x = Input.GetAxisRaw("Horizontal");
        PlayerInput.y = Input.GetAxisRaw("Vertical");

        PlayerInput.Normalize(); // Sprawia ze poruszanie sie po skosie nie jest szybsze jak poruszanie sie w inne kierunki

    }

    void FixedUpdate()
    {
        rb.velocity = PlayerInput * MoveSpeed;
    }
}
