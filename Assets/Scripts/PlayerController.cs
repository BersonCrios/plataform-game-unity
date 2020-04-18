using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private GameController _gameController;

    private Rigidbody2D playerRB;
    private Animator playerAnimator;
    private SpriteRenderer playerSr;

    public float speed;
    public float jumpForce;
    public bool isLookLeft;

    public Transform groundCheck;
    private bool isGrounded;
    private bool isAtack;

    public Transform hand;
    public GameObject hitBoxPrefab;
    public Color hitColor;
    public Color noHitColor;

    public int maxHp;
    public int cash;

    //METODOS OVERIDE
    void Start(){
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerSr = GetComponent<SpriteRenderer>();

        _gameController = FindObjectOfType(typeof(GameController)) as GameController;
        _gameController.playerTransform = this.transform;
    }


    void Update(){
        float h = Input.GetAxisRaw("Horizontal");

        if(isAtack == true  &&  isGrounded == true){
            h = 0;
        }

        if (h > 0 && isLookLeft) {
            flip();
        }
        else if (h < 0 && !isLookLeft) {
            flip();
        }

        float speedY = playerRB.velocity.y;

        if (Input.GetButtonDown("Jump")  &&  isGrounded == true) {
            _gameController.playSFX(_gameController.sfxJump, 0.5f);
            playerRB.AddForce(new Vector2(0, jumpForce));
        }

        if (Input.GetButtonDown("Fire1")  &&  isAtack == false){
            isAtack = true;
            _gameController.playSFX(_gameController.sfxAtack, 0.5f);
            playerAnimator.SetTrigger("atack");
        }

        playerRB.velocity = new Vector2(h*speed, speedY);


        playerAnimator.SetInteger("h", (int) h);
        playerAnimator.SetBool("isGrounded", isGrounded);
        playerAnimator.SetFloat("speedY", speedY);
        playerAnimator.SetBool("isAtack", isAtack);
    }

    void FixedUpdate() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "coletavel")
        {
            _gameController.playSFX(_gameController.sfxCoin, 0.5f);
            Destroy(col.gameObject);
            cash++;
        }
        else if (col.gameObject.tag == "damage") {
            StartCoroutine("damageController");
        }
    }

    //MÉTODOS CRIADOS POR MIM
    void flip() 
    {
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x * -1; // INVERTE O SINAL DO SCALE X
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    void onEndAtack() 
    {
        isAtack = false;
    }

    void hitBoxAtack() 
    {
        GameObject hitboxTemp = Instantiate(hitBoxPrefab,hand.position, transform.localRotation);
        Destroy(hitboxTemp, 0.2f);
    }

    void footStep() 
    {
        _gameController.playSFX(_gameController.sfxStep[Random.Range(0, _gameController.sfxStep.Length)], 1f);
    }

    IEnumerator damageController()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Invencivel");
        maxHp--;
        playerSr.color = hitColor;
        _gameController.playSFX(_gameController.sfxPlayerDamage, 0.5f);
        yield return new WaitForSeconds(0.3f);
        playerSr.color = noHitColor;

        for (int i = 0; i < 5; i++) {
            playerSr.enabled = false;
            yield return new WaitForSeconds(0.2f);
            playerSr.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
        this.gameObject.layer = LayerMask.NameToLayer("Player");
        playerSr.color = Color.white;
    }
}