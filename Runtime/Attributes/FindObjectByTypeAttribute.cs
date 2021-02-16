using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObjectByTypeAttribute : PropertyAttribute
{
    public Type type;
    //public bool ignoreSelf;
    public bool button;
    public string overrideAddFuncName;
    public FindObjectByTypeAttribute(Type type, bool button = false)
    {
        this.type = type;
        //this.ignoreSelf = ignoreSelf;
        this.button = button;
    }

    public FindObjectByTypeAttribute(Type type, string overrideAddFuncName)
    {
        this.type = type;
        //this.ignoreSelf = ignoreSelf;
        this.button = true;
        this.overrideAddFuncName = overrideAddFuncName;
    }
}
