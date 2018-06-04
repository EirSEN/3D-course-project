using System.IO;
using System;
using Unity3DCourse.Helpers;

namespace Unity3DCourse.Data
{
    public class StreamData : IData
    {
        private string _path;

        public void Save(CurrentGameState player)
        {
            using (var sw = new StreamWriter(_path))
            {
                sw.WriteLine(player.Name);
                sw.WriteLine(player.Hp);
                sw.WriteLine(player.IsVisible);
            }
        }

        public CurrentGameState Load()
        {
            var result = new CurrentGameState();
            if (!File.Exists(_path)) return result;
            using (StreamReader streamReader = new StreamReader(_path))
            {
                while (!streamReader.EndOfStream)
                {
                    result.Name = streamReader.ReadLine();
                    result.Hp = Convert.ToSingle(streamReader.ReadLine());
                    result.IsVisible = streamReader.ReadLine().TryBool();
                }
            }
            return result;
        }

        public void SetOptions(string path)
        {
            _path = Path.Combine(path, "Data.GeekBrains");
        }
    }
}