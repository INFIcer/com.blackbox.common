using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ConditionalSliderAttribute))]
public class ConditionalSliderAttributeDrawer : ConditionalDisplayAttributeDrawer
{
    public override void ConditionaStyle(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalSliderAttribute condHAtt = (ConditionalSliderAttribute)attribute;
        ValueRange(position, property, label, condHAtt.min, condHAtt.max, condHAtt.slider);
    }
    public void ValueRange(Rect position, SerializedProperty property, GUIContent label, float min, float max, bool slider)
    {
        if (slider)
        {
            if (property.type == "int")
                property.intValue = Mathf.RoundToInt(EditorGUI.Slider(position, label, property.intValue, min, max));
            else if (property.type == "float")
                property.floatValue = EditorGUI.Slider(position, label, property.floatValue, min, max);
        }
        else
        {
            if (property.type == "int")
                property.intValue = Mathf.RoundToInt(Mathf.Clamp(property.intValue, min, max));
            else if (property.type == "float")
                property.floatValue = Mathf.Clamp(property.floatValue, min, max);
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}