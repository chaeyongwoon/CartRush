using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ending_Ground : MonoBehaviour
{

    [SerializeField] Transform[] Ground_Trans;
    [SerializeField] int _index;
    [SerializeField] int _count;
    [SerializeField] Player_Move _player;

    public Transform[] Block;

    private void Start()
    {
        _count = transform.childCount;

        Ground_Trans = new Transform[_count];

        for (int i = 0; i < _count; i++)
        {
            Ground_Trans[i] = transform.GetChild(i);
            Ground_Trans[i].GetComponent<Renderer>().enabled = false;
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player = Game_Manager.instance.Player.GetComponent<Player_Move>();

            _player.Cam_On(4);

            StartCoroutine(Cor_Ending());



            IEnumerator Cor_Ending()
            {
                _player._ending_ground = this;
                for (int i = _player.Cart_List_cpi.Count - 1; i >= 0; i++)
                {
                    Transform _cart = _player.Ending_Lose_Cart(this);

                    if (_cart != null)
                    {
                        _cart.gameObject.layer = LayerMask.NameToLayer("NonCol");
                        _cart.DOJump(Ground_Trans[_index].position, 1.5f, 1, 0.2f).SetEase(Ease.Linear);
                        _index++;
                        if (_index >= _count)
                        {
                            _index = 0;
                        }
                    }
                    yield return new WaitForSeconds(0.1f);
                }





            }



        }
    }


    public void Ending_Block()
    {
        StartCoroutine(Cor_Block());

        IEnumerator Cor_Block()
        {
            //Debug.Log("Cor_Block");
            yield return null;
            for (int i = 0; i < _player._ending_Count; i++)
            {
                Block[i].DOMoveY(5 * i, 1f).SetEase(Ease.OutCubic);
            }
            _player.transform.DOMoveY((_player._ending_Count - 1) * 5f + 7f, 1f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                foreach (GameObject _obj in _player.GetComponent<Player_Move>().Spray_Particle)
                {
                    _obj.SetActive(true);
                }
            });
            //yield return new WaitForSeconds(1f);
            //Debug.Log("Call Ending_Panel");
            Game_Manager.instance._uiManager.Ending_Panel(true);
        }
    }

}
