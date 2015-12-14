using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SoundPlayer
{
    BGMPlayer curBGMPlayer;
    BGMPlayer fadeOutBGMPlayer;
    GameObject soundPlayerObj;
    AudioSource audioSource;
    Dictionary<string, AudioClipInfo> audioClips = new Dictionary<string, AudioClipInfo>();

    //AudioClipの中身
    class AudioClipInfo
    {
        public string resourceName;
        public string name;
        public AudioClip clip;

        public AudioClipInfo(string ResourceNames, string name)
        {
            this.resourceName = ResourceNames;
            this.name = name;
        }
    }

    public SoundPlayer()
    {
        audioClips.Add("bgm001", new AudioClipInfo("music/bgm1", "bgm001"));
        audioClips.Add("bgm002", new AudioClipInfo("music/bgm2", "bgm002"));
        audioClips.Add("bgm003", new AudioClipInfo("music/bgm3", "bgm003"));
        audioClips.Add("bgm004", new AudioClipInfo("music/bgm4", "bgm004"));
        audioClips.Add("bgm005", new AudioClipInfo("music/bgm5", "bgm005"));
        audioClips.Add("bgm006", new AudioClipInfo("music/bgm6", "bgm006"));
        audioClips.Add("bgm007", new AudioClipInfo("music/bgm7", "bgm007"));
        audioClips.Add("bgm008", new AudioClipInfo("music/bgm8", "bgm008"));
        audioClips.Add("bgm009", new AudioClipInfo("music/bgm9", "bgm009"));
        audioClips.Add("bgm010", new AudioClipInfo("music/bgm10", "bgm010"));
        audioClips.Add("bgm011", new AudioClipInfo("music/bgm11", "bgm011"));

        audioClips.Add("se001", new AudioClipInfo("music/se1", "se001"));
        audioClips.Add("se002", new AudioClipInfo("music/se2", "se002"));
        audioClips.Add("se003", new AudioClipInfo("music/se3", "se003"));
        audioClips.Add("se004", new AudioClipInfo("music/se4", "se004"));
        audioClips.Add("se005", new AudioClipInfo("music/se5", "se005"));
        audioClips.Add("se006", new AudioClipInfo("music/se6", "se006"));
        audioClips.Add("se007", new AudioClipInfo("music/se7", "se007"));
        audioClips.Add("Gbgm01", new AudioClipInfo("music/toys", "Gbgm01"));

    }
    public bool PlaySE(string seName)
    {
        if (audioClips.ContainsKey(seName) == false) return false;

        AudioClipInfo info = audioClips[seName];

        //Load
        if (info.clip == null) info.clip = (AudioClip)Resources.Load(info.resourceName);

        if (soundPlayerObj == null)
        {
            soundPlayerObj = new GameObject("SoundPlayer");
            audioSource = soundPlayerObj.AddComponent<AudioSource>();
        }
        //PlayerSEを流す
        audioSource.PlayOneShot(info.clip);
        return true;
    }
    //初回再生のみ通る
    public void playBGM(string bgmName, float fadeTime,bool volBalancer,float ControlVol)
    {
        if (fadeOutBGMPlayer != null)
        {
            //フェードアウトプレイヤーを消す
            fadeOutBGMPlayer.destory();
        }
        //今流れてるBGMをフェードアウト用のプレイヤーに移す
        if (curBGMPlayer != null)
        {
            curBGMPlayer.stopBGM(fadeTime);
            fadeOutBGMPlayer = curBGMPlayer;

        }
        //要求されたBGMがなかった場合
        if (audioClips.ContainsKey(bgmName) == false)
        {
            //空のプレイヤーを入れる
            curBGMPlayer = new BGMPlayer();
        }
        else
        {
            curBGMPlayer = new BGMPlayer(audioClips[bgmName].resourceName);
            curBGMPlayer.playBGM(fadeTime,volBalancer,ControlVol);
        }

    }
    public void playBGM()
    {
        if (curBGMPlayer != null && curBGMPlayer.hadFadeOut() == false) curBGMPlayer.playBGM();
        if (fadeOutBGMPlayer != null && fadeOutBGMPlayer.hadFadeOut() == false) fadeOutBGMPlayer.playBGM();
    }
    public void pauseBGM()
    {
        if (curBGMPlayer != null) curBGMPlayer.pauseBGM();
        if (fadeOutBGMPlayer != null) fadeOutBGMPlayer.pauseBGM();
    }
    public void stopBGM(float fadeTime)
    {
        if (curBGMPlayer != null) curBGMPlayer.stopBGM(fadeTime);
        if (fadeOutBGMPlayer != null) fadeOutBGMPlayer.stopBGM(fadeTime);
    }
    public void update()
    {
        if (curBGMPlayer != null) curBGMPlayer.update();
        if (fadeOutBGMPlayer != null) fadeOutBGMPlayer.update();
    }
    public bool curBgmNull()
    {
        if (curBGMPlayer != null)
        {
            return false;
        }
        return true;
    }
    public void BGMPlayerDelete()
    {
        if (curBGMPlayer != null) curBGMPlayer.destory();
        if (fadeOutBGMPlayer != null) fadeOutBGMPlayer.destory();
    }
}