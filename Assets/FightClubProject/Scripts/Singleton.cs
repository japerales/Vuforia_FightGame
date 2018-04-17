using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:Component {

    #region Fields

    /// <summary>
    /// The instance.
    /// </summary>
    private static T instance;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    public static T Instance
    {
        get
        {
            if (instance == null) //si la instancia es nula, es la primera vez que consultamos
            {
                instance = FindObjectOfType<T>(); //buscamos el objeto de tipo T
                if (instance == null) //Si no encuentra nada crea un nuevo objeto
                {
                    GameObject obj = new GameObject(); 
                    obj.name = typeof(T).Name;
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Use this for initialization.
    /// </summary>
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); //ya existe un objeto similar.
        }
    }

    #endregion
}
