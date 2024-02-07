using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestComtroller : MonoBehaviour
{
    public GameObject MainCanvas;
    public GameObject CPI_StopPos;


    public Material Floor_Mat;
    public Material Line_Mat;

    public Texture[] Floor_Textur;
    public Material[] Line_Color;

    public int Color_num;
    public bool isReverse = false;
    public GameObject[] Cam;



    [SerializeField] Player_Move _player;
    private void Start()
    {
        _player = Game_Manager.instance.Player.GetComponent<Player_Move>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _player.SetStage();
        }

        switch (Input.inputString)
        {
            case "a":
                Color_num++;
                if (Color_num >= Line_Color.Length) Color_num = 0;
                Change_Color();
                break;

            case "s":
                Color_num--;
                if (Color_num < 0) Color_num = Line_Color.Length - 1;
                Change_Color();
                break;

            case "y":
                Game_Manager.instance._uiManager.Ending_Panel(true);
                break;

            case "u":
                Game_Manager.instance._uiManager.Ending_Panel(false);

                break;

            case "o":
                MainCanvas.SetActive(!MainCanvas.activeSelf);
                if (MainCanvas.activeSelf == false)
                {
                    _player.state = Player_Move.State.Wait;
                }

                break;

            case "k":
                CPI_StopPos.SetActive(true);
                break;

            case "l":
                CPI_StopPos.SetActive(false);
                break;

            case "r":
                isReverse = !isReverse;
                if (isReverse)
                {
                    Cam[0].SetActive(false);
                    Cam[1].SetActive(true);
                    Game_Manager.instance.Player.GetComponent<Player_Move>().isReverse = true;
                }
                else
                {
                    Cam[1].SetActive(false);
                    Cam[0].SetActive(true);
                    Game_Manager.instance.Player.GetComponent<Player_Move>().isReverse = false;
                }

                break;


            case "1":
                _player.Stage_Level++;
                _player.SaveData();
                _player.SetStage();

                break;

            case "2":
                _player.Stage_Level--;
                _player.SaveData();
                _player.SetStage();
                break;

            case "3":
                Time.timeScale += 0.2f;

                break;

            case "4":
                Time.timeScale -= 0.2f;
                break;

            case "5":
                Time.timeScale = 1;

                break;

            case "6":
                _player.Money += 1000f;
                _player.SaveData();

                break;
            case "7":
                _player.Money = _player.Money - 1000f < 0 ? 0 : _player.Money - 1000f;
                _player.SaveData();
                break;

            case "8":
                _player.Money = 0;
                _player.Fever_Level = 0;
                _player.Money_Level = 0;
                _player.Magnet_Level = 0;
                _player.SaveData();
                break;



            default:

                break;
        }


    }

    void Change_Color()
    {
        Floor_Mat.SetTexture("_MainTex", Floor_Textur[Color_num]);
        Line_Mat.SetColor("_Color", Line_Color[Color_num].GetColor("_Color"));
        Line_Mat.SetColor("_HColor", Line_Color[Color_num].GetColor("_HColor"));
        Line_Mat.SetColor("_SColor", Line_Color[Color_num].GetColor("_SColor"));

    }

}
