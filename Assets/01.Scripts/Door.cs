using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Door : MonoBehaviour
{

    public Vector3 Open_Rot;
    public float Open_Duration = 1f;

    private void Start()
    {
        //transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Col");
            Open();
        }
    }

    public void Open()
    {
        transform.DORotateQuaternion(Quaternion.Euler(Open_Rot), Open_Duration);
    }
}
