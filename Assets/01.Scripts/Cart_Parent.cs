using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cart_Parent : MonoBehaviour
{

    public GameObject[] _Child;

    Player_Move _player;

    public bool isConnect = false;
    public int _index = 0;
    public int _num = 0;
    public int Product_Level = 0;
    public bool isReady = true;

    private void Start()
    {
        _player = Game_Manager.instance.Player.GetComponent<Player_Move>();
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Cart") && other.GetComponent<Cart_Parent>().isReady)
        {
            //other.GetComponent<Cart_Parent>().isConnect = true;
            _player.Add_Cart_Stack(transform);
        }


        if (other.CompareTag("Changer"))
        {
            ChangeCart();
            _player.PopUp_Text(300);
        }


        if (other.CompareTag("Enemy"))
        {
            if (isConnect == true)
            {
                if (other.GetComponent<Enemy>().enemytype == Enemy.EnemyType.Drop)
                {
                    //    Debug.Log("Drop");
                    _player.Drop_Cart_Stack(isMiddle: true, _num: _index);
                    //ConstantForce _force = transform.gameObject.AddComponent<ConstantForce>();
                    //_force.force = Vector3.up * -30f;
                }

                else if (other.GetComponent<Enemy>().enemytype == Enemy.EnemyType.Pool)
                {
                }
                else
                {
                    //Debug.Log("base");
                    _player.Lose_Cart_Stack(isMiddle: true, _num: _index);
                }
            }
        }
    }

    public void ChangeCart()
    {
        Game_Manager.instance.Vibe(1);
        _num++;
        if (_num > 2) _num = 2;

        for (int i = 0; i < 3; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(_num).gameObject.SetActive(true);
        TweenScale();


    }

    public void TweenScale()
    {

        DOTween.Sequence().Append(transform.DOScale(Vector3.one * 1.5f, 0.1f).SetEase(Ease.Linear))
            .Append(transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear));
    }


    public void Disconnect()
    {
        isReady = false;
        isConnect = false;

        StartCoroutine(Cor_Ready());

        IEnumerator Cor_Ready()
        {

            yield return new WaitForSeconds(0.6f);

            isReady = true;
        }
    }



}
