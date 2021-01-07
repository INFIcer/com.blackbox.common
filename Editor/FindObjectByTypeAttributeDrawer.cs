using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(FindObjectByTypeAttribute))]
public class FindObjectByTypeAttributeDrawer : PropertyDrawer
{
    public Type type;
    bool init;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        if (!init)
        {
            FindObjectByTypeAttribute Att = (FindObjectByTypeAttribute)attribute;

            //var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            //var typesList = (from a in assemblies
            //                 select a.GetTypes()).ToList();
            //var types = new List<Type>();
            //foreach (var t in typesList)
            //{
            //    types = types.Union(t).ToList();
            //}
            //var types = Framework.AllTypes;
            //获取所有类
            type = Att.type;
            //type = types.Find((t) => t.Name == Att.type.Name);//找到成员类
            init = true;
        }


        EditorGUI.BeginProperty(position, label, property);
        {
            UnityEngine.Component obj;
            if (property.objectReferenceValue)
                obj = (UnityEngine.Component)EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(UnityEngine.Component), true);
            else
                obj = (UnityEngine.Component)EditorGUI.ObjectField(position, label, null, typeof(UnityEngine.Component), true);
            if (obj == null)
                return;
            Type t = obj.GetType();
            if (
                (type.IsInterface && t.GetInterface(type.Name) != null && t.IsClass)
            || (type.IsSubclassOf(typeof(Attribute)) && t.IsDefined(type))
            || (t.IsSubclassOf(type))
            )
            {
                property.objectReferenceValue = obj;
            }
        }
        EditorGUI.EndProperty();
        //property.serializedObject.ApplyModifiedProperties();
    }
}
