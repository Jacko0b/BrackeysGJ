using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerBehaviour : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject deadGuard;
    public GameObject deadScientist;

    [SerializeField] private GameObject closestEnemy = null;
    [SerializeField] public Transform firePoint;
    [SerializeField] private bool canShoot = false;
    [SerializeField] private bool menuActive = false;
    [SerializeField] private bool notScience = true;
    [SerializeField] private bool notPlayer = false;

    [SerializeField] private float nextShootTimer;
    [SerializeField] private float fireRate=0.3f;
    [SerializeField] private PlayerControlls playerControlls;
    [SerializeField] private bool canPossess = false;
    [SerializeField] private bool currentlyPossessing = false;
    [SerializeField] private AccessLevel accessLevel;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource barabaszeSound;
    [SerializeField] private GameMaster gm;
    [SerializeField] private GameObject barabasze;
    [SerializeField] private GameObject defeatScreen;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject pauseMenuScreen;
    [SerializeField] private GameObject checkpoint0;

    // [SerializeField] private 
    private void Awake()
    {
        closestEnemy = new GameObject();
        closestEnemy.transform.position = new Vector3(100, 100, 100);
        firePoint = transform.GetChild(0);
        playerControlls = GetComponent<PlayerControlls>();
        accessLevel = GetComponent<AccessLevel>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        transform.position = gm.lastCheckPointPos;
    }
    private void FixedUpdate()
    {
        if(CompareTag( "Guard"))
        {
            anim.SetBool("notScience", true);
            anim.SetBool("notPlayer", true);
        }
        if (CompareTag("Player"))
        {
            anim.SetBool("notScience", true);
            anim.SetBool("notPlayer", false);
        }
        if (CompareTag("Scientist"))
        {
            anim.SetBool("notScience", false);
            anim.SetBool("notPlayer", true);
        }
    }

    public void possess()
    {
        if (closestEnemy.activeInHierarchy && canPossess)
        {

            //game pause
            //play minigame - win=possess, lose=death
            //game unpause
            //spawn blood patch effect
            //sound - scream

            canShoot = closestEnemy.GetComponent<EnemyControlls>().canShoot;
            playerControlls.mSpeed = closestEnemy.GetComponent<EnemyControlls>().baseSpeed;
            accessLevel.accessLevel = closestEnemy.GetComponent<AccessLevel>().accessLevel;
            accessLevel.haveKey = closestEnemy.GetComponent<AccessLevel>().haveKey;
            transform.position = closestEnemy.transform.position;
            tag = closestEnemy.tag;
            
            transform.GetChild(2).gameObject.SetActive(true);
            //swap sprites
           
            //animation
            anim.SetInteger("animationAccessLevel", accessLevel.accessLevel);
            
            
            currentlyPossessing = true;
            closestEnemy.SetActive(false);

        }
    }
    public void leaveBody()
    {
        if (currentlyPossessing)
        {
            if (tag == "Scientist")
            {
                spawnBody(1);
            } else spawnBody(2); 

            canShoot = false;
            playerControlls.mSpeed = playerControlls.basicMovSpeed;
            accessLevel.accessLevel = 0;
            accessLevel.haveKey = false;
            gameObject.tag = "Player";
            currentlyPossessing = false;
            transform.GetChild(2).gameObject.SetActive(false);
            
            //animation
            anim.SetInteger("animationAccessLevel", accessLevel.accessLevel);

            //sound 

        }

    }
    public void spawnBody(int i)
    {
        if (i==1) Instantiate(deadScientist, transform.position, transform.rotation);
        else if (i==2) Instantiate(deadGuard, transform.position, transform.rotation);
        //also spawn big blood patch
        //sound - body falling into the ground 
    }
    public void shoot()
    {
        if (canShoot)
        {
            if(Time.time > nextShootTimer)
            {
                nextShootTimer = Time.time + fireRate;
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                //sound - shoot
                //bullet effect
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (currentlyPossessing)
            {
                leaveBody();
            }
            else playerDeath();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Guard") || collision.CompareTag("Scientist"))
        {
            canPossess = true;
            if (Vector3.Distance(gameObject.transform.position, collision.gameObject.transform.position) <=
                       Vector3.Distance(gameObject.transform.position, closestEnemy.transform.position))
            {
                closestEnemy = collision.gameObject;
            }
        }
        if (collision.CompareTag("Finish"))
        {
            playerVictory();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        canPossess = false;
    }
    public void playerDeath()
    {
        Time.timeScale = 0;
        defeatScreen.SetActive(true);
        //Debug.Log("death");

    }
    public void retryButton()
    {
        Time.timeScale = 1;
        defeatScreen.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    public void replayButton()
    {
        Time.timeScale = 1;
        victoryScreen.SetActive(false);
        gm.lastCheckPointPos = checkpoint0.transform.position;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    public void exitToMenuButton()
    {
        SceneManager.LoadScene(0);

        //load menu
    }
    public void saveEverybodyVicory()
    {
        transform.position = checkpoint0.transform.position;
        GetComponent<SpriteRenderer>().color = Color.clear;
        playerControlls.mSpeed = 0;
        barabasze.SetActive(true);
        barabaszeSound.Play();
        StartCoroutine(waitForSeconds(3));
        //fadeaway
    }

    
    public void pauseMenu()
    {
        if (!menuActive)
        {
            Time.timeScale = 0;
            pauseMenuScreen.SetActive(true);
            menuActive = true;
        }
        else
        {
            Time.timeScale = 1;
            pauseMenuScreen.SetActive(false);
            menuActive = false;
        }
    }

    public void playerVictory()
    {
        Time.timeScale = 0;
        victoryScreen.SetActive(true);
        //Debug.Log("wygranko");
    }

    IEnumerator waitForSeconds(int i)
    {
        yield return new WaitForSecondsRealtime(i);
        playerVictory();

    }
}
