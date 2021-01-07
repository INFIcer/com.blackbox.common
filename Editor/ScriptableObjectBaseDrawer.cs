using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;
using System.IO;
using UnityEditorInternal;
using UnityEngine.Events;

public struct ListData
{
    public ReorderableList list;
    public string typeName;
}
public enum CreateMode
{
    //ScriptableObjectBase = 1 << 0,
    //ScriptableObjectSubClass = 1 << 1,
    //ComponentSubClass = 1 << 2,
    ScriptableObject,
    Component
}
public struct ListElementData
{
    public SerializedProperty property;
    public string typeName;
}
public class ScriptableObjectBaseDrawer : PropertyDrawer
{
    Vector2 pos;
    protected CreateMode createMode = CreateMode.ScriptableObject;
    protected FindTypeMode findTypeMode = FindTypeMode.Subclass;
    protected Type type;
    protected float height = 1;
    protected static float unityEventHeight = 5;
    protected string path = "Gen";

    SerializedObject value;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = EditorGUIUtility.singleLineHeight;
        position.width -= 36;
        EditorGUI.BeginProperty(position, label, property);
        property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, type, true);
        EditorGUI.EndProperty();
        // using (new EditorGUI.PropertyScope(position, label, property))
        {
            // 
            // UnityEngine.Object newValue=null;



            if (property.objectReferenceValue == null)
            {
                if (GUI.Button(new Rect(position) { x = position.xMax, width = 36 }, "+"))
                {
                    Menu(findTypeMode, type,
                        (t) => new ValueTuple<SerializedProperty, Type>(property, t),
                        (object target) =>
                        {
                            ValueTuple<SerializedProperty, Type> data = (ValueTuple<SerializedProperty, Type>)target;
                            SerializedProperty element = data.Item1;
                            switch (createMode)
                            {
                                case CreateMode.ScriptableObject:
                                    element.objectReferenceValue = CreateAsset(element.serializedObject, path, data.Item2.Name);
                                    //Undo.RegisterCreatedObjectUndo(element.objectReferenceValue, "Add ScriptableObject");
                                    break;
                                case CreateMode.Component:
                                    element.objectReferenceValue = (element.serializedObject.targetObject as Component).gameObject.AddComponent(data.Item2);
                                    Undo.RegisterCreatedObjectUndo(element.objectReferenceValue, "Add Component");
                                    break;
                            }
                            element.serializedObject.ApplyModifiedProperties();
                        });
                }
            }
            else
            {
                if (GUI.Button(new Rect(position) { x = position.xMax, width = 36 }, "×"))
                {
                    //remove(property);
                    switch (createMode)
                    {
                        case CreateMode.ScriptableObject:
                            // if (EditorUtility.DisplayDialog("Warnning", "Do you want to remove this asset?", "Remove", "Cancel"))
                            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetOrScenePath(property.objectReferenceValue));
                            //else
                            //{
                            // property.objectReferenceValue = null; 
                            //}
                            break;
                        case CreateMode.Component:
                            // if (EditorUtility.DisplayDialog("Warnning", "Do you want to remove this component?", "Remove", "Cancel"))
                            Undo.DestroyObjectImmediate(property.objectReferenceValue);
                            //else
                            //{
                            //    property.objectReferenceValue = null;
                            //}
                            break;
                    }
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
            //position.y += EditorGUIUtility.singleLineHeight;
            //position.width += 36;
            //switch (createMode)
            //{
            //    case CreateMode.ScriptableObject:
            //        //if (property.objectReferenceValue != null)
            //        //{
            //        //    value = new SerializedObject(property.objectReferenceValue);
            //        //    value.ApplyModifiedProperties();
            //        //}
            //        property.serializedObject.ApplyModifiedProperties();
            //        break;
            //    case CreateMode.Component:
            //        property.serializedObject.ApplyModifiedProperties();
            //        break;
            //}  //
        }


        // return;

        if (property.objectReferenceValue == null)
        {
            goto end;
        }
        /// 
        value = new SerializedObject(property.objectReferenceValue);
        position.y += EditorGUIUtility.singleLineHeight;
        position.width += 36;
        var v = new Rect(position) { height = height * EditorGUIUtility.singleLineHeight };
        var fs = (from f in property.objectReferenceValue.GetType().GetRuntimeFields()
                  where (f.FieldType.IsDefined(typeof(System.SerializableAttribute)) || f.FieldType.Namespace == (nameof(UnityEngine)) || f.FieldType.IsSubclassOf(typeof(UnityEngine.Object)))
                  &&
                  ((f.IsPublic && !f.IsDefined(typeof(HideInInspector))) || f.IsDefined(typeof(SerializeField)))
                  select f).ToList();
        var ueNum = fs.FindAll((f) => f.FieldType == typeof(UnityEvent)).Count();
        position.width -= 36;

        pos = GUI.BeginScrollView(v, pos, new Rect(position) { height = (fs.Count + (unityEventHeight - 1) * ueNum) * EditorGUIUtility.singleLineHeight });
        position.x += 18;
        foreach (var f in fs)
        {
            if (f.FieldType == typeof(UnityEvent))
            {
                position.height *= unityEventHeight;
            }
            EditorGUI.PropertyField(position, value.FindProperty(f.Name));
            if (f.FieldType == typeof(UnityEvent))
            {
                unityEventHeight = EditorGUI.Slider(new Rect(position) { x = position.x + 120, width = position.width - 120, height = EditorGUIUtility.singleLineHeight }, unityEventHeight, 5, 20);
                position.height /= unityEventHeight;
            }
            position.y += (f.FieldType == typeof(UnityEvent) ? unityEventHeight : 1) * EditorGUIUtility.singleLineHeight;

        }
        GUI.EndScrollView();
        value.ApplyModifiedProperties();
        end: { }




    }
    void remove(SerializedProperty property)
    {
        switch (createMode)
        {
            case CreateMode.ScriptableObject:
                if (EditorUtility.DisplayDialog("Warnning", "Do you want to remove this asset?", "Remove", "Cancel"))
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetOrScenePath(property.objectReferenceValue));
                else
                {
                    property.objectReferenceValue = null;
                }
                break;
            case CreateMode.Component:
                if (EditorUtility.DisplayDialog("Warnning", "Do you want to remove this component?", "Remove", "Cancel"))
                    Undo.DestroyObjectImmediate(property.objectReferenceValue);
                else
                {
                    property.objectReferenceValue = null;
                }
                break;
        }
        property.serializedObject.ApplyModifiedProperties();
    }
    private void ClickHandler(object target)
    {
        ListElementData data = (ListElementData)target;
        SerializedProperty element = data.property;
        element.objectReferenceValue = CreateAsset(element.serializedObject, path, data.typeName);
        element.serializedObject.ApplyModifiedProperties();
    }

    private static void ClickHandler2(object target)
    {
        //var data = (ValueTuple<SerializedProperty, Type>)target;
        //int index = data.list.serializedProperty.arraySize;
        //data.list.serializedProperty.arraySize++;
        //data.list.index = index;
        //SerializedProperty element = data.list.serializedProperty.GetArrayElementAtIndex(index);



        //GenericMenu menu = new GenericMenu();
        //var Types = (from t in Assembly.GetAssembly(type).GetTypes()
        //             where t.IsSubclassOf(type)
        //             select t).ToList();
        //switch (mode)
        //{
        //    case CreateMode.ScriptableObjectBase:
        //        property.objectReferenceValue = CreateAsset(property.serializedObject, path, type.Name);
        //        property.serializedObject.ApplyModifiedProperties();
        //        break;
        //    case CreateMode.ScriptableObjectSubClass:
        //        foreach (var t in Types)
        //        {
        //            menu.AddItem(new GUIContent(t.Name)
        //            , false, (object target) =>
        //            {
        //                ListElementData data = (ListElementData)target;
        //                SerializedProperty element = data.property;
        //                element.objectReferenceValue = CreateAsset(element.serializedObject, path, data.typeName);
        //                element.serializedObject.ApplyModifiedProperties();
        //            }, new ListElementData() { property = property, typeName = t.Name });
        //        }
        //        menu.ShowAsContext();
        //        break;
        //    case CreateMode.ComponentSubClass:
        //        Menu(FindTypeMode.Subclass, type,
        //            (object target) =>
        //            {
        //                ValueTuple<SerializedProperty, Type> data = (ValueTuple<SerializedProperty, Type>)target;
        //                SerializedProperty element = data.Item1;
        //                element.objectReferenceValue = (property.serializedObject.targetObject as Component).gameObject.AddComponent(data.Item2);
        //                element.serializedObject.ApplyModifiedProperties();
        //            }, (t) => new ValueTuple<SerializedProperty, Type>(property, t));
        //        break;
        //}


        //if (data.typeName != "")
        //{
        //    element.objectReferenceValue = CreateAsset(element.serializedObject, CreateAssetPath, data.typeName);
        //}
        //else
        //    element.objectReferenceValue = null;
        //data.list.serializedProperty.serializedObject.ApplyModifiedProperties();
    }
    public static void DrawHeader(ReorderableList list, Rect rect, string label, float ElementHeight)
    {
        GUI.Label(rect, label);
        //  ElementHeight = EditorGUI.Slider(new Rect(rect) { x = rect.x + 120, width = rect.width - 120 }, ElementHeight, 1, 20);
        list.elementHeight = ElementHeight * EditorGUIUtility.singleLineHeight;
    }
    public static void DrawHeader(ReorderableList list, Rect rect, string label, ref float ElementHeight)
    {
        GUI.Label(rect, label);
        ElementHeight = EditorGUI.Slider(new Rect(rect) { x = rect.x + 120, width = rect.width - 120 }, ElementHeight, 1, 20);
        list.elementHeight = ElementHeight * EditorGUIUtility.singleLineHeight;
    }
    public static void DrawElement(ReorderableList list, Rect rect, int index, bool selected, bool focused)
    {
        SerializedProperty item = list.serializedProperty.GetArrayElementAtIndex(index);
        rect.height = EditorGUIUtility.singleLineHeight;
        rect.y += 2;
        if (item.objectReferenceValue != null)
            EditorGUI.PropertyField(rect, item, new GUIContent(item.objectReferenceValue.GetType().Name));
        else
            EditorGUI.PropertyField(rect, item, new GUIContent("null"));

    }
    public static string CreateAssetPath = "Gen";
    public static void AddMenu(FindTypeMode findTypeMode, CreateMode createMode, ReorderableList list, Type type)
    {
        Menu(findTypeMode, type,
            new ValueTuple<SerializedProperty, Type>(list.serializedProperty, null),
            (t) => new ValueTuple<SerializedProperty, Type>(list.serializedProperty, t),
            (object target) =>
            {
                int index = list.serializedProperty.arraySize;
                list.serializedProperty.arraySize++;
                list.index = index;

                var data = (ValueTuple<SerializedProperty, Type>)target;
                SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);

                if (data.Item2 != null)
                {
                    switch (createMode)
                    {
                        case CreateMode.ScriptableObject:
                            element.objectReferenceValue = CreateAsset(list.serializedProperty.serializedObject, CreateAssetPath, data.Item2.Name);
                            Undo.RegisterCreatedObjectUndo(element.objectReferenceValue, "Add ScriptableObject");
                            break;
                        case CreateMode.Component:
                            element.objectReferenceValue = (list.serializedProperty.serializedObject.targetObject as Component).gameObject.AddComponent(data.Item2);
                            Undo.RegisterCreatedObjectUndo(element.objectReferenceValue, "Add Component");
                            break;
                    }
                }
                else
                    element.objectReferenceValue = null;
                element.serializedObject.ApplyModifiedProperties();
            });
        list.serializedProperty.serializedObject.ApplyModifiedProperties();
    }
    public delegate object AddMenuItemCallBack(Type type);

    public static void Menu(FindTypeMode findTypeMode, Type type, object noneObj, AddMenuItemCallBack objFunc, GenericMenu.MenuFunction2 menuFunction2)
    {
        GenericMenu menu = new GenericMenu();
        List<Type> Types = FindTypes(findTypeMode, type);
        menu.AddItem(new GUIContent("None")
                           , false, menuFunction2, noneObj);
        foreach (var t in Types)
        {
            menu.AddItem(new GUIContent(t.Name)
                , false, menuFunction2, objFunc.Invoke(t));
        }
        menu.ShowAsContext();
    }
    public static void Menu(FindTypeMode findTypeMode, Type type, AddMenuItemCallBack objFunc, GenericMenu.MenuFunction2 menuFunction2)
    {
        GenericMenu menu = new GenericMenu();
        List<Type> Types = FindTypes(findTypeMode, type);
        foreach (var t in Types)
        {
            menu.AddItem(new GUIContent(t.Name)
                , false, menuFunction2, objFunc.Invoke(t));
        }
        menu.ShowAsContext();
    }
    public static List<Type> FindTypes(FindTypeMode findTypeMode, Type type)
    {
        List<Type> Types = new List<Type>();
        if (findTypeMode == FindTypeMode.Base)
        {
            Types.Add(type);
            return Types;
        }
        switch (findTypeMode)
        {
            case FindTypeMode.Subclass:
                Types = (from t in AllTypes
                         where t.IsSubclassOf(type)
                         select t).ToList();
                break;
            case FindTypeMode.Interface:
                Types = (from t in AllTypes
                         where t.GetInterface(type.Name) != null && t.IsClass
                         select t).ToList();
                break;
            case FindTypeMode.Attribute:
                Types = (from t in AllTypes
                         where t.IsDefined(type)
                         select t).ToList();
                break;
        }
        return Types;
    }
    public static List<Type> AllTypes
    {
        get
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var typesList = (from a in assemblies
                             select a.GetTypes()).ToList();
            var types = new List<Type>();
            foreach (var t in typesList)
            {
                types = types.Union(t).ToList();
            }
            return types;
        }
    }
    //public static void MenuByAll(FindTypeMode findTypeMode, Type type, GenericMenu.MenuFunction2 menuFunction2, AddMenuItemCallBack objFunc)
    //{
    //    GenericMenu menu = new GenericMenu();
    //    var assemblies = AppDomain.CurrentDomain.GetAssemblies();

    //    var typesList = (from a in assemblies
    //                     select a.GetTypes()).ToList();
    //    var types = new List<Type>();
    //    foreach (var t in typesList)
    //    {
    //        types = types.Union(t).ToList();
    //    }
    //    List<Type> Types = new List<Type>();
    //    switch (findTypeMode)
    //    {
    //        case FindTypeMode.Subclass:
    //            Types = (from t in types
    //                     where t.IsSubclassOf(type)
    //                     select t).ToList();
    //            break;
    //        case FindTypeMode.Interface:
    //            Types = (from t in types
    //                     where t.GetInterface(type.Name) != null
    //                     select t).ToList();
    //            break;
    //        case FindTypeMode.Attribute:
    //            Types = (from t in types
    //                     where t.IsDefined(type)
    //                     select t).ToList();
    //            break;
    //    }
    //    foreach (var t in Types)
    //    {
    //        menu.AddItem(new GUIContent(t.Name)
    //            , false, menuFunction2, objFunc.Invoke(t));
    //    }
    //    menu.ShowAsContext();
    //}
    public static void Remove(CreateMode createMode, ReorderableList list)
    {
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(list.index);
        //
        if (element.objectReferenceValue != null)
        {
            switch (createMode)
            {
                case CreateMode.ScriptableObject:
                    if (EditorUtility.DisplayDialog("Warnning", "Do you want to delete this element's Asset?", "Delete", "Cancel"))
                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetOrScenePath(element.objectReferenceValue));
                    break;
                case CreateMode.Component:
                    if (EditorUtility.DisplayDialog("Warnning", "Do you want to remove this component?", "Remove", "Cancel"))
                        Undo.DestroyObjectImmediate(element.objectReferenceValue);
                    break;
            }
            ReorderableList.defaultBehaviours.DoRemoveButton(list);
        }
        ReorderableList.defaultBehaviours.DoRemoveButton(list);
    }
    public static ScriptableObject CreateAsset(SerializedObject serializedObject, string path, string typeName)
    {
        //Debug.Log(Application.dataPath + "/" + path);
        if (!Directory.Exists(Application.dataPath + "/" + path))
        {
            Directory.CreateDirectory(Application.dataPath + "/" + path);
            AssetDatabase.Refresh();
        }

        string FileName = serializedObject.targetObject.name + typeName;
        string expand = "asset";
        int IndName = 0;
        while (File.Exists("Assets/" + path + "/" + FileName + (IndName == 0 ? "" : "(" + IndName + ")") + "." + expand))
            IndName++;
        path += "/" + FileName + (IndName == 0 ? "" : "(" + IndName + ")") + "." + expand;
        AssetDatabase.CreateAsset(UnityEngine.ScriptableObject.CreateInstance(typeName), "Assets/" + path);
        return (AssetDatabase.LoadAssetAtPath<ScriptableObject>("Assets/" + path));
    }
}