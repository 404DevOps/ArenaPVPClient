using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class Event
{
    private event Action _action = delegate { };

    public void Invoke() 
    {
        _action.Invoke();
    }

    public async void InvokeDelayed(int delayInMs)
    {
        await Task.Delay(delayInMs);
        Invoke();
    }


    public void AddListener(Action listener)
    {
        _action += listener;
    }

    public void RemoveListener(Action listener)
    {
        _action -= listener;
    }
}

public class Event<T>
{
    private event Action<T> _action = delegate { };

    public void Invoke(T param)
    {
        _action.Invoke(param);
    }

    public async void InvokeDelayed(T param, int delayInMs)
    {
        await Task.Delay(delayInMs);
        Invoke(param);
    }

    public void AddListener(Action<T> listener)
    {
        _action += listener;
    }

    public void RemoveListener(Action<T> listener)
    {
        _action -= listener;
    }
}

public class Event<T1, T2>
{
    private event Action<T1, T2> _action = delegate{};

    public void Invoke(T1 param1, T2 param2)
    {
        _action.Invoke(param1, param2);
    }

    public void AddListener(Action<T1, T2> listener)
    {
        _action += listener;
    }

    public void RemoveListener(Action<T1,T2> listener)
    {
        _action -= listener;
    }
}

