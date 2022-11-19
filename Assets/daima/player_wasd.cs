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

    public bool isGround, isJump;
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
    }
    
    void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        Movement();
        Jump();
        ChangeAnim();
        //ChangeAct();
        Crouch();
        
    }
    //�ƶ�����
    void Movement()
    {
        //ˮƽ�ƶ��жϱ��� ��ȡˮƽ�ƶ�����ֵ��-1,0,1��
        float horizontalMove = Input.GetAxisRaw("Horizontal");
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
    

}   
