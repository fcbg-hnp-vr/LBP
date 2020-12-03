using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RemoveTrial : MonoBehaviour
{

    public int nbTrial;
    public int indexBlock;
    public int indexRangeOfMotion;
    public TMP_InputField nbTrialTxt;

    // Start is called before the first frame update
    void Start()
    {
        nbTrialTxt = transform.parent.Find("InputFieldNumberTrial").GetComponent<TMP_InputField>();

        GetComponent<Button>().onClick.AddListener(delegate { OnClickRemoveTrial(); });
    }

    void OnClickRemoveTrial()
    {
        
        Dictionary<int, int> tempDictionary = new Dictionary<int, int>();
        tempDictionary = ConfigurationManager.instance.nbTrialPerBlockPerRangeOfMotion[indexBlock];

        nbTrial = tempDictionary[indexRangeOfMotion-1];

        if (nbTrial > 1)
        {
            nbTrial--;
            nbTrialTxt.text = nbTrial.ToString();

            tempDictionary[indexRangeOfMotion-1] = nbTrial;

            ConfigurationManager.instance.nbTrialPerBlockPerRangeOfMotion.Remove(indexBlock);
            ConfigurationManager.instance.nbTrialPerBlockPerRangeOfMotion.Add(indexBlock, tempDictionary);
        }

    }
}
