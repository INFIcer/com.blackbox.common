using UnityEngine;
using System;
using System.Collections;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalDisplayAttribute : PropertyAttribute
{
    public string ConditionalSourceField = "";//使用枚举类型或int
    public string ConditionalSourceField2 = "";//使用枚举类型或int
    public Type type;
    public int state;//需要显示的状态
    public int state2;//需要显示的状态
    public int[] states;//需要显示的状态
    public bool Bstate = true;
    public bool Inverse = false;
    public bool Uncondition;
    public string[] Conditions;
    //public string [] BoolConditions;
    //public float min;
    //public float max;
    //public bool range;
    //public bool slider;
    // Use this for initialization
    public ConditionalDisplayAttribute() { }
    //public ConditionalDisplayAttribute(string[] Conditions, bool Inverse = false)
    //{
    //    this.Conditions = Conditions;
    //    this.Inverse = Inverse;
    //}
    //单枚举
    public ConditionalDisplayAttribute(string conditionalSourceField, int state, bool Inverse = false)
    {
        type = typeof(int);
        this.ConditionalSourceField = conditionalSourceField;
        this.state = state;
        this.Inverse = Inverse;
    }
    //独立双枚举
    public ConditionalDisplayAttribute(string conditionalSourceField, int state, string conditionalSourceField2, int state2, bool Inverse = false)
    {
        type = typeof(long);
        this.ConditionalSourceField = conditionalSourceField;
        this.state = state;
        this.ConditionalSourceField2 = conditionalSourceField2;
        this.state2 = state2;
        this.Inverse = Inverse;
    }
    //多枚举
    public ConditionalDisplayAttribute(string conditionalSourceField, int[] num, bool Inverse = false)
    {
        type = typeof(int[]);
        this.ConditionalSourceField = conditionalSourceField;
        this.states = num;
        this.Inverse = Inverse;
    }
    //布尔
    public ConditionalDisplayAttribute(string conditionalSourceField, bool state = true)
    {
        type = typeof(bool);
        this.ConditionalSourceField = conditionalSourceField;
        this.Bstate = state;
    }
    //混合
    public ConditionalDisplayAttribute(string BoolConditionalSourceField, string EnumConditionalSourceField, int state, bool Inverse = false)
    {
        type = typeof(string);
        this.ConditionalSourceField = BoolConditionalSourceField;
        this.ConditionalSourceField2 = EnumConditionalSourceField;
        this.state = state;
        this.Inverse = Inverse;
    }
    //混合
    public ConditionalDisplayAttribute(string BoolConditionalSourceField,bool Bstate, string EnumConditionalSourceField, int state, bool Inverse = false)
    {
        type = typeof(string);
        this.ConditionalSourceField = BoolConditionalSourceField;
        this.ConditionalSourceField2 = EnumConditionalSourceField;
        this.Bstate = Bstate;
        this.state = state;
        this.Inverse = Inverse;
    }
    ////区间单枚举
    //public ConditionalDisplayAttribute(string conditionalSourceField, bool Inverse, int state, float min, float max, bool slider = false)
    //{
    //    range = true;
    //    type = typeof(int);
    //    this.ConditionalSourceField = conditionalSourceField;
    //    this.state = state;
    //    this.Inverse = Inverse;
    //    this.min = min < max ? min : max;
    //    this.max = min < max ? max : min;
    //    this.slider = slider;
    //    if (Mathf.Abs(min) == Mathf.Infinity || Mathf.Abs(max) == Mathf.Infinity)
    //    {
    //        this.slider = false;
    //        Debug.LogWarning("存在无穷大，滑动条无法显示，slider = false是必要的");
    //    }
    //}
    ////区间多枚举
    //public ConditionalDisplayAttribute(string conditionalSourceField, bool Inverse, int[] num, float min, float max, bool slider = false)
    //{
    //    range = true;
    //    type = typeof(int[]);
    //    this.ConditionalSourceField = conditionalSourceField;
    //    this.states = num;
    //    this.Inverse = Inverse;
    //    this.min = min < max ? min : max;
    //    this.max = min < max ? max : min;
    //    this.slider = slider;
    //    if (Mathf.Abs(min) == Mathf.Infinity || Mathf.Abs(max) == Mathf.Infinity)
    //    {
    //        this.slider = false;
    //        Debug.LogWarning("存在无穷大，滑动条无法显示，slider = false是必要的");
    //    }
    //}
    ////区间布尔
    //public ConditionalDisplayAttribute(string conditionalSourceField, bool state, float min, float max, bool slider = false)
    //{
    //    range = true;
    //    type = typeof(bool);
    //    this.ConditionalSourceField = conditionalSourceField;
    //    this.Bstate = state;
    //    this.min = min < max ? min : max;
    //    this.max = min < max ? max : min;
    //    this.slider = slider;
    //    if (Mathf.Abs(min) == Mathf.Infinity || Mathf.Abs(max) == Mathf.Infinity)
    //    {
    //        this.slider = false;
    //        Debug.LogWarning("存在无穷大，滑动条无法显示，slider = false是必要的");
    //    }
    //}
    ////区间单枚举(参数简化)
    //public ConditionalDisplayAttribute(string conditionalSourceField, int state, float min, float max, bool slider = false)
    //{
    //    range = true;
    //    type = typeof(int);
    //    this.ConditionalSourceField = conditionalSourceField;
    //    this.state = state;
    //    this.Inverse = false;
    //    this.min = min < max ? min : max;
    //    this.max = min < max ? max : min;
    //    this.slider = slider;
    //    if (Mathf.Abs(min) == Mathf.Infinity || Mathf.Abs(max) == Mathf.Infinity)
    //    {
    //        this.slider = false;
    //        Debug.LogWarning("存在无穷大，滑动条无法显示，slider = false是必要的");
    //    }
    //}
    ////区间多枚举(参数简化)
    //public ConditionalDisplayAttribute(string conditionalSourceField, int[] num, float min, float max, bool slider = false)
    //{
    //    range = true;
    //    type = typeof(int[]);
    //    this.ConditionalSourceField = conditionalSourceField;
    //    this.states = num;
    //    this.Inverse = false;
    //    this.min = min < max ? min : max;
    //    this.max = min < max ? max : min;
    //    this.slider = slider;
    //    if (Mathf.Abs(min) == Mathf.Infinity || Mathf.Abs(max) == Mathf.Infinity)
    //    {
    //        this.slider = false;
    //        Debug.LogWarning("存在无穷大，滑动条无法显示，slider = false是必要的");
    //    }
    //}
    ////区间布尔(参数简化)
    //public ConditionalDisplayAttribute(string conditionalSourceField, float min, float max, bool slider = false)
    //{
    //    range = true;
    //    type = typeof(bool);
    //    this.ConditionalSourceField = conditionalSourceField;
    //    this.Bstate = true ;
    //    this.min = min < max ? min : max;
    //    this.max = min < max ? max : min;
    //    this.slider = slider;
    //    if (Mathf.Abs(min) == Mathf.Infinity || Mathf.Abs(max) == Mathf.Infinity)
    //    {
    //        this.slider = false;
    //        Debug.LogWarning("存在无穷大，滑动条无法显示，slider = false是必要的");
    //    }
    //}
}
