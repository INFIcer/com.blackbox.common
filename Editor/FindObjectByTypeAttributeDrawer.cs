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
    FindObjectByTypeAttribute Att;
    //public Type type;
    bool init;
    bool error;
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) + (error ? 2 : 0) * EditorGUIUtility.singleLineHeight;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = EditorGUIUtility.singleLineHeight;
        if (!init)
        {
            Att = (FindObjectByTypeAttribute)attribute;
            init = true;
        }
        error = false;
        DrawProperty(position, property, label);
        if (error)
        {
            position.y += EditorGUIUtility.singleLineHeight;
            position.height = 2 * EditorGUIUtility.singleLineHeight;
            EditorGUI.HelpBox(position, "对象类型不匹配", MessageType.Error);
        }
        //property.serializedObject.ApplyModifiedProperties();
    }
    public void DrawProperty(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.isArray)
            DrawArray(position, property, label);
        else
            DrawElement(position, property, label);
    }
    public void DrawArray(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        {
            for(int i=0;i< property.arraySize;i++)
                DrawProperty(position, property.GetArrayElementAtIndex(i), label);
        }
        EditorGUI.EndProperty();
    }
    public void DrawElement(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        {
            var ObjectPosition = new Rect(position) { width = position.width - 35 };
            var ButtonPosition = new Rect(position) { x = ObjectPosition.xMax, width = 35 };
            UnityEngine.Object obj;
            if (property.objectReferenceValue)
                obj = (UnityEngine.Object)EditorGUI.ObjectField(Att.button ? ObjectPosition : position, label, property.objectReferenceValue, typeof(UnityEngine.Object), true);
            else
                obj = (UnityEngine.Object)EditorGUI.ObjectField(Att.button ? ObjectPosition : position, label, null, typeof(UnityEngine.Object), true);
            if (Att.button)
            {

                if (obj == null)
                {
                    if (GUI.Button(ButtonPosition, "+"))
                    {
                        if (Att.overrideAddFuncName != "")
                        {
                            //foreach (var m in property.serializedObject.targetObject.GetType().GetMethods())
                            //    Debug.Log(m.Name);
                            //Debug.Log("=======================");
                            //foreach (var m in property.serializedObject.targetObject.GetType().GetRuntimeMethods())
                            //    Debug.Log(m.Name);
                            //Debug.Log(property.serializedObject.targetObject);
                            obj = (UnityEngine.Object)property.serializedObject.targetObject.GetType().GetRuntimeMethods().ToList().Find((x) => x.Name == (Att.overrideAddFuncName)).Invoke(property.serializedObject.targetObject, new object[] { property.serializedObject.targetObject.GetType(), property });
                            //property.serializedObject.ApplyModifiedProperties();
                        }
                        else
                            EditorFramework.Menu(
                               Att.type.SelectInRelatedSubTypes((x) => !x.IsAbstract && (x.IsSubclassOf(typeof(Component))), (x2) => x2),
                                (type) => { return type; },
                                (o) =>
                                {
                                    var type = o as Type;
                                    if (type.IsSubclassOf(typeof(Component)))
                                    {
                                        obj = Undo.AddComponent((property.serializedObject.targetObject as Component).gameObject, type);
                                        property.objectReferenceValue = obj;
                                        property.serializedObject.ApplyModifiedProperties();
                                    //Debug.Log(obj);
                                }
                                //else if (type.IsSubclassOf(typeof(ScriptableObject)))
                                //{

                                //    obj = ScriptableObject.CreateInstance(type);
                                //    Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
                                //        // obj = Undo.AddComponent((property.serializedObject.targetObject as Component).gameObject, type);
                                //        property.objectReferenceValue = obj;
                                //    property.serializedObject.ApplyModifiedProperties();
                                //    Debug.Log(obj);
                                //}

                            }
                                );
                        //obj = Undo.AddComponent((property.serializedObject.targetObject as Component).gameObject, Att.type);
                    }
                }
                else
                {
                    if (GUI.Button(ButtonPosition, "╳"))
                    {
                        Undo.DestroyObjectImmediate(obj);
                    }
                }
            }



            if (obj == null)
            {
                property.objectReferenceValue = null;
                return;
            }
            Type t = obj.GetType();
            if (
                t.isRelatedSubTypeOf(Att.type)
            //    (Att.type.IsInterface && t.GetInterface(Att.type.Name) != null)//obj是一个实现了Att接口的类的实例
            //|| (Att.type.IsSubclassOf(typeof(Attribute)) && t.IsDefined(Att.type))//obj是一个挂载了Att特性的类的实例
            //|| t.IsSubclassOf(Att.type)//obj是一个Att子类的实例
            //|| t == Att.type//obj是一个Att类的实例
            )
            {
                property.objectReferenceValue = obj;
                
            }
            else
            {
                error = true;
                //Debug.LogError("对象类型不匹配");
            }
        }
        EditorGUI.EndProperty();
    }
}
