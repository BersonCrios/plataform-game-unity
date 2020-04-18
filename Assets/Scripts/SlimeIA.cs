using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeIA : MonoBehaviour
{
    private GameController _GameController;
    private Rigidbody2D slimeRb;
    private Animator slimeanimator;
        
    public float speed;
    public float timeToWalk;
    public bool isLookLeft;

    public GameObject hitBox;

    private int h;

    void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;
        slimeRb = GetComponent<Rigidbody2D>();
        slimeanimator = GetComponent<Animator>();

        StartCoroutine("slimeWalk");
    }

    void Update()
    {
        if (h > 0 && isLookLeft)
        {
            flip();
        }
        else if (h < 0 && !isLookLeft)
        {
            flip();
        }

        slimeRb.velocity = new Vector2(h * speed, slimeRb.velocity.y);

        if (h != 0) {
            slimeanimator.SetBool("isWalk", true);
        }
        else
        {
            slimeanimator.SetBool("isWalk", false);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "hitBox")
        {
            h = 0;
            StartCoroutine("slimeWalk");
            Destroy(hitBox);
            _GameController.playSFX(_GameController.sfxEnemyDeath, 0.2f);
            slimeanimator.SetTrigger("dead");
        }

    }

    void flip()
    {
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x * -1; // INVERTE O SINAL DO SCALE X
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    IEnumerator slimeWalk() {
        int rand = Random.Range(0,100);

        if (rand < 33) {
            h = -1;
        }
        else if (rand < 66) {
            h = 0;
        }
        else if (rand < 100){
            h = 1;
        }

        yield return new WaitForSeconds(timeToWalk);
        StartCoroutine("slimeWalk");
    }

    void OnDead() {
        Destroy(this.gameObject);
    }
}
