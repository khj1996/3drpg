using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : EnemyBase
{
    public Enemy()
    {
        HP = 15.0f;
        power = 3.0f;
        attackCT = 2.0f;
        speed = 2.0f;
    }

    //컴포넌트
    private     GameObject      _player;
    public      Animator        _animator;
    private     NavMeshAgent    pathFinder      =   null;
    public      Image           hpBar;
    [SerializeField]
    private     GameObject      dropItem; // 드랍되는 아이템

    //위치,탐색 관련
    private     Vector3         targetPos;
    private     bool            findTarget      =   false;
    [SerializeField]
    private     float           searchRange;
    public      LayerMask       targetLayers;

    //능력치 관련
    private     float           curCT;      //현재쿨타임
    private     float           curHP;      //현재 체력

    

    private void Start()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");

        targetPos = transform.position;
        searchRange = 5.0f;
        pathFinder.isStopped = false;

        pathFinder.speed = speed;
        curHP = HP;

        ChangeState(eState.Idle);
    }

    private void Update()
    {
        switch (curState)
        {
            case eState.Invalid:
                break;
            case eState.Idle:
                OnUpdateIdleState();
                break;
            case eState.Trace:
                OnUpdateTraceState();
                break;
            case eState.Attack:
                OnUpdateAttackState();
                break;
            case eState.Dead:
                OnUpdateDeadState();
                break;
        }
    }

    public override void OnUpdateIdleState()
    {
        if (Vector3.Distance(targetPos, transform.position) < 0.1f && findTarget == false)
        {
            _animator.SetBool("IsMove", false);
        }
        else
        {
            _animator.SetBool("IsMove", true);
        }
    }
    public override void OnUpdateTraceState()
    {
        if (Vector3.Distance(targetPos, transform.position) < 2.0f && findTarget == true)
        {
            ChangeState(eState.Attack);
        }
        else if (Vector3.Distance(targetPos, transform.position) > 15f && findTarget == true)
        {
            findTarget = false;
            ChangeState(eState.Idle);
        }
    }
    public override void OnUpdateAttackState()
    {
        if (Vector3.Distance(targetPos, transform.position) > 2.0 && findTarget == true)
        {
            pathFinder.isStopped = false;
            ChangeState(eState.Trace);
        }
        else
            pathFinder.isStopped = true;

        if (curCT < 0)
        {
            curCT = attackCT;
            StartCoroutine(Attack());
        }
        else
        {
            curCT = curCT - Time.deltaTime;
        }
    }
    public override void OnUpdateDeadState()
    {
    }

    public override void OnEnterIdleState()
    {
        StartCoroutine(FindPos());
        StartCoroutine(UpdatePath());
    }

    public override void OnEnterTraceState()//추적상태시작
    {
        StartCoroutine(FindPlayer());
    }
    public override void OnEnterAttackState()
    {
        pathFinder.speed = 3.4f;
    }
    public override void OnEnterDeadState()
    {
        StopCoroutine("Attack");
        pathFinder.isStopped = true;
        _animator.SetBool("IsDie", true);        
        StartCoroutine(AfterDead());
    }

    public override void OnExitIdleState()
    {
    }
    public override void OnExitTraceState()
    {
    }
    public override void OnExitAttackState()
    {
        pathFinder.speed = 2.0f;
    }
    public override void GetDam(float amount)//데미지 계산
    {
        curHP -= amount;
        hpBar.fillAmount = curHP / HP;

        if (curHP <= 0)
        {
            ChangeState(eState.Dead);
        }
    }
    IEnumerator UpdatePath()//패스파인더 경로 수정
    {
        while (true)
        {
            pathFinder.SetDestination(targetPos);

            yield return new WaitForSeconds(0.3f);
            
            if (findTarget == false)//플레이어를 찾지 못할시 콜라이더를 이용하여 플레이어 탐색
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, searchRange, targetLayers);

                foreach (Collider col in colliders)
                {
                    if (col.tag == "Player")//범위내에서 플레이어를 찾을시 반복문 탈출
                    {
                        findTarget = true;
                        _animator.SetBool("IsMove", true);
                        ChangeState(eState.Trace);
                        break;
                    }
                }
            }
            
        }
    }
    IEnumerator FindPos()//랜덤좌표
    {
        while (!findTarget && curHP > 0)
        {
            float randomX = Random.Range(-5, 5) + transform.position.x;
            float randomZ = Random.Range(-5, 5) + transform.position.z;

            targetPos = new Vector3(randomX, transform.position.y, randomZ);
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
        }
    }
    IEnumerator FindPlayer()//플레이어 위치 탐색
    {
        while (findTarget && curHP > 0)
        {
            targetPos = _player.transform.position;
            yield return new WaitForSeconds(0.3f);
        }
    }
    IEnumerator Attack()//공격 코루틴
    {
        _animator.SetBool("IsAttack", true);

        Collider[] colliders = Physics.OverlapSphere(transform.position, 3.0f, targetLayers);

        foreach (Collider col in colliders)
        {
            if (col.tag == "Player")
            {
                col.GetComponent<Player>().GetDam(power);
                break;
            }
        }
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length+0.3f);

        _animator.SetBool("IsAttack", false);
    }

    IEnumerator AfterDead()//죽고나서 일정시간이 지나면 파괴
    {
        StatusManager.Instance.ChangeEXP(30);
        yield return new WaitForSeconds(1.0f);
        Instantiate(dropItem, transform.position + new Vector3(0, 0.3f, 0)
                    , Quaternion.identity);
        Destroy(this.gameObject);
    }
}
