using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private EnemyAI level;
    
    public float radius;
    [Range(0, 360)] public float angle;

    [HideInInspector] public GameObject playerRef;
    
    public LayerMask targetMask;
    public LayerMask blockMask;

    public bool canSeePlayer;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
        
        if (level.npcLevel == EnemyAI.NpcLevel.Easy)
        {
            radius = 20;
            angle = 90;
        }
        else if (level.npcLevel == EnemyAI.NpcLevel.Normal)
        {
            radius = 30;
            angle = 100;
        }
        else if (level.npcLevel == EnemyAI.NpcLevel.Hard)
        {
            radius = 40;
            angle = 360;
        }
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, blockMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
}
