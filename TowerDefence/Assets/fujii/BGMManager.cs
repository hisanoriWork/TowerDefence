using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [System.Serializable]
    public class AudioClipInfo
    {
        public string name;
        public AudioClip clip;
    }
    /*****singleton*****/
    public static BGMManager instance
    {
        get
        {
            if (m_instance != null)
                return m_instance;
            m_instance = FindObjectOfType<BGMManager>();
            if (m_instance != null)
                return m_instance;
            return null;
        }
    }
    protected static BGMManager m_instance;
    /**********/
    [SerializeField] protected AudioSource m_audioSource;
    [SerializeField] protected List<AudioClipInfo> m_clipList;
    protected Dictionary<string, AudioClip> m_clipDictionary = new Dictionary<string, AudioClip>();
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
    public void SetVolume(float value)
    {
        m_audioSource.volume = value;
    }
    public void Play(AudioClip clip)
    {
        if (clip)
            m_audioSource.PlayOneShot(clip);
    }

    public void Play(string clipName)
    {
        if (m_clipDictionary.ContainsKey(clipName) && m_clipDictionary[clipName])
            m_audioSource.PlayOneShot(m_clipDictionary[clipName]);
    }

    public void Mute()
    {
        m_audioSource.mute = true;
    }

    public void UnMute()
    {
        m_audioSource.mute = false;
    }
}
