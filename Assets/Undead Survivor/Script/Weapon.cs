using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

//풀 매니저에서 받아온 근접무기들을 장면 안에서 모양새있게 관리해주는 클래스..
public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        // Update 로직도 switch 문 활용하여 무기마다 로직 실행
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;

                if(timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;

        }

        // .. Test Code ..
        if (Input.GetButtonDown("Jump"))
            LevelUp(10, 1);
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if(id == 0) // 속성 변경과 동시에 근접무기의 경우 배치도 필요하니 함수 호출
            Batch();

        player.BroadcastMessage("ApplyGear" , SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for(int index=0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if(data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }
        // 왜 이렇게 힘들게 하느냐??
        // 물론 데이터에서프리펩 숫자로 넣어도 되기는한다
        // 하지만 그에 맞춰 풀매니저도 똑같이 맞춰야하기때문에
        // ## 스크립트블 오브젝트의 독립성을 위해서 인덱스가 아닌 프리펩으로 설정!!

        switch (id)
        {
            case 0:
                speed = -150;  // 음수여야 시계방향으로 돈다.
                Batch();

                break;
            default:
                speed = 0.3f; // 여기서 speed 값은 연사속도를 의미 : 적을수록 많이 발사
                break;

        }

        // Hand Set
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);


        // BroadcastMessage : 특정함수호출을 모든 자식에게 방송하는 함수
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    // 생성된 무기를 배치하는 함수
    void Batch()
    {
        for(int index=0; index<count; index++)
        {
            // 가져온 오브젝트의 Transform을 지역변수로 저장
            // 기존 오브젝트를 먼저 활용하고 모자란 것은 풀링에서 가져오기
            Transform bullet;
                
            if(index < transform.childCount)    // 자신의 자식 오브젝트 개수 확인은 childCount 속성
            {
                bullet = transform.GetChild(index); // index가 아직 childCount 범위 내라면 GetChild 함수로 가져오기
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                // 왜 하필 Transfrom ?? ==> 부모를 바꾸기 위함 !!
                bullet.parent = transform; // parent 속성을 통해 부모 변경
            }
                

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;


            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);  // Rotate 함수로 계산된 각도 적용
            bullet.Translate(bullet.up * 1.5f, Space.World);    // Translate 함수로 자신의 위쪽으로 이동 ,, 이동 방향은 Space World 기준으로



            // bullet 컴포넌트 접근하여 속성 초기화 함수 호출
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1 is Infinity Per.   
        }

    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        // 총알 방향 계산
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        // 위치, 회전 
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir); // 원거리 공격에 맞게 초기화 함수 호출하기
    }
}
