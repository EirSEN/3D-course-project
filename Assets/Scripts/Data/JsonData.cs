using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Unity3DCourse.Data
{
    public class JsonData : IData
    {
        private string _path;
        private string _privateKey = "ABCDEFGH"; // Для DES шифрования обязательно размер приватного ключа должен быть кратен 8 символам

        public void Save(CurrentGameState player)
        {
            string JsonData = JsonUtility.ToJson(player);

            string EncryptedJsonData = Encrypt(JsonData);

            File.WriteAllText(_path, EncryptedJsonData);
        }

        public CurrentGameState Load()
        {
            string EncryptedJsonData = File.ReadAllText(_path);

            string DecryptedJsonData = Decrypt(EncryptedJsonData);

            return JsonUtility.FromJson<CurrentGameState>(DecryptedJsonData);
        }

        public void SetOptions(string path)
        {
            _path = Path.Combine(path, "JsonData.u3dp");
        }

        private string Decrypt(string encryptedData)
        {
            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();

            desProvider.Mode = CipherMode.ECB;
            desProvider.Padding = PaddingMode.PKCS7;
            desProvider.Key = Encoding.ASCII.GetBytes(_privateKey);

            using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(encryptedData)))
            {
                using (CryptoStream cs = new CryptoStream(stream, desProvider.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs, Encoding.ASCII))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }

        private string Encrypt(string sourceData)
        {
            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();

            desProvider.Mode = CipherMode.ECB;
            desProvider.Padding = PaddingMode.PKCS7;
            desProvider.Key = Encoding.ASCII.GetBytes(_privateKey);

            using (MemoryStream stream = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(stream, desProvider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] data = Encoding.Default.GetBytes(sourceData);
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(stream.ToArray());
                }
            }
        }
    }
}