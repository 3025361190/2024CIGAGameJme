using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SignalTest : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Timeline≈‰÷√")]
    public PlayableDirector director;
    public TimelineAsset tutorialTimeline;

    [Header("–≈∫≈≈‰÷√")]
    public SignalAsset pauseSignal;
    public SignalAsset resumeSignal;

    private void Start()
    {
        InitializeTimeline();
        director.Play();
    }

    void InitializeTimeline()
    {
        director.playableAsset = tutorialTimeline;
        director.RebuildGraph();
    }

    // ‘›Õ£ ±º‰÷·
    public void PauseTimeline()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        Debug.Log(" ±º‰÷·“—‘›Õ£");
    }

    // ª÷∏¥≤•∑≈
    public void ResumeTimeline()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        Debug.Log(" ±º‰÷·ª÷∏¥≤•∑≈");
    }
}
