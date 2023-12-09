using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueText
{
	public int actorID;
	public int actorImageID;
	[TextArea(3, 20)]
	public string text;
	public DialogueOption[] options;
	public bool triggerInteraction;
	public int interactionID;
}

[System.Serializable]
public class Actor
{
	public string name;
	public Sprite[] images;
}

[System.Serializable]
public class DialogueOption
{
	public string text;
	public string conditionName;
	public int conditionValue;
	public DialogueText[] nextDialogue;
	public DialogueText[] alternativeDialogue;
	public bool triggerInteraction;
	public int interactionID;
}