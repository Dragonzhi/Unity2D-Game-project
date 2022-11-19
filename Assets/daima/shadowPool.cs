using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadowPool : MonoBehaviour
{
    public static shadowPool instance;
    public GameObject shadowPrefab;
    public int shadowCount;
    private Queue<GameObject> availableObjects = new Queue<GameObject>();

    void Awake()
    {
        instance = this;
        //��ʼ�������
        Fillpool();
    }
    public void Fillpool()
    {
        for (int i = 0; i < shadowCount; i++)
        {
            var newShadow = Instantiate(shadowPrefab);
            newShadow.transform.SetParent(transform);
            //ȡ�����÷��ض����
            ReturnPool(newShadow);
        }
    }
    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        availableObjects.Enqueue(gameObject);

    }
    public GameObject GetFromPool()
    {   
        if(availableObjects.Count == 0)
        {
            Fillpool();
        }
        var outShadow = availableObjects.Dequeue();
        outShadow.SetActive(true);
        return outShadow;
    }

}
