using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class ABMLogger : MonoBehaviour {
    ABMController controller;
    public int episode = 0;
    public int maxEpisode = 100;
    public float timeScale = 1f;
    public bool logToFile = false;
    public String filePath = "";
    public String fileName = "";

    void Awake() {
        TryGetComponent<ABMController>(out controller);
        if(controller == null) {
            this.enabled = false;
        }
        Time.timeScale = timeScale;
        if(logToFile) {
            if(filePath == "") filePath = Application.persistentDataPath;
            filePath += $"/Log_{fileName}.txt";

            Debug.Log(filePath);

            if(File.Exists(filePath)) {
                try {
                    File.Delete(filePath);
                } catch (System.Exception e) {
                    Debug.LogWarning("File doesn't exist, creating...");
                    File.Create(filePath);
                }
            }
        }
    }

    public void Log(String log) {
        if(log == null) return;
        Debug.Log(log);
        if(logToFile) {
            try {
                StreamWriter s = new StreamWriter(filePath, true);
                s.Write(log + "\n");
                s.Close();
            } catch (System.Exception e) {
                Debug.LogError("Cannot write into file" + " " + e.ToString());
            }
        }
    }
}
