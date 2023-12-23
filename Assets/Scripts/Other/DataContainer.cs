using System.Collections.Generic;
using System.Linq;

public class DataContainer<T> where T : class
{
    private List<T> _data;

    public DataContainer()
    {
        _data = new List<T>();
    }

    public T2 GetData<T2>() where T2 : class, T  => _data.FirstOrDefault(data => data is T2) as T2;

    public bool AddData<T2>(T data) where T2 : class, T
    {
        if (GetData<T2>() != null)
            return false;

        _data.Add(data);
        return true;
    }

    public bool AddData(T data)
    {
        System.Type dataType = data.GetType();
        if (_data.FirstOrDefault(x => x.GetType() == dataType) != null)
            return false;

        _data.Add(data);
        return true;
    }

    public bool RemoveData<T2>() where T2 : class, T
    {
        T2 data = GetData<T2>();

        if (data == null)
            return false;

        return _data.Remove(data);
    }
}