using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//게임에서 사용되는 효과음, 배경음 로드, 재생.
public enum SoundEffect
{
    //Attack = 0,
    //Enemy_Die,
    //Get_Gold,
    //Get_Booster,
    //Get_Part,
    //Fire_Misslie,

    //FX_Common_Button_01 = 0,
    //FX_Common_Button_02,
    //FX_Common_Menu_01,
    //FX_Lobby_Hero_Change,
    //FX_Lobby_Step_Change,

    //FX_Common_Screen_Change_01,
    //FX_Common_Screen_Change_02,
    //FX_Popup_Powerup,
    //FX_Popup_Reward,
    //FX_Ingame_Wave,

    //FX_Ingame_Time_Warning,
    //FX_Play_Skill_Appear,
    //FX_Play_Skill_Ready,
    //FX_Play_Skill_Use,
    //FX_Ingame_Success_Class,

    //FX_Ingame_Success_Best,
    //FX_Ingame_Success_Part,
    //FX_Ingame_Fail,
    ////FX_Play_BeShot,
    //FX_Paly_Laser,

    //FX_Play_Death,
    //FX_Play_Death_Boss,
    //FX_Play_Shoot,
    //FX_Play_Explosion,
    //FX_Play_Meteor_Alarm,

    //FX_Play_Hero_Appear_01,
    ////FX_Play_Hero_Appear_02,
    //FX_Play_Hero_Stepup,
    //FX_Play_Booster,
    //FX_Play_Gold,

    //FX_Play_Revival,
    //FX_Explosion,
    //FX_H0_Skill_01,
    //FX_H0_Skill_02_Appear,
    //FX_H0_Skill_02_Change,

    //FX_H1_Skill_02,
    //FX_H1_Skill_03,
    //FX_H2_Skill_01,
    //FX_H2_Skill_02,
    //FX_H2_Skill_03,

    //FX_H3_Skill_01,
    //FX_H3_Skill_02,
    //FX_H4_Skill_01,
    //FX_H4_Skill_02,
    //FX_B0_Egg,

    //FX_B10_Copy,
    //FX_B20_Ink,
    //FX_B30_Divide,
    //FX_B40_Transform,
    //FX_B40_Part,

    //FX_G30,
    //FX_Worldmap_Planet_Move,
    //FX_Ingame_Count

}

public enum SoundMusic
{
    Lobby_Music,
    InGame_Music,
    InGame_Boss_Music,
    Loading

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
        //effectClip.Add(Resources.Load("Sound/Effect/Explosion") as AudioClip);          // Enemy_Die.
        //effectClip.Add(Resources.Load("Sound/Effect/GetGold") as AudioClip);            // Get_Gold.
        //effectClip.Add(Resources.Load("Sound/Effect/GetBooster") as AudioClip);         // Get_Booster.
        //effectClip.Add(Resources.Load("Sound/Effect/GetPart") as AudioClip);            // Get_Part.
        //effectClip.Add(Resources.Load("Sound/Effect/Missile") as AudioClip);			  // Fire_Misslie.

        //effectClip.Add(Resources.Load("Sound/Effect/FX_Common_Button_01") as AudioClip);			// FX_Common_Button_01.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Common_Button_02") as AudioClip);            // FX_Common_Button_02.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Common_Menu_01") as AudioClip);              // FX_Common_Menu_01.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Lobby_Hero_Change") as AudioClip);           // FX_Lobby_Hero_Change.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Lobby_Step_Change") as AudioClip);           // FX_Lobby_Step_Change.

        //effectClip.Add(Resources.Load("Sound/Effect/FX_Common_Screen_Change_01") as AudioClip);		// FX_Common_Screen_Change_01.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Common_Screen_Change_02") as AudioClip);     // FX_Common_Screen_Change_02.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Popup_Powerup") as AudioClip);               // FX_Popup_Powerup.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Popup_Reward") as AudioClip);                // FX_Popup_Reward.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Ingame_Wave") as AudioClip);                 // FX_Ingame_Wave.

        //effectClip.Add(Resources.Load("Sound/Effect/FX_Ingame_Time_Warning") as AudioClip);         // FX_Ingame_Time_Warning.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Play_Skill_Appear") as AudioClip);           // FX_Play_Skill_Appear.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Play_Skill_Ready") as AudioClip);            // FX_Play_Skill_Ready.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Play_Skill_Use") as AudioClip);              // FX_Play_Skill_Use.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Ingame_Success_Class") as AudioClip);        // FX_Ingame_Success_Class.

        //effectClip.Add(Resources.Load("Sound/Effect/FX_Ingame_Success_Best") as AudioClip);         // FX_Ingame_Success_Best.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Ingame_Success_Part") as AudioClip);         // FX_Ingame_Success_Part.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Ingame_Fail") as AudioClip);                 // FX_Ingame_Fail.
        ////effectClip.Add(Resources.Load("Sound/Effect/FX_Play_BeShot") as AudioClip);                 // FX_Play_BeShot.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Paly_Laser") as AudioClip);                  // FX_Paly_Laser.

        //effectClip.Add(Resources.Load("Sound/Effect/FX_Play_Death") as AudioClip);			// FX_Play_Death.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Play_Death_Boss") as AudioClip);     // FX_Play_Death_Boss.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Play_Shoot") as AudioClip);          // FX_Play_Shoot.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Play_Explosion") as AudioClip);      // FX_Play_Explosion.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Play_Meteor_Alarm") as AudioClip);   // FX_Play_Meteor_Alarm.

        //effectClip.Add(Resources.Load("Sound/Effect/FX_Play_Hero_Appear_01") as AudioClip);		// FX_Play_Hero_Appear_01.
        ////effectClip.Add(Resources.Load("Sound/Effect/FX_Play_Hero_Appear_02") as AudioClip);   // FX_Play_Hero_Appear_02.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Play_Hero_Stepup") as AudioClip);        // FX_Play_Hero_Stepup.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Play_Booster") as AudioClip);			// FX_Play_Booster.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Play_Gold") as AudioClip);               // FX_Play_Gold.

        //effectClip.Add(Resources.Load("Sound/Effect/FX_Play_Revival") as AudioClip);            // FX_Play_Revival.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Explosion") as AudioClip);               // FX_Explosion.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_H0_Skill_01") as AudioClip);             // FX_H0_Skill_01.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_H0_Skill_02_Appear") as AudioClip);		// FX_H0_Skill_02_Appear.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_H0_Skill_02_Change") as AudioClip);      // FX_H0_Skill_02_Change.

        //effectClip.Add(Resources.Load("Sound/Effect/FX_H1_Skill_02") as AudioClip);			// FX_H1_Skill_02.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_H1_Skill_03") as AudioClip);         // FX_H1_Skill_03.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_H2_Skill_01") as AudioClip);			// FX_H2_Skill_01.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_H2_Skill_02") as AudioClip);			// FX_H2_Skill_02.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_H2_Skill_03") as AudioClip);         // FX_H2_Skill_03.

        //effectClip.Add(Resources.Load("Sound/Effect/FX_H3_Skill_01") as AudioClip);			// FX_H3_Skill_01.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_H3_Skill_02") as AudioClip);			// FX_H3_Skill_02.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_H4_Skill_01") as AudioClip);			// FX_H4_Skill_01.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_H4_Skill_02") as AudioClip);         // FX_H4_Skill_02.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_B0_Egg") as AudioClip);              // FX_B0_Egg.

        //effectClip.Add(Resources.Load("Sound/Effect/FX_B10_Copy") as AudioClip);            // FX_B10_Copy.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_B20_Ink") as AudioClip);             // FX_B20_Ink.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_B30_Divide") as AudioClip);          // FX_B30_Divide.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_B40_Transform") as AudioClip);		// FX_B40_Transform.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_B40_Part") as AudioClip);            // FX_B40_Part.

        //effectClip.Add(Resources.Load("Sound/Effect/FX_G30") as AudioClip);                     // FX_G30.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Worldmap_Planet_Move") as AudioClip);    // FX_Worldmap_Planet_Move.
        //effectClip.Add(Resources.Load("Sound/Effect/FX_Ingame_Count") as AudioClip);            // FX_Ingame_Count.

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
            //Debug.Log ("SoundName = "+ soundEffcet.clip.name);

            //if (PlayerPrefs.GetInt("FX") == 1) // 옵션값에 따라 사운드 활성화 여부 결정. 
            //    soundEffcet.enabled = true;
            //else
            //    soundEffcet.enabled = false;
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
            effectInstance.transform.parent = null;// ResourceManager.instance.transform;

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
            musicInstance.transform.parent = null;// ResourceManager.instance.transform;
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
            case SoundMusic.Lobby_Music:
                return Resources.Load("Sound/Music/BGM_Title") as AudioClip;
                break;
            case SoundMusic.InGame_Music:
                return Resources.Load("Sound/Music/BGM_Ingame") as AudioClip;
                break;
            case SoundMusic.InGame_Boss_Music:
                return Resources.Load("Sound/Music/BGM_Boss") as AudioClip;
                break;
            case SoundMusic.Loading:
                return Resources.Load("Sound/Music/FX_Common_Loding_01") as AudioClip;
                break;

        }
        return null;
    }

}