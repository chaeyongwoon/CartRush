using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Adder : MonoBehaviour
{
    public int _Num = 0;

    Cart_Parent _cart;
    GameObject _product;
    public enum Product_State
    {
        Apple,
        Banana,
        PeanutButter,

        Melon,
        Grape,
        Lemon,

        Milk,
        Sprite,
        Coke,

        Box1,
        Box2,
        Box3,

        Box4,
        Box5,
        Box6,

        Laptop,
        Phone,
        Monitor

    }
    public Product_State state;

    [SerializeField] GameObject _TempProduct;
    [SerializeField] Mesh ParticleMesh;
    private void Start()
    {
        state = (Product_State)Game_Manager.instance.Product_Group[_Num];
        _TempProduct = Resources.Load("Product_Group/" + state.ToString() + "_Group", typeof(GameObject)) as GameObject;
        ParticleMesh = _TempProduct.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
        transform.GetChild(0).GetComponent<ParticleSystemRenderer>().mesh = ParticleMesh;
        Text _nameText = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>();
        _nameText.text = string.Format("{0}", state.ToString());
        _nameText.fontSize = 80 - ((_nameText.text.ToString().Length - 5) * 6);

        Text _nameText2 = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>();
        _nameText2.text = string.Format("{0}", state.ToString());
        _nameText2.fontSize = 80 - ((_nameText.text.ToString().Length - 5) * 6);


    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cart"))
        {
            _cart = other.GetComponent<Cart_Parent>();
            if (_cart.Product_Level < 3)
            {
                _product = Instantiate(_TempProduct);
                _product.transform.SetParent(_cart.transform);
                _product.transform.localPosition = Vector3.up * _cart.Product_Level;
                _cart.TweenScale();
                _cart.Product_Level++;
                Game_Manager.instance.Player.GetComponent<Player_Move>().PopUp_Text(100);
            }
        }

        if (other.CompareTag("Player"))
        {

            Player_Move _cart = other.GetComponent<Player_Move>();
            if (_cart.Product_Level < 3)
            {
                _product = Instantiate(_TempProduct);
                _product.transform.SetParent(_cart.Products_Group);
                _product.transform.localPosition = Vector3.up * _cart.Product_Level;
                _cart.TweenScale();
                _cart.Product_Level++;
                other.GetComponent<Player_Move>().PopUp_Text(100);
            }
        }
    }


}
