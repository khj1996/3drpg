using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FollowCamera : MonoBehaviour
{
    public      GameObject      _player;
    public      Vector3         startMPos;    
    public      Vector3         curMPos;
    public      Quaternion      rot;
    public      float           amount;

    // Start is called before the first frame update
    void Start()
    {
        startMPos = Vector3.zero;
        curMPos = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _player.transform.position;

        if (Input.GetMouseButtonDown(1))//우클릭시
        {
            startMPos.x = Input.mousePosition.x;
            rot = transform.rotation;
        }

        if (Input.GetMouseButton(1))//우클릭드래그
        {
            curMPos.x = Input.mousePosition.x;

            amount = curMPos.x - startMPos.x;

            transform.rotation = Quaternion.Euler(-20.0f, rot.eulerAngles.y + amount * 0.1f, 0);//좌우 드래그로 방향회전
        }
    }
}
