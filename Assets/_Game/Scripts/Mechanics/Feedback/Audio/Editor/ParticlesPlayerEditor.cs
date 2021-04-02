using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ParticlesPlayer))]
public class ParticlesPlayerEditor : Editor
{
    bool showDebugFunctions;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);

        showDebugFunctions = GUILayout.Toggle(showDebugFunctions, "Show Debug Functions (Runtime Only)");

        if (showDebugFunctions)
        {
            ParticlesPlayer pp = target as ParticlesPlayer;

            GUILayout.Space(10);
            GUILayout.Label("Debug", EditorStyles.boldLabel);

            for (int i = 0; i < pp.GetNumParticles(); i++)
            {
                if (i > 0)
                    GUILayout.Space(5);

                GUILayout.Label("Particles #" + (i + 1), EditorStyles.miniBoldLabel);

                if (GUILayout.Button("Play Particles #" + (i + 1)))
                    pp.TryPlay(i);
                if (GUILayout.Button("Detach, Play, Destroy Particles #" + (i + 1)))
                    pp.TryDetachPlayThenDestroy(i);
                if (GUILayout.Button("Detach, Play, Reattach Particles #" + (i + 1)))
                    pp.TryDetachPlayThenReattach(i);
            }
        }
    }
}
