using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelOneEnemy : Entity
{
    public GameObject bullet;

    public float speed;

    public bool moving = false;
    public bool onMap = false;

    public Vector3 currentPos;
    public Vector3 targetPos;
    public Vector3 nextPos;
    public Vector3 prevPos;
    public Vector3 delayedPos = Vector3.zero;

    public List<Vector3> nextPath = new List<Vector3>();

    float x;
    float z;

    public float borderX;
    public float borderZ;

    int num;

    private void Start()
    {
        score = 100;
    }

    public override void StartMovement()
    {
        currentPos = transform.position;
        prevPos = currentPos;

        nextPath = FindPath(currentPos);

        num = Random.Range(0, nextPath.Count);

        targetPos = nextPath[num];

        FindMovement(targetPos);
    }

    public ParticleSystem explosion;

    public override void KillThisEnemy()
    {
        GameObject.Find("Enemy Spawner").GetComponent<EnemySpawner>().SpawnEnemy(gameObject.GetComponent<Entity>().startLocation);
        ScoreCounter.totalScore += gameObject.GetComponent<Entity>().score;
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
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

        if (nextPos == Vector3.zero)
        {
            prevPos = currentPos;
            currentPos = targetPos;
        }
        else
        {
            x = currentPos.x - prevPos.x;
            z = currentPos.z - prevPos.z;

            int n = Random.Range(0, 4);

            if (x != 0 || z != 0)
            {
                if (n == 0)
                {
                    if (onMap)
                    {
                        var t = Instantiate(bullet, new Vector3(gameObject.transform.position.x + (x / 3), 0, gameObject.transform.position.z + (z / 3)), Quaternion.identity);

                        t.GetComponent<Rigidbody>().velocity = new Vector3(x * 3, 0, z * 3);
                    }
                }
            }

            prevPos = currentPos;
            currentPos = targetPos;
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

                num = Random.Range(0, nextPath.Count);
                nextPos = nextPath[num];
            }

            if (Enumerable.Range(1, 4).Contains(int.Parse(startLocation.name)))
            {
                if (Mathf.Abs(nextPos.x) < borderX)
                {
                    layerMask = LayerMask.GetMask("Path");
                    onMap = true;
                }
            }
            else if (Enumerable.Range(5, 8).Contains(int.Parse(startLocation.name)))
            {
                if (Mathf.Abs(nextPos.z) < borderZ)
                {
                    layerMask = LayerMask.GetMask("Path");
                    onMap = true;
                }
            }
        }
    }
}
