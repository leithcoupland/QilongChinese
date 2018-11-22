using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class QuestionParser : MonoBehaviour {

	public static QuestionParser instance = null;

	const string QUESTION_PATH = "Questions/";

	DirectoryInfo dir;
	FileInfo[] fileList;
	List<string> flashcardList;
	string[] questions;
	string[] answers;

	void Awake(){
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);

			if (SceneManager.GetActiveScene().name == "Game" && (flashcardList == null || flashcardList.Count == 0)) {
				SetupFlashcards ();
			}
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	public void SetupFlashcards(int index = 0){
		CheckFolder();
		LoadFlashcards (index);
		ParseQuestions ();
	}

	public FileInfo[] CheckFolder(){
		dir = new DirectoryInfo(QUESTION_PATH);
		fileList = dir.GetFiles ("*.txt");
		return fileList;
	}

	void LoadFlashcards(int index){
		string name = fileList[index].Name;
		//Debug.Log ("LOADING FROM " + name);
		try {
			flashcardList = new List<string> ();

			using (StreamReader sr = new StreamReader (QUESTION_PATH + name)){
				string line;
				while ((line = sr.ReadLine()) != null){
					flashcardList.Add (line);
				}
			}
		} catch (Exception e){
			Debug.Log ("File " + QUESTION_PATH + name + ".txt could not be read.");
			Debug.Log (e.Message);
		}
	}

	void ParseQuestions(){
		List<string> questionList = new List<string> ();
		List<string> answerList = new List<string> ();

		foreach (string s in flashcardList){
			string[] components = s.Split (new[] {" | "}, StringSplitOptions.RemoveEmptyEntries);

			if (components.Length != 2) {
				Debug.Log ("Invalid flashcard: " + components [0]);
				questionList.Add ("");
				answerList.Add ("");
			} else {
				questionList.Add (components [0]);
				answerList.Add (components [1]);
			}
		}

		questions = questionList.ToArray ();
		answers = answerList.ToArray ();
	}

	public string[] GetAnswers(){
		return answers;
	}

	public string[] GetQuestions(){
		return questions;
	}
}
