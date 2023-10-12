using SSLAB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWrappable
{
    Vector2 Pos { get; set; }
    float WrapMargin { get; }
}

public interface ILevelWrapService : IService, ITickable, IInitializable
{
    void RegisterWrappable(IWrappable wrappableObject);
    void UnregisterWrappable(IWrappable wrappableObject);
}

public class LevelWrapService : ILevelWrapService
{
    public void Init()
    {
        _wrappables = new List<IWrappable>();
        _limits = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
    }

    public void RegisterWrappable(IWrappable wrappableObject)
    {
        _wrappables.Add(wrappableObject);
    }

    public void Tick(float _)
    {
        for (int i = 0; i < _wrappables.Count; i++)
        {
            var wrappable = _wrappables[i];

            float xLimit = _limits.x + wrappable.WrapMargin;
            float yLimit = _limits.y + wrappable.WrapMargin;

            if (wrappable.Pos.x > xLimit)
            {
                wrappable.Pos = new Vector3(-xLimit, wrappable.Pos.y, 0f);
            }
            else if (wrappable.Pos.x < -xLimit)
            {
                wrappable.Pos = new Vector3(xLimit, wrappable.Pos.y, 0f);
            }
            else if (wrappable.Pos.y > yLimit)
            {
                wrappable.Pos = new Vector3(wrappable.Pos.x, -yLimit, 0f);

            } 
            else if (wrappable.Pos.y < -yLimit)
            {
                wrappable.Pos = new Vector3(wrappable.Pos.x, yLimit, 0f);
            }
        }
    }

    public void UnregisterWrappable(IWrappable wrappableObject)
    {
        _wrappables.Remove(wrappableObject);
    }

    Vector2 _limits;
    List<IWrappable> _wrappables;
}
