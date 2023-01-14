using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    public Transform patrolRoute;
    public List<Transform> waypoints;
    private int locationIndex = 0;
    private NavMeshAgent _agent;

    private float currentDelay = 0.0f;
    private float maxDelay = 0.5f;

    [SerializeField]
    private Transform player;
    public GameManager gameManager;

    private int _lives = 3;
    public int EnemyLives {
        get
        {
            return _lives;
        }
        private set {
            _lives = value;
            if (_lives <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        InitializeWaypoints();
        MoveToNextWaypoint();

    }

    private void MoveToNextWaypoint()
    {
        if(waypoints.Count == 0)
        {
            return;
        }
        locationIndex = Random.Range(0, waypoints.Count);
        _agent.SetDestination(waypoints[locationIndex].position);
        //locationIndex = (locationIndex + 1) % waypoints.Count;
        
    }

    private void InitializeWaypoints()
    {
        //Recorre los hijos de patrolRoute
        foreach(Transform wp in patrolRoute)
        {
            waypoints.Add(wp);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            Debug.Log("Tas muy cerca papi");
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
        {
            Vector3 targetPosition = new Vector3(player.position.x, this.transform.position.y, player.position.z); 
            this.transform.LookAt(targetPosition);
            _agent.speed = 5.0f;
            _agent.SetDestination(player.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Asi lejos quiero verte papi");
            _agent.speed = 3.5f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Tag es por que lo usaran varios objetos
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("Usted quiere pelea no?");
        }

        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("Me dio :'v");
            EnemyLives--;
        }
    }

    private void Update()
    {
        if(_agent.remainingDistance < 0.5f &&
            !_agent.pathPending)
        {
            currentDelay += Time.deltaTime;
            if (maxDelay < currentDelay)
            {
                currentDelay = 0.0f;
                MoveToNextWaypoint();
            }
            
        }
    }
}
