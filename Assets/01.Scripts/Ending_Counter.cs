using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ending_Counter : MonoBehaviour
{

   


    public Vector3 Offset = new Vector3(4f, 0f, 0f);

    public bool isCol = false;

    Player_Move _player;

    private void Start()
    {
        _player = Game_Manager.instance.Player.GetComponent<Player_Move>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCol == false)
        {
            if (other.CompareTag("Cart"))
            {
                isCol = true;
                Transform _cart = _player.Ending_Lose_Cart();
                _cart.gameObject.layer = LayerMask.NameToLayer("NonCol");
                _cart.DOMove(transform.position + Offset, 0.5f);

            }
        }
    }





}
