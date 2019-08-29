using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    [Header("Stats")]
    public int              playerLife;
    public float            speed;
    public float            jumpForce;
    public bool             facingLeft;
    private bool            isGrounded;
    private bool            isAttack;
    [Header("Sprite Color")]
    public Color            hitColor;
    public Color            noHitColor;
    
    [Header("Targets")]
    public GameObject       hitBoxPrefab;
    public Transform        hand;
    public Transform        groundCheck;
    private Rigidbody2D     player2D;
    private Animator        playerAnimator;
    private gameController  _gameController;
    private SpriteRenderer playerRenderer;
    private Transform playerTransform;


    // Functions premaded by Unity
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GetComponent<Transform>();
        player2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        _gameController = FindObjectOfType(typeof(gameController)) as gameController;
        _gameController.playerTransform = this.transform;
        playerRenderer = GetComponent<SpriteRenderer>();
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
            _gameController.playSFX(_gameController.sfxJump, 0.35f);
        }

        if(Input.GetButtonDown("Fire1") && isAttack == false){
            isAttack = true;
            _gameController.playSFX(_gameController.sfxAttack, 0.35f);
            playerAnimator.SetTrigger("attack");
        }

        player2D.velocity = new Vector2(hor*speed, speedY);

        playerAnimator.SetInteger("Horizontal", (int) hor);
        playerAnimator.SetBool("isGrounded", isGrounded);
        playerAnimator.SetFloat("speedY", speedY);
        playerAnimator.SetBool("isAttack", isAttack);

        if(playerLife <= 0)
        {
           restartGame(); 
        }

    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
    }
    
    private void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Collectable")
        {
            _gameController.playSFX(_gameController.sfxCoin, 0.35f);
            Destroy(col.gameObject);
        }
        else if(col.gameObject.tag == "Damage")
        {
            _gameController.playSFX(_gameController.sfxGetHit, 0.35f);
            print("Dano");
            StartCoroutine("damageController");
        }
    }


    // Functions created by me
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

    void footStep1()
    {
        _gameController.playSFX(_gameController.sfxStep[Random.Range(0, _gameController.sfxStep.Length)], 0.35f);
    }

    /* void footStep2()
    {
        _gameController.playSFX(_gameController.sfxStep[1], 0.35f);
    }*/
    
    IEnumerator damageController()
    {
        playerLife--;

        this.gameObject.layer = LayerMask.NameToLayer("Invincible");
        playerRenderer.color = hitColor;
        yield return new WaitForSeconds(0.2f);
        playerRenderer.color = noHitColor;
        for(int i = 0; i < 5; i++)
        {
            playerRenderer.enabled = false;
            yield return new WaitForSeconds(0.2f);
            playerRenderer.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
        this.gameObject.layer = LayerMask.NameToLayer("Player");
        playerRenderer.color = Color.white;
        
    }

    void restartGame(){
        playerTransform.gameObject.SetActive(false);
        _gameController.painelGameOver.SetActive(true);
        if (_gameController.painelGameOver == true && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            _gameController.painelGameOver.SetActive(false);

        }
    }
}
