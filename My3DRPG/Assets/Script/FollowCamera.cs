using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FollowCamera : MonoBehaviour
{
    public      GameObject      _player;
    public      GameObject      cameraBase;

    private     Vector3         startMPos;    
    private     Vector3         curMPos;
    public      Quaternion      rot;
    public      float           amount;

    private     bool            isStop = false;
    // Start is called before the first frame update
    void Start()
    {
        isStop = false;
        startMPos = Vector3.zero;
        curMPos = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStop)
        {
            return;
        }
        cameraBase.transform.position = _player.transform.position;

        if (Input.GetMouseButtonDown(1))//우클릭시
        {
            startMPos.x = Input.mousePosition.x;
            rot = cameraBase.transform.rotation;
        }

        if (Input.GetMouseButton(1))//우클릭드래그
        {
            curMPos.x = Input.mousePosition.x;

            amount = curMPos.x - startMPos.x;

            cameraBase.transform.rotation = Quaternion.Euler(-20.0f, rot.eulerAngles.y + amount * 0.1f, 0);//좌우 드래그로 방향회전
        }
    }
    public void StopCamera()
    {
        isStop = true;
    }

    public void StartCamera()
    {
        isStop = false;
    }
}
