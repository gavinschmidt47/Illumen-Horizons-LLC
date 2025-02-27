using UnityEngine;
using UnityEngine.AI;

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

    //Enemy
    private Rigidbody rb;
    void Awake()
    {
        //change back to 2
        gameInfo.ability = UnityEngine.Random.Range(0 , 1);

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

                    //If camera looking toward enemy and within distance
                    if (angle < 85f && directionToPlayer.magnitude < 30f)
                    {
                        agent.isStopped = true;
                        rb.constraints = RigidbodyConstraints.FreezeAll;
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

                    //If camera looking toward enemy
                    if (angle < 85f)
                    {
                        agent.isStopped = false;
                        rb.constraints = RigidbodyConstraints.FreezeAll;
                    }
                    else
                    {
                        agent.isStopped = true;
                        rb.constraints = RigidbodyConstraints.None;
                    }
                    agent.SetDestination(player.transform.position);
                    return;
                case 2:
                    Myers();
                    return;
            }
        }
    }

    void WeepingAngel()
    {
        
    }

    void Enderman()
    {

    }

    void Myers()
    {

    }
}
