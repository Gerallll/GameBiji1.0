using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AINavigationall : MonoBehaviour
{
    public NavMeshAgent ai;
    public List<Transform> destinations;
    public Animator aiAnim;
    public float walkSpeed, chaseSpeed, minIdleTime, maxIdleTime, normalSightDistance, increasedSightDistance, catchDistance, idleChance;
    public bool chasing;
    public Transform player;
    private Transform currentDest;
    private int randNum;
    private bool isIdle = false;
    private bool canMove = true;
    private float idleTimer;
    public float sightDistance;

    public SoundEffectsPlayer1 Audio;

    public List<Transform> checkpoints;
    private int currentCheckpointIndex = -1;

    public Camera mainCamera;
    public Camera jumpscareCamera;
    public float jumpscareTime;

    void Start()
    {
        ChooseRandomDestination();
        ai.speed = walkSpeed;
        aiAnim.SetTrigger("walk");
        sightDistance = normalSightDistance;
        StartRandomIdleTimer();
    }

    void Update()
    {
        DetectPlayer();

        if (chasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void DetectPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, sightDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (!chasing)
                {
                    StartChase();
                }
            }
        }
    }

    void Patrol()
    {
        if (canMove && !isIdle)
        {
            if (ai.remainingDistance <= ai.stoppingDistance && !ai.pathPending)
            {
                StartCoroutine(IdleThenMove());
            }
            else
            {
                if (aiAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    aiAnim.ResetTrigger("idle");
                    aiAnim.SetTrigger("walk");
                    sightDistance = normalSightDistance;
                }

                if (idleTimer <= 0 && Random.Range(0f, 1f) < idleChance)
                {
                    StartCoroutine(IdleThenMove());
                    StartRandomIdleTimer();
                }
            }
        }

        if (idleTimer > 0)
        {
            idleTimer -= Time.deltaTime;
        }
    }

    IEnumerator IdleThenMove()
    {
        isIdle = true;
        aiAnim.ResetTrigger("walk");
        aiAnim.SetTrigger("idle");
        ai.speed = 0;

        sightDistance = increasedSightDistance;

        float idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);

        isIdle = false;
        aiAnim.ResetTrigger("idle");
        aiAnim.SetTrigger("walk");
        ai.speed = walkSpeed;

        if (ai.remainingDistance <= ai.stoppingDistance)
        {
            ChooseRandomDestination();
        }

        ai.SetDestination(currentDest.position);

        sightDistance = normalSightDistance;
    }

    public void ChooseRandomDestination()
    {
        randNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randNum];
        ai.SetDestination(currentDest.position);
    }

    void StartChase()
    {
        chasing = true;
        ai.speed = chaseSpeed;
        aiAnim.ResetTrigger("walk");
        aiAnim.ResetTrigger("idle");
        aiAnim.SetTrigger("sprint");
        sightDistance = normalSightDistance;
    }

    void ChasePlayer()
    {
        ai.SetDestination(player.position);

        aiAnim.ResetTrigger("walk");
        aiAnim.ResetTrigger("idle");
        aiAnim.ResetTrigger("sprint");
        aiAnim.SetTrigger("sprint");

        float distance = Vector3.Distance(player.position, ai.transform.position);
        if (distance <= catchDistance)
        {
            StartCoroutine(DeathRoutine());
        }
    }

    void StartRandomIdleTimer()
    {
        idleTimer = Random.Range(5f, 10f);
    }

    IEnumerator DeathRoutine()
    {
        if (jumpscareCamera != null && mainCamera != null)
        {
            aiAnim.ResetTrigger("sprint");
            aiAnim.SetTrigger("roar"); // Set animasi roar jika perlu
            mainCamera.gameObject.SetActive(false);
            jumpscareCamera.gameObject.SetActive(true);
            yield return new WaitForSeconds(jumpscareTime);

            jumpscareCamera.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(true);

            if (currentCheckpointIndex >= 0 && currentCheckpointIndex < checkpoints.Count)
            {
                player.position = checkpoints[currentCheckpointIndex].position;
            }
            else
            {
                player.position = Vector3.zero; // Set to initial position or other fallback
            }

            player.gameObject.SetActive(true);
            ChooseRandomDestination();
            chasing = false;
        }
    }

    public void SetCheckpoint(int checkpointIndex)
    {
        if (checkpointIndex >= 0 && checkpointIndex < checkpoints.Count)
        {
            currentCheckpointIndex = checkpointIndex;
        }
    }
}
