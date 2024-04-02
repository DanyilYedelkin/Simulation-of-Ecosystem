using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animals;
using System.IO;

public class SaveDataJSON : MonoBehaviour
{
    #region Private Members
    private AnimalBehaviourController _animalData;
    #endregion

    #region Unity Methods
    private void Start()
    {
        
    }
    #endregion

    #region API
    public void SaveData(AnimalBehaviourController _animalData)
    {
        string json = JsonUtility.ToJson(_animalData);
        Debug.Log(json);

        using (StreamWriter writer = new StreamWriter(Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json"))
        {
            writer.Write(json);
        }
    }
    #endregion
}
