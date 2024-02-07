using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellZone : MonoBehaviour
{
    public float Sell_interval = 0.2f;

    [SerializeField] bool isStay = false;
    [SerializeField] float _time = 0f;

    [SerializeField] Player_Move _player;




    private void Start()
    {
        _player = Game_Manager.instance.Player.GetComponent<Player_Move>();
        StartCoroutine(Cor_Sell());
    }

    IEnumerator Cor_Sell()
    {
        while (true)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            _time += Time.deltaTime;
            if (isStay)
            {
                if (_time >= Sell_interval)
                {
                    _time = 0;
                    //_player.SellProduct(1);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            isStay = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isStay = false;
        }
    }




}
