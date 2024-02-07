using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;



public class Enemy : MonoBehaviour
{


    public int LoseValue = 5;

    public enum EnemyType
    {
        Bar,
        Door,
        Horizontal_Gear,
        Fix_Gear,
        Horizontal_Bar,
        Pool,
        Drop
    }
    public EnemyType enemytype;

    public Vector2 MinMax_Delay = new Vector2(0f, 3f);

    [SerializeField] bool isCol = false;

    // ------------------------------------

    int startlayer_num;


    // -------------------------------------

    private void Awake()
    {
        startlayer_num = gameObject.layer;
        isCol = false;

    }

    private void Start()
    {
        StartCoroutine(Cor_Start());


        IEnumerator Cor_Start()
        {


            switch (enemytype)
            {
                case EnemyType.Bar:
                    transform.DORotate(new Vector3(0f, 360f, 0f), 2f, RotateMode.FastBeyond360)
                             .SetEase(Ease.Linear).SetRelative(true).SetLoops(-1, LoopType.Restart);

                    break;

                case EnemyType.Door:
                    yield return new WaitForSeconds(Random.Range(MinMax_Delay.x, MinMax_Delay.y));
                    transform.DOLocalMoveY(-8f, 2f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

                    break;

                case EnemyType.Horizontal_Gear:
                    yield return new WaitForSeconds(Random.Range(MinMax_Delay.x, MinMax_Delay.y));
                    transform.DORotate(new Vector3(0f, 360f, 0f), 2f, RotateMode.FastBeyond360)
                              .SetEase(Ease.Linear).SetRelative(true).SetLoops(-1, LoopType.Restart);

                    transform.DOLocalMoveX(6.5f, 2f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

                    break;

                case EnemyType.Fix_Gear:
                    transform.DORotate(new Vector3(0f, 0f, 360f), 2f, RotateMode.FastBeyond360)
                              .SetEase(Ease.Linear).SetRelative(true).SetLoops(-1, LoopType.Restart);
                    break;

                case EnemyType.Horizontal_Bar:
                    yield return new WaitForSeconds(Random.Range(MinMax_Delay.x, MinMax_Delay.y));
                    transform.DORotate(new Vector3(0f, 360f, 0f), 2f, RotateMode.FastBeyond360)
                              .SetEase(Ease.Linear).SetRelative(true).SetLoops(-1, LoopType.Restart);

                    transform.DOLocalMoveX(6.5f, 2f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
                    break;

                case EnemyType.Drop:
                    GetComponent<Renderer>().enabled = false;
                    break;

                default:

                    break;
            }
        }


    }



    [Button]
    public void ChangeObj()
    {
        Awake();
        transform.localScale = Vector3.one;

    }

    public int Hit_Player()
    {
        gameObject.layer = LayerMask.NameToLayer("NonCol");


        return LoseValue;
    }




}