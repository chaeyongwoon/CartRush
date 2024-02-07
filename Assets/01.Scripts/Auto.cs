using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Auto : MonoBehaviour
{

    //public float Passing_Speed = 5f;
    public bool isUsed = false;

    public Player_Move _player;

    public GameObject Pair;


    //public bool isSell = false;
    //[ShowIf("isSell")] public int Sell_Count = 5;
    //[ShowIf("isSell")] public float Sell_Interval = 0.2f;



    void Start()
    {
        _player = _player == null ? Game_Manager.instance.Player.GetComponent<Player_Move>() : _player;
    }




    protected void Fix_Player(Transform other, Player_Move.State _state)
    {
        if (isUsed == false)
        {
            isUsed = true;
            if(Pair!=null) Pair.GetComponent<Auto>().isUsed = true;
            other.GetComponent<Player_Move>().state = _state;

            other.DOMoveX(transform.position.x, 0.2f);
            //if (isSell)
            //{
            //    StartCoroutine(Cor_Sell());
            //}
        }

    }

    //IEnumerator Cor_Sell()
    //{
    //    for (int i = 0; i < Sell_Count; i++)
    //    {
    //        yield return new WaitForSeconds(Sell_Interval);

    //        _player.SellProduct(1);
    //    }
    //}






}
