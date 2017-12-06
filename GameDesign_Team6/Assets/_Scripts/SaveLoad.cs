using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

/**
 * People that worked on this script:
 * Matthew Stewart
**/

public static class SaveLoad {

	public static List<GameManagerData> savedGames = new List<GameManagerData>();

	private static string savedGamesPath = Path.Combine(Application.persistentDataPath , "savedGames.gd");

	public static int Save(int index) {
		if (index < 0) {
			SaveLoad.Load (-1);
			if (savedGames.Count > 2) {
				return -1;
			}
			index = savedGames.Count;
			savedGames.Add (GameManager.instance.createData());
		} else if (index >= 0 && index < savedGames.Count) {
			savedGames [index] = GameManager.instance.createData();
		} else {
			return -1;
		}
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (savedGamesPath);
		bf.Serialize(file, SaveLoad.savedGames);
		file.Close();
		return index;
	}
		
	public static GameManagerData Load(int index) {
		if(File.Exists(savedGamesPath)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(savedGamesPath, FileMode.OpenOrCreate);
			SaveLoad.savedGames = (List<GameManagerData>)bf.Deserialize(file);
			file.Close();
		}
		if (index >= 0 && index < savedGames.Count) {
			return savedGames [index];
		} else {
			return null;
		}
	}
}
