using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity3DCourse.Helpers;
using UnityEngine;

namespace Unity3DCourse.Data
{
    public class PlayerPrefsData : IData
    {
        private string _path;
        public void Save(CurrentGameState player)
        {
            PlayerPrefs.SetString("Name", player.Name);
            PlayerPrefs.SetFloat("Hp", player.Hp);
            PlayerPrefs.SetString("IsVisible", player.IsVisible.ToString());
            PlayerPrefs.Save();
        }
        public CurrentGameState Load()
        {
            CurrentGameState result = new CurrentGameState();
            var key = "Name";
            if (PlayerPrefs.HasKey(key))
            {
                result.Name = PlayerPrefs.GetString(key);
            }

            key = "Hp";
            if (PlayerPrefs.HasKey(key))
            {
                result.Hp = PlayerPrefs.GetFloat(key);
            }

            key = "IsVisible";
            if (PlayerPrefs.HasKey(key))
            {
                result.IsVisible = PlayerPrefs.GetString(key).TryBool();
            }
            return result;
        }

        public void SetOptions(string path)
        {
            _path = Path.Combine(path, "Data.GeekBrains");
        }
        public void Claer(string key)
        {
            if (PlayerPrefs.HasKey(key))
                PlayerPrefs.DeleteKey(key);
        }
    }
}