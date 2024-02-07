using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitRange : MonoBehaviour
{
    [SerializeField] bool isLeft = false;
    public float Limit = 2f;
    Player_Move _player;

    private void Awake()
    {
        if (transform.position.x < 0)
        {
            isLeft = true;
        }
        else if (transform.position.x > 0)
        {
            isLeft = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("Player"))
        {
            _player = other.GetComponent<Player_Move>();

            if (isLeft)
            {
                _player.CurrentLimit_x_left = -_player.BaseLimit_x;
                _player.CurrentLimit_x_right = -Limit;
            }
            else
            {
                _player.CurrentLimit_x_left = Limit;
                _player.CurrentLimit_x_right = _player.BaseLimit_x;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player.CurrentLimit_x_left = -_player.BaseLimit_x;
            _player.CurrentLimit_x_right = _player.BaseLimit_x;
            //if (_player.state != Player_Move.State.Fever)
            //    _player.Recovery(true);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Vector4(0f, 1f, 0f, 0.6f);
        Gizmos.DrawCube(transform.position + GetComponent<BoxCollider>().center, GetComponent<BoxCollider>().size);


    }



}
