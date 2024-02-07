using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rot_Obj : MonoBehaviour
{

    public Vector3 _rot = new Vector3(0f, 0f, -360f);
    public float Rot_Time = 4f;

    void Start()
    {
        transform.DORotate(_rot, Rot_Time, RotateMode.FastBeyond360)
           .SetEase(Ease.Linear).SetRelative(true).SetLoops(-1, LoopType.Restart);
    }


}
