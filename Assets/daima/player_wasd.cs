using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_wasd : MonoBehaviour
{   
    private Rigidbody2D rb;
    //�������
    private Animator animator;
    //��������
    public LayerMask ground;
    //���������
    public Collider2D coll;
    //��ײ����
    public float speed;
    //�ٶȱ���
    public float jumpForce;
    //��Ծ����
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        Changeact();
        //�����������
    }
    //�ƶ�����
    void Movement()
    {
        
        //ˮƽ�ƶ��жϱ��� ��ȡˮƽ�ƶ�����ֵ��-1,0,1��
        float horizontalMove = Input.GetAxis("Horizontal");
        //����
        float facedircetion = Input.GetAxisRaw("Horizontal");
        //�˶������жϱ���
        float facedircetion1 = Mathf.Abs(facedircetion);
        //��ɫ�ƶ�
        if (horizontalMove != 0)
        {
            //�ƶ�
            rb.velocity = new Vector2(horizontalMove * speed * Time.deltaTime, rb.velocity.y);
            animator.SetFloat("running", facedircetion1);
        }
        if (facedircetion != 0)
        {
            //����ı�
            transform.localScale = new Vector3(-facedircetion, 1, 1);

        }
        //��ɫ��Ծ
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
            animator.SetBool("jumping", true);
        }
    }
    
    //��Ծ���������ж�
    void Changeact()
    {
        animator.SetBool("idleing", false);
        if(animator.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)
            {
                animator.SetBool("jumping", false);
                animator.SetBool("falling", true);
            }
            
        }
        else if (coll.IsTouchingLayers(ground))
        {
            animator.SetBool("falling", false);
            animator.SetBool("idleing", true);
        }
    }
    

}   
