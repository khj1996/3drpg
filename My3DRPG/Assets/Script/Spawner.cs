using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject monster;

    [SerializeField]
    private GameObject[] spawnMonster;

    public float spawnCoolTime = 0;
    Vector3 randomPos;
    NavMeshHit hit;
    Vector3 spawnPos;
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject go = Instantiate(monster, transform.position, Quaternion.identity, transform);
            randomPos = Random.insideUnitSphere * 10.0f + transform.position;
            NavMesh.SamplePosition(randomPos, out hit, 5.0f, NavMesh.AllAreas);
            go.transform.position = hit.position;
            spawnMonster[i] = go;
        }
    }
    public void Respawn()
    {
        StartCoroutine(Respawncor());
    }


    private IEnumerator Respawncor()
    {
        yield return new WaitForSeconds(5.0f);
        foreach (GameObject monster in spawnMonster)
        {
            if (monster.activeSelf == false)
            {
                randomPos = Random.insideUnitSphere * 10.0f + transform.position;

                NavMesh.SamplePosition(randomPos, out hit, 5.0f, NavMesh.AllAreas);
                spawnPos = hit.position;
                monster.transform.position = spawnPos;
                monster.GetComponent<Enemy>().ResetMonster();
                break;
            }
        }
    }
}
