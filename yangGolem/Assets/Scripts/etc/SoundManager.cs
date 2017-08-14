using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//게임에서 사용되는 효과음, 배경음 로드, 재생.
public enum SoundEffect
{
    //Attack = 0,
}

public enum SoundMusic
{
    InGame_Music,
}

public static class SoundManager
{
    static List<AudioClip> effectClip;
    static GameObject effectInstance = null;
    static GameObject musicInstance = null;

    public static void LoadSound()
    {
        effectClip = new List<AudioClip>();
        //effectClip.Add(Resources.Load("Sound/Effect/Fire") as AudioClip);               // Attack.

    }


    // 효과음 재생.
    public static void PlayEffect(SoundEffect effectToPlay, bool isLoop = false, float volume = 1.0f, float delayTime = 0.0f, int limitCount = 0)
    {
        AudioSource soundEffcet = GetEffect(effectToPlay, limitCount);

        if (soundEffcet != null)
        {
            soundEffcet.volume = volume;

            soundEffcet.clip = GetSoundEffect(effectToPlay);
            soundEffcet.loop = isLoop;

            if (PlayerPrefs.GetInt("FX", 1) == 1) // 옵션값에 따라 사운드 활성화 여부 결정. 
            {
                if (delayTime != 0.0f)
                    soundEffcet.PlayDelayed(delayTime);
                else
                    soundEffcet.Play();
            }
        }
    }

    // 배경음악 재생.
    public static void PlayMusic(SoundMusic musicToPlay, float delayTime = 0.0f, float volume = 1.0f)
    {
        GetMusic().clip = GetSoundMusic(musicToPlay);
        GetMusic().volume = volume;
        GetMusic().loop = true;
        GetMusic().Play();
        if (delayTime != 0.0f)
        {
            GetMusic().PlayDelayed(delayTime);
        }
        else
            GetMusic().Play();

        if (PlayerPrefs.GetInt("BGM", 1) == 1)
            GameObject.Find("SoundMusicObject").GetComponent<AudioSource>().enabled = true;
        else
            GameObject.Find("SoundMusicObject").GetComponent<AudioSource>().enabled = false;
    }

    public static bool IsPlayMusic(SoundMusic musicToPlay)
    {
        AudioClip clip = GetSoundMusic(musicToPlay);
        if (GameObject.Find("SoundMusicObject").GetComponent<AudioSource>().clip == clip)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public static void MusicStop()
    {
        GetMusic().Stop();
    }


    static AudioSource GetEffect(SoundEffect snd, int limitCount)
    {
        int index = 0;

        if (effectInstance == null)
        {
            effectInstance = new GameObject("SoundEffectObject");
            effectInstance.transform.parent = null;

            for (int i = 0; i < 30; i++)        // 한번에 재생가능한 효과음을 30개로 제한.
            {
                GameObject soundEffect = new GameObject("SoundEffect");
                soundEffect.AddComponent<AudioSource>();
                soundEffect.transform.parent = effectInstance.transform;
            }
        }

        for (int i = 0; i < 30; i++)        // 현재 동작중이지 않은 사운드클립의 인덱스를 검색.
        {
            if (!effectInstance.transform.GetChild(i).GetComponent<AudioSource>().isPlaying)
            {
                index = i;
                break;
            }
            else
            {
                if (limitCount != 0)
                {
                    if (effectInstance.transform.GetChild(i).GetComponent<AudioSource>().clip.name == GetSoundEffect(snd).name)
                    {
                        limitCount--;

                        if (limitCount <= 0)
                            return null;
                    }
                }
            }
        }

        return effectInstance.transform.GetChild(index).gameObject.GetComponent<AudioSource>();
    }

    public static void DeleteEffect(SoundEffect snd)
    {
        for (int i = 0; i < 30; i++)
        {
            if (effectInstance.transform.GetChild(i).GetComponent<AudioSource>().clip != null)
            {
                if (effectInstance.transform.GetChild(i).GetComponent<AudioSource>().clip.name == GetSoundEffect(snd).name)
                {
                    effectInstance.transform.GetChild(i).GetComponent<AudioSource>().clip = null;
                    effectInstance.transform.GetChild(i).GetComponent<AudioSource>().loop = false;
                }
            }
        }
    }

    public static AudioSource GetMusic()
    {
        if (musicInstance == null)
        {
            musicInstance = new GameObject("SoundMusicObject");
            musicInstance.AddComponent<AudioSource>();
            musicInstance.transform.parent = null;
        }
        return musicInstance.GetComponent<AudioSource>();
    }



    static AudioClip GetSoundEffect(SoundEffect effectToPlay)
    {
        return effectClip[(int)effectToPlay];
    }


    static AudioClip GetSoundMusic(SoundMusic musicToPlay)
    {
        switch (musicToPlay)
        {
            case SoundMusic.InGame_Music:
                return Resources.Load("Sound/Background/BGM_" + Random.Range(1, 2+1)) as AudioClip;
                break;

        }
        return null;
    }

}