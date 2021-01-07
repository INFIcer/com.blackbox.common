using UnityEngine;
using UnityEditor;
[CustomPropertyDrawer(typeof(ConditionalHelpBoxAttribute))]
public class ConditionalHelpBoxAttributeDrawer : ConditionalDisplayAttributeDrawer
{
    //public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //{
    //    //base.OnGUI(position, property, label);
    //    ConditionHelpBoxAttribute condHAtt = (ConditionHelpBoxAttribute)attribute;

    //    EditorGUI.PropertyField(position, property, true);
    //    position.y += position.height / 3.5f;
    //    position.height *= 2.5f / 3.5f;
    //    if (property.boolValue == condHAtt.Bstate)
    //        EditorGUI.HelpBox(position, condHAtt.Msg, condHAtt.MsgT);
    //}
    public override void UnconditionaStyle(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, true);
    }
    public override void ConditionaStyle(Rect position, SerializedProperty property, GUIContent label)
    {
        position.y += position.height / 3.5f;
        position.height *= 2.5f / 3.5f;
        ConditionalHelpBoxAttribute condHAtt = (ConditionalHelpBoxAttribute)attribute;
        EditorGUI.HelpBox(position, condHAtt.Msg, (MessageType)((int)condHAtt.MsgT));
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalHelpBoxAttribute condHAtt = (ConditionalHelpBoxAttribute)attribute;
        //bool enabled = GetConditionalDisplayAttributeResult(condHAtt, property);
        bool enabled = GetConditionalDisplayAttributeResult(condHAtt, property);
        if (enabled)
        {
            return 3.5f * EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            //The property is not being drawn
            //We want to undo the spacing added before and after the property
            return EditorGUI.GetPropertyHeight(property, label);
            //return 0.0f;
        }
        //重写GetPropertyHeight 如果是需要隐藏的字段，计算字段高度让后面的字段不会被重叠绘制
    }
}
