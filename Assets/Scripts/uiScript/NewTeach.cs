using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class NewTeach : MonoBehaviour
{
    public PlayableDirector director;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnStepComplete()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }
}
