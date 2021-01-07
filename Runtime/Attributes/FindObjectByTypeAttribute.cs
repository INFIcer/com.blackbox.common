using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObjectByTypeAttribute : PropertyAttribute
{
    public Type type;

    public FindObjectByTypeAttribute(Type type)
    {
        this.type = type;

    }
}
