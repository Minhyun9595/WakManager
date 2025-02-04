using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GridInterface
{
    public abstract void Init(GameObject _gameObject);
}

public abstract class GridAbstract : GridInterface
{
    public GameObject gameObject;

    public virtual void Init(GameObject _gameObject)
    {
        gameObject = _gameObject;
    }
}
