using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;

    SpriteRenderer player;

    // �޼��� ����, �������� ��ġ .. �ű⿡ ���� ������ �������ٰ� �����غ���
    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);



    private void Awake()
    {
        // �÷��̾��� ��������Ʈ ������ ���� ���� �� �ʱ�ȭ
        player = GetComponentsInParent<SpriteRenderer>()[1];    // �ڱ��ڽ����׵� ��������Ʈ ������ ������ �װ͵� ���� (0����) �׷� �ι�°�� �θ��� ��������Ʈ �������̴�.

    }
    
    void LateUpdate()
    {
        bool isReverse = player.flipX;
        
        if(isLeft) // ���� ����
        {
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse ? 4 : 6;
        }
        else   // ���Ÿ� ����
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }

}