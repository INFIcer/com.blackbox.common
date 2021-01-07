using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class ConditionalHelpBoxAttribute : ConditionalDisplayAttribute
{
    //public string ConditionalSourceField = "";//
    //public Type type;
    //public int state;//需要显示的状态
    //public int[] states;//需要显示的状态
    //public bool Bstate;
    //public bool Inverse = false;
    public string Msg;
    public MessageType MsgT;
    public ConditionalHelpBoxAttribute(string Msg, MessageType MsgT)
    {
        Uncondition = true;
        this.Msg = Msg;
        this.MsgT = MsgT;
    }
    //单枚举
    public ConditionalHelpBoxAttribute(string conditionalSourceField, int state, string Msg, MessageType MsgT, bool Inverse = false)
    {
        type = typeof(int);
        this.ConditionalSourceField = conditionalSourceField;
        this.state = state;
        this.Inverse = Inverse;
        this.Msg = Msg;
        this.MsgT = MsgT;
    }
    //多枚举
    public ConditionalHelpBoxAttribute(string conditionalSourceField, int[] num, string Msg, MessageType MsgT, bool Inverse = false)
    {
        type = typeof(int[]);
        this.ConditionalSourceField = conditionalSourceField;
        this.states = num;
        this.Inverse = Inverse;
        this.Msg = Msg;
        this.MsgT = MsgT;
    }
    //布尔
    public ConditionalHelpBoxAttribute(string conditionalSourceField, string Msg, MessageType MsgT, bool state = true)
    {
        type = typeof(bool);
        this.ConditionalSourceField = conditionalSourceField;
        this.Bstate = state;
        this.Msg = Msg;
        this.MsgT = MsgT;
    }
    //混合
    public ConditionalHelpBoxAttribute(string BoolConditionalSourceField, string EnumConditionalSourceField, int state, bool Inverse = false)
    {
        type = typeof(string);
        this.ConditionalSourceField = BoolConditionalSourceField;
        this.ConditionalSourceField2 = EnumConditionalSourceField;
        this.state = state;
        this.Inverse = Inverse;
    }
    //单布尔自身
    //public ConditionHelpBoxAttribute(string Msg,MessageType MsgT, bool state=true )
    //{
    //    //type = typeof(bool);
    //    //this.ConditionalSourceField = conditionalSourceField;
    //    this.Bstate = state;
    //    this.Msg = Msg;
    //    this.MsgT = MsgT;
    //}
    ////单布尔外部
    //public ConditionHelpBoxAttribute(string ConditionalSourceField ,string Msg, MessageType MsgT, bool state = true)
    //{
    //    type = typeof(bool);
    //    this.ConditionalSourceField = ConditionalSourceField;
    //    this.Bstate = state;
    //    this.Msg = Msg;
    //    this.MsgT = MsgT;
    //}
    ////单枚举
    //public ConditionHelpBoxAttribute(string ConditionalSourceField,int state, string Msg, MessageType MsgT, bool Inverse = false)
    //{
    //    type = typeof(int);
    //    this.ConditionalSourceField = ConditionalSourceField;
    //    this.state = state;
    //    this.Msg = Msg;
    //    this.MsgT = MsgT;
    //}
    //public ConditionHelpBoxAttribute(string ConditionalSourceField, int[] states, string Msg, MessageType MsgT, bool Inverse = false)
    //{
    //    type = typeof(int[]);
    //    this.ConditionalSourceField = ConditionalSourceField;
    //    this.states = states;
    //    this.Msg = Msg;
    //    this.MsgT = MsgT;
    //}
    //单布尔
    //public ConditionHelpBoxAttribute(string Msg, MessageType MsgT)
    //{
    //    //type = typeof(bool);
    //    //this.ConditionalSourceField = conditionalSourceField;
    //    this.Bstate = true;
    //    this.Msg = Msg;
    //    this.MsgT = MsgT;
    //}
}
#endif