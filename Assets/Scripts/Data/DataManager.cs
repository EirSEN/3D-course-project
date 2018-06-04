
namespace Unity3DCourse.Data
{
    public class DataManager
    { 
        private IData _data;

        public void SetData<T>() where T : IData, new()
        {
            _data = new T();
        }
        public void Save(CurrentGameState state)
        {
            if (_data != null)
            {
                _data.Save(state);
            }
        }
        public CurrentGameState Load()
        {
            if (_data != null)
            {
                return _data.Load();
            }

            return null;
        }
        public void SetOptions(string path)
        {
            if (_data != null)
            {
                _data.SetOptions(path);
            }
        }
    }
}