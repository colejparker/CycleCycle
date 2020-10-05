using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CycleController : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;

    public string nameString;

    [SerializeField] CycleController opposingPlayer;

    [SerializeField] Transform frontTransform;
    [SerializeField] Transform rearTransform;
    [SerializeField] WheelCollider frontLeftCollider;
    [SerializeField] WheelCollider frontRightCollider;
    [SerializeField] WheelCollider rearLeftCollider;
    [SerializeField] WheelCollider rearRightCollider;

    [SerializeField] AudioClip deathFX;

    bool isAlive = true;

    Rigidbody rb;
    BoxCollider bc;

    [SerializeField] float centerOfMass = -5;

    public Material playerMaterial;

    Queue<GameObject> walls = new Queue<GameObject>();

    [SerializeField] int numberOfWalls = 100;

    [SerializeField] GameObject WallPrefab;

    [SerializeField] Transform motorcycleObject;
    [SerializeField] ParticleSystem deathParticles;

    [SerializeField] ScoreCanvas scoreCanvas;

    [SerializeField] List<Image> wins = new List<Image>();
    int numOfWins = 0;

    public KeyCode up;
    public KeyCode down;
    public KeyCode right;
    public KeyCode left;
    public KeyCode reset;

    [SerializeField] float breakForce;
    [SerializeField] float motorForce;
    [SerializeField] float maxSteeringAngle;

    [SerializeField] Transform spawnPoint;

    private void Start()
    {
        bc = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, centerOfMass, 0);
        frontTransform.GetComponent<MeshRenderer>().material = playerMaterial;
        rearTransform.GetComponent<MeshRenderer>().material = playerMaterial;
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            SpawnWall();
            GetInput();
            HandleMotor();
            HandleSteering();
        }
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
        //float steeringAngle = maxSteeringAngle * horizontalInput;
        //frontCollider.steerAngle = steeringAngle;

        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, horizontalInput * maxSteeringAngle, 0) * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    private void HandleMotor()
    {
        //frontCollider.motorTorque = verticalInput * motorForce;
        rb.AddForce(transform.forward * motorForce*verticalInput);
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

    private void Respawn()
    {
        if (numOfWins< wins.Count)
        {
            bc.enabled = true;
            rb.isKinematic = false;
            motorcycleObject.gameObject.SetActive(true);
            isAlive = true;
            transform.position = spawnPoint.transform.position;
            transform.rotation = spawnPoint.transform.rotation;
        }
    }

    public void Reset()
    {
        for (int i = 0; i < wins.Count; i++)
        {
            wins[i].color = Color.white;
        }
        numOfWins = 0;
        Respawn();

    }

    public void GainPoint()
    {
        numOfWins++;
        if (numOfWins < wins.Count)
        {
            wins[numOfWins-1].color = playerMaterial.color;
        }
        else
        {
            wins[numOfWins - 1].color = playerMaterial.color;
            scoreCanvas.PlayerLoses(opposingPlayer);
        }
    }

    public void hitWall()
    {
        if (isAlive)
        {
            GetComponent<AudioSource>().PlayOneShot(deathFX);
            Freeze();
            opposingPlayer.GainPoint();
            if (opposingPlayer.numOfWins < opposingPlayer.wins.Count)
            {
                Invoke("Respawn", 2f);
            }
        }
       
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Wall>())
        {
            hitWall();
        }
    }

    public void Freeze()
    {
        isAlive = false;
        motorcycleObject.gameObject.SetActive(false);
        for (int i = 0; i < walls.Count; i++)
        {
            GameObject wallToRemove = walls.Dequeue();
            Destroy(wallToRemove);
        }
        rb.velocity = Vector3.zero;
        horizontalInput = 0;
        verticalInput = 0;
        bc.enabled = false;
        rb.isKinematic = true;
    }



}
