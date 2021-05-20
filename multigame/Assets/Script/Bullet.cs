using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime);
    }
    public void shoot(Transform Rot)
    {
        transform.rotation = Rot.rotation;
    }
}
