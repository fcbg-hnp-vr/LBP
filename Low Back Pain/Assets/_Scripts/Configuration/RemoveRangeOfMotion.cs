using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RemoveRangeOfMotion : MonoBehaviour
{

    public int indexBlock;
    public Transform content;
    public TMP_InputField nbScalingFactor;


    public void Init()
    {
        GetComponent<Button>().onClick.AddListener(delegate { OnClickRemoveRangeOfMotion(); });
    }

    void OnClickRemoveRangeOfMotion()
    {
        if (ConfigurationManager.instance.nbRangeOfMotion[indexBlock] > 1)
        {
            ConfigurationManager.instance.nbRangeOfMotion[indexBlock]--;


            //Destroy and remove inputFiled
            List<GameObject> newListInputFiledRangeOfMotion = ConfigurationManager.instance.allInputFiledRangeOfMotion[indexBlock];
            Destroy(newListInputFiledRangeOfMotion[newListInputFiledRangeOfMotion.Count - 1]);
            newListInputFiledRangeOfMotion.RemoveAt(newListInputFiledRangeOfMotion.Count - 1);

            //Set dictionnary of all inputFiled
            ConfigurationManager.instance.allInputFiledRangeOfMotion.Remove(indexBlock);
            ConfigurationManager.instance.allInputFiledRangeOfMotion.Add(indexBlock, newListInputFiledRangeOfMotion);

            // ConfigurationManager.instance.nbTrialPerBlockPerRangeOfMotion.Remove(ConfigurationManager.instance.nbTrialPerBlockPerRangeOfMotion.Count - 1);

            Dictionary<int, int> tempDictionary = new Dictionary<int, int>();
            tempDictionary = ConfigurationManager.instance.nbTrialPerBlockPerRangeOfMotion[indexBlock];
            Debug.Log(tempDictionary.Count);
            /* for (int i = 0; i < tempDictionary.; i++)
             {
                 Debug.Log("tempDictionary =" + tempDictionary[i].ToString());
             }*/


            tempDictionary.Remove(ConfigurationManager.instance.nbRangeOfMotion[indexBlock]); //RangeOfMotion: 1 - nbTrial: 1
            ConfigurationManager.instance.nbTrialPerBlockPerRangeOfMotion[indexBlock] = tempDictionary;
            //ConfigurationManager.instance.nbTrialPerBlockPerRangeOfMotion.Remove(indexBlock);
            //ConfigurationManager.instance.nbTrialPerBlockPerRangeOfMotion.Add(indexBlock, tempDictionary); //Block:1 - ROM: 1 - nbTrial: 1

            nbScalingFactor.text = ConfigurationManager.instance.allInputFiledRangeOfMotion[indexBlock].Count.ToString();

           

            if (ConfigurationManager.instance.nbRangeOfMotion[indexBlock] == 1)
            {
                GetComponent<Button>().interactable = false;
            }
        }

    }

}
