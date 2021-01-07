using UnityEngine;
using System.Linq;
using UnityEditor;
[CustomPropertyDrawer(typeof(ConditionalDisplayOnlyAttribute))]
public class ConditionalDisplayOnlyAttributeDrawer : ConditionalDisplayAttributeDrawer
{
    //public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //{
    //    DisplayOnlyAttribute condHAtt = (DisplayOnlyAttribute)attribute;
    //    Framework.DisplayOnly(position, property, label, condHAtt.min, condHAtt.max, condHAtt.slider);
    //}
    public override void ConditionaStyle(Rect position, SerializedProperty property, GUIContent label)
    {
        //ConditionalDisplayOnlyAttribute condHAtt = (ConditionalDisplayOnlyAttribute)attribute;
        DisplayOnly(position, property, label);
    }
    public void DisplayOnly(Rect position, SerializedProperty property, GUIContent label)//, float min, float max, bool slider)
    {
        if (property.isArray)
        {
            position.x += 10;
            for (int i = 0; i < property.arraySize; i++)
                DisplayOnly(position, property.GetArrayElementAtIndex(i), label);
        }
        else
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    EditorGUI.LabelField(position, label.text, property.intValue.ToString());
                    break;
                case SerializedPropertyType.Float:
                    EditorGUI.LabelField(position, label.text, property.floatValue.ToString());
                    break;
                case SerializedPropertyType.String:
                    EditorGUI.LabelField(position, label.text, property.stringValue);
                    break;
                case SerializedPropertyType.Vector2:
                    EditorGUI.LabelField(position, label.text, property.vector2Value.ToString());
                    break;
                case SerializedPropertyType.Vector3:
                    EditorGUI.LabelField(position, label.text, property.vector3Value.ToString());
                    break;
                case SerializedPropertyType.Enum:
                    var list = property.enumNames.ToList();
                    if (list.Exists((e) => list.IndexOf(e) == property.enumValueIndex))
                        EditorGUI.LabelField(position, label.text, property.enumNames[property.enumValueIndex].ToString());
                    else
                        EditorGUI.LabelField(position, label.text, property.intValue.ToString()+ "(undefined enum name)");
                    break;
                case SerializedPropertyType.Boolean:
                    EditorGUI.LabelField(position, label.text, property.boolValue.ToString());
                    break;
                case SerializedPropertyType.ArraySize:
                    EditorGUI.LabelField(position, label.text, property.arraySize.ToString());
                    break;
                case SerializedPropertyType.Color:
                    EditorGUI.ColorField(position, property.colorValue);
                    break;
                case SerializedPropertyType.ObjectReference:
                    if (property.objectReferenceValue != null)
                        EditorGUI.LabelField(position, label.text, property.objectReferenceValue.ToString());
                    else
                        EditorGUI.LabelField(position, label.text, "null");
                    break;
                default:
                    Debug.LogWarning("DisplayOnly不支持" + property.propertyType + "类型");
                    break;
            }


    }
}
