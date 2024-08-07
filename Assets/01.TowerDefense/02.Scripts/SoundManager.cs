using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

 
    public List<AudioClip> bgmTracks;  
    private int currentBgmIndex = -1;  

    public AudioSource play1AudioSource;
    public AudioSource play2AudioSource;

    public List<AudioClip> SFXlist;
    public AudioSource SFX1;
    public AudioSource SFX2;
    public AudioSource SFX3;

    public enum SFXType
    {
        rock = 0
        , arrow = 1
        , fireball = 2
        , death = 3
        , newgame = 4
        , gameover = 5
        , towerBuilt = 6
        , hit = 7
        , button = 8
    }

    private AudioSource audioSource;

    private bool _soundVolumeOn;

    // BGMOn 플래그
    public bool SoundVolumeOn
    {
        get { return _soundVolumeOn; }
        set
        {
            if (_soundVolumeOn != value)
            {
                _soundVolumeOn = value;
            }
        }
    }

    public static SoundManager Instance
    {
        get
        {
            try
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SoundManager>();

                    if (_instance == null)
                    {
                        GameObject soundManagerObject = new GameObject("SoundManager");
                        _instance = soundManagerObject.AddComponent<SoundManager>();
                    }
                }

                return _instance;
            }
            catch
            {
                return null;
            }
        }
    }



    private void Awake()
    {
        try
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            audioSource = GetComponent<AudioSource>();
        }
        catch
        {

        }
    }

    private void Start()
    {
        _soundVolumeOn = true;

        try
        {
            PlayRandomBGM();
        }
        catch (System.Exception ex)
        {
            Debug.Log("Start >>>> " + ex.Message);
        }
    }

    public void PlayRandomBGM()
    {
        /*
        if (!GameManager.Instance.BGMOn)
        {
            return;
        }
        */
        
        //볼륨 복귀 후 시작.
        UpVolumeBGM();

        if (bgmTracks.Count > 0)
        {
            //Debug.Log("PlayRandomBGM() >>>>>>>>>>>>>> ");

            _soundVolumeOn = true;


            AudioSource currentlyPlayingSource = (play1AudioSource.isPlaying) ? play1AudioSource : play2AudioSource;


            if (currentlyPlayingSource.isPlaying)
            {
                Debug.Log("BGM return >>>>>>> " + currentlyPlayingSource.isPlaying);
                return;
            }

            int randomIndex = Random.Range(0, bgmTracks.Count);
            //Debug.Log("BGM randomIndex >>>>>>> " + randomIndex);

            while (randomIndex == currentBgmIndex)
            {
                randomIndex = Random.Range(0, bgmTracks.Count);
            }

            currentBgmIndex = randomIndex;
            AudioClip selectedBGM = bgmTracks[currentBgmIndex];

            if (selectedBGM != null)
            {
      
                AudioSource targetAudioSource = (currentlyPlayingSource == play1AudioSource) ? play2AudioSource : play1AudioSource;

                Debug.Log("BGM >>>>>>> " + selectedBGM.name);

   
                targetAudioSource.clip = selectedBGM;
                targetAudioSource.Play();


                StartCoroutine(WaitAndPlayRandomBGM(targetAudioSource, selectedBGM.length));

            }
        }
    }
    private IEnumerator WaitAndPlayRandomBGM(AudioSource audioSource, float clipLength)
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        //PlayRandomBGM();

        /*
        if (GameManager.Instance.BGMOn && _soundVolumeOn)
        {
            PlayRandomBGM();
        }
        */
        
    }


    // ???? BGM?? ???? (?????? ????)
    public void StopAllBGM()
    {
        //_soundVolumeOn = false;
        play1AudioSource.Stop();
        play2AudioSource.Stop();
    }

    public void DownVolumeBGM()
    {
        //_soundVolumeOn = false;
        play1AudioSource.volume = 0f;
        play2AudioSource.volume = 0f;
    }

    public void UpVolumeBGM()
    {
        //_soundVolumeOn = false;
        play1AudioSource.volume = 0.5f;
        play2AudioSource.volume = 0.5f;
    }

    public void StopAllSFX()
    {
        //_soundVolumeOn = false;
        SFX1.Stop();
        SFX2.Stop();
        SFX3.Stop();
    }

    public void PlaySFX(SFXType sfxType)
    {
        /*
        if (!GameManager.Instance.SoundOn)
        {
            return;
        }
        */
        


     // 유효하지 않은 sfxType일 경우 기본 SFXType을 사용합니다.
        int index = (int)sfxType;

        if (index < 0 || index >= SFXlist.Count)
        {
            sfxType = SFXType.button;  // SFXType 열거형에 Default 값을 추가합니다.
            index = (int)sfxType;
        }

        AudioSource availableSFXSource = GetAvailableSFXSource();
        availableSFXSource.clip = SFXlist[index];
        availableSFXSource.volume = 0.5f;
        availableSFXSource.Play();
    }

    private AudioSource GetAvailableSFXSource()
    {
        return (SFX1.isPlaying) ? (SFX2.isPlaying ? SFX3 : SFX2) : SFX1;
    }
}
