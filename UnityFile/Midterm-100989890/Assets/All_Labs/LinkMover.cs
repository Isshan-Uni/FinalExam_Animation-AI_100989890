using System.Collections;
using UnityEngine;
using UnityEngine.AI;

//This script meant to add animation when you jump via navmesh link.
[RequireComponent(typeof(NavMeshAgent))]
public class LinkMover : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    IEnumerator Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;
        while (true)
        {
            if (agent.isOnOffMeshLink)
            {
                yield return StartCoroutine(Parabola(agent, 2.0f, 1.0f));
                agent.CompleteOffMeshLink();

            }
            yield return null;
        }
    }

    IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f)
        {
            animator.SetBool("Jumping",true);            //This is the part that triggers the animation 
            float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }
    void Update()
    {
        if (agent.isOnOffMeshLink == false)
        {
            animator.SetBool("Jumping",false);         //This is the part that stops the animation 
        }
    }
}
