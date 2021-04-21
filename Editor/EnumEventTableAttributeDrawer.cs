using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(EnumEventTableAttribute))]
public class EnumEventTableAttributeDrawer : PropertyDrawer
{
    EnumEventTableAttribute Att;
    EditorFramework.EnumEventTableEditor _tableEditor;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (Att == null)
        {
            Att = (EnumEventTableAttribute)attribute;
        }
        drawEventTable(property);
    }
    private void drawEventTable(SerializedProperty property)
    {
        if (_tableEditor == null)
        {
            _tableEditor = new EditorFramework.EnumEventTableEditor(property, Att.EventType);
        }

        _tableEditor.DoGuiLayout();
    }
}
