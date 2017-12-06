using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * People that touched this script:
 * Matthew Stewart
**/

public class LevelLoader : MonoBehaviour {


	public GameObject loadingScreen;
	public Slider slider;
	public Text progressText;
	private GameManager GameManager;

	void Start() {
		GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		LoadLevel(GameManager.curScene);
	}

	public void LoadLevel (string sceneName) {
		StartCoroutine (LoadAsync (sceneName));
	}

	IEnumerator LoadAsync (string sceneName) {
		AsyncOperation op = SceneManager.LoadSceneAsync (sceneName);

		loadingScreen.SetActive (true);

		while (!op.isDone) {
			float progress = Mathf.Clamp01 (op.progress / 0.9f) * 100f;

			slider.value = progress;
			progressText.text = Mathf.Round(progress) + "%";

			yield return null;
		}
	}
}
