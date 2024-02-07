using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class CrushBlock_Parent : MonoBehaviour
{
    [Title("Exlposion value")]
    public float Scope_value = 1.5f;
    public Text Scope_Text;
    public Vector3 Explosion_Pos;
    public float Explosion_Power;
    public float Explosion_Radius;

    public bool isDrawGizmos = true;
    public Color _Color = new Vector4(0.9f, 0.2f, 0.2f, 0.8f);
    public Color Block_Color;
    [SerializeField] bool isExplosion = false;
    [SerializeField] int lose_count = 5;
    // =======================
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            Material _mat = Instantiate(transform.GetChild(i).GetComponent<MeshRenderer>().material);
            _mat.color = Block_Color;
            transform.GetChild(i).GetComponent<MeshRenderer>().material = _mat;
        }
        //StartCoroutine(Cor_Update());
        Scope_Text.text = string.Format("X {0}", Scope_value);
    }

    //IEnumerator Cor_Update()
    //{
    //    while (true)
    //    {
    //        yield return null;

    //        if (Input.GetKeyDown(KeyCode.Q))
    //        {
    //            Explosion_Cube();
    //        }

    //    }
    //}

    //public void Explosion_Cube()
    //{
    //    if (isExplosion == false)
    //    {
    //        isExplosion = true;
    //        if (Game_Manager.instance.Player.GetComponent<Player_Move>().Lose_Cart(lose_count))
    //        {
    //            Game_Manager.instance.Player.GetComponent<Player_Move>().Money_scope = Scope_value;
    //            Scope_Text.text = null;
    //            Game_Manager.instance.Vibe(3);
    //            for (int i = 0; i < transform.childCount; i++)
    //            {
    //                transform.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    //            }
    //            Collider[] Colls = Physics.OverlapSphere(transform.position + Explosion_Pos, Explosion_Radius);
    //            foreach (Collider _col in Colls)
    //            {
    //                if (_col.transform.CompareTag("CrushBlock"))
    //                    _col.GetComponent<Rigidbody>().AddExplosionForce(Explosion_Power, transform.position + Explosion_Pos, Explosion_Radius);
    //            }
    //        }

    //    }
    //}
    public void Explosion_Cube2()
    {
        if (isExplosion == false)
        {
            isExplosion = true;
            if (Game_Manager.instance.Player.GetComponent<Player_Move>().Cart_List_cpi.Count > 0)
            {
                Game_Manager.instance.Player.GetComponent<Player_Move>().Lose_Cart_Stack(1);
                //Game_Manager.instance.Player.GetComponent<Player_Move>().Money_scope = Scope_value;

                Scope_Text.text = null;
                Game_Manager.instance.Vibe(3);
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                }
                Collider[] Colls = Physics.OverlapSphere(transform.position + Explosion_Pos, Explosion_Radius);
                foreach (Collider _col in Colls)
                {
                    if (_col.transform.CompareTag("CrushBlock"))
                        _col.GetComponent<Rigidbody>().AddExplosionForce(Explosion_Power, transform.position + Explosion_Pos, Explosion_Radius);
                }
            }
            else
            {
                Game_Manager.instance.Player.GetComponent<Player_Move>().Finish(true);
            }

        }
    }

    private void OnDrawGizmos()
    {
        if (isDrawGizmos)
        {
            Gizmos.color = _Color;
            Gizmos.DrawSphere(transform.position + Explosion_Pos, Explosion_Radius);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Player") || other.CompareTag("EndingCart"))
        //{
        //    Explosion_Cube();
        //}

        //else
        if (other.CompareTag("Cart") || other.CompareTag("Player"))
        {
            Explosion_Cube2();
        }
    }

}
