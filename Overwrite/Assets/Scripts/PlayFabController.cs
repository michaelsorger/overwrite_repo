using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class PlayFabController : MonoBehaviour
{
    //references to inspector
    [SerializeField] private GameObject _levelsPanel; //ui panel to represent current levels saved in catalogue
    [SerializeField] private LevelInformation _draftLevelInfo; //ref to script obj file created in inspector for use in draft and uploading

    //constants
    private string titleId = "16C73"; //from PlayFab tab -> settings -> titleid = number in parenthesis only 
    private string thisCustomId = "GettingStartedGuide";

    //ugc variables
    private EntityTokenResponse entityToken;
    private string entityId;
    private string entityType;
    private Dictionary<string, string> _entityFileJson = new Dictionary<string, string>();
    private string levelDataFilePath = "LevelInfoFiles/ALevelInfo";

    //public variable for knowing when its safe to access content management system
    public int GlobalFileLock;

    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = titleId;
        }
        var request = new LoginWithCustomIDRequest { CustomId = thisCustomId, CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnSharedFailure);
        GlobalFileLock = 0;
    }

    public void OnSaveDraftButtonClick()
    {
        LevelInformation thisLevel = LevelCreatorManager.SerializeLevelIntoScriptObj(GameManager.theTempLevel);
        _draftLevelInfo.tagList = thisLevel.tagList;
        _draftLevelInfo.tagGameObjectListJSON = thisLevel.tagGameObjectListJSON;
        _draftLevelInfo.switcherControlJSON = thisLevel.switcherControlJSON;
        _draftLevelInfo.playerStartPosition = thisLevel.playerStartPosition;

#if UNITY_EDITOR
        EditorUtility.SetDirty(_draftLevelInfo);
        AssetDatabase.SaveAssets();
#endif
    }

    public void OnPublishLevelButtonClick()
    {
        //publishes the current draft Temo Level Info
        string LevelDraftAbsPath = Application.dataPath + "\\Scriptable Objects\\Level_Draft.asset";
        Debug.Log("OnPublishLevelButtonClick" + LevelDraftAbsPath);
        byte[] scriptObj = File.ReadAllBytes(LevelDraftAbsPath);
        if(!_entityFileJson.ContainsKey(levelDataFilePath))
        {
            _entityFileJson.Add(levelDataFilePath, Encoding.UTF8.GetString(scriptObj));
        }
        else
        {
            _entityFileJson[levelDataFilePath] = Encoding.UTF8.GetString(scriptObj);
        }

        Debug.Log("levelDataFilePath = " + levelDataFilePath);
        Debug.Log("bytes = " + _entityFileJson[levelDataFilePath]);

        UploadLevelDataFile();
    }

    public void OnDisplayLevelsButtonClick()
    {
        //displays all ugc levels, able to pick and load one
    }

    public void OnDismissLevelsButtonClick()
    {
        //dismisses display of ugc levels (basically an exit button)
    }

    private void OnLoginSuccess(LoginResult result)
    {     
        entityId = result.EntityToken.Entity.Id;
        entityType = result.EntityToken.Entity.Type;
        Debug.Log("OnLoginSuccess and got entity.id = " + entityId + " and entity.type = " + entityType);
    }

    private void LoadAllFiles()
    {
        if (GlobalFileLock != 0)
            throw new Exception("This example overly restricts file operations for safety. Careful consideration must be made when doing multiple file operations in parallel to avoid conflict.");

        GlobalFileLock += 1; // Start GetFiles
        var request = new PlayFab.DataModels.GetFilesRequest {
            Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType }
        };
        PlayFabDataAPI.GetFiles(request, OnGetFileMeta, OnSharedFailure);
    }

    private void OnGetFileMeta(PlayFab.DataModels.GetFilesResponse result)
    {
        Debug.Log("Loading " + result.Metadata.Count + " files");

        _entityFileJson.Clear();
        foreach (var eachFilePair in result.Metadata)
        {
            _entityFileJson.Add(eachFilePair.Key, null); //filename, null
            GetActualFile(eachFilePair.Value); //populates this filename with its filecontents
        }
        GlobalFileLock -= 1; // Finish GetFiles
    }

    private void GetActualFile(PlayFab.DataModels.GetFileMetadata fileData)
    {
        GlobalFileLock += 1; // Start Each SimpleGetCall
        PlayFabHttp.SimpleGetCall(fileData.DownloadUrl,
        resultByteArr => { _entityFileJson[fileData.FileName] = Encoding.UTF8.GetString(resultByteArr);
            GlobalFileLock -= 1;
            Debug.Log("OnSimpleGetCall success with file = " + fileData.FileName);
        }, // Success SimpleGetCall on this file, filename value = these bytes
        error => { Debug.Log("OnSimpleGetCall error with error msg = " + error); GlobalFileLock -= 1; });
    }

    private void UploadLevelDataFile()
    {
        if (GlobalFileLock != 0)
            throw new Exception("This example overly restricts file operations for safety. Careful consideration must be made when doing multiple file operations in parallel to avoid conflict.");

        GlobalFileLock += 1; // Start InitiateFileUploads
        var request = new PlayFab.DataModels.InitiateFileUploadsRequest
        {
            Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType },
            FileNames = new List<string> { levelDataFilePath },
        };
        PlayFabDataAPI.InitiateFileUploads(request, OnInitFileUpload, OnSharedFailure);
    }

    private void OnInitFileUpload(PlayFab.DataModels.InitiateFileUploadsResponse response)
    {
        string payloadStr;
        if (!_entityFileJson.TryGetValue(levelDataFilePath, out payloadStr))
            payloadStr = "{}";
        var payload = Encoding.UTF8.GetBytes(payloadStr);

        GlobalFileLock += 1; // Start SimplePutCall
        PlayFabHttp.SimplePutCall(response.UploadDetails[0].UploadUrl,
            payload,
            FinalizeUpload,
            error => { Debug.Log(error); }
        );
        GlobalFileLock -= 1; // Finish InitiateFileUploads
    }

    private void FinalizeUpload(byte[] resultFileBytes)
    {
        GlobalFileLock += 1; // Start FinalizeFileUploads
        var request = new PlayFab.DataModels.FinalizeFileUploadsRequest
        {
            Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType },
            FileNames = new List<string> { levelDataFilePath },
        };
        PlayFabDataAPI.FinalizeFileUploads(request, OnUploadSuccess, OnSharedFailure);
        GlobalFileLock -= 1; // Finish SimplePutCall
    }

    private void OnUploadSuccess(PlayFab.DataModels.FinalizeFileUploadsResponse result)
    {
        Debug.Log("File upload success: " + levelDataFilePath);
        GlobalFileLock -= 1; // Finish FinalizeFileUploads
    }

    //handle failure callback
    private void OnSharedFailure(PlayFabError error)
    {
        Debug.LogWarning("OnSharedFailure, failed to perform operation due to:");
        Debug.LogError(error.GenerateErrorReport());
        GlobalFileLock -= 1;
    }
}
