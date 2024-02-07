using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public float Floating_Time = 1f;
    public float Floating_Height = 2f;
    public float Cost = 10;

    [SerializeField] Text _text;

    private void Awake()
    {
        _text = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();

    }



    private void Start()
    {
        StartCoroutine(Cor_Floating());
        //_text.text = string.Format("$ {0:N0}", Cost);
        _text.text = string.Format("+{0}", Cost);
    }


    IEnumerator Cor_Floating()
    {
        transform.DOMoveY(transform.position.y + Floating_Height, Floating_Time);
        transform.DOMoveZ(transform.position.z + 0.1f, 0.1f);
        Color _color = _text.color;
        _text.DOColor(new Vector4(_color.r, _color.g, _color.b, 0.8f), Floating_Time);
        yield return new WaitForSeconds(Floating_Time);

        //gameObject.SetActive(false);
        Destroy(this.gameObject);


    }
}
