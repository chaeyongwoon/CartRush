using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TreasureBox : MonoBehaviour
{

    Player_Move _player;

    private void Start()
    {
        _player = Game_Manager.instance.Player.GetComponent<Player_Move>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("EndingCart") || other.CompareTag("Cart"))
        {
            DOTween.Sequence().Append(transform.GetChild(0).DOLocalRotate(new Vector3(-120f, 0f, 0f), 0.5f))
                .AppendCallback(() => transform.GetChild(4).GetComponent<ParticleSystem>().Play());

            _player.End_Treasure();
        }
    }
}
