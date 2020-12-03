using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddTrial : MonoBehaviour
{

    public int nbTrial = 1;
    public int indexBlock;
    public int indexRangeOfMotion;
    public Button removeButton;
    public TMP_InputField nbTrialTxt;

    // Start is called before the first frame update
    void Start()
    {
        nbTrialTxt = transform.parent.Find("InputFieldNumberTrial").GetComponent<TMP_InputField>();

        nbTrialTxt.text = nbTrial.ToString();// ConfigurationManager.instance.allInputFiledRangeOfMotion[indexBlock - 1].Count.ToString();
        GetComponent<Button>().onClick.AddListener(delegate { OnClickAddTrial(); });
    }

    void OnClickAddTrial()
    {

        Dictionary<int, int> tempDictionary = new Dictionary<int, int>();
        tempDictionary = ConfigurationManager.instance.nbTrialPerBlockPerRangeOfMotion[indexBlock];
        nbTrial = tempDictionary[indexRangeOfMotion-1];

        nbTrial++;
        tempDictionary[indexRangeOfMotion-1] = nbTrial;
        //tempDictionary.Add(indexRangeOfMotion, nbTrial); //RangeOfMotion: 1 - nbTrial: 1

        ConfigurationManager.instance.nbTrialPerBlockPerRangeOfMotion.Remove(indexBlock);
        ConfigurationManager.instance.nbTrialPerBlockPerRangeOfMotion.Add(indexBlock, tempDictionary);

        nbTrialTxt.text = nbTrial.ToString();
    }
}
