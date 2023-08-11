using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    Rigidbody2D rigid;

    void Awake()
    {
       rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if(per > -1)    // ������ -1 (����)���� ū �Ϳ� ���ؼ��� �ӵ� ����
        {
            rigid.velocity = dir * 15f;     // �ӷ��� �����־� �Ѿ��� ���ư��� �ӵ� ������Ű��.
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -1)
            return;


        // ���� ���� �ϳ��� �پ��鼭 -1 �� �Ǹ� ��Ȱ��ȭ
        per--;

        if (per == -1)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
        
    }
}
