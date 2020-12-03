using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using OBSWebsocketDotNet;
using TMPro;
using SFB;

public class OBSRecorder : MonoBehaviour {

	private bool isRecording = false;
	private Process obsFirstView;
	private Process obsThirdView;
	private Process obsCommandFirstView;
	private Process obsCommandThirdView;

	OBSWebsocket oBSWebsocketFirstView;
	OBSWebsocket oBSWebsocketThirdView;

	public string firstViewOutPutPath;
	public string thirdViewOutPutPath;

	public int indexVideoFirstView = 1;
	public int indexVideoThirdView = 1;

	string[] separator = new string[] { "/" };

	public static OBSRecorder Instance { get; private set; }

	// Use this for initialization
	void Awake () {
		if (Instance == null) {
			Instance = this;
		}  else if (Instance != this) {
			return;
		}

		DontDestroyOnLoad(gameObject);

		if (FindObjectsOfType (GetType ()).Length > 1) {
			Destroy (gameObject);
		}

		foreach (var process in Process.GetProcessesByName("obs64"))
		{
			process.Kill();
		}

	}


    public void KillRecording() {
		if (!isRecording) {
			return;
		}
		isRecording = false;
	}

   

    public IEnumerator CreateProcessFirstView()
    {
		obsFirstView = new Process();
		obsFirstView.StartInfo.FileName = "C:\\Program Files\\obs-studio\\bin\\64bit\\obs64.exe";
		obsFirstView.StartInfo.Arguments = "--multi -m --scene LBP_Scene_FirstView --profile LBP_FirstView";
		obsFirstView.StartInfo.WorkingDirectory = "C:\\Program Files\\obs-studio\\bin\\64bit";
		obsFirstView.StartInfo.CreateNoWindow = true;
		obsFirstView.StartInfo.UseShellExecute = false;
		obsFirstView.Start();
		//--minimize-to-tray 

		yield return new WaitForSeconds(2);

		var processInfo = new ProcessStartInfo("cmd.exe", "C: cd C:\\Program Files\\obs-studio\\OBSCommand\\OBSCommand.exe /server==127.0.0.1:4445");
		processInfo.CreateNoWindow = true;
		processInfo.UseShellExecute = false;
	    obsCommandFirstView = new Process();
		obsCommandFirstView.StartInfo = processInfo;
		obsCommandFirstView.Start();
	}

	public void StartRecordingFirstView()
    {
		UnityEngine.Debug.Log("StartRecordingFirstView" );

		//UnityEngine.Debug.Log(path);
		oBSWebsocketFirstView = new OBSWebsocket();
		oBSWebsocketFirstView.Connect("ws://127.0.0.1:4445", "");
		//UnityEngine.Debug.Log(oBSWebsocketFirstView.GetStreamSettings().Settings.Server);
		UnityEngine.Debug.Log("OBS Websocket FirstView is connected = " + oBSWebsocketFirstView.IsConnected);
		oBSWebsocketFirstView.SetCurrentProfile("LBP_FirstView");
		oBSWebsocketFirstView.SetRecordingFolder(WriteData.instance.path);
		oBSWebsocketFirstView.SetFilenameFormatting("FirstView" + "_" + Settings.instance.ParticipantId +"_"+ indexVideoFirstView/*"_%CCYY-%MM-%DD-%hh-%mm-%ss"*/);
		oBSWebsocketFirstView.StartRecording();

		indexVideoFirstView++;
	}

	public void StopRecordingFirstView()
    {
		if (obsFirstView != null /*&& oBSWebsocketFirstView.IsConnected*/)
        {
            try
            {
				UnityEngine.Debug.Log("Stop recording First view");
				oBSWebsocketFirstView.StopRecording();
				oBSWebsocketFirstView.Disconnect();
			}
            catch (System.Exception e)
            {
				UnityEngine.Debug.LogError(e.ToString());
			}
			
		}
	}

	public IEnumerator CreateProcessThirdView()
	{
		obsThirdView = new Process();
		obsThirdView.StartInfo.FileName = "C:\\Program Files\\obs-studio\\bin\\64bit\\obs64.exe";
		obsThirdView.StartInfo.Arguments = "--multi -m --scene LBP_Scene_ThirdView --profile LBP_ThirdView";
		obsThirdView.StartInfo.WorkingDirectory = "C:\\Program Files\\obs-studio\\bin\\64bit";
		obsThirdView.StartInfo.CreateNoWindow = true;
		obsThirdView.StartInfo.UseShellExecute = false;
		obsThirdView.Start();
		//--minimize-to-tray 

		//yield return true;
		yield return new WaitForSeconds(2);

		var processInfo = new ProcessStartInfo("cmd.exe", "C: cd C:\\Program Files\\obs-studio\\OBSCommand\\OBSCommand.exe /server==127.0.0.1:4444");
		processInfo.CreateNoWindow = true;
		processInfo.UseShellExecute = false;
		obsCommandThirdView = new Process();
		obsCommandThirdView.StartInfo = processInfo;
		obsCommandThirdView.Start();
		//--minimize-to-tray 
	}

	public void StartRecordingThirdView()
	{

		oBSWebsocketThirdView = new OBSWebsocket();
		oBSWebsocketThirdView.Connect("ws://127.0.0.1:4444", "");
		UnityEngine.Debug.Log("OBS Websocket ThirdView is connected = "+ oBSWebsocketThirdView.IsConnected);
		oBSWebsocketThirdView.SetCurrentProfile("LBP_ThirdView");
		oBSWebsocketThirdView.SetRecordingFolder(WriteData.instance.path);
		oBSWebsocketThirdView.SetFilenameFormatting("ThirdView" + "_" + Settings.instance.ParticipantId + "_" + indexVideoThirdView/*"_%CCYY-%MM-%DD-%hh-%mm-%ss"*/);
		oBSWebsocketThirdView.StartRecording();

		indexVideoThirdView++;
	}

	public void StopRecordingThirdView()
	{
		if (obsThirdView != null /*&& oBSWebsocketThirdView.IsConnected*/)
        {
            try
            {
				UnityEngine.Debug.Log("Stop recording Third view");
				oBSWebsocketThirdView.StopRecording();
				oBSWebsocketThirdView.Disconnect();
			}
            catch (System.Exception e)
            {
				UnityEngine.Debug.LogError(e.ToString());
				
            }

			
		}

	}


	public void KillProcessFirstView()
    {
		//if(obsCommandFirstView != null)
			//obsCommandFirstView.Kill();
		
		obsFirstView.Kill();
	}

	public void KillProcessThirdView()
	{
		//if(obsCommandThirdView != null)
			//obsCommandThirdView.Kill();

		obsThirdView.Kill();
	}

	public void SaveOutputDataPathFirstView(TMP_InputField _inputField)
	{
		var _path = StandaloneFileBrowser.SaveFilePanel("Title", "", "FirstView", "mp4");
		if (_path.Length > 0 && !string.IsNullOrEmpty(_path))
		{
			firstViewOutPutPath = _path;
			_inputField.text = firstViewOutPutPath;
		}
	}

	public void SaveOutputDataPathThirdView(TMP_InputField _inputField)
	{
		var _path = StandaloneFileBrowser.SaveFilePanel("Title", "", "ThirdView", "mp4");
		if (_path.Length > 0 && !string.IsNullOrEmpty(_path))
		{
			thirdViewOutPutPath = _path;
			_inputField.text = thirdViewOutPutPath;
		}
	}

	private void OnDestroy()
	{
		if (Instance != this)
		{
			return;
		}

		if (oBSWebsocketFirstView != null && oBSWebsocketFirstView.IsConnected && Manager.instance.GetStatusRecordingFirstView == true)
			oBSWebsocketFirstView.StopRecording();

		if (oBSWebsocketThirdView != null && oBSWebsocketThirdView.IsConnected && Manager.instance.GetStatusRecordingThirdView == true)
			oBSWebsocketThirdView.StopRecording();

		if (obsFirstView != null)
		{
			KillProcessFirstView();
		}
		if (obsThirdView != null)
		{
			KillProcessThirdView();
		}
	}

	/*private void OnApplicationQuit()
	{
		if (Instance != this)
		{
			return;
		}

		if (oBSWebsocketFirstView != null && oBSWebsocketFirstView.IsConnected)
			oBSWebsocketFirstView.StopRecording();

		if (oBSWebsocketThirdView != null && oBSWebsocketThirdView.IsConnected)
			oBSWebsocketThirdView.StopRecording();

		if (obsFirstView != null)
		{
			KillProcessFirstView();
		}
		if (obsThirdView != null)
		{
			KillProcessThirdView();
		}
	}*/

}
	