using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_wasd : MonoBehaviour
{   
    private Rigidbody2D rb;//刚体变量
    private Animator animator;//动画变量

    public LayerMask ground;//检测地面变量
    public Collider2D coll;//碰撞变量
    public Collider2D discoll;//可以关闭的碰撞    
    public float speed;//速度变量
    public float jumpForce;//跳跃变量
    public Transform CellingCheck;//头顶有没有碰撞？
    public int Weapon_type = 0;//武器类型
    public Text Weapon_type_Num;//武器类型显示


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
        
        //调用这个函数
    }
    //移动函数
    void Movement()
    {
        Crouch();
        //水平移动判断变量 获取水平移动按键值（-1,0,1）
        float horizontalMove = Input.GetAxis("Horizontal");
        //面向
        float facedircetion = Input.GetAxisRaw("Horizontal");
        //运动动画判断变量
        float facedircetion1 = Mathf.Abs(facedircetion);
        //角色移动
        if (horizontalMove != 0)
        {
            //移动
            rb.velocity = new Vector2(horizontalMove * speed * Time.deltaTime, rb.velocity.y);
            animator.SetFloat("running", facedircetion1);
        }
        if (facedircetion != 0)
        {
            //朝向改变
            transform.localScale = new Vector3(-facedircetion, 1, 1);

        }
        //角色跳跃
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
            animator.SetBool("jumping", true);
        }

        


    }
    
    //动画-跳跃上升下落判断
    void ChangeAct()
    {
        animator.SetBool("idleing", false);
        //掉落时候变成下落动作
        if(rb.velocity.y<0.1f && !coll.IsTouchingLayers(ground))
        {
            animator.SetBool("falling", true);
        }
        //跳跃上升
        if(animator.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)
            {
                animator.SetBool("jumping", false);
                animator.SetBool("falling", true);
            }
            
        }
        //跳跃接触地面
        else if (coll.IsTouchingLayers(ground))
        {
            animator.SetBool("falling", false);
            animator.SetBool("idleing", true);
        }
    }
    //下蹲
    void Crouch()
    {
        //角色蹲下
        if (Input.GetButton("Crouch"))
        {
            //动画播放
            animator.SetBool("crouching", true);
            discoll.enabled = false;
        }
        else if(!Input.GetButton("Crouch"))
        {
            animator.SetBool("crouching", false);
            discoll.enabled = true;
        }

    }
    //收集物品
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
