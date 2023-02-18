using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTapJump : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpSpeed = 50f;
    public Renderer somethingSilly;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        somethingSilly = GetComponent<Renderer>();
    }

    void Update()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate() //fixed update to apply constant force
    {
        if (Input.GetMouseButton(0))
        {
            rb.MovePosition(transform.position + Vector3.up * jumpSpeed * Time.deltaTime);
            somethingSilly.material.color = Color.red;
        }
        else
        {
            somethingSilly.material.color = Color.white;
        }
    }

}
