using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    [System.Serializable]
    public class AudioClipInfo
    {
        public string name;
        public AudioClip clip;
    }
    /*****singleton*****/
    public static SEManager instance
    {
        get
        {
            if (m_instance != null)
                return m_instance;
            m_instance = FindObjectOfType<SEManager>();
            if (m_instance != null)
                return m_instance;
            return null;
        }
    }
    protected static SEManager m_instance;
    /**********/
    [SerializeField] protected AudioSource m_audioSource;
    [SerializeField] protected List<AudioClipInfo> m_clipList;
    protected Dictionary<string, AudioClip> m_clipDictionary = new Dictionary<string,AudioClip>();
    void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (var i in m_clipList)
            m_clipDictionary[i.name] = i.clip;
        m_clipList.Clear();

    }
    //音量(0から1)をセットする
    public void SetVolume(float value)
    {
        m_audioSource.volume = value;
    }

    public float GetVolume()
    {
        return m_audioSource.volume;
    }
    //オーディオクリップから再生
    public void Play(AudioClip clip)
    {
        if(clip)
            m_audioSource.PlayOneShot(clip);
    }
    //オーディオクリップの名前から再生
    public void Play(string clipName)
    {
        if (m_clipDictionary.ContainsKey(clipName) && m_clipDictionary[clipName])
            m_audioSource.PlayOneShot(m_clipDictionary[clipName]);
    }
    //ミュート
    public void Mute()
    {
        m_audioSource.mute = true;
    }
    //ミュート解除
    public void UnMute()
    {
        m_audioSource.mute = false;
    }
}
