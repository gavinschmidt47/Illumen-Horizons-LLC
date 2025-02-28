using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    //GameInfo
    public GameInfo gameInfo;

    //NavMesh
    private NavMeshAgent agent;
    public Vector3[] spawn;

    //Player
    public GameObject player;
    public LayerMask playerLayer;
    private Vector3 directionToPlayer;
    private float angle;
    private RaycastHit hit;

    //Enemy
    private Rigidbody rb;
    void Awake()
    {
        //change back to 2
        gameInfo.ability = UnityEngine.Random.Range(0 , 2);

        rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        //Sends to random place
        agent.SetDestination(spawn[UnityEngine.Random.Range(0, 3)]);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameInfo.inStart)
        {
            switch (gameInfo.ability)
            {
                case 0:
                    //Vector to player and its angle to camera
                    directionToPlayer = player.transform.position - transform.position;
                    angle = Vector3.Angle(directionToPlayer, -player.transform.forward);

                    //Raycast to check if player is in line of sight and within distance
                    if (Physics.Raycast(transform.position, directionToPlayer, out hit, directionToPlayer.magnitude))
                    {
                        if (hit.collider.gameObject == player && angle < 85f && directionToPlayer.magnitude < 30f)
                        {
                            agent.isStopped = true;
                            rb.constraints = RigidbodyConstraints.FreezeAll;
                        }
                        else
                        {
                            agent.isStopped = false;
                            rb.constraints = RigidbodyConstraints.None;
                        }
                    }
                    else
                    {
                        agent.isStopped = false;
                        rb.constraints = RigidbodyConstraints.None;
                    }
                    agent.SetDestination(player.transform.position);
                    return;
                case 1:
                    //Vector to player and its angle to camera
                    directionToPlayer = player.transform.position - transform.position;
                    angle = Vector3.Angle(directionToPlayer, -player.transform.forward);

                    //Raycast to check if player is in line of sight and within distance
                    if (Physics.Raycast(transform.position, directionToPlayer, out hit, directionToPlayer.magnitude))
                    {
                        if (hit.collider.gameObject == player && angle < 85f)
                        {
                            agent.SetDestination(player.transform.position);
                        }
                        else
                        {
                            agent.SetDestination(transform.position);
                        }
                    }
                    else
                    {
                        agent.SetDestination(transform.position);
                    }
                    
                    return;
                case 2:
                    //Vector to player and its angle to camera
                    directionToPlayer = player.transform.position - transform.position;
                    angle = Vector3.Angle(directionToPlayer, -player.transform.forward);

                    //Raycast to check if player is in line of sight
                    if (Physics.Raycast(transform.position, directionToPlayer, out hit, directionToPlayer.magnitude))
                    {
                        if (hit.collider.gameObject == player)
                        {
                            Debug.Log("Player in line of sight");
                            agent.isStopped = true;
                            rb.constraints = RigidbodyConstraints.FreezeAll;
                        }
                        else
                        {
                            Debug.Log("Player not in line of sight");
                            agent.isStopped = false;
                            rb.constraints = RigidbodyConstraints.None;
                            agent.SetDestination(player.transform.position);
                        }
                    }
                    else
                    {
                        Debug.Log("Raycast did not hit anything");
                        agent.isStopped = false;
                        rb.constraints = RigidbodyConstraints.None;
                        agent.SetDestination(player.transform.position);
                    }
                    return;
            }
        }
    }

    private IEnumerator StopAgentForSeconds(float seconds)
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(seconds);
        agent.isStopped = false;
    }
}
