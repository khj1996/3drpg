using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PlayerCtrl : MonoBehaviour
{
    //컴포넌트
    private Rigidbody _rigidbody;         //리지드바디
    public Animator _animator;          //애니메이터
    public GameObject cameraBase;         //카메라
    public GameObject attackPoint;        //공격점
    public LayerMask attackLayer;        //공격레이어
    public PlayerManager playerManager;

    //조이스틱관련
    public Transform Stick;              //조이스틱   
    public RectTransform StickBase;          //조이스틱배경   
    private Vector3 StickFirstPos;      //조이스틱의 처음 위치
    private Vector3 JoyVec;             //조이스틱의 벡터
    private float Radius;             //조이스틱 배경의 반 지름
    private bool MoveFlag;           //플레이어 움직임 스위치

    //플레이어 상태관련
    private bool IsAttack = false;   //공격상태
    private bool isGround = false;   //점프상태

    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();

        StickFirstPos = Stick.transform.position;//스틱의 위치
        Radius = StickBase.rect.height / 2;

        MoveFlag = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (playerManager.isDie == true)
        {
            _animator.SetBool("IsDie", true);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }

        Move();//이동
        Jump();
    }

    void Jump()//점프
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGround == true)//점프
        {
            isGround = false;
            _rigidbody.velocity = transform.up * 4.5f;
        }
        else
        {
            isGround = Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0)
                                        , Vector3.down, 0.2f);
        }
    }
    void Move()
    {
        Vector3 cameraLocalForward = cameraBase.transform.forward;
        cameraLocalForward.y = 0;

        float Horizontal = Input.GetAxisRaw("Horizontal");
        float Vertical = Input.GetAxisRaw("Vertical");

        bool isInputKey = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (!(Horizontal == 0 && Vertical == 0) && isInputKey)
        {
            if (IsAttack)
            {
                _animator.SetBool("IsMove", true);

                return;
            }

            float deg = Mathf.Atan2(Vertical, Horizontal) - Mathf.PI / 2;
            float x = Mathf.Cos(deg) * cameraLocalForward.x - Mathf.Sin(deg) * cameraLocalForward.z;
            float z = Mathf.Cos(deg) * cameraLocalForward.z + Mathf.Sin(deg) * cameraLocalForward.x;

            Vector3 dir = new Vector3(x, 0, z);

            dir = dir.normalized;
            transform.rotation = Quaternion.Euler(0, Mathf.Atan2(x, z) * Mathf.Rad2Deg, 0);
            transform.position += (dir * playerManager.movSpeed * Time.deltaTime);

            _animator.SetBool("IsMove", true);
        }
        else
        {
            _animator.SetBool("IsMove", false);
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


    public void Attack()
    {
        if (IsAttack == false)
        {
            StartCoroutine(Attack(playerManager.AttackPower));
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

