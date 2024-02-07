using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy_Person : MonoBehaviour
{

    public int LoseValue = 5;


    public float Check_Distance = 5f;
    public float Speed = 10f;
    [SerializeField] bool isCol = false;
    public bool isHit = false;

    [SerializeField] Transform _player;
    int startlayer_num;
    [SerializeField] Transform[] _arr;

    Animator _animator;

    public enum State
    {
        Idle,
        Run,
        Still,
        Hit
    }
    public State enemyState;


    // ================================
    [SerializeField] Transform Product_Pos;
    [SerializeField] bool isDraw = false;
    public Color _color;
    [SerializeField] float _distance;
    public float Col_limit_distance = 4f;
    Vector3 _tempPos;

    [SerializeField] Rigidbody[] _rigTest;
    // ==========================================
    private void Awake()
    {
        startlayer_num = gameObject.layer;
        isCol = false;
        _animator = GetComponent<Animator>();
        _animator.SetBool("Run", false);
        _animator.SetBool("Hug", false);
        if (Product_Pos == null)
        {
            Product_Pos = transform.GetChild(2).transform;
        }

    }

    private void Start()
    {
        _player = Game_Manager.instance.Player.transform;
        StartCoroutine(CheckPlayerDistance());
        int a = Random.Range(0, 2);
        _tempPos = a == 0 ? new Vector3(-4.5f, 0f, -5f) : new Vector3(4.5f, 0f, -5f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 50f);
        }
    }

    IEnumerator CheckPlayerDistance()
    {
        yield return null;
        while (true)
        {
            yield return null;
            _distance = Vector3.Distance(transform.position, _player.position);
            if (_distance <= Check_Distance && isHit == false)
            {

                if (isCol == false)
                {
                    _animator.SetBool("Run", true);

                    transform.LookAt(new Vector3(_player.position.x, transform.position.y, _player.position.z));
                    transform.Translate(transform.forward * Speed * Time.deltaTime, Space.World);
                    if (Vector3.Distance(transform.position, _player.position) <= Col_limit_distance)
                    {
                        isCol = true;
                        //if (_player.GetComponent<Player_Move>().state != Player_Move.State.Fever)
                        //{
                        //    _animator.SetBool("Hug", true);
                        //    _arr = _player.GetComponent<Player_Move>().StillProduct(LoseValue);
                        //    foreach (Transform _obj in _arr)
                        //    {
                        //        if (_obj == null) break;
                        //        _obj.SetParent(transform);
                        //        _obj.DOLocalMove(Product_Pos.localPosition, 0.2f);
                        //    }
                        //}
                    }

                    if (transform.position.z < _player.transform.position.z - 10)
                    {
                        break;
                    }
                }
                else
                {


                    transform.LookAt(transform.position + _tempPos);
                    transform.Translate(transform.forward * Speed * Time.deltaTime, Space.World);
                }
            }
        }


    }
    private void OnDrawGizmosSelected()
    {
        if (isDraw)
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(transform.position, Check_Distance);
        }
    }

    ////////////////////////////////////////////
    //if (isCol == false)
    //{
    //    if (Vector3.Distance(transform.position, _player.position) <= Check_Distance)
    //    {
    //        isCol = true;
    //        _animator.SetBool("Run", true);
    //        transform.SetParent(_player);
    //        Vector3 _tempDes = Random.Range(0, 2) == 0 ? new Vector3(-5f, 0f, Check_Distance) : new Vector3(5f, 0f, Check_Distance);
    //        DOTween.Sequence().Append(transform.DOLocalMove(Vector3.forward * 2f, 1.5f)).SetEase(Ease.Linear)
    //        .AppendCallback(() =>
    //        {
    //            _arr = _player.GetComponent<Player_Move>().StillProduct(LoseValue);
    //            foreach (Transform _obj in _arr)
    //            {
    //                _obj.SetParent(transform);
    //                _obj.DOLocalMove(Vector2.up, 0.2f);
    //            }
    //        })
    //        .Append(transform.DOLocalMove(_tempDes, 1.5f)).SetEase(Ease.Linear)
    //        .OnComplete(() =>
    //        {
    //            transform.SetParent(null);
    //        });


    //    }
    //}

    public void Hit(Vector3 Power)
    {
        if (isHit == false)
        {
            gameObject.layer = LayerMask.NameToLayer("NonCol");
            isHit = true;
            _animator.SetBool("Run", false);
            _animator.SetBool("Hug", false);
            _animator.enabled = false;
            foreach (Rigidbody _rig in _rigTest)
            {
                _rig.isKinematic = false;
                //_rig.useGravity = true;
                //_rig.AddForce(Power);
                _rig.gameObject.layer = LayerMask.NameToLayer("NonCol");
            }
            _rigTest[0].useGravity = true;
            _rigTest[0].AddForce(Power);

            //GetComponent<Rigidbody>().AddForce(Power);
            //GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    //public void On_Rigid()
    //{
    //    foreach (Rigidbody _rig in _rigTest)
    //    {
    //        _rig.isKinematic = false;
    //        _rig.useGravity = true;
    //    }
    //}

}
