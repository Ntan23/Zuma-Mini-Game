using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance {get; private set;}
    void Awake()
    {
        if(instance == null) 
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion
    [SerializeField] private Audio[] audios;

    private void Play(string name)
    {
        Audio s = System.Array.Find(audios, sound => sound.name == name);

        if(s == null) return;

        s.source.Play();
    }

    private void Stop(string name)
    { 
        Audio s = System.Array.Find(audios, sound => sound.name == name);

        if(s == null) return;

        s.source.Stop();
    }


    void Start() 
    {
        foreach (Audio s in audios)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        Play("BGM");
    } 

    public void StopAllSFX()
    {
        foreach(Audio s in audios) 
        {
            if(s.name != "BGM") s.source.Stop();
        }
    }

    public void PlaySFX(string name) => Play(name);
} 

