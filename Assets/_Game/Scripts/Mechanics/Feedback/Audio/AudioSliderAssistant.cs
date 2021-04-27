using UnityEngine;

public class AudioSliderAssistant : MonoBehaviour
{
    public void SetMasterVolume(float setMasterVolume)
    {
        GlobalAudioSliders.masterVolume = setMasterVolume;
        RefreshAllSoundPlayerVolumes();
        RefreshMusicPlayerVolume();
    }
    public void SetSoundVolume(float setSoundVolume)
    {
        GlobalAudioSliders.soundVolume = setSoundVolume;
        RefreshAllSoundPlayerVolumes();
    }
    public void SetMusicVolume(float setMusicVolume)
    {
        GlobalAudioSliders.musicVolume = setMusicVolume;
        RefreshMusicPlayerVolume();
    }

    void RefreshAllSoundPlayerVolumes()
    {
        foreach (SoundPlayer soundPlayer in FindObjectsOfType<SoundPlayer>())
            soundPlayer.RefreshSoundVolumes();
    }
    void RefreshMusicPlayerVolume() { MusicPlayer.instance.RefreshMusicVolume(); }

    // Testing
    //[Range(0, 1)]
    //public float masterVolumeTesting = 1;
    //[Range(0, 1)]
    //public float soundVolumeTesting = 1;
    //[Range(0, 1)]
    //public float musicVolumeTesting = 1;

    //private void Start()
    //{
    //    masterVolumeTesting = GlobalAudioSliders.masterVolume;
    //    soundVolumeTesting = GlobalAudioSliders.musicVolume;
    //    musicVolumeTesting = GlobalAudioSliders.soundVolume;
    //}

    //private void Update()
    //{
    //    SetMasterVolume(masterVolumeTesting);
    //    SetSoundVolume(soundVolumeTesting);
    //    SetMusicVolume(musicVolumeTesting);
    //}
}
