using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FindTypeMode
{
    Base,
    Subclass,
    Interface,
    Attribute,
}

public class FindTypeNameByTypeAttribute : PropertyAttribute
{
    public Type type;
 
    public FindTypeNameByTypeAttribute(Type type)
    {
        this.type = type;
       
    }
}
