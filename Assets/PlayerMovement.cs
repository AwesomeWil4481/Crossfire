using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody playerBody;
    public bool invincible = true;
    public LayerMask walls;

    public int speed;

    public GameObject currentTile;

    public enum Movement
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public Movement currentDirection;

    public Vector3 desiredMovement;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && !currentTile.name.Contains("W"))
        {
            Vector3 _move = new Vector3(speed, 0, 0);

            if (playerBody.velocity == Vector3.zero)
            {
                playerBody.velocity = _move;
            }

            else if (playerBody.velocity.x < -1)
            {
                desiredMovement = Vector3.zero;
                playerBody.velocity = _move;
            }

            else
            {
                desiredMovement = _move;
            }
        }

        if (Input.GetKeyDown(KeyCode.S) && !currentTile.name.Contains("S"))
        {
            Vector3 _move = new Vector3(-speed,0,0);

            if (playerBody.velocity == Vector3.zero)
            {
                playerBody.velocity = _move;
            }

            else if (playerBody.velocity.x > 1)
            {
                desiredMovement = Vector3.zero;
                playerBody.velocity = _move;
            }

            else
            {
                desiredMovement = _move; 
            }
        }

        if (Input.GetKeyDown(KeyCode.A) && !currentTile.name.Contains("A"))
        {
            Vector3 _move = new Vector3(0, 0, speed);

            if (playerBody.velocity == Vector3.zero)
            {
                playerBody.velocity = _move;
            }

            else if (playerBody.velocity.z < -1)
            {
                desiredMovement = Vector3.zero;
                playerBody.velocity = _move;
            }

            else
            {
                desiredMovement = _move;
            }
        }

        if (Input.GetKeyDown(KeyCode.D) && !currentTile.name.Contains("D"))
        {
            Vector3 _move = new Vector3(0, 0, -speed);

            if (playerBody.velocity == Vector3.zero)
            {
                playerBody.velocity = _move;
            }

            else if (playerBody.velocity.z > 1)
            {
                desiredMovement = Vector3.zero;
                playerBody.velocity = _move;
            }

            else
            {
                desiredMovement = _move;
            }
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.name != "Wall")
        {
            print(_other.name);

            currentTile = _other.gameObject;

            StartCoroutine(MoveCheck(_other.gameObject));
        }
    }

    public IEnumerator FinishMove()
    {
        var step = speed * Time.deltaTime;

        bool _arrived()
        {
            if (Vector3.Distance(gameObject.transform.position, currentTile.transform.position) < 0.001f)
            {
                return true;
            }

            else
            {
                step += speed * Time.deltaTime;

                transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(currentTile.transform.position.x, 0, currentTile.transform.position.z), step);
                return false;
            }
        }

        yield return new WaitUntil(_arrived);

        if (desiredMovement != null && desiredMovement != Vector3.zero)
        {
            Vector3 _nextMove = currentTile.transform.position;

            if (desiredMovement.x != 0)
            {
                // W
                if (desiredMovement.x > 1)
                {
                    _nextMove = new Vector3(currentTile.transform.position.x + 11, currentTile.transform.position.y, currentTile.transform.position.z);
                }

                // S
                else if (desiredMovement.x < -1)
                {
                    _nextMove = new Vector3(currentTile.transform.position.x - 11, currentTile.transform.position.y, currentTile.transform.position.z);
                }
            }
            else if (desiredMovement.z != 0)
            {
                // A
                if (desiredMovement.z > 1)
                {
                    _nextMove = new Vector3(currentTile.transform.position.x, currentTile.transform.position.y, currentTile.transform.position.z - 12);
                }

                // D
                else if (desiredMovement.z < -1) 
                {
                    _nextMove = new Vector3(currentTile.transform.position.x, currentTile.transform.position.y, currentTile.transform.position.z - 11);
                }
            }

            var t = Physics.OverlapSphere(_nextMove, 1, walls);
            if (t.Length == 0)
            {
                playerBody.velocity = desiredMovement;
                desiredMovement = Vector3.zero;
            }
            else
            {

            }           
        }
    }
    
    public IEnumerator MoveCheck(GameObject _collider)
    {
        bool _arrived()
        {
            if (Vector3.Distance(currentTile.transform.position, gameObject.transform.position) < 1f)
            {
                StartCoroutine(FinishMove());
                return true;
            }

            return false;
        }

        yield return new WaitUntil (_arrived);

        if (_collider.name.Contains("Edge"))
        {
            if (playerBody.velocity.x > 1 && currentTile.name.Contains("W"))
            {
                playerBody.velocity = Vector3.zero;
            }

            if (playerBody.velocity.x < -1 && currentTile.name.Contains("S"))
            {
                playerBody.velocity = Vector3.zero;
            }

            if (playerBody.velocity.z > 1 && currentTile.name.Contains("A"))
            {
                playerBody.velocity = Vector3.zero;
            }

            if (playerBody.velocity.z < -1 && currentTile.name.Contains("D"))
            {
                playerBody.velocity = Vector3.zero;
            }
        }
    }
}