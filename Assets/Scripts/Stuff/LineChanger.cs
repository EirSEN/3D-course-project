using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineChanger : MonoBehaviour
{
    [Header("Bolt Properties")]
    [Tooltip("Генерируемая амплитура луча")]
    [SerializeField] float _keyFrameValue;
    [Tooltip("Конечная точка луча. Начальная точка - позиция объекта")]
    public Vector3 EndPoint = new Vector3(10, 1, 2);      

    [Header("Bolt rendering")]
    [SerializeField] LineRenderer _rayRenderer;      

    Vector3 vectorOfBolt;                           

    public void ChangePhase()
    {
        vectorOfBolt = EndPoint - transform.position;

        AnimationCurve curve = new AnimationCurve();
        for (int i = 0; i < 10; i++)
        {
            float keyFrameTime = (float) i / 10;
            curve.AddKey(keyFrameTime, Random.Range(-_keyFrameValue, _keyFrameValue));
        }
        
        _rayRenderer.positionCount = curve.keys.Length;
        
        
        for (int index = 0; index < curve.keys.Length; ++index)
        {
            Keyframe key = curve.keys[index];
            Vector3 point = transform.position + vectorOfBolt * key.time;
            point += Vector3.up * key.value;
            
            _rayRenderer.SetPosition(index, point);
        }
    }
}
