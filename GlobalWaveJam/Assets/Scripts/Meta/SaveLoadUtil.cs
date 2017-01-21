using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


// serializable vector class
[System.Serializable]
public struct V3Ser
{
    public float x;
    public float y;
    public float z;

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public V3Ser(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public V3Ser(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}


public static class SaveLoadUtil
{
    public static T Clone<T>(T input)
    {
        if (!typeof(T).IsSerializable)
        {
            Debug.LogWarning("The type must be serializable.");
            return default(T);
        }

        // Don't serialize a null object, simply return the default for that object
        if (Object.ReferenceEquals(input, null))
        {
            return default(T);
        }

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new MemoryStream();
        using (stream)
        {
            formatter.Serialize(stream, input);
            stream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(stream);
        }
    }


    // custom class serialization to byte array (for Photon custom class)
    public static byte[] Serialize<T>(object customobject)
    {
        T vo = (T)customobject;

        if (!typeof(T).IsSerializable)
        {
            Debug.LogWarning("The type must be serializable.");
            return null;
        }

        var formatter = new BinaryFormatter();
        using (var stream = new MemoryStream())
        {
            formatter.Serialize(stream, vo);
            return stream.ToArray();
        }
    }

    // custom class serialization to byte array (for Photon custom class)
    public static object Deserialize<T>(byte[] bytes)
    {
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(bytes, 0, bytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            T obj = (T)binForm.Deserialize(memStream);
            return obj;
        }
    }

    // save settings data
    public static void SaveSettings<T>(T input, string fileName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + fileName);
        bf.Serialize(file, input);
        file.Close();
        Debug.LogWarning("[SaveLoad] Settings saved in " + Application.persistentDataPath + "/" + fileName);
    }

    // delete file
    public static void DeleteSettings(string fileName)
    {
        File.Delete(Application.persistentDataPath + "/" + fileName);
        Debug.LogWarning("[SaveLoad] Settings deleted at " + Application.persistentDataPath + "/" + fileName + " !!");
    }

    // load settings data
    public static T LoadSettings<T>(string fileName)
    {
        //Check if file needs to be unpacked
        if (!File.Exists(Application.persistentDataPath + "/" + fileName))
        {
            Debug.Log("[SaveLoad] Loading settings from streaming assets: " + fileName);
            UnpackMobileFile(fileName);
        }
        else
        {
            Debug.LogWarning("[SaveLoad] Loading settings from persistant data path: " + Application.persistentDataPath + "/" + fileName);
        }

        //check again in case it didn't work
        T target = default(T);

        if (File.Exists(Application.persistentDataPath + "/" + fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();//engine that will interface with file
            FileStream file = File.Open(Application.persistentDataPath + "/" + fileName, FileMode.Open);

            //Load data from file to loader
            try
            {
                target = (T)bf.Deserialize(file);    //restore instance of below class as sample save
                file.Close();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("[SaveLoad] Settings file in persistant data path (" + Application.persistentDataPath + "/" + fileName + ") is either outdated or corrupted. Overriding with StreamingAssets copy - " + e.ToString());
                file.Close();

                // unpack file (force overwrite)
                UnpackMobileFile(fileName, true);
                // try reading again
                file = File.Open(Application.persistentDataPath + "/" + fileName, FileMode.Open);
                target = (T)bf.Deserialize(file);    //restore instance of below class as sample save
                file.Close();
            }
        }
        else
        {
            Debug.LogError("[SaveLoad] Can't find the file at " + Application.persistentDataPath + "/" + fileName + ".");
        }

        return target;
    }

    //copies and unpacks file from apk to persistentDataPath where it can be accessed
    static void UnpackMobileFile(string fileName, bool forceoverwrite = false)      // force overwrite when the persistant file is corrupted/out of date
    {
        string destinationPath = System.IO.Path.Combine(Application.persistentDataPath, fileName);
        string sourcePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

        //if DB does not exist in persistent data folder (folder "Documents" on iOS) or source DB is newer then copy it
        if (forceoverwrite || !System.IO.File.Exists(destinationPath) || (System.IO.File.GetLastWriteTimeUtc(sourcePath) > System.IO.File.GetLastWriteTimeUtc(destinationPath)))
        {
            if (sourcePath.Contains("://"))
            {// Android  
                WWW www = new WWW(sourcePath);
                while (!www.isDone) { ;}                // Wait for download to complete - not pretty at all but easy hack for now 
                if (System.String.IsNullOrEmpty(www.error))
                {
                    System.IO.File.WriteAllBytes(destinationPath, www.bytes);
                }
                else
                {
                    Debug.Log("ERROR: the file DB named " + fileName + " doesn't exist in the StreamingAssets Folder, please copy it there.");
                }
            }
            else
            {                // Mac, Windows, Iphone                
                //validate the existens of the DB in the original folder (folder "streamingAssets")
                if (System.IO.File.Exists(sourcePath))
                {
                    //copy file - alle systems except Android
                    System.IO.File.Copy(sourcePath, destinationPath, true);
                }
                else
                {
                    Debug.Log("ERROR: the file DB named " + fileName + " doesn't exist in the StreamingAssets Folder, please copy it there.");
                }
            }
        }
    }
}
