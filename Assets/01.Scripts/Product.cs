using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Product : MonoBehaviour
{
    public bool isCol = false;
    public bool ischeck = false;

    [SerializeField] float Origin_Size = 1f;
    public float Height = 0.5f;
    [SerializeField] float Start_height;
    public float Interval = 0.7f;
    public bool isWave = false;
    public int Cost;


    // ==================================



    Transform Origin_Parent;
    Sequence _sequence;
    Rigidbody _rigid;
    public bool Use_String = false;

    [HideIf("Use_String")] public Mesh[] ProductMesh;
    [ShowIf("Use_String")] public string Base_Path = "Mesh/Product/";
    [ShowIf("Use_String")] public string[] ProductMesh_Path; // 

    public enum ProductType
    {
        Apple,
        Banana,
        Grape,
        Lemon,
        Melon,
        Box_B1,
        Box_B2,
        Box_B3,
        Box_S1,
        Box_S2,
        Box_S3,
        Coke,
        Milk,
        PeanutButter,
        Sprite

    }
    public ProductType _producttype;
    public enum ColliderType
    {
        Sphere,
        Box,
        Capsule,
        Mesh
    }
    public ColliderType _colType;
    public int[] ProductCost;


    // ------------------------
    MeshFilter _meshfilter;
    MeshCollider _meshcollider;
    SphereCollider _sphereCol;
    BoxCollider _boxCol;
    CapsuleCollider _CapsuleCol;
    int startlayer_num;
    // -----------------------
    private void Awake()
    {
        _sequence = DOTween.Sequence();
        Start_height = transform.position.y;
        Origin_Size = transform.localScale.x;
        Origin_Parent = transform.parent;
        _rigid = GetComponent<Rigidbody>();
        SetComponent();
    }
    void SetComponent()
    {
        startlayer_num = gameObject.layer;
        _meshfilter = GetComponent<MeshFilter>();

    }

    private void OnEnable() => Resetting();
    public void Resetting()
    {
        this.gameObject.layer = startlayer_num;
        isCol = false;
        transform.SetParent(Origin_Parent);
        transform.localScale = Vector3.one * Origin_Size;
        GetComponent<ConstantForce>().force = Vector3.zero;
        _rigid.constraints = RigidbodyConstraints.FreezePositionZ;
        if (isWave == true)
        {
            Wave();
        }
        _rigid.useGravity = false;
        //_rigid.isKinematic = true;

    }

    private void Start()
    {
        GetComponent<BoxCollider>().size = GetComponent<BoxCollider>().size + new Vector3(1f, 3f, 1f) * 2f + Vector3.one * Game_Manager.instance.Player.GetComponent<Player_Move>().Magnet_Level;
        GetComponent<BoxCollider>().isTrigger = true;

        switch (_producttype)
        {
            case ProductType.Apple:
            case ProductType.Banana:
            case ProductType.Grape:
            case ProductType.Lemon:
            case ProductType.Melon:

                _colType = ColliderType.Sphere;
                break;

            case ProductType.Coke:
            case ProductType.Sprite:
            case ProductType.PeanutButter:
                _colType = ColliderType.Capsule;
                break;

            case ProductType.Box_B1:
            case ProductType.Box_B2:
            case ProductType.Box_B3:
            case ProductType.Box_S1:
            case ProductType.Box_S2:
            case ProductType.Box_S3:
            case ProductType.Milk:
                _colType = ColliderType.Box;
                break;
        }


        switch (_colType)
        {
            case ColliderType.Sphere:
                _sphereCol = gameObject.AddComponent<SphereCollider>();
                _sphereCol.enabled = true;
                _sphereCol.radius = _sphereCol.radius * 0.9f < 0.6f ? 0.6f : _sphereCol.radius * 0.9f;

                break;

            case ColliderType.Box:
                _boxCol = gameObject.AddComponent<BoxCollider>();
                _boxCol.enabled = true;

                break;

            case ColliderType.Capsule:
                _CapsuleCol = gameObject.AddComponent<CapsuleCollider>();
                _CapsuleCol.enabled = true;
                break;

            case ColliderType.Mesh:
                _meshcollider = gameObject.AddComponent<MeshCollider>();
                _meshcollider.enabled = true;
                _meshcollider.sharedMesh = _meshfilter.mesh;
                _meshcollider.convex = true;
                break;

        }
    }

    public void Wave()
    {
        _sequence.Append(transform.DOMoveY(Start_height + Height, Interval)
            .SetEase(Ease.Linear))
            .SetLoops(-1, LoopType.Yoyo);

    }

    public void Coll_Cart(Transform _parent)
    {
        transform.SetParent(_parent);

        _sequence.Kill(); // wave 트윈 삭제

        isCol = true;
        _rigid.useGravity = true;
        _rigid.constraints = RigidbodyConstraints.None;

    }



    [HorizontalGroup("Split", 0.5f)]
    [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
    public void ChangeObj()
    {
        SetComponent();

        int num = (int)_producttype;

        _meshfilter.sharedMesh = Use_String
            ? Resources.Load("Mesh/Product/" + ProductMesh_Path[num], typeof(Mesh)) as Mesh
            : ProductMesh[num];
        //_meshcollider.sharedMesh = Use_String
        //    ? Resources.Load("Mesh/Product/" + ProductMesh_Path[num], typeof(Mesh)) as Mesh
        //    : ProductMesh[num];
        Cost = ProductCost[num];
        if (num >= 5 && num <= 10)
        {
            _colType = ColliderType.Box;
        }

        DestroyImmediate(GetComponent<BoxCollider>());
        gameObject.AddComponent<BoxCollider>();


    }
    [HorizontalGroup("Split", 0.5f)]
    [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
    public void RandomChange()
    {
        SetComponent();

        int num = Random.Range(0, System.Enum.GetValues(typeof(ProductType)).Length);

        _meshfilter.sharedMesh = Use_String
            ? Resources.Load(Base_Path + ProductMesh_Path[num], typeof(Mesh)) as Mesh
            : ProductMesh[num];
        //_meshcollider.sharedMesh = Use_String
        //    ? Resources.Load(Base_Path + ProductMesh_Path[num], typeof(Mesh)) as Mesh
        //    : ProductMesh[num];
        Cost = ProductCost[num];
        if (num >= 5 && num <= 10)
        {
            _colType = ColliderType.Box;
        }
        DestroyImmediate(GetComponent<BoxCollider>());
        gameObject.AddComponent<BoxCollider>();
    }

    public void SetChange(int num = 0)
    {
        _meshfilter = GetComponent<MeshFilter>();
        _meshfilter.sharedMesh = Use_String ? Resources.Load("Mesh/Product/" + ProductMesh_Path[num], typeof(Mesh)) as Mesh : ProductMesh[num];

        Cost = ProductCost[num];
        if (num >= 5 && num <= 10)
        {
            _colType = ColliderType.Box;
        }

        DestroyImmediate(GetComponent<BoxCollider>());
        gameObject.AddComponent<BoxCollider>();
    }

    public void SetBigBoxcol()
    {
        GetComponent<BoxCollider>().size = Vector3.one * 6f;
    }



}
