using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeEvent : MonoBehaviour
{
    public float RestTime = 10;
    public float TimingSpeed;
    public float Time = 10;
    public bool isPlaying;
    public UnityEvent OnStartTiming = new UnityEvent();
    public UnityEvent OnPauseTiming = new UnityEvent();
    public UnityEvent OnStopTiming = new UnityEvent();
    public void StartTiming() { }
    public void PauseTiming() { }
    public void StopTiming() { Time = RestTime; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
