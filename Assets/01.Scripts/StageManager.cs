using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StageManager : MonoBehaviour
{
    [Title("Map Value")]
    public GameObject[] Floor_Prefab;
    public int Floor_Prefab_Count;
    public float Floor_Prefab_Interval = 50f;
    [SerializeField] GameObject Floor_Group;

    [Title("Product Value")]
    public GameObject Product_Group;
    public GameObject[] Products;
    public int Product_Count;
    public float Product_Interval;

    [Title("Enemy Value")]
    public GameObject Enemy_Group;
    public GameObject[] Enemy;
    public float Limit_x = 4f;
    public float Enemy_interval = 10f;
    public int Enemy_Count = 10;


    [Title("Ending_Obj")]
    public GameObject Ending_Part;


    public GameObject[] Resetting_Obj;
    // =============================

    [Button]
    public void SpawnStage()
    {
        DestroyImmediate(transform.GetChild(0).gameObject);

        Floor_Group = new GameObject();
        Floor_Group.transform.SetParent(transform);
        Floor_Group.transform.name = "0.Stage_Floor";
        Floor_Group.transform.SetSiblingIndex(0);


        GameObject _Base1 = Instantiate(Floor_Prefab[0], new Vector3(0f, -1.2f, -1 * Floor_Prefab_Interval), Quaternion.identity);
        _Base1.transform.SetParent(Floor_Group.transform);

        for (int i = 0; i < Floor_Prefab_Count; i++)
        {
            GameObject _obj = Instantiate(Floor_Prefab[0], new Vector3(0f, -1.2f, i * Floor_Prefab_Interval), Quaternion.identity);
            _obj.transform.SetParent(Floor_Group.transform);
        }


        GameObject _Endingpart = Instantiate(Ending_Part
                                 , new Vector3(0f, 0f, ((Floor_Prefab_Count - 1) * Floor_Prefab_Interval) + 69.7f)
                                 , Quaternion.identity);

        _Endingpart.transform.SetParent(Floor_Group.transform);


    }


    [Button]
    public void Spawn_Products()
    {
        DestroyImmediate(transform.GetChild(1).gameObject);
        Product_Group = new GameObject();
        Product_Group.transform.SetParent(transform);
        Product_Group.transform.name = "1.Product_Group";
        Product_Group.transform.SetSiblingIndex(1);

        int _count = Product_Group.transform.childCount;
        for (int i = 0; i < _count; i++)
        {
            DestroyImmediate(Product_Group.transform.GetChild(0).gameObject);
        }

        for (int i = 0; i < Product_Count; i++)
        {
            int _product_num = Random.Range(0, Products.Length);
            GameObject _product = Instantiate(Products[_product_num]
                , new Vector3(Limit_x * Random.Range(-1, 2), 1f, 60f + Product_Interval * i), Quaternion.identity);
            _product.transform.SetParent(Product_Group.transform);



        }
    }

    [Button]
    public void Spawn_Enemy()
    {
        DestroyImmediate(transform.GetChild(2).gameObject);
        Enemy_Group = new GameObject();
        Enemy_Group.transform.SetParent(transform);
        Enemy_Group.transform.name = "2.Enemy_Group";
        Enemy_Group.transform.SetSiblingIndex(2);

        int _count = Enemy_Group.transform.childCount;
        for (int i = 0; i < _count; i++)
        {
            DestroyImmediate(Enemy_Group.transform.GetChild(0).gameObject);
        }

        for (int i = 0; i < Enemy_Count; i++)
        {
            int _enemy_num = Random.Range(0, Enemy.Length);
            GameObject _enemy = Instantiate(Enemy[_enemy_num]
                , new Vector3(Limit_x * Random.Range(-1, 2), 0.2f, 60f + Enemy_interval * i), Quaternion.identity);
            _enemy.transform.SetParent(Enemy_Group.transform);


            switch (_enemy_num)
            {
                case 1:

                    break;
                case 2:
                case 4:
                    _enemy.transform.position = new Vector3(0f, _enemy.transform.position.y, _enemy.transform.position.z);

                    _enemy.transform.rotation =
                        Random.Range(0, 2) == 0 ? Quaternion.Euler(Vector3.up * 0f) : Quaternion.Euler(Vector3.up * 180f);
                    break;

                case 3:
                    _enemy.transform.rotation =
                      Random.Range(0, 2) == 0 ? Quaternion.Euler(Vector3.up * 0f) : Quaternion.Euler(Vector3.up * 180f);
                    break;

                default:
                    _enemy.transform.position = new Vector3(0f, _enemy.transform.position.y, _enemy.transform.position.z);
                    break;
            }

        }


    }


    public void Resetting()
    {
        foreach (GameObject _obj in Resetting_Obj)
        {
            _obj.SetActive(false);
            _obj.SetActive(true);
        }
    }

}
