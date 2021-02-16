using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public static class EditorFramework 
{
    public static void Menu<T>(List<T> list, Func<T, object> objFunc, GenericMenu.MenuFunction2 menuFunction2)
    {
        GenericMenu menu = new GenericMenu();
        for (int i = 0; i < list.Count; i++)
        {
            menu.AddItem(new GUIContent(list[i].ToString())
                , false, menuFunction2, objFunc.Invoke(list[i]));
        }
        menu.ShowAsContext();
    }
    public static void Menu<T>(List<T> list, object noneObj, Func<T, object> objFunc, GenericMenu.MenuFunction2 menuFunction2)
    {
        GenericMenu menu = new GenericMenu();

        menu.AddItem(new GUIContent("None")
                           , false, menuFunction2, noneObj);
        for (int i = 0; i < list.Count; i++)
        {
            menu.AddItem(new GUIContent(list[i].ToString())
                , false, menuFunction2, objFunc.Invoke(list[i]));
        }
        menu.ShowAsContext();
    }
}
