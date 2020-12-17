using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Threading;
using SFB;
using TMPro;

public class WriteData : MonoBehaviour
{
    public static WriteData instance;

    char[] separator;

    public List<string> data;
    public Dictionary<int, List<string>> dictionnaryData;
    int index;

    public string path;
    public GameObject configSection;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Two instance of WriteData is not possible!");
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        separator = new char[2];
        separator[0] = '\n';
        separator[1] = ';';

        data = new List<string>();
        dictionnaryData = new Dictionary<int, List<string>>();

        ColumnTitle();//Create your the first row with titles
    }

    public void SaveOutputDataPath(TMP_InputField _inputField)
    {
        var _path = StandaloneFileBrowser.OpenFolderPanel("Data", "",false);
        //var _path = StandaloneFileBrowser.SaveFilePanel("Title", "", "Data", "csv");
        if (_path.Length > 0 && !string.IsNullOrEmpty(_path[0]))
        {
            path = _path[0];
            _inputField.text = path;
        }
    }

    void ColumnTitle()
    {
        data = new List<string>();

        data.Add("Id" + separator[1]);
        data.Add("Trial_number"+ separator[1]);
        data.Add("Scaling_factor" + separator[1]);
        data.Add("Validate" + separator[1]);
        data.Add("StayBellowTheLine");

        dictionnaryData.Add(index, data);

        index++;
    }

    ///Add data 
    public void AddData(string _id, string _trialNumber, string _scalingFactor, string _validate, string _stayBellowTheLine)
    {
        data = new List<string>();

        data.Add(_id + separator[1]); //Use separtor ";" to pass to next row
        data.Add(_trialNumber + separator[1]); //Use separtor ";" to pass to next row
        data.Add(_scalingFactor + separator[1]); //Use separtor ";" to pass to next row
        data.Add(_validate + separator[1]); //Use separtor ";" to pass to next row
        data.Add(_stayBellowTheLine);  //Don't use separtor "; " because it's the last row

        dictionnaryData.Add(index, data);

        index++;
    }

    public void ClearData()
    {
        dictionnaryData = new Dictionary<int, List<string>>();
        index = 0;
    }

    public void WriteAllData()
    {
        if (path != "")
        {
            string logPath = path + "\\Log_" + Settings.instance.participantId + ".csv";
            StreamWriter outStream = System.IO.File.AppendText(logPath);
            
            //Write data
            for (int i = 0; i < dictionnaryData.Keys.Count; i++)
            {
                foreach (string item in dictionnaryData[i])
                {
                    outStream.Write(item);
                }

                outStream.Write(separator[0]);//Use separtor "\n" to pass to next line
            }

            outStream.Close();
        }
    }

    
}
