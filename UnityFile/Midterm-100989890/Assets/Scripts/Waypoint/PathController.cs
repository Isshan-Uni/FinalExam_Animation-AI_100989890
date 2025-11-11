using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [SerializeField]
    public PathManager pathManager;

    List<Waypoint> thePath;
    Waypoint target;
    public Transform player;
    public float ChaseDistance;
    public float ChaseSpeed;
    private Animator anim;

    public float MoveSpeed;
    public float RotateSpeed;
    private int num = 1;
    private int num2 = 1;

    //public Animator animator;
    bool outofRange;

    private void Start()
    {
        thePath = pathManager.GetPath();
        if(thePath != null && thePath.Count > 0)
        {
            target = thePath[0]; // set starting target to the first waypoint
        }
       
        anim = GetComponent<Animator>();
        anim.SetLayerWeight(1, 1);


    }

    void rotateTowardsTarget()
    {
        float stepSize = RotateSpeed * Time.deltaTime;

        Vector3 targetDir = target.pos - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void moveForward()
    {
        float stepSize = Time.deltaTime * MoveSpeed;
        float distanceToTarget = Vector3.Distance(transform.position, target.pos);
        if(distanceToTarget < stepSize)
        {
            return;
        }

        Vector3 moveDir = Vector3.forward;
        transform.Translate(moveDir * stepSize);

    }

    void rotateTowardsPlayer()
    {
        float stepSize = RotateSpeed * Time.deltaTime;

        Vector3 targetDir = player.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void moveForwardToPlayer()
    {
        float stepSize = Time.deltaTime * MoveSpeed;
        float distanceToTarget = Vector3.Distance(transform.position, player.position);
        if (distanceToTarget < stepSize)
        {
            return;
        }

        Vector3 moveDir = Vector3.forward;
        transform.Translate(moveDir * stepSize);

    }

    private void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= ChaseDistance)
        {
            rotateTowardsPlayer();
            moveForwardToPlayer();
            if(num < 1) StartCoroutine(StartRunning(1f));
            num2 = 0;
        }
        else 
        {
            rotateTowardsTarget();
            moveForward();

            if (num2 < 1) StartCoroutine(SlowDown(1f));
            num = 0;
        }

        

        
    }

    IEnumerator StartRunning(float duration)
    {

        num += 1;
        float normalizedTime = 0.0f;

        while (normalizedTime < 1.0f)
        {

            anim.SetLayerWeight(2, normalizedTime);
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }

        anim.SetLayerWeight(1, 0);


    }

    IEnumerator SlowDown(float duration)
    {

        num2 += 1;
        float normalizedTime = 0.0f;

        anim.SetLayerWeight(1, 1);
        while (normalizedTime < 1.0f)
        {
            print("SlowDown");
            anim.SetLayerWeight(2, 1- normalizedTime);
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }

        anim.SetLayerWeight(2, 0);
        


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PathPoint"))
        {
            target = pathManager.GetNextTarget();
        }
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit");
            anim.SetLayerWeight(3, 1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit");
            anim.SetLayerWeight(3, 0);
        }
    
    }



}
