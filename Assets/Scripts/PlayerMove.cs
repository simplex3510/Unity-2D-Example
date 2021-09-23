using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    CapsuleCollider2D capsuleCollider2D;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        // Jump
        if (Input.GetButtonDown("Jump") && (rigid.velocity.y == 0))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetBool("isJump", true);
        }

        if (Input.GetButtonUp("Horizontal"))
        {
            // Stop Speed
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.1f, rigid.velocity.y);
        }

        // Direction Sprite
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1 ? true : false;
        }

        // Animation
        if (Mathf.Abs(rigid.velocity.x) <= 0.3)
        {
            animator.SetBool("isWalk", false);
        }
        else
        {
            animator.SetBool("isWalk", true);
        }

    }

    void FixedUpdate()
    {
        // Move By Key Control
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        // Max Speed
        if (rigid.velocity.x > maxSpeed)             // right Max Speed
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < maxSpeed * (-1))  // Left Max Speed
        {
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }

        // Landing Platform
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down / 2, new Color(1, 0, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector2.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance <= 0.5)
                {
                    animator.SetBool("isJump", false);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // Attack
            if (rigid.velocity.y < 0 && transform.position.y > other.transform.position.y)
            {
                OnAttack(other.transform);
            }
            else // 
            {
                OnDamaged(other.transform.position);
            }
        }
    }

    void OnAttack(Transform enemy)
    {
        // Point
        gameManager.stagePoint += 100;

        // Reaction Force
        rigid.AddForce(Vector2.up * 12, ForceMode2D.Impulse);

        // Enemy Die
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Item")
        {
            // Point
            bool isBronze = other.gameObject.name.Contains("Bronze");
            bool isSilver = other.gameObject.name.Contains("Silver");
            bool isGold = other.gameObject.name.Contains("Gold");

            if (isBronze)
            {
                gameManager.stagePoint += 50;
            }
            else if (isSilver)
            {
                gameManager.stagePoint += 70;
            }
            else if (isGold)
            {
                gameManager.stagePoint += 100;
            }

            // Deactive Item
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.tag == "Finish")
        {
            // Next Stage
            gameManager.NextStage(); 
        }

    }

    void OnDamaged(Vector2 targetPosition)
    {
        // Health Down
        gameManager.HealthDown();

        // Change Layer
        gameObject.layer = 9;

        // View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // Reaction Force
        int dirc = transform.position.x - targetPosition.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);

        // Animation
        animator.SetTrigger("doDamaged");

        Invoke("OffDamaged", 1);
    }

    void OffDamaged()
    {
        // Change Layer
        gameObject.layer = 8;

        // View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
         // Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // Sprite Flip y
        spriteRenderer.flipY = true;

        // Collider Diasble
        capsuleCollider2D.enabled = false;

        // Die Effect Jump
        rigid.AddForce(Vector2.up * 3.0f, ForceMode2D.Impulse);
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }

}