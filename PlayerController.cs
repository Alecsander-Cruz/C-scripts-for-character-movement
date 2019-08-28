using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private Rigidbody2D     player2D;
    private Animator        playerAnimator;
    public Transform        hand;
    public Transform        groundCheck;
    public float            speed;
    public float            jumpForce;
    public bool             facingLeft;
    private bool            isGrounded;
    private bool            isAttack;

    public GameObject       hitBoxPrefab;



    // Start is called before the first frame update
    void Start()
    {
        player2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float hor = Input.GetAxisRaw("Horizontal");

        if(isAttack == true && isGrounded == true){
            hor = 0;
        }
        
        if(hor > 0 && facingLeft){
            Flip();
        }else if(hor < 0 && !facingLeft){
            Flip();
        }

        float speedY = player2D.velocity.y;

        if(Input.GetButtonDown("Jump") && isGrounded == true){
            player2D.AddForce(new Vector2(0, jumpForce));
        }

        if(Input.GetButtonDown("Fire1") && isAttack == false){
            isAttack = true;
            playerAnimator.SetTrigger("attack");
        }

        player2D.velocity = new Vector2(hor*speed, speedY);

        playerAnimator.SetInteger("Horizontal", (int) hor);
        playerAnimator.SetBool("isGrounded", isGrounded);
        playerAnimator.SetFloat("speedY", speedY);
        playerAnimator.SetBool("isAttack", isAttack);

    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
    }
    void Flip()
    {
        facingLeft = !facingLeft;
        float x = (transform.localScale.x * -1); // Invert X scale
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    void OnEndAttack()
    {
        isAttack = false;
    }

    void hitBoxAttack()
    {
        GameObject hitBoxTemp = Instantiate(hitBoxPrefab, hand.position, transform.localRotation);
        Destroy(hitBoxTemp, 0.2f);
    }
}
