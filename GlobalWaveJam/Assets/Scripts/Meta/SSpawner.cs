using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]		// rather than MonoBehaviour, cause MonoBehaviour array only instantiates at runtime (not in the inspector)
public class ObjectCache
{
    // SETTINGS
    public GameObject Prefab;		// define the projectile prefab
    private int Size;	        // define the default size of the cache (elements can be added on runtime afterwards) + can be modified on runtime to add more elements

    // VARIABLE
    private List<GameObject> cache = new List<GameObject>();

    // REFERENCE
    GameObject Parent;

    // instanciate all objects in the cache
    public void Init(GameObject parent)
    {
        Parent = parent;

        if (Size > 0)
            InitElements();
    }

    public void AddElements(int elements)
    {
        if (elements <= 0)
            return;

        Size += elements;

        if (Parent != null)
            InitElements();
    }

    private void InitElements()
    {
        for (int i = 0; i < Size; i++)
        {
            if (i >= cache.Count)
            {
                GameObject instancied = (GameObject)GameObject.Instantiate(Prefab);
                instancied.SetActive(false);
                instancied.name = Prefab.name + i;
                cache.Add(instancied);

                // put under the spawner hierarchy
                instancied.transform.SetParent(Parent.transform);
            }
        }

    }

    // find 1st inactive object in the cache
    public GameObject GetNext()
    {
        int idx = -1;

        for (int i = 0; i < cache.Count; i++)
        {
            if (cache[i].activeSelf == false)
            {
                idx = i;
                break;
            }
        }

        if (idx < 0)
        {
            Debug.LogWarning("Cache overflow (" + Prefab.name + ")");
            return null;
        }

        return cache[idx];
    }
}


public class SSpawner : MonoBehaviour
{

    // SETTINGS
    public List<ObjectCache> caches = new List<ObjectCache>();

    // VARIABLES
    private Hashtable ActiveCachedObjects;


    // Use this for initialization
    void Awake()
    {
        // static part
        instance = this;

        for (int i = 0; i < caches.Count; i++)
        {
            caches[i].Init(instance.gameObject);		// instantiate under self
        }

        // init the active object hashtable
        ActiveCachedObjects = new Hashtable();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static GameObject Spawn(GameObject Prefab, Vector3 Position, Quaternion Rotation)
    {
        if (instance == null)
        {
            Instantiate();
        }

        GameObject obj = null;

        // search right cache to register
        int idx = -1;
        for (int i = 0; i < instance.caches.Count; i++)
        {
            if (instance.caches[i].Prefab.Equals(Prefab))
            {
                idx = i;
                break;
            }
        }

        // if no such cache, simple instantiation
        if (idx < 0)
        {
            return (GameObject)GameObject.Instantiate(Prefab);
        }

        // set object properties and active
        obj = instance.caches[idx].GetNext();
        if (obj)
        {
            obj.transform.position = Position;
            obj.transform.rotation = Rotation * Prefab.transform.rotation;      // add prefab rotation to the mix
            obj.transform.localScale = Prefab.transform.localScale;     // reset scale to prefab
            obj.SetActive(true);

            // set active in hashtable
            instance.ActiveCachedObjects[obj] = true;
        }

        return obj;
    }


    public static GameObject Spawn(string prefabName, Vector3 Position, Quaternion Rotation)
    {
        if (instance == null)
        {
            Instantiate();
        }

        GameObject obj = null;

        // search right cache to register
        foreach (ObjectCache cache in instance.caches)
        {
            if (cache.Prefab.name == prefabName)
            {
                obj = cache.Prefab;
                break;
            }
        }

        // if no such cache, simple instantiation
        if (obj == null)
        {
            Debug.LogWarning("[SSpawner] no such object to spawn: " + prefabName);
            return null;
        }

        return Spawn(obj, Position, Rotation);
    }


    public static GameObject GetPrefab(string prefabName)
    {
        if (instance == null)
        {
            Instantiate();
        }

        ObjectCache cache = instance.caches.Find(x => (x.Prefab.name == prefabName));

        if (cache != null)
            return cache.Prefab;

        return null;
    }


    public static void Destroy(GameObject Obj)
    {
        if (Obj == null)
        {
            Debug.LogWarning("[SSpawner] can't destroy null !");
            return;
        }

        // find the object in the hashtable and just deactivate it (fast method)
        if (instance.ActiveCachedObjects.ContainsKey(Obj))
        {
            //Obj.transform.SetParent(instance.transform);
            instance.ActiveCachedObjects[Obj] = false;
            Obj.SetActive(false);
        }
        // if doesn't exist just destroy
        else
        {
            // never destroy the player
            if (Obj.tag != "Player")
                GameObject.Destroy(Obj);
        }
    }

    // create new elements on runtime
    public static void AddElements(string path, int count)
    {
        GameObject prefab = (GameObject)Resources.Load(path);
        if (prefab == null)
        {
            Debug.LogError("[SSpawner] prefab " + path + " doesn't exist.");
            return;
        }

        AddElements(prefab, count);
    }

    public static void AddElements(GameObject prefab, int count)
    {
        if (prefab == null)
        {
            Debug.LogError("[SSpawner] null prefab, can't add elements.");
            return;
        }

        if (instance == null)
        {
            Instantiate();
        }

        // search right cache to register
        ObjectCache cache = null;
        foreach(ObjectCache c in instance.caches)
        {
            if (c.Prefab.Equals(prefab))
            {
                cache = c;
                break;
            }
        }

        // if none: create
        if (cache == null)
        {
            cache = new ObjectCache();
            cache.Prefab = prefab;
            cache.Init(instance.gameObject);
            instance.caches.Add(cache);
        }

        // if existing: add objects
        cache.AddElements(count);
        //Debug.Log("[SSpawner] Added " + count + " " + prefab.name + " to spawner pools.");
    }

    //********************************************************
    // STATIC PART (no need for singleton here)
    private static SSpawner instance;

    public static void Instantiate()
    {
        if (instance != null)
        {
            Debug.LogWarning("[Spawner] already instantiated");
            return;
        }

        GameObject obj = new GameObject("ObjectsSpawner");
        instance = obj.AddComponent<SSpawner>();
        instance.Awake();
    }


}
