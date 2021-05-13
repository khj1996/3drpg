﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Player : MonoBehaviour
{
    //컴포넌트
    private     Rigidbody       _rigidbody;         //리지드바디
    public      Animator        _animator;          //애니메이터
    public      GameObject      cameraBase;         //카메라
    public      GameObject      attackPoint;        //공격점
    public      LayerMask       attackLayer;        //공격레이어

    //조이스틱관련
    public      Transform       Stick;              //조이스틱   
    public      RectTransform   StickBase;          //조이스틱배경   
    private     Vector3         StickFirstPos;      //조이스틱의 처음 위치
    private     Vector3         JoyVec;             //조이스틱의 벡터
    private     float           Radius;             //조이스틱 배경의 반 지름
    private     bool            MoveFlag;           //플레이어 움직임 스위치

    //플레이어 상태관련
    private     bool            IsAttack = false;   //공격상태
    private     bool            isGround = false;   //점프상태
        
    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();

        StickFirstPos = Stick.transform.position;//스틱의 위치
        Radius = StickBase.rect.height / 2;

        MoveFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!StatusManager.Instance.end)//사망상태가 아닐시
        {
            if (IsAttack == false)//공격상태가 아닐시
            {
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A)
                    || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))//키보드입력
                {
                    Move();//이동
                }
                else if (MoveFlag)//조이스틱
                {
                    transform.Translate(Vector3.forward * Time.deltaTime * StatusManager.Instance.movSpeed);
                }
                else if (!MoveFlag)//가만히 있을경우
                {
                    _animator.SetBool("IsMove", false);
                }
            }
            Jump();
        }
        else
        {
            _animator.SetBool("IsDie", true);
        }
    }

    void Jump()//점프
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround == true)//점프
        {
            isGround = false;
            _rigidbody.velocity = transform.up * 4.5f;
        }
        else
        {
            isGround = Physics.Raycast(transform.position+new Vector3(0,0.1f,0)
                                        , Vector3.down, 0.2f);
        }
    }

    //플레이어 이동(키보드)
    void Move()
    {
        _animator.SetBool("IsMove", true);
        float Horizontal = Input.GetAxisRaw("Horizontal");
        float Vertical = Input.GetAxisRaw("Vertical");

        Vector3 mov = new Vector3(Horizontal, 0, Vertical);
        
        mov = mov.normalized * Time.deltaTime * StatusManager.Instance.movSpeed;

        //카메라회전시 이동방향 계산
        Quaternion v3Rotation = Quaternion.Euler(0f, cameraBase.transform.rotation.eulerAngles.y, 0f);
        Vector3 v3RotatedDirection = v3Rotation * mov;

        if (mov != Vector3.zero)
        {
            Rot_dir(v3RotatedDirection);//이동방향으로 회전
        }

        transform.Translate(Vector3.forward * Time.deltaTime * StatusManager.Instance.movSpeed);
    }
    //드래그(이동은 update에서 처리)
    public void Drag(BaseEventData _Data)
    {
        MoveFlag = true;
        _animator.SetBool("IsMove", true);
        PointerEventData Data = _Data as PointerEventData;
        Vector3 Pos = Data.position;

        JoyVec = (Pos - StickFirstPos).normalized;

        float Dis = Vector3.Distance(Pos, StickFirstPos);

        if (Dis < Radius)
        {
            Stick.position = StickFirstPos + JoyVec * Dis;
        }
        else
        {
            Stick.position = StickFirstPos + JoyVec * Radius;
        }

        //카메라회전시 이동방향 계산
        JoyVec = new Vector3(JoyVec.x, 0, JoyVec.y);// y축을 기준으로 회전시키기 위해서 yz축을 변환
        Quaternion v3Rotation = Quaternion.Euler(0f, cameraBase.transform.rotation.eulerAngles.y, 0f);
        Vector3 v3RotatedDirection = v3Rotation * JoyVec;

        if (JoyVec != Vector3.zero)
        {
            Rot_dir(v3RotatedDirection);//이동방향으로 회전
        }
    }
    // 드래그 끝.
    public void DragEnd()
    {
        Stick.position = StickFirstPos; // 스틱을 원래의 위치로.
        JoyVec = Vector3.zero;          // 방향을 0으로.
        MoveFlag = false;
        _animator.SetBool("IsMove", false);
    }
    //플레이어 회전
    private void Rot_dir(Vector3 targetPos)//플레이어 회전
    {
        Quaternion targetRot = Quaternion.LookRotation(targetPos);
        Quaternion frameRot = Quaternion.RotateTowards(transform.rotation,
                                                       targetRot, 540f * Time.deltaTime);

        transform.rotation = frameRot;
    }

    public void Attack()
    {
        if (!StatusManager.Instance.end)
        {
            if (IsAttack == false)
            {
                StartCoroutine(Attack(StatusManager.Instance.AttackPower));
            }
            
        }
    }

    IEnumerator Attack(float AttackPower)//공격코루틴
    {
        IsAttack = true;
        _animator.SetBool("IsAttack", true);

        yield return new WaitForSeconds(0.4f);
        attackPoint.SetActive(true);

        yield return new WaitForSeconds(0.3f);
        attackPoint.SetActive(false);
        IsAttack = false;
        _animator.SetBool("IsAttack", false);
    } 
}

