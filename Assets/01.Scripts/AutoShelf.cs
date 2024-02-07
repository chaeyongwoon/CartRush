using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoShelf : Auto
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _player.state != Player_Move.State.Auto)
        {
            Fix_Player(other.transform, Player_Move.State.Auto);
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        if (_player.state != Player_Move.State.Fever)
    //            _player.Recovery(false);

    //        if (_player.Current_Fever_Guage >= _player.Max_Fever_Guage)
    //        {
    //            _player.Current_Fever_Guage = _player.Max_Fever_Guage;
    //            _player.FeverMode();
    //        }



    //    }
    //}

}
