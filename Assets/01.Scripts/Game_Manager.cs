using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using MoreMountains.NiceVibrations;
using MoreMountains.NiceVibrations;




public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;
    public UI_Manager _uiManager;
    public GameObject Player;
    public Material Tiling_Mat;
    [SerializeField] float _tile = 0f;


    [SerializeField] float Vibe_Time;
    [SerializeField] float Vibe_Interval = 0.1f;
    [SerializeField] float Sound_Time;
    [SerializeField] float Sound_Interval = 0.1f;

    public AudioSource _audiosource; // 
    public AudioClip[] _clip;

    public bool SoundOn = true;
    public bool VibOn = true;

    public int Product_Group_Num = 0;
    public int[] Product_Group = new int[3];


    //= ==========================
    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (instance == null) instance = this;
        if (_uiManager == null) _uiManager = GetComponent<UI_Manager>();
        if (Player == null) Player = GameObject.FindGameObjectWithTag("Player");
        _audiosource = GetComponent<AudioSource>();

    }


    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Retry();
        //}
        _tile -= Time.deltaTime;
        Tiling_Mat.SetTextureOffset("_MainTex", new Vector2(0f, _tile));

        Vibe_Time += Time.deltaTime;
        Sound_Time += Time.deltaTime;
        //Debug.Log("Test");
        //print("Test");
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Press X");
            print("Print X");
        }
    }



    public void Retry()
    {
        UnityEngine.SceneManagement.SceneManager
               .LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);

    }


    public void Vibe(int _num)
    {

        if ((Vibe_Time >= Vibe_Interval) && VibOn)
        {
            Vibe_Time = 0f;
            switch (_num)
            {
                case 1:
                    MMVibrationManager.Haptic(HapticTypes.LightImpact);
                    break;

                case 2:
                    MMVibrationManager.Haptic(HapticTypes.MediumImpact);
                    break;

                case 3:
                    MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
                    break;

                default:
                    break;
            }
        }

    }

    public void Sound(int _num)
    {
        if ((Sound_Time >= Sound_Interval) && SoundOn)
        {
            if (_num > 0)
            {
                _audiosource.pitch = 1f;
            }
            else
            {
                //_audiosource.pitch = _audiosource.pitch += 0.1f > 2 ? 1f : _audiosource.pitch += 0.1f;
                //Debug.Log("Call Sound2");
            }
            Sound_Time = 0f;
            _audiosource.clip = _clip[_num];
            _audiosource.Play();

        }
    }


    public void Resetting()
    {
        _uiManager.Resetting();
    }

    public void SetProductGroup()
    {

        for (int i = 0; i < 3; i++)
        {
            Product_Group[i] = 3 * Product_Group_Num + i;
        }
    }

}
