using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int speed;

    bool moving;

    Vector3 currentPos;
    Vector3 targetPos;
    Vector3 nextPos;

    public List<Vector3> nextPath = new List<Vector3>();

    LayerMask layerMask;

    void Start()
    {
        currentPos = transform.position;

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
                paths.Add(c.gameObject.transform.position);
            }
        }

        return paths;
    }

    public IEnumerator MoveTo()
    {
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

                transform.position = Vector3.MoveTowards(currentPos, targetPos, step);
            }

            return false;
        }


        yield return new WaitUntil(_arrived);
    }

    private void Update()
    {
        if (gameObject.GetComponent<Rigidbody>().velocity == Vector3.zero)
        {
            moving = false;
        }
        else
        {
            moving = true;
        }

        if (Vector3.Distance(gameObject.transform.position, targetPos) < 0.01f)
        {
            currentPos = targetPos;
            nextPath = FindPath(currentPos);
            if (nextPos == null)
            {

            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            targetPos = new Vector3(currentPos.x + 12, currentPos.y, currentPos.z);

            foreach(Vector3 v in nextPath)
            {
                if (Vector3.Distance(targetPos, v) < 2f) 
                {
                    targetPos = v;
                }
            }

            if (moving)
            {
                nextPos = targetPos;
                nextPath = FindPath(nextPos);
            }
            else
            {
                StartCoroutine( MoveTo());
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            targetPos = new Vector3(currentPos.x - 12, currentPos.y, currentPos.z);

            foreach (Vector3 v in nextPath)
            {
                if (Vector3.Distance(targetPos, v) < 2f)
                {
                    targetPos = v;
                }
            }


            if (moving)
            {
                nextPos = targetPos;
                nextPath = FindPath(nextPos);
            }
            else
            {
                StartCoroutine( MoveTo());
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            targetPos = new Vector3(currentPos.x, currentPos.y, currentPos.z + 12);

            foreach (Vector3 v in nextPath)
            {
                if (Vector3.Distance(targetPos, v) < 2f)
                {
                    targetPos = v;
                }
            }

            if (moving)
            {
                nextPos = targetPos;
                nextPath = FindPath(nextPos);
            }
            else
            {
                StartCoroutine( MoveTo());
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            targetPos = new Vector3(currentPos.x, currentPos.y, currentPos.z - 12);

            foreach (Vector3 v in nextPath)
            {
                if (Vector3.Distance(targetPos, v) < 2f)
                {
                    targetPos = v;
                }
            }

            if (moving)
            {
                nextPos = targetPos;
                nextPath = FindPath(nextPos);
            }
            else
            {
                StartCoroutine( MoveTo());
            }
        }
    }
}
