using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeIA : MonoBehaviour
{

    private gameController _gameController;
    private Rigidbody2D slimeRB2D;
    private Animator slimeAnimator;
    public float speed;
    public float timeWalk;
    public GameObject hitBox;

    private int hor;
    public bool facingLeft;

    // Start is called before the first frame update
    void Start()
    {
        _gameController = FindObjectOfType(typeof(gameController)) as gameController;
        slimeRB2D = GetComponent<Rigidbody2D>();
        slimeAnimator = GetComponent<Animator>();
        StartCoroutine("slimeWalk");
    }

    // Update is called once per frame
    void Update()
    {
        slimeRB2D.velocity = new Vector2(hor * speed, slimeRB2D.velocity.y);

        if(hor > 0 && facingLeft == true)
        {
            Flip();
        }
        else if(hor < 0 && facingLeft == false)
        {
            Flip();
        }

        if(hor != 0)
        {
            slimeAnimator.SetBool("isWalking", true);
        }
        else
        {
            slimeAnimator.SetBool("isWalking", false);
        }


    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "HitBox")
        {
            hor = 0;
            StopCoroutine("slimeWalk");
            Destroy(hitBox);
             _gameController.playSFX(_gameController.sfxEnemyDeath, 0.35f);
             slimeAnimator.SetTrigger("dead");
        }      
    }

    IEnumerator slimeWalk()
    {
        int random = Random.Range(0, 100);

        if(random < 33)
        {
            hor = -1;
        }
        else if(random < 66)
        {
            hor = 0;
        }
        else
        {
            hor = 1;
        }

        yield return new WaitForSeconds(timeWalk);
        StartCoroutine("slimeWalk");
    }

    void OnDead()
    {
        Destroy(this.gameObject);
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        float x = transform.localScale.x * (-1);
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
}
