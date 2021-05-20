using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float angle;
    Vector2 target, mouse;
    public GameObject SP;
    public GameObject bulletPre;
    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Dir();   
        Move();
        Attack();
    }

    void Move()
    {
        float Horizontal = Input.GetAxisRaw("Horizontal");
        float Vertical = Input.GetAxisRaw("Vertical");

        Vector3 mov = new Vector3(Horizontal, Vertical, 0);

        mov = mov.normalized * Time.deltaTime;

        transform.position += mov * 2.5f;
    }
    void Dir()
    {
        target = transform.position;
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mouse.y - target.y, mouse.x - target.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject go;
            go = Instantiate(bulletPre, SP.transform.position, Quaternion.identity);
            go.GetComponent<Bullet>().shoot(transform);
        }
    }
}
