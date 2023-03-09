using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public GameObject startLocation;

    public LayerMask layerMask;

    public int score;

    public abstract void StartMovement();

    public abstract void KillThisEnemy();
}
