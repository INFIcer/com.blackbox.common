using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class Framework
{
    //public static int TerrainLayer { get => LayerMask.GetMask("Land", "Water"); }
    //public static int PropertyLayer { get => LayerMask.GetMask("Unit", "Building"); }
    //public static int FogLayer { get => LayerMask.GetMask("Fog", "ClearFog"); }
    public static Vector2 V3ToV2(Vector3 value, Quaternion rotation)
    {
        var forward = rotation * (Quaternion.Euler(90, 0, 0) * Vector3.up);
        var right = rotation * (Quaternion.Euler(0, 0, -90) * Vector3.up);
        var x = Vector3.Project(value, right).magnitude;
        if (Vector3.Angle(Vector3.Project(value, right), right) > 90)
            x = -x;
        var y = Vector3.Project(value, forward).magnitude;
        if (Vector3.Angle(Vector3.Project(value, forward), forward) > 90)
            y = -y;
        return new Vector2(x, y);
    }
    public static Vector2 V3ToV2(Vector3 value)
    {
        return V3ToV2(value, Quaternion.identity);
    }
    public static Vector3 V2ToV3(Vector2 value, float y, Quaternion rotation)
    {
        return rotation * new Vector3(value.x, y, value.y);
        var up = rotation * Vector3.up;
        var forward = rotation * (Quaternion.Euler(90, 0, 0) * Vector3.up);
        //forward=rotation *forward;
        var right = rotation * (Quaternion.Euler(0, 0, -90) * Vector3.up);
        return right * value.x + forward * value.y + up * y;
    }
    public static Vector3 V2ToV3(Vector2 value, float y)
    {
        return V2ToV3(value, y, Quaternion.identity);
    }
    public static Vector3 V2ToV3(Vector2 value)
    {
        //Type.GetType(nameof(Framework)).f()
        return V2ToV3(value, 0, Quaternion.identity);
    }
    public static Rect AnchoredToScreenRect(RectTransform rectTransform)
    {
        Vector2 pos = rectTransform.position;
        var size = rectTransform.rect.size * rectTransform.lossyScale;
        return new Rect(pos - new Vector2(size.x * rectTransform.pivot.x, size.y * rectTransform.pivot.y), size);
    }
    public static Vector2 ScreenToNormalizedPoint(Rect ScreenRect, Vector2 ScreenPoint)
    {
        var RelativePos = ScreenPoint - ScreenRect.position;
        return new Vector2(RelativePos.x / ScreenRect.size.x, RelativePos.y / ScreenRect.size.y);
        //return Rect.PointToNormalized(ScreenRect, ScreenPoint);
    }
    public static Vector2 NormalizedToScreenPoint(Rect ScreenRect, Vector2 NormalizedPoint)
    {
        return ScreenRect.position + new Vector2(NormalizedPoint.x * ScreenRect.size.x, NormalizedPoint.y * ScreenRect.size.y);
        //return Rect.NormalizedToPoint(ScreenRect, NormalizedPoint);
    }
    //public static Vector3 NormalizedToWorldPoint(Map map, Vector2 NormalizedPoint, float height)
    //{
    //    var aimPos = map.bounds.min + new Vector3(NormalizedPoint.x * map.bounds.size.x, 0, NormalizedPoint.y * map.bounds.size.z);
    //    aimPos.y = height;
    //    return aimPos;
    //}
    //public static Vector2 WorldToNormalizedPoint(Map map, Vector3 WorldPoint)
    //{
    //    var aimPos = WorldPoint - map.bounds.min;
    //    aimPos.x /= map.bounds.size.x;
    //    aimPos.z /= map.bounds.size.z;
    //    //aimPos = ClampRect(aimPos, new Rect(0, 0, 1, 1));
    //    return new Vector2(aimPos.x, aimPos.z);
    //}
    public static float Approach(float value, float stableValue, float approachValue)
    {
        var offest = stableValue - value;
        return value + dir(offest) * Mathf.Min(Mathf.Abs(offest), Mathf.Abs(approachValue));
    }
    public static Vector2 Approach(Vector2 value, Vector2 stableValue, float approachValue)
    {
        var offest = stableValue - value;
        return value + offest.normalized * Mathf.Min(offest.magnitude, approachValue);
    }
    public static float HalfLife(float value, float stableValue, float deltaTime, float halfLife)
    {
        if (deltaTime == 0)
            return value;
        if (halfLife == 0)
            return stableValue;
        var offest = value - stableValue;
        return stableValue + offest * Mathf.Pow(0.5f, deltaTime / halfLife);
    }
    public static Vector3 HalfLife(Vector3 value, Vector3 stableValue, float deltaTime, float halfLife)
    {
        var result = new Vector3();
        result.x = HalfLife(value.x, stableValue.x, deltaTime, halfLife);
        result.y = HalfLife(value.y, stableValue.y, deltaTime, halfLife);
        result.z = HalfLife(value.z, stableValue.z, deltaTime, halfLife);
        return result;
    }
    public static int findMinIndex(RaycastHit[] raycastHits, int len)
    {
        int index = 0;
        float dis = raycastHits[0].distance;
        for (int i = 1; i < len; i++)
            if (raycastHits[i].distance < dis)
            {
                dis = raycastHits[i].distance;
                index = i;
            }
        return index;
    }
    public static int findMinYIndex(Vector3 origin, RaycastHit[] raycastHits, int len)
    {
        int index = 0;
        float dis = Mathf.Abs(origin.y - raycastHits[0].point.y);
        for (int i = 1; i < len; i++)
            if (raycastHits[i].distance < dis)
            {
                dis = Mathf.Abs(origin.y - raycastHits[i].point.y);
                index = i;
            }
        return index;
    }
    public static int findNearestIndex(Vector3 point, List<Vector3> points)
    {
        int index = 0;
        float dis = Vector3.Distance(point, points[0]);
        for (int i = 1; i < points.Count; i++)
            if (Vector3.Distance(point, points[i]) < dis)
            {
                dis = Vector3.Distance(point, points[i]);
                index = i;
            }
        return index;
    }
    public static int findNearestIndex(Vector3 point, List<GameObject> gameobjects)
    {
        if (gameobjects.Count == 0)
            return -1;
        int index = 0;
        float dis = Vector3.Distance(point, gameobjects[0].transform.position);
        for (int i = 1; i < gameobjects.Count; i++)
            if (Vector3.Distance(point, gameobjects[i].transform.position) < dis)
            {
                dis = Vector3.Distance(point, gameobjects[i].transform.position);
                index = i;
            }
        return index;
    }
    /// <summary>
    /// 以矩形边界包夹
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="clamp">矩形边界</param>
    /// <returns></returns>
    public static Vector2 ClampRect(Vector2 value, Rect clamp)
    {
        value.x = Mathf.Clamp(value.x, clamp.x, clamp.x + clamp.width);
        value.y = Mathf.Clamp(value.y, clamp.y, clamp.y + clamp.height);
        return value;
    }
    public static Vector3 ClampRect(Vector3 value, Rect clamp)
    {
        value.x = Mathf.Clamp(value.x, clamp.x, clamp.x + clamp.width);
        value.z = Mathf.Clamp(value.z, clamp.y, clamp.y + clamp.height);
        return value;
    }
    public static Vector3 ClampBounds(Vector3 value, Bounds clamp)
    {
        value.x = Mathf.Clamp(value.x, clamp.min.x, clamp.max.x);
        value.y = Mathf.Clamp(value.y, clamp.min.y, clamp.max.y);
        value.z = Mathf.Clamp(value.z, clamp.min.z, clamp.max.z);
        return value;
    }
    /// <summary>
    /// 从资源文件("Resources"文件夹下)中加载数据
    /// </summary>
    /// <typeparam name="T">需要加载的资源文件类型</typeparam>
    /// <param name="name">需要加载的资源文件名</param>
    /// <returns>加载得到的数据</returns>
    public static T FindInResources<T>(string name) where T : UnityEngine.Object
    {
        var prefabs = Resources.LoadAll<T>("");
        foreach (T p in prefabs)
            if (p.name == name)
                return p;
        return null;
    }
    /// <summary>
    /// 从资源文件("Resources"文件夹下)中加载数据
    /// </summary>
    /// <typeparam name="T">需要加载的资源文件类型</typeparam>
    /// <returns>加载得到的数据</returns>
    public static List<T> FindInResources<T>() where T : UnityEngine.Object
    {
        return Resources.LoadAll<T>("").ToList();
    }
    public static int dir(float f)
    {
        if (f == 0)
            return 0;
        return (int)(f / Mathf.Abs(f));
    }
    public static bool intersect(object A, object B)
    {
        return ((int)A & (int)B) != 0;
    }

    public static List<T> RandomSort<T>(this List<T> list)
    {
        //var random = new System.Random();
        var newList = new List<T>();
        foreach (var item in list)
        {
            newList.Insert(UnityEngine.Random.Range(0, newList.Count + 1), item);
        }
        return newList;
    }
    public static IEnumerator GetTex(this Image image, string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);// new Texture2D(width, height);
                //texture.LoadImage(results);
                yield return new WaitForSeconds(0.01f);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                image.sprite = sprite;
                //transform.GetComponent<RectTransform>().sizeDelta = new Vector2(texture.width, texture.height);
                yield return new WaitForSeconds(0.01f);
                Resources.UnloadUnusedAssets();
            }
        }
    }
    public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }
        result.Apply();
        return result;
    }
    public static float Height2Vy(float JumpHeight, float G)
    {
        return Mathf.Sqrt(2 * -G * JumpHeight);
    }
    public static float Vy2Height(float Vy, float G)
    {
        return Mathf.Pow(Vy, 2) / 2 / -G;
    }
    public static Vector3 ProjectionPosition(Vector3 origin, Vector3 direction, float maxDistance, int layerMask, float safeDistance = 0, int Fineness = 3)
    {
        RaycastHit[] result = new RaycastHit[Fineness];
        var len = Physics.RaycastNonAlloc(origin, direction, result, maxDistance + safeDistance, layerMask);
        if (len > 0)
        {
            return result[Framework.findMinIndex(result, len)].point - safeDistance * direction;
        }
        return origin + maxDistance * direction;
    }
    public static Vector3 ProjectionPosition(Ray ray, float maxDistance, int layerMask, float safeDistance = 0, int Fineness = 3)
    {
        RaycastHit[] result = new RaycastHit[Fineness];
        var len = Physics.RaycastNonAlloc(ray, result, maxDistance + safeDistance, layerMask);
        if (len > 0)
        {
            return result[Framework.findMinIndex(result, len)].point - safeDistance * ray.direction;
        }
        return ray.origin + maxDistance * ray.direction;
    }
    public static GameObject ProjectionObject(Ray ray, float maxDistance, int layerMask, float safeDistance = 0, int Fineness = 3)
    {
        RaycastHit[] result = new RaycastHit[Fineness];
        var len = Physics.RaycastNonAlloc(ray, result, maxDistance + safeDistance, layerMask);
        if (len > 0)
        {
            return result[Framework.findMinIndex(result, len)].collider.gameObject;
        }
        return null;
    }
    public static IEnumerator Trigger(System.Func<bool> WhenEnter, System.Action enter)
    {
        for (; ; )
        {
            //离开状态
            if (WhenEnter())//进入条件
            {
                //进入时
                enter();
                for (; ; )
                {
                    //进入状态
                    if (!WhenEnter())//离开条件
                    {
                        //离开时
                        break;
                    }
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public static IEnumerator FixedBinaryState(System.Func<bool> WhenEnter, System.Action enter, System.Action inside, System.Func<bool> WhenExit, System.Action exit, System.Action outside)
    {
        for (; ; )
        {
            //离开状态
            outside();
            if (WhenEnter())//进入条件
            {
                //进入时
                enter();
                for (; ; )
                {
                    //进入状态
                    inside();
                    if (WhenExit())//离开条件
                    {
                        //离开时
                        exit();
                        break;
                    }
                    yield return new WaitForFixedUpdate();
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
    public static IEnumerator BinaryState(System.Func<bool> WhenEnter, System.Action enter, System.Action inside, System.Func<bool> WhenExit, System.Action exit, System.Action outside)
    {
        for (; ; )
        {
            //离开状态
            outside();
            if (WhenEnter())//进入条件
            {
                //进入时
                enter();
                for (; ; )
                {
                    //进入状态
                    inside();
                    if (WhenExit())//离开条件
                    {
                        //离开时
                        exit();
                        break;
                    }
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public static IEnumerator BinaryState(System.Func<bool> WhenEnter, System.Action enter, System.Action inside, System.Action exit, System.Action outside)
    {
        for (; ; )
        {
            //离开状态
            outside();
            if (WhenEnter())//进入条件
            {
                //进入时
                enter();
                for (; ; )
                {
                    //进入状态
                    inside();
                    if (!WhenEnter())//离开条件
                    {
                        //离开时
                        exit();
                        break;
                    }
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public static Vector3 ProjectionPosition(Vector3 origin, Vector3 direction, out RaycastHit[] result, float maxDistance, int layerMask, float safeDistance = 0, int Fineness = 3)
    {
        result = new RaycastHit[Fineness];
        var len = Physics.RaycastNonAlloc(origin, direction, result, maxDistance + safeDistance, layerMask);
        if (len > 0)
        {
            return result[Framework.findMinIndex(result, len)].point - safeDistance * direction;
        }
        return origin + maxDistance * direction;
    }
    public static List<Type> AllTypes
    {
        get
        {
            if (!AllTypesInit)
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                var typesList = (from a in assemblies
                                 select a.GetTypes()).ToList();
                _AllTypes = new List<Type>();
                foreach (var t in typesList)
                {
                    _AllTypes = _AllTypes.Union(t).ToList();
                }
                AllTypesInit = true;
            }
            return _AllTypes;
        }
    }
    static List<Type> _AllTypes;
    static bool AllTypesInit;
    public delegate T2 SelectFunc<out T2, T>(T t);
    public static bool isRelatedSubTypeOf(this Type t, Type type)
    => (type.IsInterface && t.GetInterface(type.Name) != null && t.IsClass)
            || (type.IsSubclassOf(typeof(Attribute)) && t.IsDefined(type))
            || (t.IsSubclassOf(type));
    public static List<T> SelectInRelatedSubTypes<T>(this Type type, SelectFunc<T, Type> selectFunc)
    {
        var types = AllTypes;

        //获取所有类
        //var type = types.Find((t) => { return predicate(t); });//找到成员类

        List<T> results = new List<T>();
        foreach (var t in types)
            if (t.isRelatedSubTypeOf(type))
                results.Add(selectFunc(t));
        //if (type.IsInterface)
        //{
        //    foreach (var t in types)
        //        if (t.GetInterface(type.Name) != null && t.IsClass)
        //            results.Add(selectFunc(t));
        //}
        //else if (type.IsSubclassOf(typeof(Attribute)))
        //{
        //    foreach (var t in types)
        //        if (t.IsDefined(type))
        //            results.Add(selectFunc(t));
        //}
        //else
        //{
        //    foreach (var t in types)
        //        if (t.IsSubclassOf(type))
        //            results.Add(selectFunc(t));
        //}
        return results;
    }
    //public static List<string> FindTypeNamesByType(string typeName)
    //{
    //    var assemblies = AppDomain.CurrentDomain.GetAssemblies();

    //    var typesList = (from a in assemblies
    //                     select a.GetTypes()).ToList();
    //    var types = new List<Type>();
    //    foreach (var t in typesList)
    //    {
    //        types = types.Union(t).ToList();
    //    }
    //    //获取所有类
    //    var type = Type.GetType(typeName);
    //    //var type = types.Find((t) => { return t.Name == typeName; });//找到成员类

    //    List<string> results = new List<string>();

    //    if (type.IsInterface)
    //    {
    //        foreach (var t in types)
    //            if (t.GetInterface(type.Name) != null && t.IsClass)
    //                results.Add(t.Name);
    //    }
    //    else if (type.IsSubclassOf(typeof(Attribute)))
    //    {
    //        foreach (var t in types)
    //            if (t.IsDefined(type))
    //                results.Add(t.Name);
    //    }
    //    else
    //    {
    //        foreach (var t in types)
    //            if (t.IsSubclassOf(type))
    //                results.Add(t.Name);
    //    }
    //    return results;
    //}
    public static List<MethodInfo> FindMethodsByAttribute(this Type type, Type attribute)
    {
        return (from MethodInfo m in type.FindMembers(MemberTypes.Method, BindingFlags.Public, (m, o) => m.IsDefined(attribute), null)
                select m).ToList();
    }


    // <summary> 
    /// 序列化 
    /// </summary> 
    /// <param name="data">要序列化的对象</param> 
    /// <returns>返回存放序列化后的数据缓冲区</returns> 
    public static byte[] ToByteStream(this object data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream rems = new MemoryStream();
        formatter.Serialize(rems, data);
        return rems.GetBuffer();
    }

    /// <summary> 
    /// 反序列化 
    /// </summary> 
    /// <param name="data">数据缓冲区</param> 
    /// <returns>对象</returns> 
    public static object ToObject(this byte[] data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream rems = new MemoryStream(data);
        data = null;
        return formatter.Deserialize(rems);
    }

    public static byte[] SubByteStream(this byte[] data, int startIndex, int length)
    {
        byte[] res = new byte[length];
        for (int i = 0; i < length; i++)
            res[i] = data[i + startIndex];
        data = null;
        return res;
    }
}