using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicManager : MonoBehaviour {
 
    public AudioSource audiosource;
 
    public static Sound _instance;
    void Awake () {
        audiosource = gameObject.AddComponent<AudioSource>();
        
        audiosource.playOnAwake = false;  //playOnAwake设为false时，通过调用play()方法启用
 
        _instance = this; //通过Sound._instance.方法调用
	}
	
    //在指定位置播放音频 PlayClipAtPoint()
    public void PlayAudioByName(string name)
    {
        //这里目标文件处在 Resources/Sounds/目标文件name
        AudioClip clip = Resources.Load<AudioClip>("Sounds/"+name);
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }
 
    //如果当前有其他音频正在播放，停止当前音频，播放下一个
    public void PlayMusicByName(string name)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + name);
 
        if (audiosource.isPlaying)
        {
            audiosource.Stop();
        }
 
        audiosource.clip = clip;
        audiosource.Play();
    }

