using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TeacherWalking : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform pointA;
    public Transform pointB;
    public float arrivalTolerance = 0.1f;
    public float waitTime = 5.0f; // How long to stand at the end before turning

    private NavMeshAgent agent;
    private Animator animator; 
    private Transform currentTarget;

    private bool isWaiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();

        currentTarget = pointB;
        agent.SetDestination(currentTarget.position);

        if (animator != null) animator.SetBool("IsWalking", true);
    }

    void Update()
    {
        if (isWaiting) return;

        float zDistance = Mathf.Abs(transform.position.z - currentTarget.position.z);

        if (zDistance <= arrivalTolerance)
        {
            StartCoroutine(WaitAndTurn());
        }
    }

    private IEnumerator WaitAndTurn()
    {
        isWaiting = true; 
        agent.isStopped = true; 

        if (animator != null)
        {
            animator.SetBool("IsWalking", false); 
            animator.SetTrigger("ArrivedAtPoint"); 
        }

        // Turn and face the class and give some teaching instructions
        float randomYAngle = (currentTarget == pointA) ? Random.Range(0f, 80f) : Random.Range(90f, 180f);
        transform.rotation = Quaternion.Euler(0, randomYAngle, 0);


        yield return new WaitForSeconds(waitTime);

        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
        }

        currentTarget = (currentTarget == pointA) ? pointB : pointA;
        agent.SetDestination(currentTarget.position);



        agent.isStopped = false;
        isWaiting = false;
    }
}
