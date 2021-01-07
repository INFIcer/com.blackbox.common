using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ConditionalDisplayAttribute))]
public class ConditionalDisplayAttributeDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalDisplayAttribute condHAtt = (ConditionalDisplayAttribute)attribute;
        UnconditionaStyle(position, property, label);
        GUI.enabled = GetConditionalDisplayAttributeResult(condHAtt, property);
        if (GUI.enabled)
            ConditionaStyle(position, property, label);
    }
    public virtual void UnconditionaStyle(Rect position, SerializedProperty property, GUIContent label) { }
    public virtual void ConditionaStyle(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label, true);
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalDisplayAttribute condHAtt = (ConditionalDisplayAttribute)attribute;
        bool enabled = GetConditionalDisplayAttributeResult(condHAtt, property);
        if (enabled)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            //The property is not being drawn
            //We want to undo the spacing added before and after the property
            return -EditorGUIUtility.standardVerticalSpacing;
            //return 0.0f;
        }
        //重写GetPropertyHeight 如果是需要隐藏的字段，计算字段高度让后面的字段不会被重叠绘制
    }

    public bool GetConditionalDisplayAttributeResult(ConditionalDisplayAttribute condHAtt, SerializedProperty property)
    {
        //if (condHAtt.Uncondition)
        //    return true;
        //bool result = false;
        //foreach (string Condition in condHAtt.Conditions)
        //{
        //    var s = Condition.Split(' ');
        //    int i;
        //    //bool and;
        //    SerializedProperty SourcePropertyValue = property.serializedObject.FindProperty(s[0]);
        //    //switch (s[1])
        //    //{
        //    //    case "==":
        //    //        break;
        //    //    case "!=":
        //    //        break;
        //    //    case ">=":
        //    //        break;
        //    //    case "<=":
        //    //        break;
        //    //}
        //    if (int.TryParse(s[1], out i))
        //        result = Framework.intersect(SourcePropertyValue.intValue, i);
        //    else
        //        result = s[1] == "true" ? SourcePropertyValue.boolValue : !SourcePropertyValue.boolValue;
        //}
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(condHAtt.ConditionalSourceField);
        SerializedProperty sourcePropertyValue2 = property.serializedObject.FindProperty(condHAtt.ConditionalSourceField2);
        if (condHAtt.Uncondition)
            return true;
        bool result = false;
        if (condHAtt.type == typeof(int))
        {
            if (Framework.intersect(sourcePropertyValue.intValue, condHAtt.state))
                result = true;
        }
        else if (condHAtt.type == typeof(long))
        {
            if (Framework.intersect(sourcePropertyValue.intValue, condHAtt.state) && Framework.intersect(sourcePropertyValue2.intValue, condHAtt.state2))
                result = true;
        }
        else if (condHAtt.type == typeof(bool))
        {
            if (sourcePropertyValue.boolValue == condHAtt.Bstate)
                result = true;
        }
        else if (condHAtt.type == typeof(int[]))
        {
            foreach (int i in condHAtt.states)
                if (Framework.intersect(sourcePropertyValue.intValue, i))
                {
                    result = true;
                    break;
                }
        }
        else if (condHAtt.type == typeof(string))
        {
            if (sourcePropertyValue.boolValue == condHAtt.Bstate && Framework.intersect(sourcePropertyValue2.intValue, condHAtt.state))
                result = true;
        }
        return condHAtt.Inverse ? !result : result;
    }
    //调用GetConditionalDisplayAttributeResult函数来检查字段是否应该显示。

}