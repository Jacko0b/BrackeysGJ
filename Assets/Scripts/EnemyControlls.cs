using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyControlls : MonoBehaviour
{
    public float mSpeed = 1.2f;
    public float patrolSpeed = 0.7f;
    public float chaseSpeed = 1.2f;
    public float baseSpeed = 1f;
    public bool canShoot = true;
    public bool useAlarm = false;
    public GameObject bulletPrefab;
    public GameObject deadBody;
    public AccessLevel accessLevel;
    public Vector3 playerPos;

    [SerializeField] private AIPath EnemyPathfindingMovement;
    [SerializeField] private AIDestinationSetter destinationSetter;
    [SerializeField] private Patrol patrol;
    [SerializeField] private Alarm closestAlarm;
    [SerializeField] private Transform defaultTransform;
    [SerializeField] private Animator anim;

    [SerializeField] private float nextShootTimer;
    [SerializeField] private float fireRate = 0.6f;

    public State currentState;
    public State defaultState;

    public enum State
    {
        Idle,
        WalkRandomly,
        Patrol,
        PlayerChase,
        RunAway,
    }
    private void Awake()
    {
        accessLevel = GetComponent<AccessLevel>();
        anim = GetComponent<Animator>();
        EnemyPathfindingMovement = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        patrol = GetComponent<Patrol>();
        currentState = defaultState;
        defaultTransform = transform;
    }
    private void Start()
    {
        anim.SetInteger("access", accessLevel.accessLevel);
        anim.SetBool("notKey", !accessLevel.haveKey);
        if (CompareTag("Guard"))
        {
            anim.SetBool("Guard", true);
        }
        else
        {
            anim.SetBool("Guard", false);
        }

    }
    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                GetComponent<AIDestinationSetter>().enabled = true;
                GetComponent<Patrol>().enabled = false;
                destinationSetter.target = defaultTransform;

                break;

            case State.WalkRandomly:
                GetComponent<AIDestinationSetter>().enabled = true;
                GetComponent<Patrol>().enabled = false;
                destinationSetter.target = GetComponentInChildren<RandomPoint>().transform;

                mSpeed = baseSpeed;
                break;

            case State.Patrol:
                GetComponent<AIDestinationSetter>().enabled = false;
                GetComponent<Patrol>().enabled = true;
                mSpeed = patrolSpeed;
                break;

            case State.PlayerChase:
                GetComponent<AIDestinationSetter>().enabled = true;
                GetComponent<Patrol>().enabled = false;
                mSpeed = chaseSpeed;
                destinationSetter.target = GameObject.FindObjectOfType<PlayerControlls>().transform;
                transform.GetChild(2).gameObject.SetActive(true);
                break;

            case State.RunAway:
                GetComponent<AIDestinationSetter>().enabled = true;
                GetComponent<Patrol>().enabled = false;
                mSpeed = chaseSpeed;
                destinationSetter.target = closestAlarm.transform;
                useAlarm = true;
                break;

            default:
                break;
        }
        anim.SetFloat("velocity", Mathf.Max(EnemyPathfindingMovement.velocity.x, EnemyPathfindingMovement.velocity.y));


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Instantiate(deadBody,transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (useAlarm && collision.CompareTag("Alarm"))
        {
            if (Vector3.Distance(transform.position, closestAlarm.transform.position) < 0.2f)
            {
                closestAlarm.alarmActivated();
                useAlarm = false;
                currentState = State.WalkRandomly;
            }
        }
        if (collision.CompareTag("Player"))
        {
            transform.GetChild(2).gameObject.SetActive(true);

            if (tag == "Scientist")
            {
                currentState = State.RunAway;
            }
            else
            {
                currentState = State.PlayerChase;
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
            if (currentState== State.PlayerChase)
            {
                shoot();
                rotateTowardsPlayer();
            }
            //chasePlayer()
        

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            transform.GetChild(2).gameObject.SetActive(true);
        }

    }
    public void shoot()
    {
        if (canShoot)
        {
            if (Time.time > nextShootTimer)
            {
                nextShootTimer = Time.time + fireRate;
                Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                //sound - shoot
                //bullet effect
            }
        }
    }
    public void rotateTowardsPlayer()
    {
        float angle = Mathf.Atan2(
            GameObject.FindObjectOfType<PlayerControlls>().transform.position.y - transform.position.y,
            GameObject.FindObjectOfType<PlayerControlls>().transform.position.x - transform.position.x) 
            *Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));
    }



}
