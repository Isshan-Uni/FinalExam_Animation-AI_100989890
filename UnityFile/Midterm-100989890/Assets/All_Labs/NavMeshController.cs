using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{
    public GameObject Target;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

    }
    void Update()
    {

        agent.destination = Target.transform.position;

    }

    private void OnTriggerEnter(Collider other)      //if it hits the target
    {
        if (other.name == "Target")
        {
            //edit here
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Target")
        {           
            //edit here
        }
    }

}
