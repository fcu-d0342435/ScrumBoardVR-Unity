using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System;
public class Speech : MonoBehaviour {
    Process Processee;
    StreamWriter myStreamWriter;
	public InputField[] textField = new InputField[5];
	Text inputtotext;
    Thread thread;
    bool start = false;
    string stringtemp = "";
	int linetemp;
	int fieldNumber = 0;
	TextGenerator ge;

    private void Start()
    {
		textField[fieldNumber].lineType = InputField.LineType.MultiLineNewline;
        Processee = new Process();
        string jsonCredentialsPath = Path.Combine(Application.streamingAssetsPath, "application_default_credentials.json");
        Processee.StartInfo.Arguments = jsonCredentialsPath;
        Processee.StartInfo.FileName = Path.Combine(Application.streamingAssetsPath, "ConsoleApplication3.exe");
        thread = new Thread(ProcesswStart);
        thread.IsBackground = true;
        thread.Start();
		changeField (fieldNumber);



        //UnityEngine.Debug.Log("ss");
        //SmartLogger.LogError(DebugFlags.GoogleStreamingSpeechToText, "This service is only supported on Windows.");
    }

    public  void ProcesswStart()
    {
        Processee.StartInfo.UseShellExecute = false;
        Processee.StartInfo.RedirectStandardInput = true;
        Processee.StartInfo.RedirectStandardOutput = true;
		Processee.StartInfo.StandardOutputEncoding = System.Text.Encoding.Default;
        
        //Processee.StartInfo.CreateNoWindow = true;
        // Processee.Kill();
        //StartCoroutine(Nowplay());
        Processee.Start();
		beginRead();
        //Processee.StandardOutput.ReadToEnd();

        //Processee.StandardOutput.Read();


        //UnityEngine.Debug.Log(Processee.StandardOutput.Read());
        //Processee.WaitForExit();
        //s2.text= Processee.StandardOutput.ReadToEnd().ToString();


    }
    public void speechStop()
    {
        myStreamWriter = Processee.StandardInput;
        myStreamWriter.WriteLine("Stop");
        start = false;

    }
    public void speechStart()
    {
		
        myStreamWriter = Processee.StandardInput;
		textField[fieldNumber].ActivateInputField ();
        myStreamWriter.WriteLine("Start");
        start = true;
		UnityEngine.Debug.Log (textField[fieldNumber].name);
		UnityEngine.Debug.Log ("speechok");
    }
    public void readText()
    {

        // if (!Processee.StandardOutput.EndOfStream) {
        while (Processee.StandardOutput.EndOfStream != true) {
            stringtemp = Processee.StandardOutput.ReadLine();

            UnityEngine.Debug.Log(stringtemp.Length);
            start = true;
        }
       
        // Processee.Dispose();
        //   }

        //Processee.Kill();
    }
    public  void beginRead()
    {

		 thread = new Thread(readText);
         thread.IsBackground = true;
         thread.Start();

        //s2.text += "ss中文";
        //s2.text;
        // UnityEngine.Debug.Log(Processee.StandardOutput.ReadToEnd());
        //Processee.Close();
        //Processee.Kill();
    }

	public  void cleanText()
	{
		textField[fieldNumber].text = "";
		linetemp = 1;
	}

	public  void deleteText()
	{
		textField[fieldNumber].text = textField[fieldNumber].text.Remove(textField[fieldNumber].caretPosition-1,1);
		caretleft ();
		linetemp = 1;
	}


	public  void caretleft()
	{

		if(linetemp >= 1){
			if(textField[fieldNumber].caretPosition == ge.lines[linetemp-1].startCharIdx)
			{
				UnityEngine.Debug.Log ("second   "+linetemp);
				linetemp--;
			}

		}
		textField[fieldNumber].caretPosition--;
	}

	public  void caretRight()
	{
		//UnityEngine.Debug.Log ("second   "+linetemp);
		//UnityEngine.Debug.Log ("second   "+ge.lineCount);
		if(linetemp <= ge.lineCount)
		{
			textField[fieldNumber].caretPosition++;
			if(linetemp > 1 && textField[fieldNumber].caretPosition == ge.lines[linetemp].startCharIdx )
			{
				linetemp++;
			}
		}


	}

	//public  void caretUp()
	//{
	//	if(linetemp>0){
			
	//		linetemp--;
	//		s2.caretPosition = ge.lines[linetemp].startCharIdx-1;
	//		UnityEngine.Debug.Log(linetemp);
	//		UnityEngine.Debug.Log(s2.caretPosition);
	//	}

	//}
	//public  void caretDown()
	//{
		
	//	linetemp++;
	//	s2.caretPosition = ge.lines[linetemp].startCharIdx;
	//	UnityEngine.Debug.Log(linetemp);
	//	UnityEngine.Debug.Log(s2.caretPosition);
	//}


	public void newline()
	{
		textField[fieldNumber].text = textField[fieldNumber].text.Insert (textField[fieldNumber].caretPosition, "\n");
		//UnityEngine.Debug.Log ("Temp"+linetemp);
		textField[fieldNumber].caretPosition = ge.lines[linetemp-1].startCharIdx;
	}
    void Update()
    {
        if (start)
        {
			
			textField[fieldNumber].text = textField[fieldNumber].text.Insert(textField[fieldNumber].caretPosition,stringtemp);
			textField[fieldNumber].caretPosition += stringtemp.Length;
			stringtemp = "";
			UnityEngine.Debug.Log (textField[fieldNumber].name);
        }
        start = false;

    }
	public void changeField(int NewField){
		fieldNumber=NewField;
		inputtotext = textField[fieldNumber].GetComponentsInChildren<Text> () [1];
		ge = inputtotext.cachedTextGenerator;
		UnityEngine.Debug.Log (textField[fieldNumber].name);
		//input.caretPosition-=1;
		linetemp = 1;
	}

}
