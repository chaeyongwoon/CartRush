using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using MondayOFF;

public class UI_Manager : MonoBehaviour
{
    [Title("UI_Panel")]
    public GameObject Base_UI_Panel;
    public GameObject InGame_Panel;
    public GameObject Ready_Panel;
    public GameObject End_Panel;
    //public GameObject Fail_Panel;

    public Image Setting_Img;
    public Button Setting_Button;
    public Button Sound_Button;
    public Button Vib_Button;
    public Image Fever_Fire;
    [SerializeField] bool isSetting = false;
    public Text Ending_Money_Text;
    public GameObject Next_Button;
    public GameObject Skin_Panel;
    public Sprite[] Products_Sprite;
    [SerializeField] Button[] Skin_Button;
    [SerializeField] int Skin_Page = 0;
    public Transform Skin_Product_Group;
    public Button[] Page_Button;


    [Title("InGame")]
    public Slider Fever_Slider;
    public Slider Race_Slider;
    public Text Level_Text;
    public Text Money_Text;





    [Title("Interval")]
    public float Ending_Panel_Interval = 1f;

    [Space(10)]
    [Title("Player")]
    [SerializeField] Player_Move _player;
    [SerializeField] int Fever_Up_Price;
    [SerializeField] int Magnet_Up_Price;
    [SerializeField] int Money_Up_Price;
    [SerializeField] Button[] UpgradeButton;

    [SerializeField] Transform _endingLine;

    [SerializeField] bool isEnd = false;
    [SerializeField] float _tempMoney = 0f;
    //===============================

    private void Start()
    {
        StartCoroutine(Cor_Update());
        if (_endingLine == null)
        {
            try
            {
                _endingLine = GameObject.FindGameObjectWithTag("GoalCart").transform;
            }
            catch { }
        }


        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((BaseEventData) =>
        {
            Ready_Panel.SetActive(false);
            InGame_Panel.SetActive(true);
            _player.state = Player_Move.State.Wait;
        }
        );


        Ready_Panel.AddComponent<EventTrigger>().triggers.Add(entry);


        UpgradeButton = new Button[3];
        for (int i = 0; i < 3; i++)
        {
            UpgradeButton[i] = Ready_Panel.transform.GetChild(i).GetComponent<Button>();
        }
        UpgradeButton[0].onClick.AddListener(Money_Upgrade);
        UpgradeButton[1].onClick.AddListener(Fever_Upgrade);
        UpgradeButton[2].onClick.AddListener(Magnet_Upgrade);

        Setting_Button.onClick.AddListener(SettingOnOff);
        Sound_Button.onClick.AddListener(SoundOnOff);
        Vib_Button.onClick.AddListener(VibOnOff);




        for (int i = 0; i < 4; i++)
        {
            Skin_Button[i] = Skin_Panel.transform.GetChild(0).GetChild(i).GetComponent<Button>();
            int index = i;
            Skin_Button[i].onClick.AddListener(() => SetSkinButton(index));
            //Skin_Button[i].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => AdsManager.ShowRewarded(() => RV_Button(index)));
            Skin_Button[i].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => AdsManager.ShowRewarded(() => RV_Button(index)));//  MondayOFF.AdsManager.instance.ShowRewardedVideo(() => RV_Button(index)));
        }
        SetSkinButton(0, true);
        //CheckSkinButton();
        Page_Change();
        DOTween.Sequence().Append(Skin_Product_Group.DOMoveY(0.5f, 1f).SetEase(Ease.Linear)).SetLoops(-1, LoopType.Yoyo);






        Ready_Panel.transform.GetChild(7).GetChild(1).GetComponent<RectTransform>().DOLocalMoveX(250f, 1.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        Resetting();

        SetPrice();
        InteractableCheck();
    }

    IEnumerator Cor_Update()
    {
        while (true)
        {
            yield return new WaitForSeconds(Time.deltaTime);

            //SetFeverSlider();
            //Money_Text.text = string.Format("{0:N0}", _player.Money);
            Ending_Money_Text.text = string.Format("Score : {0:N0}", _tempMoney);


            try
            {
                Race_Slider.value = (_player.transform.position.z / _endingLine.position.z);
            }
            catch { }

        }
    }

    //void SetFeverSlider()
    //{
    //    Fever_Slider.DOValue((_player.Current_Fever_Guage / _player.Max_Fever_Guage), 0.2f);
    //    Fever_Fire.enabled
    //        = Game_Manager.instance.Player.GetComponent<Player_Move>().state
    //        == Player_Move.State.Fever ? true : false;
    //}


    public void Ending_Panel(bool isClear)
    {

        if (isEnd == false)
        {
            isEnd = true;
            StartCoroutine(Cor_Panel());
        }

        IEnumerator Cor_Panel()
        {
            yield return new WaitForSeconds(Ending_Panel_Interval);
            _player.Stage_Level = isClear ? _player.Stage_Level + 1 : _player.Stage_Level;
            InGame_Panel.SetActive(false);

            End_Panel.SetActive(true);

            if (isClear)
            {
                //MondayOFF.EventsManager.instance.ClearStage(_player.Stage_Level);
                EventTracker.ClearStage(_player.Stage_Level);
                //MondayOFF.EventTracker.ClearStage(_player.Stage_Level);


                Game_Manager.instance.Sound(2);
                DOTween.Sequence()
                    .AppendCallback(() =>
                    {
                        End_Panel.transform.GetChild(1).gameObject.SetActive(true);
                        End_Panel.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-1300f, 526f);
                        End_Panel.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                        End_Panel.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
                    })
                    .Append(End_Panel.transform.GetChild(1).GetComponent<RectTransform>().DOAnchorPosX(0f, 0.3f).SetEase(Ease.OutCubic))
                    .AppendInterval(0.2f)
                     .AppendCallback(() =>
                     {
                         End_Panel.transform.GetChild(0).gameObject.SetActive(true);
                         End_Panel.transform.GetChild(0).transform.localScale = Vector3.zero;
                         End_Panel.transform.GetChild(0).DORotate(new Vector3(0f, 0f, 360f), 4f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetRelative(true).SetLoops(-1, LoopType.Restart);
                     })
                    .Append(End_Panel.transform.GetChild(0).transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack))
                    .AppendCallback(() =>
                    {
                        End_Panel.transform.GetChild(2).gameObject.SetActive(true);
                        End_Panel.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(1300f, 0f);
                    })
                    .Append(End_Panel.transform.GetChild(2).GetComponent<RectTransform>().DOAnchorPosX(0f, 0.3f).SetEase(Ease.OutCubic))
                    .AppendInterval(0.3f)
                    .AppendCallback(() =>
                    {
                        End_Panel.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
                    })
                    .AppendInterval(0.2f)
                    .AppendCallback(() =>
                    {
                        Game_Manager.instance.Sound(1);
                        DOTween.To(() => _tempMoney, x => _tempMoney = x, _player.totalsell_money, 0.5f);
                    }).AppendInterval(0.5f)
                    .AppendCallback(() =>
                    {
                        //_player.Money += _tempMoney;
                        _player.SaveData();
                        //TimeInterstitialShower.instance.CheckTimeAndShowInterstitial();
                        AdsManager.ShowInterstitial();
                        // add IS Ad.
                    }).AppendInterval(0.3f)
                    .AppendCallback(() =>
                    {
                        End_Panel.transform.GetChild(3).gameObject.SetActive(true);
                        End_Panel.transform.GetChild(3).transform.localScale = Vector3.zero;
                    })
                    .Append(End_Panel.transform.GetChild(3).DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
            }
            else
            {
                Game_Manager.instance.Sound(3);
                DOTween.Sequence()
                    .AppendCallback(() =>
                    {
                        End_Panel.transform.GetChild(1).gameObject.SetActive(true);
                        End_Panel.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-1300f, 526f);
                        End_Panel.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
                        End_Panel.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                    })
                    .Append(End_Panel.transform.GetChild(1).GetComponent<RectTransform>().DOAnchorPosX(0f, 0.3f).SetEase(Ease.OutCubic))
                    .AppendInterval(0.2f)
                    // .AppendCallback(() =>
                    // {
                    //     End_Panel.transform.GetChild(0).gameObject.SetActive(true);
                    //     End_Panel.transform.GetChild(0).transform.localScale = Vector3.zero;
                    //     End_Panel.transform.GetChild(0).DORotate(new Vector3(0f, 0f, 360f), 4f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetRelative(true).SetLoops(-1, LoopType.Restart);
                    // })
                    //.Append(End_Panel.transform.GetChild(0).transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack))
                    .AppendCallback(() =>
                    {
                        End_Panel.transform.GetChild(2).gameObject.SetActive(true);
                        End_Panel.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(1300f, 0f);
                    })
                    .Append(End_Panel.transform.GetChild(2).GetComponent<RectTransform>().DOAnchorPosX(0f, 0.3f).SetEase(Ease.OutCubic))
                    .AppendInterval(0.3f)
                    .AppendCallback(() =>
                    {
                        End_Panel.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
                    })
                    .AppendInterval(0.2f)
                    .AppendCallback(() =>
                    {
                        Game_Manager.instance.Sound(1);
                        DOTween.To(() => _tempMoney, x => _tempMoney = x, _player.totalsell_money, 0.5f);
                    }).AppendInterval(0.5f)
                    .AppendCallback(() =>
                    {
                        //_player.Money += _tempMoney;
                        _player.SaveData();
                        // add IS Ad.
                    }).AppendInterval(0.3f)
                    .AppendCallback(() =>
                    {
                        End_Panel.transform.GetChild(4).gameObject.SetActive(true);
                        End_Panel.transform.GetChild(4).transform.localScale = Vector3.zero;
                    })
                    .Append(End_Panel.transform.GetChild(4).DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
            }


        }
    }

    void SetPrice()
    {
        Fever_Up_Price = 5000 + _player.Fever_Level * 5000;
        Magnet_Up_Price = 2000 + _player.Magnet_Level * 2000;
        Money_Up_Price = 2000 + _player.Money_Level * 2000;

        UpgradeButton[0].transform.GetChild(0).GetComponent<Text>().text = string.Format("{0}", Money_Up_Price);
        UpgradeButton[1].transform.GetChild(0).GetComponent<Text>().text = string.Format("{0}", Fever_Up_Price);
        UpgradeButton[2].transform.GetChild(0).GetComponent<Text>().text = string.Format("{0}", Magnet_Up_Price);

        UpgradeButton[0].transform.GetChild(1).GetComponent<Text>().text = string.Format("+{0}%", _player.Money_Level * 5);
        UpgradeButton[1].transform.GetChild(1).GetComponent<Text>().text = string.Format("+{0}s", _player.Fever_Level);
        UpgradeButton[2].transform.GetChild(1).GetComponent<Text>().text = string.Format("+{0}", _player.Magnet_Level);


    }


    void Fever_Upgrade()
    {
        if (_player.Fever_Level < 5)
        {
            if (_player.Money >= Fever_Up_Price)
            {
                _player.Money -= Fever_Up_Price;
                _player.Fever_Level++;
                _player.SaveData();
                SetPrice();
            }
        }
        InteractableCheck();
    }

    void Magnet_Upgrade()
    {
        if (_player.Magnet_Level < 10)
        {
            if (_player.Money >= Magnet_Up_Price)
            {
                _player.Money -= Magnet_Up_Price;
                _player.Magnet_Level++;
                _player.SaveData();
                SetPrice();
            }
        }
        InteractableCheck();
    }

    void Money_Upgrade()
    {
        if (_player.Money >= Money_Up_Price)
        {
            _player.Money -= Money_Up_Price;
            _player.Money_Level++;
            _player.SaveData();
            SetPrice();
        }
        InteractableCheck();
    }

    void SettingOnOff()
    {
        isSetting = !isSetting;

        if (isSetting)
        {
            Setting_Img.GetComponent<RectTransform>().offsetMin = new Vector2(94.5f, 1776.3f);
            Setting_Img.GetComponent<RectTransform>().offsetMax = new Vector2(-1026.5f, -39.25f);
            Sound_Button.gameObject.SetActive(true);
            Vib_Button.gameObject.SetActive(true);

        }
        else
        {
            Setting_Img.GetComponent<RectTransform>().offsetMin = new Vector2(94.5f, 2034f);
            Setting_Img.GetComponent<RectTransform>().offsetMax = new Vector2(-1026.5f, -39.25f);
            Sound_Button.gameObject.SetActive(false);
            Vib_Button.gameObject.SetActive(false);

        }



    }

    void SoundOnOff()
    {
        Game_Manager.instance.SoundOn = !Game_Manager.instance.SoundOn;
        Sound_Button.transform.GetChild(0).gameObject.SetActive(Game_Manager.instance.SoundOn);
    }
    void VibOnOff()
    {
        Game_Manager.instance.VibOn = !Game_Manager.instance.VibOn;
        Vib_Button.transform.GetChild(0).gameObject.SetActive(Game_Manager.instance.VibOn);
    }

    void InteractableCheck()
    {

        if (_player.Money >= Money_Up_Price)
        {
            UpgradeButton[0].interactable = true;
            //var _color = UpgradeButton[0].colors;
            //_color.normalColor = new Vector4(1f, 1f, 1f, 1f);
            //UpgradeButton[0].colors = _color;
        }
        else
        {
            UpgradeButton[0].interactable = false;
            //var _color = UpgradeButton[0].colors;
            //_color.normalColor = new Vector4(0.7f, 0.7f, 0.7f, 1f);
            //UpgradeButton[0].colors = _color;
        }


        if (_player.Fever_Level < 5)
        {
            if (_player.Money >= Fever_Up_Price)
            {
                UpgradeButton[1].interactable = true;
                //var _color = UpgradeButton[1].colors;
                //_color.normalColor = new Vector4(1f, 1f, 1f, 1f);
                //UpgradeButton[1].colors = _color;
            }
            else
            {
                UpgradeButton[1].interactable = false;
                //var _color = UpgradeButton[1].colors;
                //_color.normalColor = new Vector4(0.7f, 0.7f, 0.7f, 1f);
                //UpgradeButton[1].colors = _color;
            }
        }
        else
        {
            UpgradeButton[1].interactable = false;
            //    UpgradeButton[1].transform.GetChild(0).GetComponent<Text>().text = string.Format("MAX");
            //    var _color = UpgradeButton[1].colors;
            //    _color.normalColor = new Vector4(0.7f, 0.7f, 0.7f, 1f);
            //    UpgradeButton[1].colors = _color;
        }


        if (_player.Magnet_Level < 10)
        {
            if (_player.Money >= Magnet_Up_Price)
            {
                UpgradeButton[2].interactable = true;
                //var _color = UpgradeButton[2].colors;
                //_color.normalColor = new Vector4(1f, 1f, 1f, 1f);
                //UpgradeButton[2].colors = _color;

            }
            else
            {
                UpgradeButton[2].interactable = false;
                //var _color = UpgradeButton[2].colors;
                //_color.normalColor = new Vector4(0.7f, 0.7f, 0.7f, 1f);
                //UpgradeButton[2].colors = _color;
            }
        }
        else
        {
            UpgradeButton[2].interactable = false;
            //UpgradeButton[2].transform.GetChild(0).GetComponent<Text>().text = string.Format("MAX");
            //var _color = UpgradeButton[2].colors;
            //_color.normalColor = new Vector4(0.7f, 0.7f, 0.7f, 1f);
            //UpgradeButton[2].colors = _color;
        }



    }

    public void Resetting()
    {
        if (_player == null) _player = Game_Manager.instance.Player.GetComponent<Player_Move>();
        Base_UI_Panel.SetActive(true);
        InGame_Panel.SetActive(false);
        Ready_Panel.SetActive(true);
        Next_Button.SetActive(false);
        End_Panel.SetActive(false);
        for (int i = 0; i < End_Panel.transform.childCount; i++)
        {
            End_Panel.transform.GetChild(i).gameObject.SetActive(false);
        }
        try
        {
            InteractableCheck();
        }
        catch { }
        //Fail_Panel.SetActive(false);
        isEnd = false;
        Fever_Slider.value = 0;
        Race_Slider.value = 0;
        Level_Text.text = string.Format("Level : {0}", Game_Manager.instance.Player.GetComponent<Player_Move>().Stage_Level);
        _tempMoney = 0f;
        Ending_Money_Text.text = string.Format("X 0");
    }


    void CheckSkinButton()
    {
        for (int i = 0; i < 4; i++)
        {
            Skin_Button[i].transform.GetChild(0).GetComponent<Image>().sprite = Products_Sprite[Skin_Page * 4 + i];
            Skin_Button[i].transform.GetChild(0).GetComponent<Image>().color = Color.black;
            Skin_Button[i].transform.GetChild(1).GetComponent<Image>().color = new Vector4(0.7411765f, 0.8705882f, 1f, 1f);
            Skin_Button[i].transform.GetChild(2).gameObject.SetActive(true);
            Skin_Button[i].interactable = false;

            if (Skin_Page * 4 + i >= 6)
            {
                Skin_Button[i].transform.GetChild(2).gameObject.SetActive(false);
            }

            //Debug.Log(string.Format("num:{0},value:{1}", Skin_Page * 4 + i, PlayerPrefs.GetInt(string.Format("Skin_{0}", Skin_Page * 4 + i))));

            if (PlayerPrefs.GetInt(string.Format("Skin_{0}", Skin_Page * 4 + i)) == 1)
            {
                Skin_Button[i].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                Skin_Button[i].transform.GetChild(2).gameObject.SetActive(false);
                if (Game_Manager.instance.Product_Group_Num == (Skin_Page * 4 + i))
                {
                    Skin_Button[i].transform.GetChild(1).GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 1f);
                }
                Skin_Button[i].interactable = true;
            }
        }


    }

    void SetSkinButton(int _num, bool isInit = false)
    {
        if (!isInit)
        {
            Game_Manager.instance.Product_Group_Num = Skin_Page * 4 + _num;
        }

        Game_Manager.instance.SetProductGroup();

        for (int i = 0; i < 4; i++)
        {
            Skin_Button[i].transform.GetChild(1).GetComponent<Image>().color = new Vector4(0.7411765f, 0.8705882f, 1f, 1f);

            if (Game_Manager.instance.Product_Group_Num == (Skin_Page * 4 + i))
            {
                Skin_Button[i].transform.GetChild(1).GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 1f);
                DOTween.Sequence().Append(Skin_Product_Group.DOScale(Vector3.zero, 0.1f).SetEase(Ease.Linear))
                    .AppendCallback(() =>
                    {
                        for (int i = 0; i < Skin_Product_Group.childCount; i++)
                        {
                            Skin_Product_Group.GetChild(i).gameObject.SetActive(false);
                        }
                        Skin_Product_Group.GetChild(Game_Manager.instance.Product_Group_Num).gameObject.SetActive(true);
                    })
                    .Append(Skin_Product_Group.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBack));
            }
        }
        Game_Manager.instance.Player.GetComponent<Player_Move>().SaveData();

    }

    void RV_Button(int _num)
    {
        PlayerPrefs.SetInt(string.Format("Skin_{0}", Skin_Page * 4 + _num), 1);
        // add RV

        CheckSkinButton();


    }

    public void Skin_PanelOnOff()
    {
        bool isOn = !Skin_Panel.activeSelf;
        Skin_Panel.SetActive(isOn);

        if (isOn)
        {

        }
        else
        {

            Game_Manager.instance.Player.GetComponent<Player_Move>().SetStage();
        }

    }

    public void Page_Change(int _val = 0)
    {
        Skin_Page += _val;
        if (Skin_Page == 0)
        {
            Page_Button[0].gameObject.SetActive(false);
            Page_Button[1].gameObject.SetActive(true);
        }
        else if (Skin_Page == 1)
        {
            Page_Button[0].gameObject.SetActive(true);
            Page_Button[1].gameObject.SetActive(false);
        }

        CheckSkinButton();
    }


}
