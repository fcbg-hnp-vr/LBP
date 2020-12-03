using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ConfigurationManager : MonoBehaviour
{
    public static ConfigurationManager instance;

    [Header("Prefab")]
    public GameObject bloc;
    public GameObject inputFiledRangeOfMotion;

    [Header("Block")]
    public Button addBlock;
    public Button removeBlock;
    public TMP_InputField inputFiledNumberOfBlock;
    public GameObject contentBloc;
    public int nbBlock = 1;

    [Header("Range of Motion")]
    public List<int> nbRangeOfMotion;
    public List<GameObject> buttonsAddRangeOfMotion;
    public List<GameObject> buttonsRemoveRangeOfMotion;

    [Header("Trial")]
    public Dictionary<int, Dictionary<int, int>> nbTrialPerBlockPerRangeOfMotion;
    public List<int> nbTrial;
    public List<GameObject> buttonsAddTrial;
    public List<GameObject> buttonsRemoveTrial;

    [Header("All content")]
    public List<GameObject> allBlocs;
    public List<GameObject> allContentRangeOfMotion;
    public Dictionary<int,List<GameObject>> allInputFiledRangeOfMotion;

    public GameObject contentRangeOfMotion;
    public GameObject addRangeOfMotion;
    public GameObject removeRangeOfMotion;
    public TMP_InputField numberScalingFactor;
    public GameObject addTrial;
    public GameObject removeTrial;
    public TMP_InputField numberTrial;

    UnityAction myEventAddInputField;
    UnityAction myEventRemoveInputField;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //init List Bloc
        allBlocs = new List<GameObject>();
        allBlocs.Add(contentBloc.transform.GetChild(0).gameObject);

        //init list all content range of motion
       //GameObject contentRangeOfMotion = GameObject.Find("ContentRangeOfMotion");
        allContentRangeOfMotion = new List<GameObject>();
        allContentRangeOfMotion.Add(contentRangeOfMotion);

        //init list all input range of motion
        allInputFiledRangeOfMotion = new Dictionary<int, List<GameObject>>();
        List<GameObject> inputFiledRangeOfMotion = new List<GameObject>();
        contentRangeOfMotion.transform.GetChild(0).gameObject.GetComponent<TMP_InputField>().text = "1";
        inputFiledRangeOfMotion.Add(contentRangeOfMotion.transform.GetChild(0).gameObject);
        allInputFiledRangeOfMotion.Add(0, inputFiledRangeOfMotion);

       
        //Call back
        addBlock.onClick.AddListener(delegate { AddBlock(); });
        removeBlock.onClick.AddListener(delegate { RemoveBlock(); });

        inputFiledNumberOfBlock.text = nbBlock.ToString();

        //int list int nb range of motion
        nbRangeOfMotion = new List<int>();
        nbRangeOfMotion.Add(1);
        nbRangeOfMotion[0] = 1;

        //Remove input range of motion button
        removeRangeOfMotion.AddComponent<RemoveRangeOfMotion>();
        removeRangeOfMotion.GetComponent<RemoveRangeOfMotion>().indexBlock = nbBlock - 1;
        removeRangeOfMotion.GetComponent<RemoveRangeOfMotion>().content = allContentRangeOfMotion[allContentRangeOfMotion.Count - 1].transform;
        removeRangeOfMotion.GetComponent<RemoveRangeOfMotion>().nbScalingFactor = numberScalingFactor;
        removeRangeOfMotion.GetComponent<Button>().interactable = false;
        removeRangeOfMotion.GetComponent<RemoveRangeOfMotion>().Init();

        buttonsRemoveRangeOfMotion = new List<GameObject>();
        buttonsRemoveRangeOfMotion.Add(removeRangeOfMotion);

        //Add input range of motion button
        addRangeOfMotion.AddComponent<AddRangeOfMotion>();
        addRangeOfMotion.GetComponent<AddRangeOfMotion>().indexBlock = nbBlock - 1;
        addRangeOfMotion.GetComponent<AddRangeOfMotion>().content = allContentRangeOfMotion[allContentRangeOfMotion.Count - 1].transform;
        addRangeOfMotion.GetComponent<AddRangeOfMotion>().removeButton = buttonsRemoveRangeOfMotion[buttonsRemoveRangeOfMotion.Count - 1].GetComponent<Button>();
        addRangeOfMotion.GetComponent<AddRangeOfMotion>().nbScalingFactor = numberScalingFactor;
        addRangeOfMotion.GetComponent<AddRangeOfMotion>().Init();

        buttonsAddRangeOfMotion = new List<GameObject>();
        buttonsAddRangeOfMotion.Add(addRangeOfMotion);

        //Trial
        nbTrialPerBlockPerRangeOfMotion = new Dictionary<int, Dictionary<int, int>>();
        buttonsAddTrial = new List<GameObject>();
        buttonsRemoveTrial = new List<GameObject>();

        Dictionary<int, int> tempDictionary = new Dictionary<int, int>();
        tempDictionary.Add(0, 1); //RangeOfMotion: 0 - nbTrial: 1
        nbTrialPerBlockPerRangeOfMotion.Add(nbBlock-1, tempDictionary); //Block:0 - ROM: 0 - nbTrial: 1
        numberTrial.text = "1";

        //Add input trial
        addTrial.AddComponent<AddTrial>();
        addTrial.GetComponent<AddTrial>().indexBlock = nbBlock-1;
        addTrial.GetComponent<AddTrial>().indexRangeOfMotion = nbRangeOfMotion[nbBlock-1];
        addTrial.GetComponent<AddTrial>().nbTrialTxt = numberTrial;
        buttonsAddTrial.Add(addTrial);

        //Remove input trial
        removeTrial.AddComponent<RemoveTrial>();
        removeTrial.GetComponent<RemoveTrial>().indexBlock = nbBlock-1;
        removeTrial.GetComponent<RemoveTrial>().indexRangeOfMotion = nbRangeOfMotion[nbBlock - 1];
        removeTrial.GetComponent<RemoveTrial>().nbTrialTxt = numberTrial;
        buttonsRemoveTrial.Add(removeTrial);

        removeBlock.interactable = false;
    }

   
    public void AddBlock()
    {
        //Increment
        nbBlock++;

        inputFiledNumberOfBlock.text = nbBlock.ToString();

        //Add block
        GameObject newbloc = Instantiate(bloc, contentBloc.transform);
        allBlocs.Add(newbloc);

        //Title block
        newbloc.transform.Find("Index_Block").GetComponent<TextMeshProUGUI>().text = "Block "+ nbBlock;

       
        //Set list inputFiled range of motion
        Transform[] allChildren = newbloc.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.name.Contains("ContentRangeOfMotion"))
            {
                allContentRangeOfMotion.Add(child.gameObject);
                child.transform.GetChild(0).gameObject.GetComponent<TMP_InputField>().text = "1";
            }
        }

        foreach (Transform child in allChildren)
        {
            if (child.name.Contains("RemoveRangeOfMotion"))
            {
                child.gameObject.AddComponent<RemoveRangeOfMotion>();
                child.gameObject.GetComponent<RemoveRangeOfMotion>().indexBlock = nbBlock - 1;
                child.gameObject.GetComponent<RemoveRangeOfMotion>().content = allContentRangeOfMotion[allContentRangeOfMotion.Count - 1].transform;

                buttonsRemoveRangeOfMotion.Add(child.gameObject);

            }
        }

        foreach (Transform child in allChildren)
        {

            if (child.name.Contains("AddRangeOfMotion"))
            {
                child.gameObject.AddComponent<AddRangeOfMotion>();
                child.gameObject.GetComponent<AddRangeOfMotion>().indexBlock = nbBlock - 1;
                child.gameObject.GetComponent<AddRangeOfMotion>().content = allContentRangeOfMotion[allContentRangeOfMotion.Count - 1].transform;
                child.gameObject.GetComponent<AddRangeOfMotion>().removeButton = buttonsRemoveRangeOfMotion[buttonsRemoveRangeOfMotion.Count - 1].GetComponent<Button>();

                buttonsAddRangeOfMotion.Add(child.gameObject);
            }
        }

        foreach (Transform child in allChildren)
        {
            if (child.name == "InputFieldNumberScalingFactor")
            {
                buttonsRemoveRangeOfMotion[buttonsRemoveRangeOfMotion.Count - 1].GetComponent<RemoveRangeOfMotion>().nbScalingFactor = child.gameObject.GetComponent<TMP_InputField>();
                buttonsAddRangeOfMotion[buttonsAddRangeOfMotion.Count - 1].GetComponent<AddRangeOfMotion>().nbScalingFactor = child.gameObject.GetComponent<TMP_InputField>(); 
            }
        }

        buttonsRemoveRangeOfMotion[buttonsRemoveRangeOfMotion.Count - 1].GetComponent<RemoveRangeOfMotion>().Init();
        buttonsAddRangeOfMotion[buttonsAddRangeOfMotion.Count - 1].GetComponent<AddRangeOfMotion>().Init();


        //RANGE OF MOTION
        nbRangeOfMotion.Add(1);
        nbRangeOfMotion[nbBlock - 1] = 1;
        List<GameObject> inputFiledRangeOfMotion = new List<GameObject>();
        inputFiledRangeOfMotion.Add(allContentRangeOfMotion[nbBlock - 1].transform.GetChild(0).gameObject);
        allInputFiledRangeOfMotion.Add(nbBlock - 1, inputFiledRangeOfMotion);



        //Add trial button
        foreach (Transform child in allChildren)
        {
            if (child.name == "AddTrial")
            {
                child.gameObject.AddComponent<AddTrial>();
                child.gameObject.GetComponent<AddTrial>().indexBlock = nbBlock - 1;
                child.gameObject.GetComponent<AddTrial>().indexRangeOfMotion = nbRangeOfMotion[nbBlock - 1];
            }
        }

        //Remove trial button
        foreach (Transform child in allChildren)
        {
            if (child.name == "RemoveTrial")
            {
                child.gameObject.AddComponent<RemoveTrial>();
                child.gameObject.GetComponent<RemoveTrial>().indexBlock = nbBlock - 1;
                child.gameObject.GetComponent<RemoveTrial>().indexRangeOfMotion = nbRangeOfMotion[nbBlock - 1];
            }
        }

        //Get and set trial dictionary per block
        Dictionary<int, int> tempDictionary = new Dictionary<int, int>();
        tempDictionary.Add(0, 1); //RangeOfMotion: 1 - nbTrial: 1
        nbTrialPerBlockPerRangeOfMotion.Add(nbBlock - 1, tempDictionary); //Block:1 - ROM: 1 - nbTrial: 1


        if (nbBlock > 1)
        {
            removeBlock.interactable = true;
        }
    }

    void RemoveBlock()
    {
        if (nbBlock > 1)
        {         
            //Destroy gameObject
            Destroy(allBlocs[allBlocs.Count - 1]);

            //Clear dictionary trail
            nbTrialPerBlockPerRangeOfMotion.Remove(nbBlock - 1); //Remove key

            //Clear list
            buttonsAddRangeOfMotion.RemoveAt(buttonsAddRangeOfMotion.Count-1);
            buttonsRemoveRangeOfMotion.RemoveAt(buttonsRemoveRangeOfMotion.Count - 1);
            allBlocs.RemoveAt(allBlocs.Count - 1);
            allContentRangeOfMotion.RemoveAt(allContentRangeOfMotion.Count - 1);
            nbRangeOfMotion.RemoveAt(nbRangeOfMotion.Count - 1);

            allInputFiledRangeOfMotion.Remove(nbBlock-1);

            //Decrement
            nbBlock--;
            inputFiledNumberOfBlock.text = nbBlock.ToString();


            if (nbBlock == 1)
            {
                removeBlock.interactable = false;
            }
        }
    }

}
