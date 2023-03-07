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
    public float speed;
    public bool invincible = false;

    Vector3 currentPos;
    Vector3 targetPos;
    Vector3 nextPos;
    Vector3 prevPos;
    Vector3 delayedPos = Vector3.zero;

    List<Vector3> nextPath = new List<Vector3>();

    LayerMask layerMask;

    float x;
    float z;
    float driftX;
    float driftZ;

    public float borderX;
    public float borderZ;

    public enum MovementDirection
    {
        Stopped,
        Up,
        Down,
        Left,
        Right
    }

    public MovementDirection movementDirection;

    void Start()
    {
        currentPos = transform.position;
        prevPos = currentPos;

        layerMask = LayerMask.GetMask("Path");
        nextPath = FindPath(currentPos);
    }

    public List<Vector3> FindPath(Vector3 _startPos)
    {
        List<Vector3> paths = new List<Vector3>();

        Collider[] t = Physics.OverlapSphere(_startPos, 6, layerMask);

        foreach (Collider c in t)
        {
            if (c.transform.position != _startPos)
            {
                var _c = new Vector3(c.transform.position.x, 0, c.transform.position.z);
                paths.Add(_c);
            }
        }

        return paths;
    }

    public IEnumerator MoveTo()
    {
        if (x != 0)
        {
            if ( x < 1) // if the player is moving up
            {
                movementDirection = MovementDirection.Up;
            }
            else if (x > 1) // if the player is moving down
            {
                movementDirection = MovementDirection.Down;
            }
        }

        if (z != 0)
        {
            if (z < 1) // if the player is moving right
            {
                movementDirection = MovementDirection.Right;
            }
            else if (z > 1) // if the player is moving left
            {
                movementDirection = MovementDirection.Left;
            }
        }

        var step = speed * Time.deltaTime;

        bool _arrived()
        {
            if (Vector3.Distance(gameObject.transform.position, targetPos) < 0.001f)
            {
                return true;
            }
            else
            {
                step += speed * Time.deltaTime;

                transform.position = Vector3.MoveTowards(currentPos, new Vector3(targetPos.x, 0, targetPos.z), step);
            }

            return false;
        }


        yield return new WaitUntil(_arrived);

        if (nextPos == Vector3.zero)
        {
            prevPos = currentPos;
            currentPos = targetPos;
            movementDirection = MovementDirection.Stopped;
        }
        else
        {
            prevPos = currentPos;
            currentPos = targetPos;
            FindMovement(nextPos);
        }
    }

    public IEnumerator ReverseMove(Vector3 _targetPos, Vector3 _currentPos)
    {
        var step = speed * Time.deltaTime;

        bool _arrived()
        {
            if (Vector3.Distance(gameObject.transform.position, _targetPos) < 0.001f)
            {
                return true;
            }
            else
            {
                step += speed * Time.deltaTime;

                transform.position = Vector3.MoveTowards(_currentPos, new Vector3(_targetPos.x, 0, _targetPos.z), step);
            }

            return false;
        }

        yield return new WaitUntil(_arrived);

        if (nextPos == Vector3.zero)
        {

        }
        else
        {
            nextPos = new Vector3(currentPos.x - x,0,currentPos.z - z);

            FindMovement(nextPos);
        }
    }

    public void FindMovement(Vector3 vector3)
    {
        {
            targetPos = vector3;

            bool _foundTile = false;
            foreach (Vector3 v in nextPath)
            {
                if (Vector3.Distance(targetPos, v) < 2f)
                {
                    targetPos = v;
                    _foundTile = true;
                    break;
                }
            }

            if (_foundTile)
            {
                nextPath = FindPath(targetPos);

                StartCoroutine(MoveTo());

                x = targetPos.x - currentPos.x;
                z = targetPos.z - currentPos.z;

                if (delayedPos == Vector3.zero)
                {
                    nextPos = new Vector3(targetPos.x + x, 0, targetPos.z + z);
                }
                else
                {
                    nextPos = new Vector3(delayedPos.x + x, 0 , delayedPos.z+ z);
                    delayedPos = Vector3.zero;
                }
            }
            else
            {
                if (Mathf.Abs(nextPos.x) > borderX || Mathf.Abs(nextPos.z) > borderZ)
                {
                    nextPos = Vector3.zero;
                }
                else
                {
                    x = currentPos.x - prevPos.x - driftX;
                    z = currentPos.z - prevPos.z - driftZ;

                    driftX = 0;
                    driftZ = 0;
                    targetPos = new Vector3(currentPos.x + x, 0, currentPos.z + z);

                    delayedPos = nextPos;
                    FindMovement(targetPos);
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            print("x = " + x);
            if (nextPos == Vector3.zero)
            {
                FindMovement(new Vector3(currentPos.x + 12, 0, currentPos.z));
                movementDirection = MovementDirection.Up;
            }
            else if (movementDirection == MovementDirection.Down)
            {
                StopAllCoroutines();

                movementDirection = MovementDirection.Up;
                driftX = -12;
                driftZ = 0;
                StartCoroutine(ReverseMove(prevPos, gameObject.transform.position));
            }
            else
            {
                nextPos = new Vector3(targetPos.x + 12, 0, targetPos.z);
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            print("x = " + x);

            if (nextPos == Vector3.zero)
            {
                FindMovement(new Vector3(currentPos.x - 12, 0, currentPos.z));
                movementDirection = MovementDirection.Down;
            }
            else if (movementDirection == MovementDirection.Up)
            {
                StopAllCoroutines();

                movementDirection = MovementDirection.Down;
                driftX = 12;
                driftZ = 0;
                StartCoroutine(ReverseMove(prevPos, gameObject.transform.position));
            }
            else
            {
                nextPos = new Vector3(targetPos.x - 12,0, targetPos.z);
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            print("z = " + z);

            if (nextPos == Vector3.zero)
            {
                FindMovement(new Vector3(currentPos.x, 0, currentPos.z + 12));
                movementDirection = MovementDirection.Left;
            }
            else if (movementDirection == MovementDirection.Right)
            {
                StopAllCoroutines();

                movementDirection = MovementDirection.Left;
                driftX = 0;
                driftZ = -12;
                StartCoroutine(ReverseMove(prevPos, gameObject.transform.position));
            }
            else
            {
                nextPos = new Vector3(targetPos.x, 0, targetPos.z + 12);
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            print("z = " + z);

            if (nextPos == Vector3.zero)
            {
                FindMovement(new Vector3(currentPos.x, 0, currentPos.z - 12));
                movementDirection = MovementDirection.Right;
            }
            else if (movementDirection == MovementDirection.Left)
            {
                StopAllCoroutines();

                movementDirection = MovementDirection.Right;
                driftX = 0;
                driftZ = 12;
                StartCoroutine(ReverseMove(prevPos, gameObject.transform.position));
            }
            else
            {
                nextPos = new Vector3(targetPos.x, 0, targetPos.z - 12);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            nextPos = Vector3.zero;
        }
    }
}
