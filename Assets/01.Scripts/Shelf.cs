using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Shelf : MonoBehaviour
{

    [SerializeField] Transform[] Child_Obj;
    [SerializeField] Vector3[] Start_Pos;

    int _count;
    public bool OnlyTop = false;
    private void Awake()
    {
        _count = transform.childCount;

        Child_Obj = new Transform[_count];
        Start_Pos = new Vector3[_count];

        for (int i = 0; i < _count; i++)
        {
            Child_Obj[i] = transform.GetChild(i);
            Start_Pos[i] = transform.GetChild(i).position;
        }
    }

    private void OnEnable() => Resetting();


    public Product.ProductType ChildType;


    private void Resetting()
    {
        for (int i = 0; i < _count; i++)
        {
            Child_Obj[i].gameObject.SetActive(false);
            if (OnlyTop && i < 23)
            {
                Child_Obj[i].gameObject.SetActive(true);
            }
            else if (OnlyTop == false)
            {
                Child_Obj[i].gameObject.SetActive(true);
            }
            Child_Obj[i].position = Start_Pos[i];
            if (Child_Obj[i].GetComponent<Rigidbody>() != null) // 오브젝트 모델링 변경되면 삭제
            {
                Child_Obj[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
            }

        }
    }

    [HorizontalGroup("Split", 0.5f)]
    [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
    public void ChildChange()
    {
        Awake();

        for (int i = 0; i < _count; i++)
        {
            if (Child_Obj[i].GetComponent<Product>() != null)
            {
                Child_Obj[i].GetComponent<Product>().SetChange(((int)ChildType));
                Child_Obj[i].GetComponent<Product>().SetBigBoxcol();
            }
        }
    }

    [HorizontalGroup("Split", 0.5f)]
    [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
    public void ChildRandomChange()
    {
        Awake();

        for (int i = 0; i < _count; i++)
        {
            if (Child_Obj[i].GetComponent<Product>() != null)
            {
                Child_Obj[i].GetComponent<Product>().RandomChange();
                Child_Obj[i].GetComponent<Product>().SetBigBoxcol();
            }

        }
    }




}
