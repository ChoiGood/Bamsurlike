using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

//Ǯ �Ŵ������� �޾ƿ� ����������� ��� �ȿ��� �����ְ� �������ִ� Ŭ����..
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
        // Update ������ switch �� Ȱ���Ͽ� ���⸶�� ���� ����
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

        if(id == 0) // �Ӽ� ����� ���ÿ� ���������� ��� ��ġ�� �ʿ��ϴ� �Լ� ȣ��
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
        // �� �̷��� ����� �ϴ���??
        // ���� �����Ϳ��������� ���ڷ� �־ �Ǳ���Ѵ�
        // ������ �׿� ���� Ǯ�Ŵ����� �Ȱ��� ������ϱ⶧����
        // ## ��ũ��Ʈ�� ������Ʈ�� �������� ���ؼ� �ε����� �ƴ� ���������� ����!!

        switch (id)
        {
            case 0:
                speed = -150;  // �������� �ð�������� ����.
                Batch();

                break;
            default:
                speed = 0.3f; // ���⼭ speed ���� ����ӵ��� �ǹ� : �������� ���� �߻�
                break;

        }

        // Hand Set
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);


        // BroadcastMessage : Ư���Լ�ȣ���� ��� �ڽĿ��� ����ϴ� �Լ�
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    // ������ ���⸦ ��ġ�ϴ� �Լ�
    void Batch()
    {
        for(int index=0; index<count; index++)
        {
            // ������ ������Ʈ�� Transform�� ���������� ����
            // ���� ������Ʈ�� ���� Ȱ���ϰ� ���ڶ� ���� Ǯ������ ��������
            Transform bullet;
                
            if(index < transform.childCount)    // �ڽ��� �ڽ� ������Ʈ ���� Ȯ���� childCount �Ӽ�
            {
                bullet = transform.GetChild(index); // index�� ���� childCount ���� ����� GetChild �Լ��� ��������
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                // �� ���� Transfrom ?? ==> �θ� �ٲٱ� ���� !!
                bullet.parent = transform; // parent �Ӽ��� ���� �θ� ����
            }
                

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;


            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);  // Rotate �Լ��� ���� ���� ����
            bullet.Translate(bullet.up * 1.5f, Space.World);    // Translate �Լ��� �ڽ��� �������� �̵� ,, �̵� ������ Space World ��������



            // bullet ������Ʈ �����Ͽ� �Ӽ� �ʱ�ȭ �Լ� ȣ��
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1 is Infinity Per.   
        }

    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        // �Ѿ� ���� ���
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        // ��ġ, ȸ�� 
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir); // ���Ÿ� ���ݿ� �°� �ʱ�ȭ �Լ� ȣ���ϱ�
    }
}
