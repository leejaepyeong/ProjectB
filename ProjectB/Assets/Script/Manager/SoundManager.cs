using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : BaseManager
{
    [SerializeField] private AudioMixer audioMixer;
    public AudioMixer AudioMixer => audioMixer;

    public static SoundManager Instance
    {
        get { return Manager.Instance.GetManager<SoundManager>(); }
    }

    public void MasterAudioControl(float value)
    {
        value = Mathf.Clamp(value, -40,0);
        audioMixer.SetFloat("Master", value);
    }

    public void BgmrAudioControl(float value)
    {
        value = Mathf.Clamp(value, -40, 0);
        audioMixer.SetFloat("Bgm", value);
    }
    public void EffectAudioControl(float value)
    {
        value = Mathf.Clamp(value, -40, 0);
        audioMixer.SetFloat("Effect", value);
    }
}
