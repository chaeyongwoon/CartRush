using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending_Line : MonoBehaviour
{

    [SerializeField] bool iscol = false;

    private void OnTriggerEnter(Collider other)
    {
        if (iscol == false)
        {
            if (other.CompareTag("Cart") || other.CompareTag("Player"))
            {
                iscol = true;
                Game_Manager.instance.Player.GetComponent<Player_Move>().EndingLine_Col(transform.GetChild(0));

            }
        }
    }

}
