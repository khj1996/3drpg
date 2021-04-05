using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public enum eState
    {
        Invalid, // 초기화 안된 상태
        Idle,// 대기 상태
        Trace, //추적 상태
        Attack, //공격 상태
        Dead, // 죽어 있는 상태
    }

    public      eState      curState        =       eState.Invalid;

    public float attackCT { get; set; }   //공격쿨타임
    public float power { get; set; }      //공격력
    public float HP { get; set; }         //체력
    public float speed { get; set; }      //이동속도


    public void ChangeState(eState targetState)
    {
        if (curState == targetState)
            return;

        //State가 끝날 때 메소드 호출
        switch (curState)
        {
            case eState.Invalid:
                break;
            case eState.Idle:
                OnExitIdleState();
                break;
            case eState.Trace:
                OnExitTraceState();
                break;
            case eState.Attack:
                OnExitAttackState();
                break;            
        }

        curState = targetState;
        //State가 전환되고 나서 메소드 호출
        switch (curState)
        {
            case eState.Invalid:
                break;
            case eState.Idle:
                OnEnterIdleState();
                break;
            case eState.Trace:
                OnEnterTraceState();
                break;
            case eState.Attack:
                OnEnterAttackState();
                break;
            case eState.Dead:
                OnEnterDeadState();
                break;
        }
    }

    public virtual void OnUpdateIdleState()
    {
    }
    public virtual void OnUpdateTraceState()
    {
    }
    public virtual void OnUpdateAttackState()
    {
    }
    public virtual void OnUpdateDeadState()
    {
    }

    public virtual void OnEnterIdleState()
    {
    }
    public virtual void OnEnterTraceState()
    {
    }
    public virtual void OnEnterAttackState()
    {
    }
    public virtual void OnEnterDeadState()
    {
    }

    public virtual void OnExitIdleState()
    {
    }
    public virtual void OnExitTraceState()
    {
    }
    public virtual void OnExitAttackState()
    {

    }

    public virtual void GetDam(float amount)
    {

    }
}
