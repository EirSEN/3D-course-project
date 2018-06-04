using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
    private static int _numberScene = 0;

    #if UNITY_EDITOR
    [MenuItem("Homework/Save objects", false, 1)]
    #endif
    public static void SaveLevelObjects()
    {
        string savePath = Path.Combine(Application.dataPath, "TestFile.xml");

        GameObject[] props = FindObjectsOfType<GameObject>();
        List<SerializableGameObject> propsList = new List<SerializableGameObject>();
        foreach (var o in props)
        {
            var trans = o.transform;
            var collider = o.GetComponent<Collider>();
            SerializableGameObject forSave = new SerializableGameObject();
            forSave.Name = o.name;
            forSave.Pos = trans.position;
            forSave.Rot = trans.rotation;
            forSave.Scale = trans.localScale;
            if (collider != null)
                forSave.ObjectCollider = collider;

            propsList.Add(forSave);
        }

        Debug.Log(savePath);
        SerializableToXML.Save(propsList.ToArray(), savePath);
    }

    #if UNITY_EDITOR
    [MenuItem("Homework/Load objects", false, 2)]
    #endif
    public static void LoadLevelObjects()
    {
        string savePath = Path.Combine(Application.dataPath, "TestFile.xml");
        SerializableGameObject[] props = SerializableToXML.Load(savePath);

        foreach (var o in props)
        {
            GameObject go = Instantiate((GameObject)Resources.Load(o.Name));
            go.transform.position = o.Pos;
            go.transform.rotation = o.Rot;
            go.transform.localScale = o.Scale;
            Collider col = go.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = o.ObjectCollider.Enabled;
                col.isTrigger = o.ObjectCollider.IsTrigger;
                col.contactOffset = o.ObjectCollider.ContactOffset;
            }
        }
    }
}
