using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	public GameObject mainMenuHolder;
	public GameObject optionsMenuHolder;
	public GameObject highscoresMenuHolder;

	public Slider[] volumeSliders;
	public Toggle[] resolutionToggles;
	public Toggle fullscreenToggle;
	public int[] screenWidths;
	int activeScreenResIndex;

	public Text[] highscoreText;
	public Highscore[] highscoresList;

	void Start()
	{
		activeScreenResIndex = PlayerPrefs.GetInt("screen res index");
		bool isFullscreen = (PlayerPrefs.GetInt("fullscreen") == 1) ? true : false;

		volumeSliders[0].value = AudioManager.instance.masterVolumePercent;
		volumeSliders[1].value = AudioManager.instance.musicVolumePercent;
		volumeSliders[2].value = AudioManager.instance.sfxVolumePercent;

		for (int i = 0; i < resolutionToggles.Length; i++)
		{
			resolutionToggles[i].isOn = i == activeScreenResIndex;
		}

		fullscreenToggle.isOn = isFullscreen;
		StartCoroutine(GetCurrentHighscore());
	}

	public void UpdateHighscoreUI(Highscore[] hsList)
	{
		for (int i = 0; i < highscoreText.Length; i++)
		{
			highscoreText[i].text = i + 1 + ". ";
			if (hsList.Length > i)
			{
				highscoreText[i].text += hsList[i].username + " - " + hsList[i].score;
			}
		}
	}

	
	public void Play()
	{
		SceneManager.LoadScene("Game");
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void OptionsMenu()
	{
		mainMenuHolder.SetActive(false);
		optionsMenuHolder.SetActive(true);
		highscoresMenuHolder.SetActive(false);
	}

	public void HighscoresMenu()
	{
		mainMenuHolder.SetActive(false);
		optionsMenuHolder.SetActive(false);
		highscoresMenuHolder.SetActive(true);
	}

	public void MainMenu()
	{
		mainMenuHolder.SetActive(true);
		optionsMenuHolder.SetActive(false);
		highscoresMenuHolder.SetActive(false);
	}

	public void SetScreenResolution(int i)
	{
		if (resolutionToggles[i].isOn)
		{
			activeScreenResIndex = i;
			float aspectRatio = 16 / 9f;
			Screen.SetResolution(screenWidths[i], (int)(screenWidths[i] / aspectRatio), false);
			PlayerPrefs.SetInt("screen res index", activeScreenResIndex);
			PlayerPrefs.Save();
		}
	}

	public void SetFullscreen(bool isFullscreen)
	{
		for (int i = 0; i < resolutionToggles.Length; i++)
		{
			resolutionToggles[i].interactable = !isFullscreen;
		}

		if (isFullscreen)
		{
			Resolution[] allResolutions = Screen.resolutions;
			Resolution maxResolution = allResolutions[allResolutions.Length - 1];
			Screen.SetResolution(maxResolution.width, maxResolution.height, true);
		}
		else
		{
			SetScreenResolution(activeScreenResIndex);
		}

		PlayerPrefs.SetInt("fullscreen", ((isFullscreen) ? 1: 0));
		PlayerPrefs.Save();
	}

	public void SetMasterVolume(float value)
	{
		AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
	}

	public void SetMusicVolume(float value)
	{
		AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);
	}

	public void SfxVolume(float value)
	{
		AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Sfx);
	}

	IEnumerator GetCurrentHighscore()
	{
		var url = "http://www.d3bg.org/telerikacademy/unity/TDS/scoreservice/index.php";
		WWW www = new WWW(url);

		yield return www;

		if (www.error == null)
		{
			FormatHighscores(www.text);
		}
		else
		{
			Debug.Log("Error: " + www.error);
		}
	}

	void FormatHighscores(string textStream)
	{
		string[] entries = textStream.Split(new char[] { '|' }, System.StringSplitOptions.RemoveEmptyEntries);
		highscoresList = new Highscore[entries.Length];

		for (int i = 0; i < entries.Length; i++)
		{
			string[] entryInfo = entries[i].Split(new char[] { '-' }, System.StringSplitOptions.RemoveEmptyEntries);
			string username = entryInfo[0];
			int score = int.Parse(entryInfo[1]);
			highscoresList[i] = new Highscore(username, score);
			print(highscoresList[i].username + ": " + highscoresList[i].score);
		}
		UpdateHighscoreUI(highscoresList);
	}

	public struct Highscore
	{
		public string username;
		public int score;

		public Highscore(string _username, int _score)
		{
			username = _username;
			score = _score;
		}
	}
}
