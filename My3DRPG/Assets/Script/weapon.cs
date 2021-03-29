using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "monster")
        {
            other.GetComponent<EnemyBase>().GetDam(3.0f);
        }
    }
}
