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

        if(per >= 0)    // 관통이 -1 (무한)보다 큰 것에 대해서는 속도 적용
        {
            rigid.velocity = dir * 15f;     // 속력을 곱해주어 총알이 날아가는 속도 증가시키기.
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -100)
            return;


        // 관통 값이 하나씩 줄어들면서 -1 이 되면 비활성화
        per--;

        if (per < 0)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || per == -100)
            return;

        gameObject.SetActive(false);

    }
}
