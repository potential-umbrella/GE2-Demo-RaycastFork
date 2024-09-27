using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Sam Robichaud 
// NSCC Truro 2024
// This work is licensed under CC BY-NC-SA 4.0 (https://creativecommons.org/licenses/by-nc-sa/4.0/)

public class LevelManager : MonoBehaviour
{
    [Header("Script References")]
    public GameStateManager _gameStateManager;
    public CameraManager _cameraManager;
    public GameManager _gameManager;
    public PlayerManager _playerManager;
    public UIManager _uIManager;

    AsyncOperation loadOperation;
    public int nextScene;

    // Important scene refs.
    [SerializeField] string mainMenuName = "MainMenu";

    public void Awake()
    {
        // Check for missing script references
        if (_gameStateManager == null) { Debug.LogError("LevelManager is not assigned to LevelManager in the inspector!"); }
        if (_cameraManager == null) { Debug.LogError("CameraManager is not assigned to LevelManager in the inspector!"); }
        if (_gameManager == null) { Debug.LogError("GameManager is not assigned to LevelManager in the inspector!"); }
        if (_playerManager == null) { Debug.LogError("PlayerManager is not assigned to LevelManager in the inspector!"); }
        if (_uIManager == null) { Debug.LogError("UIManager is not assigned to LevelManager in the inspector!"); }
    }

    private void Update()
    {
        LoadBarHelper();

        void LoadBarHelper()
        {
            if (loadOperation != null) { _uIManager.UpdateLoadBar(loadOperation.progress); Debug.Log(loadOperation.progress); }
        }
    }

    void LoadScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void LoadNextlevel()
    {
        // I feel like this is a wonky way of connecting the other loadscene, but it should work.
        nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        LoadScene(SceneManager.GetSceneByBuildIndex(nextScene).name);
        _gameStateManager.SwitchToState(_gameStateManager.gameState_GamePlay);
    }

    public void LoadMainMenuScene()
    {
        LoadScene(mainMenuName);
        _gameStateManager.SwitchToState(_gameStateManager.gameState_GameInit);
    }

    public void ReloadCurrentScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
        _gameStateManager.SwitchToState(_gameStateManager.gameState_GamePlay);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void OnLoadFinish(Scene ignored1, LoadSceneMode ignored2) { OnLoadFinish(); }
    void OnLoadFinish()
    {
        string s = SceneManager.GetActiveScene().name;
        switch (s)
        {
            case "MainMenu":
                _uIManager.DisableLoadScreen(
                //_uIManager.mainMenuUI
                                                          ); break;
            case "TestLevel":
                _uIManager.DisableLoadScreen(
                //_uIManager.gamePlayUI
                                                           ); break;
            default: Debug.LogError(new NotImplementedException($"{s} is not implemented in load finish.")); break;
        }

        SceneManager.sceneLoaded -= OnLoadFinish;
        loadOperation = null;
    }

    public void LoadScene(string name)
    {
        switch (name)
        {
            case "MainMenu":
                _uIManager.UILoadingScreen(
                //_uIManager.mainMenuUI
                );
                _gameStateManager.SwitchToState(_gameStateManager.gameState_GameInit); break;
            case "TestLevel":
                _uIManager.UILoadingScreen(
                    //_uIManager.gamePlayUI
                    );
                _gameStateManager.SwitchToState(_gameStateManager.gameState_GamePlay);
                break;
            default: Debug.LogError(new NotImplementedException($"{name} is not implemented in scene load start. Cancelling load.")); return;
        }

        // Actually load the scene.
        loadOperation = SceneManager.LoadSceneAsync(name);
        SceneManager.sceneLoaded += OnLoadFinish;
    }
}