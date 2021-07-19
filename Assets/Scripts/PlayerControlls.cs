using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerControlls : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private FieldOfView fov;
    [SerializeField] private PlayerBehaviour player;
    [SerializeField] private EndgameButton endButton;
    [SerializeField] private Animator anim;

    public float basicMovSpeed = 3f;
    public Camera cam;
    public Vector2 shootingTarget;

    /*public BoxCollider2D playerCollider;
    public LayerMask groundLayer;
    public AudioSource runAudio;
    public AudioSource jumpAudio;
    public Animator anim;*/

    private Vector2 moveInput;
    public float mSpeed = 3;
    public void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<PlayerBehaviour>();
        rb = GetComponent<Rigidbody2D>();

        //playerCollider = GetComponent<BoxCollider2D>();
       // anim = GetComponent<Animator>();
    }
    private void Start()
    {
      

    }
    private void FixedUpdate()
    {
        move();
        fov.setOrigin(transform.position);
    }
    public void move()
    {
        rb.velocity=new Vector2(
            moveInput.x*mSpeed, 
            moveInput.y * mSpeed
            );
        float angle = Mathf.Atan2(shootingTarget.y, shootingTarget.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
        anim.SetFloat("velocity", Mathf.Max(rb.velocity.x, rb.velocity.y));
    }
    public void getMoveInput(InputAction.CallbackContext value)
    {
        moveInput = value.ReadValue<Vector2>();
    }

    public void menu(InputAction.CallbackContext value)
    {
        if (value.started) player.pauseMenu();
    }


    public void fire(InputAction.CallbackContext value)
    {
        if (value.started) player.shoot();
    }
    public void use(InputAction.CallbackContext value)
    {
        if (value.started && endButton.enabledButton == false) player.possess();
        else if (endButton.enabledButton == true) player.saveEverybodyVicory();
    }
    public void leaveBody(InputAction.CallbackContext value)
    {
        if (value.started) player.leaveBody();
    }
    public void PointerPosition(InputAction.CallbackContext value)
    {
        mousePos(value.ReadValue<Vector2>());
    }
    public void mousePos(Vector2 mouseClick)
    {
        //look dir = mouse pos - RB pos
        shootingTarget = cam.ScreenToWorldPoint(
            new Vector3(mouseClick.x, mouseClick.y, 0)) - 
            new Vector3(rb.position.x,rb.position.y,0);
    }
}
