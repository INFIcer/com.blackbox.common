using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumEventTableTest : MonoBehaviour
{
    public enum EventType { A, B }
    [EnumEventTable(typeof(EventType))]
    [SerializeField]
    Framework.EnumEventTable enumEventTable = new Framework.EnumEventTable();

    Action A;
    Action<int> B;
    private void setupCallback(ref Action action, EventType type)
    {
        if (enumEventTable.HasUnityEvent((int)type))
        {
            action += () => enumEventTable.Invoke((int)type);
        }
        else
        {
            action += () => { };
        }
    }

    private void setupCallback<T>(ref Action<T> action, EventType type)
    {
        if (enumEventTable.HasUnityEvent((int)type))
        {
            action += (h) => enumEventTable.Invoke((int)type);
        }
        else
        {
            action += (h) => { };
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        setupCallback(ref A, EventType.A);
        setupCallback(ref B, EventType.B);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
