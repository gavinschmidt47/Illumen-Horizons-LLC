using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //GameInfo
    public GameInfo gameInfo;

    //NavMesh
    private NavMeshAgent agent;

    //Player
    public GameObject player;
    void Awake()
    {
        //gameInfo.ability = UnityEngine.Random.Range(0 , 2);
        gameInfo.ability = 0;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void OnEnable()
    {
        Debug.Log("Enabled");
    }
    void OnDisable()
    {
        Debug.Log("Disabled");
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameInfo.ability)
        {
            case 0:
                WeepingAngel();
                return;
            case 1:
                Enderman();
                return;
            case 2:
                Myers();
                return;
        }
    }

    void WeepingAngel()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, player.transform.forward, out hit))
        {
            if (hit.transform == transform)
            {
            // Player is looking at this object
            agent.isStopped = true;
            }
            else
            {
            // Player is not looking at this object
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
            }
        }

    }

    void Enderman()
    {

    }

    void Myers()
    {

    }
}
