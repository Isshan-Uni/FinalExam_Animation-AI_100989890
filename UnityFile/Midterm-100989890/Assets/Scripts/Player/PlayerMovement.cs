using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    public float moveSpeed = 10f;

    public float SampleDistance = 0.5f;
    public LayerMask groundLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.speed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            print("Hi");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                
                if(NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, SampleDistance, NavMesh.AllAreas))
                {
                    agent.SetDestination(navMeshHit.position);
                    anim.SetTrigger("Walk");
                    
                }
            }
        }
        else
        {
            anim.SetTrigger("Idle");
        }
    }

}
