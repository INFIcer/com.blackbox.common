﻿using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        /*
         * 绘制多值枚举选择框,0 全部不选, -1 全部选中, 其他是枚举之和
         * 枚举值 = 当前下标值 ^ 2
         * 默认[0^2 = 1 , 1 ^2 = 2,  4, 16 , .....]
         */
        property.intValue = EditorGUI.MaskField(position, label, property.intValue
                                                , property.enumNames);

        //Debug.Log("图层的值:" + property.intValue);
        //Debug.Log(Mathf.Clamp(2, 2, 1));

    }
}