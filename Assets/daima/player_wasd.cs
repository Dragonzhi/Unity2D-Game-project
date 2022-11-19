using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_wasd : MonoBehaviour
{   
    private Rigidbody2D rb;//�������
    private Animator animator;//��������

    public LayerMask ground;//���������
    public Collider2D coll;//��ײ����
    public Collider2D coll1;//��ײ����1
    public Collider2D discoll;//���Թرյ���ײ    
    public float speed;//�ٶȱ���
    public float jumpForce;//��Ծ����
    public Transform CellingCheck;//ͷ����û����ײ��
    public Transform groundCheck;//������
    public int Weapon_type = 0;//��������
    public Text Weapon_type_Num;//����������ʾ
    private float horizontalMove;//�ƶ�
    [Header("��̱���")]
    public float dashTime;//Dashʱ��
    private float dashTimeless;//Dashʣ��ʱ��
    private float dashLast = -10f;//�ϴ�Dash��ʱ���
    public float dashCD;
    public float dashSpeed;

    public bool isGround, isJump,isDashing;
    bool jumpPressed;
    int jumpCount;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coll1 = GetComponent<Collider2D>();
    }
    void Update()
    {
        if(Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if(Time.time >= (dashLast + dashCD))
            {
                //����Dash
                ReadToDash();
            }
        }
    }
    
    void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        Movement();
        Dash();
        if (isDashing)
            return;
        Jump();
        ChangeAnim();
        //ChangeAct();
        Crouch();
        
    }
    //�ƶ�����
    void Movement()
    {
        //ˮƽ�ƶ��жϱ��� ��ȡˮƽ�ƶ�����ֵ��-1,0,1��
        horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
        //��ɫ�ƶ�
        if (horizontalMove != 0)
        {
            //����ı�
            transform.localScale = new Vector3(-horizontalMove, 1, 1);
            //animator.SetFloat("running", -horizontalMove);
        }
        /*
        //��ɫ��Ծ
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
            animator.SetBool("jumping", true);
        }*/
    }
    //�µ���Ծ������
    void Jump()
    {   
        if (isGround)
        {
            jumpCount = 2;
            isJump = false;
        }
        if(isGround && jumpPressed)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        else if(isJump && jumpPressed && jumpCount > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
    }
    //����
    void ChangeAnim()
    {
        animator.SetFloat("running", Mathf.Abs(rb.velocity.x));
        if (isGround)
        {
            animator.SetBool("falling", false);
        }
        else if (!isGround && rb.velocity.y > 0)
        {
            animator.SetBool("jumping", true);
        }
        else if (rb.velocity.y < 0)
        {
            animator.SetBool("jumping", false);
            animator.SetBool("falling", true);
        }
        if (isDashing)
        {
            animator.SetBool("dashing", true);
        }
        else if (!isDashing)
        {
            animator.SetBool("dashing", false);
        }
    }

    /*
    void ChangeAct()
    {
        animator.SetBool("idleing", false);
        //����ʱ�������䶯��
        if(rb.velocity.y<0.1f && !coll.IsTouchingLayers(ground))
        {
            animator.SetBool("falling", true);
        }
        //��Ծ����
        if(animator.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)
            {
                animator.SetBool("jumping", false);
                animator.SetBool("falling", true);
            }
            
        }
        //��Ծ�Ӵ�����
        else if (coll.IsTouchingLayers(ground))
        {
            animator.SetBool("falling", false);
            animator.SetBool("idleing", true);
        }
    }
    */

    //�¶�
    void Crouch()
    {
        //��ɫ����
        if (Input.GetButton("Crouch"))
        {
            //��������
            animator.SetBool("crouching", true);
            discoll.enabled = false;
        }
        else if(!Input.GetButton("Crouch"))
        {
            animator.SetBool("crouching", false);
            discoll.enabled = true;
        }

    }
    //�ռ���Ʒ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collection")
        {
            Destroy(collision.gameObject);
            Weapon_type = 1;
            Weapon_type_Num.text = Weapon_type.ToString();
        }
    }
    //���׼��
    void ReadToDash()
    {
        isDashing = true;
        dashTimeless = dashTime;
        dashLast = Time.time;
    }
    //���
    void Dash()
    {
        if(isDashing)
        {   
            if(dashTimeless > 0)
            {   
                if(rb.velocity.y >0 && !isGround)
                {
                    rb.velocity = new Vector2(dashSpeed * horizontalMove, jumpForce/2);
                }
                rb.velocity = new Vector2(dashSpeed * horizontalMove,rb.velocity.y);
                dashTimeless -= Time.deltaTime;
                shadowPool.instance.GetFromPool();

            }
            if(dashTimeless <= 0)
            {
                isDashing = false;
                if (!isGround)
                {
                    rb.velocity = new Vector2(dashSpeed * horizontalMove, jumpForce/2);
                }

            }
        }
    }
}   
