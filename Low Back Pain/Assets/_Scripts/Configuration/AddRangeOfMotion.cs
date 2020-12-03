using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddRangeOfMotion : MonoBehaviour
{
    public int indexBlock;
    public Transform content;
    public Button removeButton;
    public TMP_InputField nbScalingFactor;

    public void Init()
    {
        nbScalingFactor.text = "1";// ConfigurationManager.instance.allInputFiledRangeOfMotion[indexBlock - 1].Count.ToString();
        GetComponent<Button>().onClick.AddListener(delegate { OnClickAddRangeOfMotion(); });
    }

    void OnClickAddRangeOfMotion()
    {
        ConfigurationManager.instance.nbRangeOfMotion[indexBlock]++;

       
        //Instantiate new inputFiled in the content block
        GameObject newInputFiledRangeOfMotion = Instantiate(ConfigurationManager.instance.inputFiledRangeOfMotion, content);
        newInputFiledRangeOfMotion.GetComponent<TMP_InputField>().text = "1";

        //New list of all inputFiled range of motion on this block
        List<GameObject> newListInputFiledRangeOfMotion = ConfigurationManager.instance.allInputFiledRangeOfMotion[indexBlock];
        newListInputFiledRangeOfMotion.Add(newInputFiledRangeOfMotion);

        //Set dictionnary of all inputFiled
        ConfigurationManager.instance.allInputFiledRangeOfMotion.Remove(indexBlock);
        ConfigurationManager.instance.allInputFiledRangeOfMotion.Add(indexBlock, newListInputFiledRangeOfMotion);

        //Set text number scaling factor
        nbScalingFactor.text = ConfigurationManager.instance.allInputFiledRangeOfMotion[indexBlock].Count.ToString();

       
        //Add trial button
        GameObject addTrialButton = newInputFiledRangeOfMotion.transform.Find("AddTrial").gameObject;
        addTrialButton.AddComponent<AddTrial>();
        addTrialButton.GetComponent<AddTrial>().indexBlock = indexBlock;
        addTrialButton.GetComponent<AddTrial>().indexRangeOfMotion = ConfigurationManager.instance.nbRangeOfMotion[indexBlock];

        //Remove trial button
        GameObject removeTrialButton = newInputFiledRangeOfMotion.transform.Find("RemoveTrial").gameObject;
        removeTrialButton.AddComponent<RemoveTrial>();
        removeTrialButton.GetComponent<RemoveTrial>().indexBlock = indexBlock;
        removeTrialButton.GetComponent<RemoveTrial>().indexRangeOfMotion = ConfigurationManager.instance.nbRangeOfMotion[indexBlock];

        //////////////////////////
        Dictionary<int, int> tempDictionary = new Dictionary<int, int>();
       

        //Get and set trial dictionary per block
 
        tempDictionary = ConfigurationManager.instance.nbTrialPerBlockPerRangeOfMotion[indexBlock];


        //tempDictionary.Add(ConfigurationManager.instance.nbRangeOfMotion[indexBlock], 1); //RangeOfMotion: 1 - nbTrial: 1
        tempDictionary.Add(ConfigurationManager.instance.nbRangeOfMotion[indexBlock] - 1, 1); //RangeOfMotion: 0 - nbTrial: 1
        ConfigurationManager.instance.nbTrialPerBlockPerRangeOfMotion.Remove(indexBlock);
        ConfigurationManager.instance.nbTrialPerBlockPerRangeOfMotion.Add(indexBlock, tempDictionary); //Block:1 - ROM: 1 - nbTrial: 1


       

        if (ConfigurationManager.instance.nbRangeOfMotion[indexBlock] > 1)
        {
            removeButton.interactable = true;
        }
    }
}
