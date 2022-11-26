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
    public Collider2D coll1;//碰撞变量1
    public Collider2D discoll;//可以关闭的碰撞    
   
    public Transform CellingCheck;//头顶有没有碰撞？
    public Transform groundCheck;//地面检测
    public int Weapon_type = 0;//武器类型
    public Text Weapon_type_Num;//武器类型显示
    private float horizontalMove;//移动
    [Header("移动的参数")]
    public float speed;//速度变量
    public float walkSpeed;//走路速度变量
    public float runSpeed;//跑步速度变量
    public float jumpForce;//跳跃变量
    [Header("CD的UI组件")]
    public Image cdImage;//cd图形

    [Header("冲刺变量")]
    public float dashTime;//Dash时间
    private float dashTimeless;//Dash剩余时间
    private float dashLast = -10f;//上次Dash的时间点
    public float dashCD;
    public float dashSpeed;
    //[Header("状态显示")]
    public bool isGround, isJump, isDashing;
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
        if (Input.GetButtonDown("Dash"))
        {
            if(Time.time >= (dashLast + dashCD))
            {
                //可以Dash
                ReadToDash();
            }
        }
        cdImage.fillAmount -= 1.0f / dashCD * Time.deltaTime;
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
    //移动函数
    void Movement()
    {
        //水平移动判断变量 获取水平移动按键值（-1,0,1）
        horizontalMove = Input.GetAxisRaw("Horizontal");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
        //角色走移动


        //角色跑步移动
        if (horizontalMove != 0)
        {
            //朝向改变
            transform.localScale = new Vector3(-horizontalMove, 1, 1);
            //animator.SetFloat("running", -horizontalMove);
        }
        
        /*
        //角色跳跃
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
            animator.SetBool("jumping", true);
        }*/
    }
    //新的跳跃函数！
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
    //动画
    void ChangeAnim()
    {   
        //移动部分
        animator.SetFloat("running", Mathf.Abs(rb.velocity.x));
        //跳跃部分
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
    */

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
    //冲刺准备
    void ReadToDash()
    {
        isDashing = true;
        dashTimeless = dashTime;
        dashLast = Time.time;
        cdImage.fillAmount = 1;
    }
    //冲刺
    void Dash()
    {
        if(isDashing)
        {   
            if(dashTimeless > 0)
            {   
                if(rb.velocity.y >0 && !isGround)
                {
                    rb.velocity = new Vector2(-dashSpeed * gameObject.transform.localScale.x, jumpForce/2);
                }
                rb.velocity = new Vector2(-dashSpeed * gameObject.transform.localScale.x, rb.velocity.y);
                dashTimeless -= Time.deltaTime;
                shadowPool.instance.GetFromPool();

            }
            if(dashTimeless <= 0)
            {
                isDashing = false;
                if (!isGround)
                {
                    rb.velocity = new Vector2(-dashSpeed * gameObject.transform.localScale.x, jumpForce/2);
                }

            }
        }
    }
}   
