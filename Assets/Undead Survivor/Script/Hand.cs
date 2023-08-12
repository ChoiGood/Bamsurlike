using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;

    SpriteRenderer player;

    // 왼손은 방향, 오른손은 위치 .. 거기에 대한 정보를 변수에다가 저장해보자
    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);



    private void Awake()
    {
        // 플레이어의 스프라이트 렌더러 변수 선언 및 초기화
        player = GetComponentsInParent<SpriteRenderer>()[1];    // 자기자신한테도 스프라이트 렌더러 있으면 그것도 포함 (0으로) 그럼 두번째가 부모의 스프라이트 렌더러이다.

    }
    
    void LateUpdate()
    {
        bool isReverse = player.flipX;
        
        if(isLeft) // 근접 무기
        {
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse ? 4 : 6;
        }
        else   // 원거리 무기
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }

}
