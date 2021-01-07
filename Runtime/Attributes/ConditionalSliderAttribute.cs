using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalSliderAttribute : ConditionalDisplayAttribute
{
    public float min;
    public float max;
    //public bool range;
    public bool slider;
    //区间单枚举
    public ConditionalSliderAttribute(float min, float max, bool slider = false)
    {
        Uncondition = true;
        this.min = min < max ? min : max;
        this.max = min < max ? max : min;
        this.slider = slider;
        if (slider && (Mathf.Abs(min) == Mathf.Infinity || Mathf.Abs(max) == Mathf.Infinity))
        {
            this.slider = false;
            Debug.LogWarning("存在无穷大，滑动条无法显示，slider = false是必要的");
        }
    }
    public ConditionalSliderAttribute(string conditionalSourceField, bool Inverse, int state, float min, float max, bool slider = false)
    {
        //range = true;
        type = typeof(int);
        this.ConditionalSourceField = conditionalSourceField;
        this.state = state;
        this.Inverse = Inverse;
        this.min = min < max ? min : max;
        this.max = min < max ? max : min;
        this.slider = slider;
        if (slider && (Mathf.Abs(min) == Mathf.Infinity || Mathf.Abs(max) == Mathf.Infinity))
        {
            this.slider = false;
            Debug.LogWarning("存在无穷大，滑动条无法显示，slider = false是必要的");
        }
    }
    //区间多枚举
    public ConditionalSliderAttribute(string conditionalSourceField, bool Inverse, int[] num, float min, float max, bool slider = false)
    {
        //range = true;
        type = typeof(int[]);
        this.ConditionalSourceField = conditionalSourceField;
        this.states = num;
        this.Inverse = Inverse;
        this.min = min < max ? min : max;
        this.max = min < max ? max : min;
        this.slider = slider;
        if (slider && (Mathf.Abs(min) == Mathf.Infinity || Mathf.Abs(max) == Mathf.Infinity))
        {
            this.slider = false;
            Debug.LogWarning("存在无穷大，滑动条无法显示，slider = false是必要的");
        }
    }
    //区间布尔
    public ConditionalSliderAttribute(string conditionalSourceField, bool state, float min, float max, bool slider = false)
    {
        //range = true;
        type = typeof(bool);
        this.ConditionalSourceField = conditionalSourceField;
        this.Bstate = state;
        this.min = min < max ? min : max;
        this.max = min < max ? max : min;
        this.slider = slider;
        if (slider && (Mathf.Abs(min) == Mathf.Infinity || Mathf.Abs(max) == Mathf.Infinity))
        {
            this.slider = false;
            Debug.LogWarning("存在无穷大，滑动条无法显示，slider = false是必要的");
        }
    }
    //区间单枚举(参数简化)
    public ConditionalSliderAttribute(string conditionalSourceField, int state, float min, float max, bool slider = false)
    {
        //range = true;
        type = typeof(int);
        this.ConditionalSourceField = conditionalSourceField;
        this.state = state;
        this.Inverse = false;
        this.min = min < max ? min : max;
        this.max = min < max ? max : min;
        this.slider = slider;
        if (slider && (Mathf.Abs(min) == Mathf.Infinity || Mathf.Abs(max) == Mathf.Infinity))
        {
            this.slider = false;
            Debug.LogWarning("存在无穷大，滑动条无法显示，slider = false是必要的");
        }
    }
    //区间多枚举(参数简化)
    public ConditionalSliderAttribute(string conditionalSourceField, int[] num, float min, float max, bool slider = false)
    {
        // range = true;
        type = typeof(int[]);
        this.ConditionalSourceField = conditionalSourceField;
        this.states = num;
        this.Inverse = false;
        this.min = min < max ? min : max;
        this.max = min < max ? max : min;
        this.slider = slider;
        if (slider && (Mathf.Abs(min) == Mathf.Infinity || Mathf.Abs(max) == Mathf.Infinity))
        {
            this.slider = false;
            Debug.LogWarning("存在无穷大，滑动条无法显示，slider = false是必要的");
        }
    }
    //区间布尔(参数简化)
    public ConditionalSliderAttribute(string conditionalSourceField, float min, float max, bool slider = false)
    {
        //range = true;
        type = typeof(bool);
        this.ConditionalSourceField = conditionalSourceField;
        this.Bstate = true;
        this.min = min < max ? min : max;
        this.max = min < max ? max : min;
        this.slider = slider;
        if (slider &&(Mathf.Abs(min) == Mathf.Infinity || Mathf.Abs(max) == Mathf.Infinity))
        {
            this.slider = false;
            Debug.LogWarning("存在无穷大，滑动条无法显示，slider = false是必要的");
        }
    }
    //区间混合
    public ConditionalSliderAttribute(string BoolConditionalSourceField, string EnumConditionalSourceField, int state, bool Inverse, float min, float max, bool slider = false)
    {
        type = typeof(string);
        this.ConditionalSourceField = BoolConditionalSourceField;
        this.ConditionalSourceField2 = EnumConditionalSourceField;
        this.state = state;
        this.Inverse = Inverse;
        this.min = min < max ? min : max;
        this.max = min < max ? max : min;
        this.slider = slider;
        if (slider && (Mathf.Abs(min) == Mathf.Infinity || Mathf.Abs(max) == Mathf.Infinity))
        {
            this.slider = false;
            Debug.LogWarning("存在无穷大，滑动条无法显示，slider = false是必要的");
        }
    }
    //区间混合简化
    public ConditionalSliderAttribute(string BoolConditionalSourceField, string EnumConditionalSourceField, int state, float min, float max, bool slider = false)
    {
        type = typeof(string);
        this.ConditionalSourceField = BoolConditionalSourceField;
        this.ConditionalSourceField2 = EnumConditionalSourceField;
        this.state = state;
        this.Inverse = false;
        this.min = min < max ? min : max;
        this.max = min < max ? max : min;
        this.slider = slider;
        if (slider && (Mathf.Abs(min) == Mathf.Infinity || Mathf.Abs(max) == Mathf.Infinity))
        {
            this.slider = false;
            Debug.LogWarning("存在无穷大，滑动条无法显示，slider = false是必要的");
        }
    }
}
