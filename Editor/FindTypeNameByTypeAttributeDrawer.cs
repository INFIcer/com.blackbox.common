using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(FindTypeNameByTypeAttribute))]
public class FindTypeNameByTypeAttributeDrawer : PropertyDrawer
{
    public List<string> results = new List<string>();
    bool init;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        FindTypeNameByTypeAttribute Att = (FindTypeNameByTypeAttribute)attribute;
        if (!init)
        {
            results = Att.type.SelectInRelatedSubTypes((t) => t.Name);
            init = true;
        }

        EditorGUI.BeginProperty(position, label, property);
        //  position.width -= 36;


        if (results.Contains(property.stringValue))
            property.stringValue = results[EditorGUI.Popup(position, label.text, results.IndexOf(property.stringValue)
                                                    , results.ToArray())];
        else if (results.Count > 0)
            property.stringValue = results[0];
        else
        {
            property.stringValue = "null";
            EditorGUI.HelpBox(position, label.text + "没有搜索到可用的类", MessageType.Warning);
        }
        EditorGUI.EndProperty();
    }

}
