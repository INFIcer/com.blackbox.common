using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumEventTableAttribute : PropertyAttribute
{
    public Type EventType;
    public EnumEventTableAttribute(Type EventType)
    {
        this.EventType = EventType;
    }
}
