using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGameWin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.Instance.SetGameWin();
    }
}
