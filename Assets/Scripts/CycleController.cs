using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleController : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;

    [SerializeField] Transform frontTransform;
    [SerializeField] Transform rearTransform;
    [SerializeField] WheelCollider frontCollider;
    [SerializeField] WheelCollider rearCollider;

    Rigidbody rb;

    [SerializeField] Material playerMaterial;

    Queue<GameObject> walls = new Queue<GameObject>();

    [SerializeField] int numberOfWalls = 100;

    [SerializeField] GameObject WallPrefab;

    public KeyCode up;
    public KeyCode down;
    public KeyCode right;
    public KeyCode left;

    [SerializeField] float breakForce;
    [SerializeField] float motorForce;
    [SerializeField] float maxSteeringAngle;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        frontTransform.GetComponent<MeshRenderer>().material = playerMaterial;
        rearTransform.GetComponent<MeshRenderer>().material = playerMaterial;
    }

    private void FixedUpdate()
    {
        //SpawnWall();
        GetInput();
        HandleMotor();
        HandleSteering();
    }

    private void SpawnWall()
    {
        if (transform.InverseTransformDirection(rb.velocity).z > 0)
        {
            GameObject go = Instantiate(WallPrefab, transform.localPosition - (transform.forward*(1+WallPrefab.transform.localScale.z)), transform.rotation);
            go.GetComponent<MeshRenderer>().material = playerMaterial;
            walls.Enqueue(go);
            if (walls.Count > numberOfWalls)
            {
                GameObject wallToRemove = walls.Dequeue();
                Destroy(wallToRemove);
            }
        }
    }

    private void HandleSteering()
    {
        float steeringAngle = maxSteeringAngle * horizontalInput;
        frontCollider.steerAngle = steeringAngle;
    }

    private void HandleMotor()
    {
        frontCollider.motorTorque = verticalInput * motorForce;
    }

    private void GetInput()
    {
        if (Input.GetKey(up))
        {
            verticalInput = Mathf.Clamp(verticalInput + Time.deltaTime, -1, 1);
        } else if (Input.GetKey(down))
        {
            verticalInput = Mathf.Clamp(verticalInput - Time.deltaTime, -1, 1);
        } else
        {
            if (verticalInput > 0)
            {
                verticalInput = Mathf.Clamp(verticalInput - Time.deltaTime, -1, 1);

            } else if (verticalInput < 0)
            {
                verticalInput = Mathf.Clamp(verticalInput + Time.deltaTime, -1, 1);
            }
        }

        if (Input.GetKey(right))
        {
            horizontalInput = Mathf.Clamp(horizontalInput + Time.deltaTime, -1, 1);
        }
        else if (Input.GetKey(left))
        {
            horizontalInput = Mathf.Clamp(horizontalInput - Time.deltaTime, -1, 1);
        } else
        {
            if (horizontalInput > 0)
            {
                horizontalInput = Mathf.Clamp(horizontalInput - Time.deltaTime, -1, 1);

            }
            else if (horizontalInput < 0)
            {
                horizontalInput = Mathf.Clamp(horizontalInput + Time.deltaTime, -1, 1);
            }
        }


    }
    public void hitWall()
    {
        print("Hit a wall");
    }
}
