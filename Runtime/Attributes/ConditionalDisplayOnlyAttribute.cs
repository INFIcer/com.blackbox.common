using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ConditionalDisplayOnlyAttribute : ConditionalDisplayAttribute
{
    //public float min;
    //public float max;
    //public bool slider;
    /// <summary>
    /// 无条件的
    /// </summary>
    public ConditionalDisplayOnlyAttribute()
    {
        Uncondition = true;
    }
    /// <summary>
    /// 单枚举
    /// </summary>
    public ConditionalDisplayOnlyAttribute(string conditionalSourceField, int state, bool Inverse = false)
    {
        type = typeof(int);
        this.ConditionalSourceField = conditionalSourceField;
        this.state = state;
        this.Inverse = Inverse;
    }
    //多枚举
    public ConditionalDisplayOnlyAttribute(string conditionalSourceField, int[] num, bool Inverse = false)
    {
        type = typeof(int[]);
        this.ConditionalSourceField = conditionalSourceField;
        this.states = num;
        this.Inverse = Inverse;
    }
    //布尔
    public ConditionalDisplayOnlyAttribute(string conditionalSourceField, bool state = true)
    {
        type = typeof(bool);
        this.ConditionalSourceField = conditionalSourceField;
        this.Bstate = state;
    }
    //混合
    public ConditionalDisplayOnlyAttribute(string BoolConditionalSourceField, string EnumConditionalSourceField, int state, bool Inverse = false)
    {
        type = typeof(string);
        this.ConditionalSourceField = BoolConditionalSourceField;
        this.ConditionalSourceField2 = EnumConditionalSourceField;
        this.state = state;
        this.Inverse = Inverse;
    }
}
