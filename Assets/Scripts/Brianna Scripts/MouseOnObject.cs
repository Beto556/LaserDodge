using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOnObject : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpSpeed = 50f;
    
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
       
        if (Input.GetMouseButton(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.tag == "Player")
                {
                    Debug.Log("tapped player");
                    rb.MovePosition(transform.position + Vector3.up * jumpSpeed * Time.deltaTime);
                }
            }
        }
    }
}
