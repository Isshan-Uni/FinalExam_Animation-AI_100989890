using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using DitzelGames.FastIK;
using Unity.VisualScripting;

public class JumpMovementOnLink : MonoBehaviour
{
    public float jumpHeight = 2f;      
    public float jumpDuration = 0.6f;
    public float animationspeed = 3f;
    public GameObject Camera1;
    public GameObject Camera2;
    public GameObject Camera3;
    public GameObject Sword;
    public GameObject Bow;
    private Collider col;
    private Animator anim;
    private int items;
    
    public FastIKFabric ikscript;
    public GameObject attractionObject;
    
    private NavMeshAgent agent;
    private bool isJumping;

    bool onItem;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();
        Sword.SetActive(false);
        Bow.SetActive(false);
        Camera2.SetActive(false);
        Camera3.SetActive(false);

    }

    void Update()
    {
        if (agent.isOnOffMeshLink && !isJumping)
        {
            StartCoroutine(Parabola(agent, jumpHeight, jumpDuration));
            anim.speed = 1f;
            anim.SetTrigger("Jump");
            
            
        }
        col.isTrigger = isJumping;

        if (Input.GetMouseButtonDown(1) && onItem)
        {

            gameObject.transform.LookAt(attractionObject.transform);
            items += 1;
            onItem = false;
            Camera3.SetActive(true);
            Camera1.SetActive(false);
            StartCoroutine(PickUp(1f));
            ikscript.ResolvingIK = true;
            ikscript.Attraction = attractionObject.transform;
            print("Hittt");
        }

        if(items > 1)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Sword.SetActive(false);
                Bow.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Sword.SetActive(true);
                Bow.SetActive(false);
            }
        }
    }

    IEnumerator PickUp(float duration)
    {
        float normalizedTime = 0.0f;
        Sword.SetActive(false);
        while (normalizedTime < 1.0f)
        {
            
            anim.SetLayerWeight(1, normalizedTime);
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
        attractionObject.SetActive(false);
        StartCoroutine(PickUpDone(1f));
    }

    IEnumerator PickUpDone(float duration)
    {
        
        Sword.SetActive(true);
        float normalizedTime = 0.0f;

        while (normalizedTime < 1.0f)
        {

            anim.SetLayerWeight(1, 1 - normalizedTime);
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
        ikscript.ResolvingIK = false;
        anim.SetLayerWeight(1, 0);
        Camera1.SetActive(true);
        Camera3.SetActive(false);

    }

    IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
    {
        isJumping = true;
        agent.isStopped = true;
        Camera2.SetActive(true);
        Camera1.SetActive(false);
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * (agent.baseOffset + 0.2f);

        float normalizedTime = 0.0f;

        while (normalizedTime < 1.0f)
        {

            float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);

            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;

            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }

        agent.transform.position = endPos;

        agent.CompleteOffMeshLink();
        agent.isStopped = false;
        isJumping = false;
        anim.SetTrigger("Idle");
        Camera1.SetActive(true);
        Camera2.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Item"))
        {
            
            onItem = true;
            print("Hit");
            attractionObject = other.gameObject;

        }
    }
}
