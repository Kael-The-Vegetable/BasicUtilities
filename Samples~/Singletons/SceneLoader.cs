using BasicUtilities;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
	protected override void Initialize() {}

	public void LoadScene(string sceneName)
	{
		EditorSceneManager.LoadSceneAsyncInPlayMode(
			AssetDatabase.GUIDToAssetPath(
				AssetDatabase.FindAssets(
					"t:SceneAsset " + sceneName,
					new string[] { "Assets/Samples/Basic Utilities" }
				)[0]
			), new LoadSceneParameters { loadSceneMode = LoadSceneMode.Single }
		);
	}
}
