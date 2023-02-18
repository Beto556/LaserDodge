using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTapJump : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpSpeed; 
    
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Input.touchCount > 0) // are there any touches
        {
           Touch touch = Input.GetTouch(0);  //create a touch variable
            //touch.position  // current position of touch in coordinates
            //touch.phase // information about what phase it's in began, end, move, stationary, canceled
            
            rb.MovePosition(transform.position + Vector3.up * jumpSpeed * Time.deltaTime);

        }
    }
}
