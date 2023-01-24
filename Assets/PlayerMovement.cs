using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int speed;

    public bool moving;

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
                var _c = new Vector3(c.transform.position.x, 0, c.transform.position.z);
                paths.Add(_c);
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

                transform.position = Vector3.MoveTowards(currentPos, new Vector3(targetPos.x, 0, targetPos.z), step);
            }

            return false;
        }


        yield return new WaitUntil(_arrived);
    }

    public void FindMovement(Vector3 vector3)
    {
        if (!moving)
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

            if (moving)
            {
                nextPos = targetPos;
                nextPath = FindPath(nextPos);
            }
            else if (_foundTile)
            {
                moving = true;
                StartCoroutine(MoveTo());
            }
        }
    }

    private void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, new Vector3(targetPos.x , 0, targetPos.z)) < 0.01f)
        {
            currentPos = targetPos;
            nextPath = FindPath(currentPos);
            moving = false;
            if (nextPos == null)
            {

            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            FindMovement(new Vector3 (currentPos.x + 12, currentPos.y, currentPos.z));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            FindMovement(new Vector3(currentPos.x - 12, currentPos.y, currentPos.z));
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            FindMovement(new Vector3(currentPos.x, currentPos.y, currentPos.z + 12));
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            FindMovement(new Vector3(currentPos.x, currentPos.y, currentPos.z - 12));
        }
    }
}
