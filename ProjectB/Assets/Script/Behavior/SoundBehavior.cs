using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBehavior : MonoBehaviour
{
    [SerializeField] private AudioSource audio;

    private SoundManager manager;
    private AudioClip clip;
    private float duration;
    private float elasped;
    private bool isInit;

    public void Init(SoundEvent soundEvent)
    {
        manager = SoundManager.Instance;
        clip = manager.ResourcePool.Load<AudioClip>(soundEvent.soundClip.RuntimeKey);

        transform.SetParent(manager.transform);
        duration = soundEvent.duration == 0 ? clip.length : soundEvent.duration;
        audio.clip = clip;
        audio.volume = soundEvent.volume;
        audio.Play();
        audio.outputAudioMixerGroup = SoundManager.Instance.AudioMixer.FindMatchingGroups(GetAudioMixerType(soundEvent.soundType))[0];

        elasped = 0;
        isInit = true;
    }

    public void UnInit()
    {
        isInit = false;
        clip = null;
        elasped = 0;
        manager.GameObjectPool.Return(gameObject);
    }

    private void Update()
    {
        UpdateFrame(Time.deltaTime);
    }

    public void UpdateFrame(float deltaTime)
    {
        if (isInit == false)
            return;

        elasped += deltaTime;
        if (duration < elasped)
            UnInit();
    }

    private string GetAudioMixerType(SoundEvent.eSoundType type)
    {
        switch (type)
        {
            case SoundEvent.eSoundType.Effect:
                return "Effect";
            case SoundEvent.eSoundType.Bgm:
                return "Bgm";
        }

        return "";
    }
}
