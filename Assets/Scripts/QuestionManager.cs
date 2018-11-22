using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour {

	public static QuestionManager instance = null;

	public Text questionText;
	public Text answerPrompt;
	public float minTimeBetweenAnswerSpawns;
	public float maxTimeBetweenAnswerSpawns;
	float timeBeforeNextAnswerSpawn;

	string[] questions;
	string[] answers;

	Hashtable qAndA = new Hashtable();
	Queue questionQueue = new Queue();
	string currentQuestion = "";
	float timeSinceLastAnswerSpawn;

	void Awake(){
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	void Start(){
		questions = QuestionParser.instance.GetQuestions ();
		answers = QuestionParser.instance.GetAnswers ();

		List<string> qList = new List<string> ();

		for (int i = 0; i < Mathf.Min(questions.Length, answers.Length); i++){
			qAndA.Add (questions [i], answers [i]);
			qList.Add (questions [i]);
		}

		Shuffle (ref qList);
		foreach (string s in qList){
			questionQueue.Enqueue (s);
		}

		ResetSpawnTimers ();
		NewQuestion ();
	}

	void Update(){
		questionText.text = currentQuestion;
		timeSinceLastAnswerSpawn += Time.deltaTime;
	}

	void NewQuestion(){
		if (questionQueue.Count != 0) {
			currentQuestion = (string)questionQueue.Dequeue ();
		} else {
			currentQuestion = "QUESTIONS COMPLETE";
		}
	}

	public string RandomAnswer(bool badLuckProtection = false){
		if (badLuckProtection) {
			if (timeSinceLastAnswerSpawn >= timeBeforeNextAnswerSpawn && qAndA.ContainsKey (currentQuestion)) {
				ResetSpawnTimers ();
				return (string)qAndA [currentQuestion];
			}
		}
		return answers [Random.Range (0, answers.Length)];
	}

	public static void Shuffle<T>(ref List<T> list){
		for (int i = 0; i < list.Count; i++){
			int k = Random.Range (0, list.Count);
			T temp = list [k];
			list [k] = list [i];
			list [i] = temp;
		}
	}

	void ResetSpawnTimers(){
		timeSinceLastAnswerSpawn = 0;
		timeBeforeNextAnswerSpawn = Random.Range (minTimeBetweenAnswerSpawns, maxTimeBetweenAnswerSpawns);
	}

	public bool TryAnswer(string answer){
		if (qAndA.ContainsKey(currentQuestion) && qAndA[currentQuestion].Equals(answer)){
			NewQuestion();
			return true;
		}
		return false;
	}

	public void LoadQuestionData(string[] _questions, string[] _answers){
		questions = _questions;
		answers = _answers;
	}

	public void FlashAnswerOn(){
		answerPrompt.text = (string)qAndA [currentQuestion];
		answerPrompt.gameObject.SetActive (true);
		Invoke ("FlashAnswerOff", 0.1f);
	}

	public void FlashAnswerOff(){
		answerPrompt.gameObject.SetActive (false);
	}
}
