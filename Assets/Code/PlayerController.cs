using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("General Movement")]
    public Camera movementCam;
    public float speed = 15;
    public float jumpDetectDistance = 2;
    public LayerMask groundLayers;
    
    public enum jump // your custom enumeration
    {
        staticJump, 
        chargeJump,
        variableJump
    };
    
    [Header("Global Jump Option")] 
    public jump jumpType = jump.staticJump;

    [Header("Static Jump Options")] 
    public float jumpStrength = 7;
    
    [Header("Charge Jump Options")]
    public float jumpStrengthMin = 2;
    public float jumpTimeStrengthMultiplier = 2;
    
    [Header("Variable Jump Options")] 
    public float vJumpStrengthMin = 0.2f;
    public float vJumpTimeMax = 0.2f;
        
    private Rigidbody rb;
    private SphereCollider col;
    private PhysicsScene pScene;
    private float jumpStartTime, totalJumpStrength, jumpTimeCounter;
    private bool isJumping = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
        pScene = HamsterBallSimulator.getPhysicsScene();
    }

    void Update()
    {
        if(Input.GetKey("w"))
        {
            Vector3 force = movementCam.transform.forward * speed * Time.deltaTime;
            force = new Vector3(force.x, 0.0f, force.z);
            rb.AddForce(force, ForceMode.VelocityChange);
        }

        if(Input.GetKey("s"))
        {
            Vector3 force = movementCam.transform.forward * -speed * Time.deltaTime;
            force = new Vector3(force.x, 0.0f, force.z);
            rb.AddForce(force, ForceMode.VelocityChange);
        }
        
        if(Input.GetKey("a"))
        {
            rb.AddForce(movementCam.transform.right * -speed * Time.deltaTime, ForceMode.VelocityChange);
        }
        
        if(Input.GetKey("d"))
        {
            rb.AddForce(movementCam.transform.right * speed * Time.deltaTime, ForceMode.VelocityChange);
        }

        if (jumpType == jump.chargeJump)
        {
            if (Input.GetKeyDown (KeyCode.Space))
            {
                jumpStartTime = Time.time;
            }
            
            if ((Input.GetKeyUp(KeyCode.Space) && IsGrounded()))
            {
                totalJumpStrength = jumpStrengthMin + ((Time.time - jumpStartTime) * jumpTimeStrengthMultiplier);
                //Debug.Log("Jump Strength: " + totalJumpStrength);
                rb.AddForce(Vector3.up * totalJumpStrength, ForceMode.Impulse);
                totalJumpStrength = 0;
            }
            
        } 
        else if (jumpType == jump.staticJump)
        {
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            }
        }
        else if (jumpType == jump.variableJump)
        {
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                isJumping = true;
                jumpTimeCounter = vJumpTimeMax;
                rb.AddForce(Vector3.up * vJumpStrengthMin, ForceMode.Impulse);
            }

            if (Input.GetKey(KeyCode.Space) && isJumping)
            {
                if (jumpTimeCounter > 0)
                {
                    rb.AddForce(Vector3.up * vJumpStrengthMin, ForceMode.Impulse);
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }
    private bool IsGrounded()
    {
        return pScene.Raycast(col.bounds.center, new Vector3(0, -1, 0), jumpDetectDistance, groundLayers);
    }
}