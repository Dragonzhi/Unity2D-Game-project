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
    public Collider2D discoll;//���Թرյ���ײ    
    public float speed;//�ٶȱ���
    public float jumpForce;//��Ծ����
    public Transform CellingCheck;//ͷ����û����ײ��
    public int Weapon_type = 0;//��������
    public Text Weapon_type_Num;//����������ʾ


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
        ChangeAct();
        Crouch();
        
        //�����������
    }
    //�ƶ�����
    void Movement()
    {
        Crouch();
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
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
            animator.SetBool("jumping", true);
        }

        


    }
    
    //����-��Ծ���������ж�
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
