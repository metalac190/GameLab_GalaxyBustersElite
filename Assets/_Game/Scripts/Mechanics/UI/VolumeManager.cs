using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [Header("Volume Sliders")]
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider soundVolumeSlider;

    [SerializeField] AudioSliderAssistant audioSliderAssist;

    private void OnEnable()
    {
        masterVolumeSlider.value = GlobalAudioSliders.masterVolume;
        musicVolumeSlider.value = GlobalAudioSliders.musicVolume;
        soundVolumeSlider.value = GlobalAudioSliders.soundVolume;
    }

    private void Update()
    {
        audioSliderAssist.SetMasterVolume(masterVolumeSlider.value);
        audioSliderAssist.SetMusicVolume(musicVolumeSlider.value);
        audioSliderAssist.SetSoundVolume(soundVolumeSlider.value);
    }
}
