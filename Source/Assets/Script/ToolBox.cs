using UnityEngine;

public class ToolBox
{
    public static GameObject FindChildObject(GameObject parent, string childName)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).transform.name == childName)
            {
                return parent.transform.GetChild(i).gameObject;
            }
        }
        Debug.LogError("Child '" + childName + "' not found in Parent '" + parent.name + "'");
        return null;
    }

    public static bool CheckSerializeField(ref GameObject serializeField)
    {
        if (serializeField != null)
        {
            return true;
        }
        else
        {
            serializeField = null;
            Debug.LogError("A Field from " + new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name + " was not initalized");
            return false;
        }
    }

    public static bool CheckSerializeField(ref int serializeField)
    {
        if (serializeField != 0)
        {
            return true;
        }
        else
        {
            serializeField = 0;
            Debug.LogError("A Field from " + new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name + " was not initalized");
            return false;
        }
    }

    public static bool CheckSerializeField(ref float serializeField)
    {
        if (serializeField != 0f)
        {
            return true;
        }
        else
        {
            serializeField = 0f;
            Debug.LogError("A Field from " + new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name + " was not initalized");
            return false;
        }
    }

    public static bool CheckSerializeField(ref LayerMask serializeField)
    {
        if (serializeField.value != 0)
        {
            return true;
        }
        else
        {
            serializeField.value = 0;
            Debug.LogError("A Field from " + new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name + " was not initalized");
            return false;
        }
    }

    public static bool CheckSerializeField(ref AudioClip serializeField)
    {
        if (serializeField != null)
        {
            return true;
        }
        else
        {
            serializeField = null;
            Debug.LogError("A Field from " + new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name + " was not initalized");
            return false;
        }
    }
}