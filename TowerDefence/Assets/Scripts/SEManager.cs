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

        m_audioSource.volume = PlayerPrefs.GetFloat("SEVolume", 1f);

    }
    public void SetVolume(float value)
    {
        m_audioSource.volume = value;
        PlayerPrefs.SetFloat("SEVolume", value);
    }
    public float GetVolume()
    {
        return m_audioSource.volume;
    }
    public void Play(AudioClip clip)
    {
        if(clip)
            m_audioSource.PlayOneShot(clip);
    }
    public void Play(string clipName)
    {
        if (m_clipDictionary.ContainsKey(clipName))
            Play(m_clipDictionary[clipName]);
    }
    public void Stop()
    {
        m_audioSource.Stop();
    }
    public void Pause()
    {
        m_audioSource.Pause();
    }
    public void Resume()
    {
        m_audioSource.UnPause();
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
