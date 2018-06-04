using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using Unity3DCourse.Helpers;

namespace Unity3DCourse.Data
{
    public class XMLData : IData
    {
        private string _path;


        public void Save(CurrentGameState player)
        {
            var xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("Player");
            xmlDoc.AppendChild(rootNode);

            var element = xmlDoc.CreateElement("Name");
            element.SetAttribute("value", player.Name);
            rootNode.AppendChild(element);

            element = xmlDoc.CreateElement("Hp");
            element.SetAttribute("value", player.Hp.ToString());
            rootNode.AppendChild(element);

            element = xmlDoc.CreateElement("IsVisible");
            element.SetAttribute("value", player.IsVisible.ToString());
            rootNode.AppendChild(element);
            XmlNode userNode = xmlDoc.CreateElement("Info");
            var attribute = xmlDoc.CreateAttribute("Unity");
            attribute.Value = Application.unityVersion;
            if (userNode.Attributes != null)
            {
                userNode.Attributes.Append(attribute);
            }
            userNode.InnerText = "System Language: " + Application.systemLanguage;
            rootNode.AppendChild(userNode);
            xmlDoc.Save(_path);
        }
        public CurrentGameState Load()
        {
            var reult = new CurrentGameState();
            if (!File.Exists(_path)) return reult;
            using (XmlTextReader reader = new XmlTextReader(_path))
            {
                var key = "Name";
                while (reader.Read())
                {
                    if (reader.IsStartElement(key))
                    {
                        reult.Name = reader.GetAttribute("value");
                    }
                    key = "Hp";
                    if (reader.IsStartElement(key))
                    {
                        reult.Hp =
                        System.Convert.ToSingle(reader.GetAttribute("value"));
                    }
                    key = "IsVisible";
                    if (reader.IsStartElement(key))
                    {
                        reult.IsVisible = reader.GetAttribute("value").TryBool();
                    }
                }
            }
            return reult;
        }

        public void SetOptions(string path)
        {
            _path = Path.Combine(path, "Data.GeekBrains");
        }
    }
}