using UnityEngine;
using UnityEngine.Windows;
//using UnityEditor.IMGUI.Controls;
using UnityEditor;
public class CreateAssets : Editor
{
    // 在菜单栏创建功能项
    // [MenuItem("Assets/Create/" + nameof(Command))]
    // static void CreateCommand()
    // {
    //     Create<Command>();
    // }
    static void Create<T>() where T : ScriptableObject
    {
        var t = CreateInstance<T>();
        //获取当前路径
        string[] strs = Selection.assetGUIDs;
        string path = AssetDatabase.GUIDToAssetPath(strs[0]);
        //命名文件
        string FileName = "New" + typeof(T).Name;
        int IndName = 0;
        while (File.Exists(path + "/" + FileName + (IndName == 0 ? "" : "(" + IndName + ")") + ".asset"))
            IndName++;
        path += "/" + FileName + (IndName == 0 ? "" : "(" + IndName + ")") + ".asset";
        //创建
        AssetDatabase.CreateAsset(t, path);
        //AssetDatabase.StartAssetEditing()
        //AssetDatabase.RenameAsset(t);
        //RenameEndedArgs
        Selection.objects = new Object[] { t };

    }
}
