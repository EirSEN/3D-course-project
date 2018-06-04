using System.Collections;
using System.Collections.Generic;
using Unity3DCourse.Data;
using UnityEngine;

public interface IData
{
    void Save(CurrentGameState state);
    CurrentGameState Load();
    void SetOptions(string path);
}
