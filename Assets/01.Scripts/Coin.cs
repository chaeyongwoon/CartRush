using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    public float Height = 0.5f;
    [SerializeField] float Start_height;
    public float Interval = 0.7f;
    public bool isWave = false;
    Sequence _sequence;
    public int value =5;

    // Start is called before the first frame update
    void Start()
    {
        _sequence = DOTween.Sequence();
        Start_height = transform.position.y;

        if (isWave == true)
        {
            Wave();
        }
    }

    public void Wave()
    {
        _sequence.Append(transform.DOMoveY(Start_height + Height, Interval).SetEase(Ease.Linear))            
            .SetLoops(-1, LoopType.Yoyo);

        transform.DORotate(new Vector3(-90f, 180f, 0f), Interval).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);

    }

}
