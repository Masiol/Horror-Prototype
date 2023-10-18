using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [Header("Zombie Settings")]
    [SerializeField] private float chaseRadius = 4f;
    [SerializeField] private float biteRadius = 2.5f;
    [SerializeField] private float handRange = 1.25f;
    [SerializeField] private float blindedDuration = 8f;
    [SerializeField] private float resistanceForNextBlind = 6f;

    [Header("LayerMasks")]
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    [Header("Patroling")]    
    public float radius;
    [Range(0, 360)] 
    public float angle;
    public bool canSeePlayer;
    public Transform[] patrolPoints;
    private int currentPatrolIndex;
    private bool patroling;
    public bool canHearPlayer = true;
    private bool heardPlayer;

    [Header("Others")]
    public GameObject playerRef;
    private Animator animator; 
    private NavMeshAgent navMeshAgent; 
    private SphereCollider chaseTrigger;
    private bool attackedPlayer;
    [SerializeField] private Transform hand;
    private horrorFlashlightBasic horror;
    [SerializeField] private bool blinded;
    public bool canBlindEnemy;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        chaseTrigger = gameObject.AddComponent<SphereCollider>();
        chaseTrigger.isTrigger = true;
        chaseTrigger.radius = chaseRadius;
        StartCoroutine(FOVRoutine());
    }

    private void Update()
    {
        if (playerRef == null)
        {
            playerRef = GameObject.FindGameObjectWithTag("Player");
            horror = playerRef.transform.GetComponentInChildren<horrorFlashlightBasic>();
            GetReference();
        }
        bool isAnimationPlaying = GetComponent<Animator>()?.GetCurrentAnimatorStateInfo(0).IsName("Blinded") ?? false;
        float distanceToPlayer = Vector3.Distance(transform.position, playerRef.transform.position);
        if (horror.enoughIntensityForBlindEnemy && horror.turnedOn && canSeePlayer && distanceToPlayer <= chaseRadius && !blinded && canBlindEnemy && !resistanceBlind)
        {
            Debug.Log("Oslep enemy");
            BlindEnemy();
        }

        
       // Debug.Log(distanceToPlayer + transform.name) ;
        if (canSeePlayer)
        {
            if(distanceToPlayer < biteRadius)
                BitePlayer();
        }
       

        if (canSeePlayer || heardPlayer && !isAnimationPlaying)
        {
            StopPatrol();
            ChasePlayer();          
        }
        else if(!blinded)
        {
            patroling = true;
            Patrol();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (canHearPlayer)
            {
                heardPlayer = true;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (canHearPlayer)
            {
                heardPlayer = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            heardPlayer = false;
        }
    }

    private void GetReference()
    {
        PlayerInput playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        playerInput.OnSitStatusChanged += PlayerHandleSitStatusChanged;
    }
    private void OnDisable()
    {
        if (playerRef != null)
        {
            PlayerInput playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
            playerInput.OnSitStatusChanged -= PlayerHandleSitStatusChanged;
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

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

    private void Patrol()
    {
        if (patroling)
        {
            if (patrolPoints.Length > 0)
            {
                navMeshAgent.speed = 0.45f;

                float distanceToPatrolPoint = Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position);

                if (distanceToPatrolPoint < navMeshAgent.stoppingDistance)
                {
                    currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                }
                navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position);

                animator.SetBool("Walk", true);
                animator.SetBool("Run", false);
            }
        }
    }

    private void StopPatrol()
    {
        patroling = false;
        animator.SetBool("Walk", false);
    }

    private void ChasePlayer()
    {
        if (!blinded)
        {
            navMeshAgent.SetDestination(playerRef.transform.position);

            if (!attackedPlayer)
            {
                navMeshAgent.speed = 2f;
            }

            animator.SetBool("Run", true);

            Vector3 directionToPlayer = playerRef.transform.position - transform.position;
            directionToPlayer.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            float rotationSpeed = 25f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    private void PlayerHandleSitStatusChanged(bool isSitting)
    {
        if (isSitting)
        {
            canHearPlayer = false;
        }
        else
        {
            canHearPlayer = true;
        }
    }

    private void BitePlayer()
    {
        if (!attackedPlayer && !blinded)
        {
            Debug.Log("Bite");
            
            navMeshAgent.speed = 0;
            animator.SetTrigger("Bite");
            attackedPlayer = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, biteRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(hand.transform.position, handRange);
    }
    private bool playerHit = false;
    public void Attack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(hand.transform.position, handRange, targetMask);

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHit = true;
                    Debug.Log("Trafiony gracz");
                    playerRef.GetComponent<PlayerInput>().enabled = false;
                    playerRef.GetComponent<PhysicalCC>().enabled = false;
                    playerRef.GetComponentInChildren<CameraController>().target = this.GetComponentInChildren<Head>().transform;
                    playerHealth.Die();
                    break;
                }
            }
        }

        if (!playerHit)
        {
            // Jeœli wrog nie trafi³ gracza, zresetuj animacjê ataku po pewnym czasie
            StartCoroutine(ResetAttack());
            Debug.Log("Nietrafiony gracz");
        }
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("ResetBite");
        animator.ResetTrigger("Bite");
        yield return new WaitForSeconds(0.5f);
        navMeshAgent.speed = 0.45f;
        playerHit = false;
        animator.SetBool("Walk", true);
        attackedPlayer = false;
    }
    private void BlindEnemy()
    {
        if (!blinded)
        {
            StopPatrol();
            resistanceBlind = true;
            navMeshAgent.speed = 0;
            animator.SetTrigger("Blinded");
            blinded = true;
            Invoke("ResetBlindness", blindedDuration);
        }
    }
    private void ResetBlindness()
    {
        animator.SetTrigger("ResetBlind");
        animator.ResetTrigger("Blinded");
        blinded = false;
        Debug.Log("RESETBLIND");
        if (canSeePlayer || canHearPlayer)
        {
            //animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Walk", true);
        }
        Invoke("ResistanceBlind", resistanceForNextBlind);      
    }
    private bool resistanceBlind = false;
    private void ResistanceBlind()
    {
        Debug.Log("RESETRESISTANCE");
        resistanceBlind = false;
    }
}