using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System.Linq;

public class PyramidBuilder : EditorWindow
{
    private static List<int> _buildedInstancesID = new List<int>();

    private static GameObject mainParent;
    public GameObject ObjectInstantiate;
    string _nameObject = "Brick";
    int _baseWidth = 5;
    int _offset = 0;
    //bool _revert = false;


    [MenuItem("Homework/Pydimid Builder", false, 0)]
    public static void ShowWindow()
    {
        GetWindow(typeof(PyramidBuilder));

    }
    void OnGUI()
    {
        GUILayout.Label("Основные настройки", EditorStyles.boldLabel);
        ObjectInstantiate = EditorGUILayout.ObjectField("Из чего строим пирамиду", ObjectInstantiate, typeof(GameObject), true) as GameObject;
        _nameObject = EditorGUILayout.TextField("Имя объекта", _nameObject);
        _baseWidth = EditorGUILayout.IntSlider("Основание пирамиды", _baseWidth, 3, 50);
        _offset = EditorGUILayout.IntSlider("Отступ между блоками", _offset, 0, 10);
        //_revert = EditorGUILayout.Toggle("Перевернуть пирамиду", _revert);

        if (GUILayout.Button("За Тутанхамона!"))
        {
            if (ObjectInstantiate)
            {
                mainParent = GameObject.Find("MainPyramidParent");
                if (!mainParent)
                {
                    mainParent = new GameObject("MainPyramidParent");
                    _buildedInstancesID.Add(mainParent.GetInstanceID());
                }
                GameObject root = new GameObject();
                root.transform.parent = mainParent.transform;
                root.transform.SetAsLastSibling();
                root.name = GameObjectUtility.GetUniqueNameForSibling(mainParent.transform, "Pyramid");
                _buildedInstancesID.Add(root.GetInstanceID());                          

                if (_baseWidth % 2 == 0)
                    _baseWidth++;

                Renderer renderer = ObjectInstantiate.GetComponent<Renderer>();
                if (!renderer)
                    return;

                float sizeX = renderer.bounds.size.x;
                float sizeY = renderer.bounds.size.y;
                float sizeZ = renderer.bounds.size.z;

                int pyramidHeight = 0;
                int tempWidth = _baseWidth;
                while (tempWidth > 0)
                {
                    tempWidth -= 2;
                    pyramidHeight++;
                }
                
                Vector3 position = Vector3.zero;
                var iterator = _baseWidth;
                for (var y = 0; y < pyramidHeight; y++)
                {
                    position.x = y * (sizeX + _offset);
                    position.z = y * (sizeZ + _offset);
                    for (var x = 0; x < iterator; x++)
                    {
                        for (var z = 0; z < iterator; z++)
                        {
                            GameObject block = Instantiate(ObjectInstantiate, position, ObjectInstantiate.transform.rotation, root.transform);
                            block.name = GameObjectUtility.GetUniqueNameForSibling(root.transform, root.name + " " + _nameObject);
                            _buildedInstancesID.Add(root.transform.GetInstanceID());
                            position.z += sizeZ + _offset;
                        }
                        position.x += sizeX + _offset;
                        position.z = y * (sizeZ + _offset);
                    }
                    position.y += sizeY + _offset;
                    iterator -= 2;
                }
            }
        }
    }

    [MenuItem("Homework/Remove all pyramids", false, 0)]
    public static void DeleteCreatedObjects()
    {
        var objectsInScene = FindObjectsOfType<GameObject>();
        foreach (var obj in objectsInScene)
        {
            int objID = obj.GetInstanceID();
            if (_buildedInstancesID.Contains(objID))
            {
                _buildedInstancesID.Remove(objID);
                DestroyImmediate(obj);
            }
        }
    }
}
