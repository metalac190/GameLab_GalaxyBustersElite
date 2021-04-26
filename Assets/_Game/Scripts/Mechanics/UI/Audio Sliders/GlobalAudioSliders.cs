public static class GlobalAudioSliders
{
    public static float masterVolume = 1;
    public static float musicVolume = 1;
    public static float soundVolume = 1;

    // Set to true anytime any volume value is changed.
    // This tells the Sound and Music Players to refresh the volumes of their AudioSources.
    public static bool anySliderValueChanged = false;
}
