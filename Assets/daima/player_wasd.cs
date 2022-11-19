using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_wasd : MonoBehaviour
{   
    private Rigidbody2D rb;
    //刚体变量
    private Animator animator;
    //动画变量
    public LayerMask ground;
    //检测地面变量
    public Collider2D coll;
    //碰撞变量
    public float speed;
    //速度变量
    public float jumpForce;
    //跳跃变量
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
        //调用这个函数
    }
    //移动函数
    void Movement()
    {
        
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
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
            animator.SetBool("jumping", true);
        }
    }
    
    //跳跃上升下落判断
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
