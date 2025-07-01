using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    protected GridBasedMovement target;

    protected virtual void Start()
    {
        GameManager.Instance.RegisterEnemy(this);
    }

    public void SetTarget(GridBasedMovement target)
    {
        this.target = target;
    }
}