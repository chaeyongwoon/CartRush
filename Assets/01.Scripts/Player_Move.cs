using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using MondayOFF;





public class Player_Move : MonoBehaviour
{
    // === Private ======
    Vector3 Init_Pos;
    Vector3 Start_Pos;
    Vector3 Start_Point, End_Point;

    [Title("Data")]
    public int Stage_Level;
    public int Money_Level;
    public int Fever_Level;
    public int Magnet_Level;

    public int Max_Stage_Level = 10;


    [Title("Input & Cart Value")]
    public float Move_Speed = 10f;
    public float Sense = 0.05f;
    public float Move_x;
    public float BaseLimit_x = 4.2f;
    public float CurrentLimit_x_left = -4.2f;
    public float CurrentLimit_x_right = 4.2f;
    public float Power = 100f;
    public State state;
    public float Stun_Delay = 0.5f;
    public float DecreaseSpeed = 1f;
    public Vector3 Explosion_DirValue;
    [SerializeField] float Ending_Speed = 0;
    public float Max_Speed = 50f;
    [SerializeField] float _Speed;
    public float Money;
    public float totalsell_money;
    public float Money_scope = 1.5f;
    public ParticleSystem[] _effect;


    [Title("Cart Value")]
    public float Hit_Jump_Power = 100f;
    public float Hit_Power = 100f;
    public float Hit_Recovery_interval = 0.5f;
    public float Hit_Jump_Duration = 0.5f;



    [Space(10)]
    [Title("Tween & Product_Value")]
    public Vector3 Offset = new Vector3(0f, 0f, -1.5f);
    public float Jump_Power = 3f;
    public float Jump_duration = 0.5f;
    public float InProducts_Size = 0.5f;
    public float InProducts_ConstantForce_y = -10f;

    [Space(10)]
    [Title("Bounce")]
    public float Bounce_Power_Min = 100f;
    public float Bounce_Power_Max = 300f;
    public float Bounce_Interval = 3f;

    [Space(10)]
    [Title("Count")]
    public int Max_Count = 200;
    public int Current_Count = 0;
    public float Weight = 0f;
    public Text Weight_Text;
    public int Product_Level = 0;
    public int Cart_Num = 0;


    [Space(10)]
    [Title("Test")]
    public bool Connect_Cart;
    public int Cart_Count;
    public float Cart_Interval = 0.5f;
    public float Cart_Move_Interval = 0.2f;
    [SerializeField] float temp_speed;
    public bool isReverse = false;
    public float CartStack_Speed = 10f;
    public float CartStack_Interval = 2.5f;
    public Transform Products_Group;

    public enum State
    {
        Ready,
        Wait,
        Run,
        Hit,
        Ending1,
        Ending2,
        Finish,
        Auto
    }

    // ---------------------------
    [SerializeField] bool isClick = false;

    public Queue<Transform> Products_Queue;
    public Stack<Transform> Cart_Stack;
    [SerializeField] public Animator[] _animator;
    [SerializeField] float Ani_Speed = 1f;
    public GameObject[] Cam_List;
    [SerializeField] GameObject Current_Stage;
    [SerializeField] bool isSlide = false;
    public List<Transform> Cart_List_cpi;
    [SerializeField] Transform curBodyPart;
    [SerializeField] Transform PrevBodyPart;
    public Ending_Ground _ending_ground;
    public int _ending_Count;

    public GameObject[] Spray_Particle;

    /// ////////////////////////////////////

    private void Awake()
    {
        Init_Pos = transform.position;
        Products_Queue = new Queue<Transform>();
        Cart_Stack = new Stack<Transform>();
        Cart_List_cpi = new List<Transform>();

        //StartCoroutine(Cor_Bounce());

        LoadData();
        Game_Manager.instance.SetProductGroup();
        SetStage();

    }

    void Start()
    {
        Init_Pos = transform.position;
        Start_Pos = transform.position;

        foreach (ParticleSystem eff in _effect)
        {
            eff.Stop();
        }


    }

    void Update()
    {
        Move();
        Move_Cart();

        if (state == State.Ending1)
        {
            transform.Translate(transform.forward * Ending_Speed * Time.deltaTime);
        }
        else if (state == State.Auto)
        {
            transform.Translate(transform.forward * Move_Speed * 0.7f * Time.deltaTime);
        }
        else if (state == State.Hit && isSlide)
        {
            //transform.Translate(Vector3.forward * Move_Speed * 0.5f * Time.deltaTime);
            transform.position += Vector3.forward * Move_Speed * 0.5f * Time.deltaTime;
        }


        switch (state)
        {
            case State.Run:
                _Speed = Move_Speed;
                break;
            case State.Ending1:
            case State.Ending2:
                _Speed = 0f;
                break;

            default:

                break;
        }



        if (Input.GetKey(KeyCode.Q))
        {
            CartStack_Interval += 0.1f;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            CartStack_Interval -= 0.1f;
        }
        //}
    }

    public void Move()
    {
        _animator[0].speed = Ani_Speed;


        if (Input.GetMouseButtonDown(0))
        {
            isClick = true;
            Start_Point = Input.mousePosition;
            End_Point = Input.mousePosition;

            Start_Pos = transform.position;

            if (state == State.Wait)
            {
                state = State.Run;
                _animator[0].SetBool("Run", true);
                _effect[0].Play();
                _effect[1].Play();


            }

        }
        else if (Input.GetMouseButton(0))
        {
            if (state == State.Run || state == State.Ending1)
            {
                End_Point = Input.mousePosition;

                switch (state)
                {
                    case State.Run:
                        _Speed = Move_Speed;
                        break;

                    case State.Ending1:
                    case State.Ending2:
                        _Speed = 0f;
                        break;

                    default:

                        break;
                }


                transform.Translate(transform.forward * _Speed * Time.deltaTime);

                Move_x = End_Point.x - Start_Point.x;
                if (isReverse)
                {
                    transform.position = new Vector3(Start_Pos.x - Move_x * Sense, transform.position.y, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(Start_Pos.x + Move_x * Sense, transform.position.y, transform.position.z);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isClick = false;
            if (state != State.Ending1 && state != State.Ending2 && state != State.Auto)
            {

                if (state == State.Run)
                {
                    state = State.Wait;
                    _animator[0].SetBool("Run", false);
                    _effect[0].Stop();
                    _effect[1].Stop();
                    _effect[2].Stop();
                    _effect[4].Stop();
                }

            }
        }


        if (transform.position.x > CurrentLimit_x_right)
        {
            transform.position = new Vector3(CurrentLimit_x_right, transform.position.y, transform.position.z);
            Start_Point = Input.mousePosition;
            Start_Pos = transform.position;
            Move_x = 0f;
        }
        else if (transform.position.x < CurrentLimit_x_left)
        {
            transform.position = new Vector3(CurrentLimit_x_left, transform.position.y, transform.position.z);
            Start_Point = Input.mousePosition;
            Start_Pos = transform.position;
            Move_x = 0f;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {

            case "GoalCart": // 카트 모으기 전 골 라ㅇㅣㄴ
                GoalCartCheck();
                break;

            case "GoalBlock": // 벽 부수기 전 라인
                GoalBlockCheck();

                break;


            case "EndingCart":
                if (Connect_Cart == false)
                {
                    other.gameObject.layer = LayerMask.NameToLayer("ColEndingCart");

                    Vector3 dir;
                    dir = new Vector3((other.transform.position.x - transform.position.x) * Explosion_DirValue.x,
                         Explosion_DirValue.y,
                         Explosion_DirValue.z);

                    other.GetComponent<Rigidbody>().AddForce(dir.normalized * Power);
                    other.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-360, 360f), Random.Range(-360, 360f), Random.Range(-360, 360f)));
                    other.GetComponent<ConstantForce>().force = Vector3.up * InProducts_ConstantForce_y;
                    //LoseProduct(5);
                }
                else
                {
                    Game_Manager.instance.Vibe(1);
                    other.transform.SetParent(transform.GetChild(0));
                    other.gameObject.layer = LayerMask.NameToLayer("NonCol");
                    other.transform.localRotation = Quaternion.identity;

                    other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    Cart_Count++;
                    Cart_Stack.Push(other.transform);
                    other.transform.DOLocalMove(new Vector3(0f, 0f, Cart_Interval * Cart_Count), Cart_Move_Interval).SetEase(Ease.Linear);
                }
                break;

            case "Enemy":

                if (other.GetComponent<Enemy>())
                {
                    if (other.GetComponent<Enemy>().enemytype != Enemy.EnemyType.Pool)
                    {
                        Game_Manager.instance.Vibe(3);
                        Hit(false);

                        Lose_Cart_Stack(Cart_List_cpi.Count, false);

                    }
                    else
                    {
                        Game_Manager.instance.Vibe(3);
                        other.GetComponent<Enemy>().Hit_Player();
                        Hit(true);
                    }
                }


                break;


            case "CPI_Stop_Block":
                //state = State.Ready;

                _animator[0].SetBool("Run", false);
                _effect[0].Stop();
                _effect[1].Stop();
                _effect[2].Stop();
                _effect[4].Stop();

                DOTween.To(() => Ending_Speed, x => Ending_Speed = x, 0f, 1f).SetEase(Ease.OutBack);


                break;

            case "Cart":
                if (other.GetComponent<Cart_Parent>().isConnect == false && other.GetComponent<Cart_Parent>().isReady)
                {
                    Add_Cart_Stack(other.transform);

                }

                break;

            case "Changer":

                ChangeCart();
                PopUp_Text(300);
                break;



            default:
                break;
        }


    }

    public void ChangeCart()
    {
        Cart_Num++;
        if (Cart_Num > 2) Cart_Num = 2;

        for (int i = 0; i < 3; i++)
        {
            transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(0).GetChild(Cart_Num).gameObject.SetActive(true);


        TweenScale();


    }



    void GoalCartCheck()
    {
        state = State.Ending1;
        _animator[0].SetBool("Run", true);
        _effect[0].Play();
        _effect[1].Play();
        _effect[2].Play();
        _effect[3].Play();
        _effect[4].Play();


        Ending_Speed = Max_Speed; // Products_Queue.Count * 1f > Max_Speed ? Max_Speed : Products_Queue.Count;
        Game_Manager.instance._uiManager.InGame_Panel.SetActive(false);

        Current_Count = Products_Queue.Count;
        totalsell_money = 0;
        Product _tempProduct;
        while (Products_Queue.Count > 0)
        {
            _tempProduct = Products_Queue.Dequeue().GetComponent<Product>();
            totalsell_money += _tempProduct.Cost;
            _tempProduct.gameObject.SetActive(false);
        }
        GameObject _moneyPopupText2;

        _moneyPopupText2 = Instantiate(Resources.Load("UI/Money_Popup", typeof(GameObject)) as GameObject);
        _moneyPopupText2.GetComponent<FloatingText>().Cost = totalsell_money * (1 + Money_Level * 0.05f);
        /*Money*/
        totalsell_money += (totalsell_money * (1 + Money_Level * 0.05f));
        _moneyPopupText2.transform.SetParent(transform);
        _moneyPopupText2.transform.position = transform.position + Vector3.up * 0.5f;



    }

    void GoalBlockCheck()
    {
        state = State.Ending2;
        Ani_Speed = 2f;
        Cam_On(2);
        transform.DOMoveX(0f, 0.5f);
    }



    public void Hit(bool isPool)
    {
        if (state != State.Hit)
        {
            state = State.Hit;
            _animator[0].SetBool("Run", false);
            _effect[0].Stop();
            _effect[1].Stop();
            _effect[2].Stop();
            _effect[4].Stop();
            if (!isPool)
            {
                Debug.Log("isNot Pool");
                transform.DOJump(transform.position + new Vector3(0f, 0f, -4f), Hit_Jump_Power, 0, Hit_Jump_Duration).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    Recovery(false);
                });
            }
            else
            {
                isSlide = true;
                DOTween.Sequence().Append(transform.DORotateQuaternion(Quaternion.Euler(new Vector3(0f, 90f, 0f)), 0.1f).SetEase(Ease.Linear))
                    .Append(transform.DORotateQuaternion(Quaternion.Euler(new Vector3(0f, 0f, 0f)), 0.1f).SetEase(Ease.Linear))
                    .Append(transform.DORotateQuaternion(Quaternion.Euler(new Vector3(0f, -90f, 0f)), 0.1f).SetEase(Ease.Linear))
                    .Append(transform.DORotateQuaternion(Quaternion.Euler(new Vector3(0f, 180f, 0f)), 0.1f).SetEase(Ease.Linear))

                    .Append(transform.DORotateQuaternion(Quaternion.Euler(new Vector3(0f, 90f, 0f)), 0.1f).SetEase(Ease.Linear))
                    .Append(transform.DORotateQuaternion(Quaternion.Euler(new Vector3(0f, 0f, 0f)), 0.1f).SetEase(Ease.Linear))
                    .Append(transform.DORotateQuaternion(Quaternion.Euler(new Vector3(0f, -90f, 0f)), 0.1f).SetEase(Ease.Linear))
                    .Append(transform.DORotateQuaternion(Quaternion.Euler(new Vector3(0f, 180f, 0f)), 0.1f).SetEase(Ease.OutCubic))
                    .OnComplete(() =>
                    {
                        Recovery(true);
                        transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                    });
            }
        }
    }





    public void Recovery(bool isPool)
    {
        if (state != State.Finish)
        {
            if (isClick)
            {
                state = State.Run;
                _animator[0].SetBool("Run", true);
                _effect[0].Play();
                _effect[1].Play();

            }
            else
            {
                state = State.Wait;
                _animator[0].SetBool("Run", false);
                _effect[0].Stop();
                _effect[1].Stop();
                _effect[2].Stop();
                _effect[4].Stop();

            }

            isSlide = false;
            Start_Point = Input.mousePosition;
            Start_Pos = transform.position;
        }

    }

    public void Cam_On(int _num)
    {
        for (int i = 1; i < Cam_List.Length; i++)
        {
            Cam_List[i].SetActive(false);
        }
        Cam_List[_num].SetActive(true);
    }

    void LoadData()
    {
        if (PlayerPrefs.HasKey("Stage_Level"))
        {
            Stage_Level = PlayerPrefs.GetInt("Stage_Level");
            //Fever_Level = PlayerPrefs.GetInt("Fever_Level");
            //Magnet_Level = PlayerPrefs.GetInt("Magnet_Level");
            Money_Level = PlayerPrefs.GetInt("Money_Level");
            Money = PlayerPrefs.GetFloat("Money");
            Game_Manager.instance.Product_Group_Num = PlayerPrefs.GetInt("Product_Group_Num");


            Debug.Log("Load PlayerPrefs Data");
        }
        else
        {
            Stage_Level = 1;
            //Fever_Level = 0;
            //Magnet_Level = 0;
            Money_Level = 0;
            Game_Manager.instance.Product_Group_Num = 0;
            Debug.Log("check");
            PlayerPrefs.SetInt("Skin_0", 1);
            PlayerPrefs.SetInt("Skin_1", 0);
            PlayerPrefs.SetInt("Skin_2", 0);
            PlayerPrefs.SetInt("Skin_3", 0);
            PlayerPrefs.SetInt("Skin_4", 0);
            PlayerPrefs.SetInt("Skin_5", 0);
            PlayerPrefs.SetInt("Skin_6", 0);
            PlayerPrefs.SetInt("Skin_7", 0);

            SaveData();

        }
    }

    public void SaveData()
    {

        PlayerPrefs.SetInt("Stage_Level", Stage_Level);
        //PlayerPrefs.SetInt("Fever_Level", Fever_Level);
        //PlayerPrefs.SetInt("Magnet_Level", Magnet_Level);
        PlayerPrefs.SetInt("Money_Level", Money_Level);
        PlayerPrefs.SetFloat("Money", Money);
        PlayerPrefs.SetInt("Product_Group_Num", Game_Manager.instance.Product_Group_Num);


        Debug.Log("Save PlayerPrefs Data");


    }

    public void End_Treasure()
    {
        Cam_On(0);
        state = State.Finish;
        _animator[0].SetBool("Run", false);
        _effect[0].Stop();
        _effect[1].Stop();
        _effect[2].Stop();
        _effect[4].Stop();

        DOTween.Sequence().AppendInterval(1f)
            .AppendCallback(() =>
            {
                Game_Manager.instance._uiManager.Ending_Panel(true);


                SaveData();

            });
    }

    public void SetStage()
    {
        foreach (GameObject _obj in Spray_Particle)
        {
            _obj.SetActive(false);
        }

        if (Current_Stage != null)
        {
            Destroy(Current_Stage);
        }

        try
        {
            GameObject[] _trash = GameObject.FindGameObjectsWithTag("EndingCart");

            foreach (GameObject _obj in _trash)
            {
                Destroy(_obj);
            }

            _trash = GameObject.FindGameObjectsWithTag("Product");
            foreach (GameObject _obj in _trash)
            {
                Destroy(_obj);
            }

        }
        catch { }

        //Stage_Level = Stage_Level > Max_Stage_Level ? Stage_Level - Max_Stage_Level : Stage_Level;
        Stage_Level = Stage_Level < 1 ? Max_Stage_Level : Stage_Level;
        //Stage_Level = Stage_Level % Max_Stage_Level == 0 ? Max_Stage_Level : Stage_Level % Max_Stage_Level;
        int current_Level = Stage_Level % Max_Stage_Level == 0 ? Max_Stage_Level : Stage_Level % Max_Stage_Level;
        //Current_Stage = Instantiate(Resources.Load("Stage/Stage_" + current_Level, typeof(GameObject)) as GameObject);
        Current_Stage = Instantiate(Resources.Load("Stage_Stack/Stage_" + current_Level, typeof(GameObject)) as GameObject);
        Game_Manager.instance.Resetting();

        transform.position = Init_Pos;
        Ani_Speed = 1f;
        //Products_Queue.Clear();
        //Cart_Stack.Clear();
        Cart_Count = 0;



        state = State.Ready;
        _animator[0].SetBool("Run", false);
        Current_Count = 0;
        Cam_On(0);
        CurrentLimit_x_left = -BaseLimit_x;
        CurrentLimit_x_right = BaseLimit_x;
        Cart_List_cpi = new List<Transform>();


        try
        {
            Destroy(Products_Group.GetChild(0).gameObject);
            Destroy(Products_Group.GetChild(1).gameObject);
            Destroy(Products_Group.GetChild(2).gameObject);
        }
        catch { }
        //for (int i = 0; i < Products_Group.childCount; i++)
        //{
        //    Debug.Log(i);
        //    Debug.Log(Products_Group.GetChild(0).gameObject);
        //    Destroy(Products_Group.GetChild(0).gameObject);
        //}
        Product_Level = 0;
        Cart_Num = -1;
        ChangeCart();


        //MondayOFF.EventsManager.instance.TryStage(Stage_Level);
        //MondayOFF.EventTracker.TryStage(Stage_Level);
        EventTracker.TryStage(Stage_Level);

    }

    public void PopUp_Text(int _score)
    {
        GameObject PopupText;

        PopupText = Instantiate(Resources.Load("UI/Money_Popup", typeof(GameObject)) as GameObject);
        PopupText.GetComponent<FloatingText>().Cost = _score;

        PopupText.transform.SetParent(transform);
        PopupText.transform.position = transform.position + Vector3.up * 0.5f + Vector3.back * 0.5f;
        totalsell_money += _score;
    }


    public void Add_Cart_Stack(Transform _cart)
    {

        if (_cart.GetComponent<Cart_Parent>().isConnect == false && _cart.GetComponent<Cart_Parent>().isReady)
        {
            Game_Manager.instance.Vibe(1);
            _cart.GetComponent<Cart_Parent>().isConnect = true;
            _cart.GetComponent<Cart_Parent>()._index = Cart_List_cpi.Count;
            Cart_List_cpi.Add(_cart);
            _cart.transform.localScale = Vector3.zero;
            _cart.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.Linear);
            PopUp_Text(500);

        }


    }
    public void Lose_Cart_Stack(int _count = 1, bool isMiddle = false, int _num = 0)
    {
        if (!isMiddle) // player col
        {
            if (Cart_List_cpi.Count == 0)
            {
                state = State.Finish;
                Game_Manager.instance._uiManager.Ending_Panel(false);
                return;
            }

            for (int i = 0; i < _count; i++)
            {
                if (Cart_List_cpi.Count > 0)
                {
                    Transform _cart = Cart_List_cpi[Cart_List_cpi.Count - 1];
                    Cart_List_cpi.Remove(_cart);
                    //Cart_List_cpi.RemoveAt(Cart_List_cpi.Count - 1);
                    _cart.DOJump(_cart.transform.position + new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(7f, 9f))
                        , 1.5f, 1, 0.4f).SetEase(Ease.Linear)
                        .OnComplete(() => _cart.GetComponent<Cart_Parent>().isConnect = false);
                    totalsell_money -= 500;
                }

            }
        }
        else // cart middle col
        {
            for (int i = Cart_List_cpi.Count - 1; i >= _num; i--)
            {
                Transform _cart = Cart_List_cpi[i];
                Cart_List_cpi.Remove(_cart);

                _cart.GetComponent<Cart_Parent>().Disconnect();
                //Cart_List_cpi.RemoveAt(Cart_List_cpi.Count - 1);
                _cart.DOJump(_cart.transform.position + new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(7f, 9f))
                    , 1.5f, 1, 0.4f).SetEase(Ease.Linear);
                //.OnComplete(() => _cart.GetComponent<Cart_Parent>().isConnect = false);

            }
        }
    }
    public void Drop_Cart_Stack(int _count = 1, bool isMiddle = false, int _num = 0)
    {
        if (!isMiddle) // player col
        {
            for (int i = 0; i < _count; i++)
            {
                if (Cart_List_cpi.Count > 0)
                {
                    Transform _cart = Cart_List_cpi[Cart_List_cpi.Count - 1];
                    Cart_List_cpi.Remove(_cart);

                    _cart.GetComponent<Cart_Parent>().Disconnect();
                    _cart.GetComponent<Rigidbody>().isKinematic = false;
                    _cart.GetComponent<Rigidbody>().useGravity = true;

                }
            }
        }
        else // cart middle col
        {
            for (int i = Cart_List_cpi.Count - 1; i >= _num; i--)
            {
                Transform _cart = Cart_List_cpi[i];
                Cart_List_cpi.Remove(_cart);

                //Debug.Log(string.Format("\"{0}\" i:{1} , _num :{2}", _cart.name, i, _num));
                //if (i == _num)
                //{
                _cart.GetComponent<Rigidbody>().isKinematic = false;
                _cart.GetComponent<Rigidbody>().useGravity = true;
                //_cart.GetComponent<Cart_Parent>().isConnect = true;
                _cart.gameObject.layer = LayerMask.NameToLayer("NonCol");
                //}
                //else
                //{
                //    _cart.DOJump(_cart.transform.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(2f, 1f))
                //   , 1.5f, 1, 0.4f).SetEase(Ease.Linear)
                //   .OnComplete(() => _cart.GetComponent<Cart_Parent>().isConnect = false);
                //}


            }
        }
    }




    void Move_Cart()
    {
        if (Cart_List_cpi.Count > 0)
        {
            Cart_List_cpi[0].position = Vector3.Slerp(Cart_List_cpi[0].position, transform.position + Vector3.forward * CartStack_Interval, Time.deltaTime * CartStack_Speed);
            Cart_List_cpi[0].position = new Vector3(Cart_List_cpi[0].position.x, Cart_List_cpi[0].position.y, transform.position.z + CartStack_Interval);
            Cart_List_cpi[0].rotation = Quaternion.Slerp(Cart_List_cpi[0].rotation, transform.GetChild(0).rotation, Time.deltaTime * CartStack_Speed * 2f);


            for (int i = 1; i < Cart_List_cpi.Count; i++)
            {
                curBodyPart = Cart_List_cpi[i];
                PrevBodyPart = Cart_List_cpi[i - 1];

                float dis = Vector3.Distance(PrevBodyPart.position, curBodyPart.position);
                Vector3 newpos = PrevBodyPart.position;


                float T = Time.deltaTime * dis * CartStack_Speed;

                //if (T > 0.5f)
                //    T = 0.5f;
                curBodyPart.position = Vector3.Slerp(curBodyPart.position, /* newpos*/ PrevBodyPart.position, T);
                curBodyPart.position = new Vector3(curBodyPart.position.x, curBodyPart.position.y, transform.position.z + (i + 1) * CartStack_Interval);
                curBodyPart.rotation = Quaternion.Slerp(curBodyPart.rotation, PrevBodyPart.rotation, T);
            }
        }
    }

    public void Finish(bool isWin)
    {
        foreach (Animator _ani in _animator)
        {
            _ani.SetBool("Run", false);
        }
        _animator[0].SetBool("Run", false);

        _effect[0].Stop();
        _effect[1].Stop();
        _effect[2].Stop();
        _effect[4].Stop();

        state = State.Finish;
        Game_Manager.instance._uiManager.Ending_Panel(isWin);
    }

    public void TweenScale()
    {
        DOTween.Sequence().Append(transform.DOScale(Vector3.one * 1.5f, 0.1f).SetEase(Ease.Linear))
            .Append(transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear));
    }


    public void EndingLine_Col(Transform _endingTrans)
    {
        _ending_Count = Cart_List_cpi.Count;
        state = State.Ending2;
        Ani_Speed = 2f;
        Cam_On(3);
        transform.DOMoveX(0f, 0.5f);
        DOTween.Sequence().AppendCallback(() =>
                        {
                            foreach (Animator _ani in _animator)
                            {
                                _ani.SetBool("Run", false);
                            }
                            _animator[0].SetBool("Run", false);

                            _effect[0].Stop();
                            _effect[1].Stop();
                            _effect[2].Stop();
                            _effect[4].Stop();

                            state = State.Finish;
                        }).Append(transform.DOMoveZ(_endingTrans.position.z, 3f).SetEase(Ease.Linear))
                        .AppendInterval(0.5f)
                          .OnComplete(() => TestBolock());

    }

    public Transform Ending_Lose_Cart(Ending_Ground _tempendingground = null)
    {
        _ending_ground = _tempendingground;

        if (Cart_List_cpi.Count > 0)
        {
            Transform _cart = Cart_List_cpi[Cart_List_cpi.Count - 1];
            Cart_List_cpi.Remove(_cart);

            return _cart;
        }
        else
        {
            return null;
        }


    }

    void TestBolock()
    {
        //Debug.Log("TestBlock");
        _ending_ground.Ending_Block();
    }



}
