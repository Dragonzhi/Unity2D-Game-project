using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadowControl : MonoBehaviour
{
    private Transform player;
    private Color color;

    //[Header("ʱ����Ʋ���")]
    public float activeTime;//��ʾʱ��
    public float activeStartTime;//��ʼ��ʾʱ��
    private SpriteRenderer playerSprite;
    private SpriteRenderer thisSprite;

    //[Header("��͸���ȿ���")]
    private float alpha;
    public float alphaSet;//��ʼֵ
    public float alphaMultiplier;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisSprite = GetComponent<SpriteRenderer>();
        //playerSprite = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        //thisSprite.sprite = playerSprite.sprite;
        transform.position = player.position;
        transform.localScale = player.localScale; 
        transform.rotation = player.rotation;

        activeStartTime = Time.time;
    }


    void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(0.6f, 0.8f, 1, alpha);
        thisSprite.color = color;
        if(Time.time >= activeStartTime + activeTime)
        {
            shadowPool.instance.ReturnPool(this.gameObject);
        }

    }

}
