using UnityEngine;
using System.Collections;

public class AudioExchange : MonoBehaviour
{

    public AudioSource backmusic;		// 背景音乐
    public AudioSource audiodie;		// 死亡音效
    public AudioSource victorySource;  // 胜利音效

    public GameObject ball; //小球对象

    public AudioSource[] audios;      //声音数组

    void Start()
    {
        audios[0].volume = UIdata.volumes[1] * UIdata.musicVolume;//设置背景音乐音量
        for (int i = 1; i < audios.Length-1; i++)
        {
            audios[i].volume = UIdata.volumes[1]*UIdata.soundVolume;//设置音效音量
        }

        audios[3].volume = UIdata.volumes[2] * UIdata.soundVolume;//死亡警告音量
        audios[audios.Length-1].volume = UIdata.volumes[2] * UIdata.soundVolume;//调节警告音量

        ball.GetComponent<CollisionEffect>().audio_hitcube.volume = UIdata.volumes[0] * UIdata.soundVolume;
        ball.GetComponent<CollisionEffect>().audio_hitzhui.volume = UIdata.volumes[2] * UIdata.soundVolume;
        ball.GetComponent<CollisionEffect>().audio_hitglass.volume = UIdata.volumes[2] * UIdata.soundVolume;
        ball.GetComponent<CollisionEffect>().audio_hitswitch.volume = UIdata.volumes[2] * UIdata.soundVolume;
    }

    void Update()
    {
        if (UIdata.sco == 0)
        {			// 如果小球数为0了
            audiodie.Play();			// 播放死亡音效
            backmusic.Stop();			// 暂停背景音乐
            this.enabled = false;		// 本脚本消失
        }
    }

    //游戏胜利播放音效
    public void victory()
    {
        //backmusic.clip = victory_audio; //切换胜利音效
        //backmusic.loop = false; //取消循环播放
        backmusic.Stop(); //暂停背景音乐
        victorySource.Play();//播放胜利音乐
    }

}