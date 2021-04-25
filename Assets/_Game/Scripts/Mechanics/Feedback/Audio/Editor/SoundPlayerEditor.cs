using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundPlayer))]
public class SoundPlayerEditor : Editor
{
    bool showDebugFunctions;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);

        showDebugFunctions = GUILayout.Toggle(showDebugFunctions, "Show Debug Functions (Runtime Only)");

        if (showDebugFunctions)
        {
            SoundPlayer sp = target as SoundPlayer;

            GUILayout.Space(10);
            GUILayout.Label("Debug", EditorStyles.boldLabel);

            for (int i = 0; i < sp.GetNumSounds(); i++)
            {
                if (i > 0)
                    GUILayout.Space(5);

                GUILayout.Label(sp.GetSoundLabel(i), EditorStyles.miniBoldLabel);

                if (GUILayout.Button("Play " + sp.GetSoundLabel(i)))
                    sp.TryPlay(i);
                if (GUILayout.Button("Stop " + sp.GetSoundLabel(i)))
                    sp.TryStop(i);
                if (GUILayout.Button("Detach, Play, Destroy " + sp.GetSoundLabel(i)))
                    sp.TryDetachPlayThenDestroy(i);
                if (GUILayout.Button("Detach, Play, Reattach " + sp.GetSoundLabel(i)))
                    sp.TryDetachPlayThenReattach(i);

                GUILayout.Space(3);
                if (GUILayout.Button("Sound Pooling " + sp.GetSoundLabel(i) + " x5"))
                    sp.TestSoundPoolingHelper(i, 5);
                if (GUILayout.Button("Sound Pooling " + sp.GetSoundLabel(i) + " x10"))
                    sp.TestSoundPoolingHelper(i, 10);
                if (GUILayout.Button("Sound Pooling " + sp.GetSoundLabel(i) + " x30"))
                    sp.TestSoundPoolingHelper(i, 30);
            }
        }
    }
}
