using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
	[Header("Input Settings")]
	[SerializeField] string select = "e";
	[SerializeField] string nextOption = "s";
	[SerializeField] string previousOption = "w";
	[SerializeField] string quit = "q";

	[Header("UI Components")]
	[SerializeField] GameObject dialoguePanel;
	[SerializeField] TMP_Text characterName;
	[SerializeField] Image characterImage;
	[SerializeField] TMP_Text dialogueText;
	[SerializeField] TMP_Text[] optionTexts;
	[SerializeField] Button[] optionButtons;
	[Header("Dialogue Options")]
	[SerializeField] float loadingSpeed;
	[Header("Other")]
	[SerializeField] PlayerMovement playerMovement;

	DialogueText[] currentDialogue;
	Actor[] currentActors;
	string displayedText = "";
	int dialogueTextID = 0;
	DialogueText message;
	Actor actor;
	float loadedLetters;
	string currentText = "";
	int selectedOptionID = 0;
	DialogueOption selectedOption;
	InteractionManager interactionManager;
	bool dialogueIsActive = false;
	bool inputsAreActive = false;
	bool buttonsAreDisplayed = false;

	public Dictionary<string, int> condidtions = new Dictionary<string, int>();

	private void Start()
	{
		condidtions.Add("hasCheese", 0);

		interactionManager = FindAnyObjectByType<InteractionManager>();
	}

	public void StartDialogue(DialogueText[] dialogue, Actor[] actors)
	{
		if (dialogueIsActive) return;
		playerMovement.freezePlayer = true;
		currentDialogue = dialogue;
		currentActors = actors;
		dialoguePanel.SetActive(true);
		dialogueTextID = 0;
		dialogueIsActive = true;
		ClearPanel();
	}

	private void ClearPanel()
	{
		message = currentDialogue[dialogueTextID];
		actor = currentActors[message.actorID];
		characterImage.sprite = actor.images[message.actorImageID];
		characterName.text = actor.name;
		currentText = currentDialogue[dialogueTextID].text;
		selectedOptionID = 0;
		loadedLetters = 0f;
		displayedText = "";
		buttonsAreDisplayed = false;
		optionButtons[0].gameObject.SetActive(false);
		optionButtons[1].gameObject.SetActive(false);
		optionButtons[2].gameObject.SetActive(false);
	}

	private void LoadDialogueText()
	{
		if (loadedLetters < currentText.Length)
		{
			loadedLetters += Time.unscaledDeltaTime * loadingSpeed;
			if (displayedText.Length < loadedLetters)
			{
				if (displayedText.Length == currentText.Length) return;
				displayedText += currentText[displayedText.Length];
			}
			dialogueText.text = displayedText;
		}
	}

	private void DisplayOptions()
	{
		if (message.options.Length == 0) return;
		if (displayedText.Length == currentText.Length)
		{
			int i = 0;
			foreach (DialogueOption option in message.options)
			{
				optionButtons[i].gameObject.SetActive(true);
				optionTexts[i].text = option.text;
				i++;
			}
			optionButtons[selectedOptionID].Select();
			buttonsAreDisplayed = true;
		}
	}

	private void LoadNextMessage()
	{
		dialogueTextID++;
		ClearPanel();
	}

	private void HandleInputs()
	{
		if (Input.GetKeyDown(select))
		{
			if (!inputsAreActive)
			{
				inputsAreActive = true;
				return;
			}
			if (buttonsAreDisplayed)
			{
				SelectOption(selectedOptionID);
				return;
			}
			if (displayedText.Length < currentText.Length)
			{
				loadedLetters = currentText.Length;
				displayedText = currentText;
				dialogueText.text = displayedText;
				return;
			}
			if (message.triggerInteraction)
			{
				TriggerInteraction(message.interactionGroup, message.interactionID, message.interactionState);
			}
			if (dialogueTextID == currentDialogue.Length - 1)
			{
				EndDialogue();
			}
			else if (message.options.Length == 0)
			{
				LoadNextMessage();
			}
		}
		if (Input.GetKeyDown(previousOption))
		{
			ChangeOption(-1);
		}
		if (Input.GetKeyDown(nextOption))
		{
			ChangeOption(1);
		}
		if (Input.GetKeyDown(quit))
		{
			EndDialogue();
		}
	}

	private void ChangeOption(int value)
	{
		if (message.options.Length == 0) return;
		selectedOptionID += value;
		if (selectedOptionID < 0) selectedOptionID = message.options.Length - 1;
		selectedOptionID %= message.options.Length;
	}

	public void SelectOption(int optionID)
	{
		selectedOption = message.options[optionID];
		dialogueIsActive = false;
		if (selectedOption.conditionName != "")
		{
			if (condidtions[selectedOption.conditionName] == selectedOption.conditionValue)
			{
				StartDialogue(selectedOption.nextDialogue, currentActors);
			}
			else
			{
				StartDialogue(selectedOption.alternativeDialogue, currentActors);
			}
		}
		else if (selectedOption.triggerInteraction)
		{
			TriggerInteraction(selectedOption.interactionGroup, selectedOption.interactionID, selectedOption.interactionState);
		}
		else
		{
			if (selectedOption.nextDialogue.Length <= 0)
			{
				EndDialogue();
			}
			else
			{
				StartDialogue(selectedOption.nextDialogue, currentActors);
			}
		}
	}

	private void TriggerInteraction(int group, int id, int state)
	{
		interactionManager.UseInteractionSwitch(group, id, state);
	}

	private void EndDialogue()
	{
		playerMovement.freezePlayer = false;
		dialogueIsActive = false;
		inputsAreActive = false;
		dialoguePanel.SetActive(false);
	}

	private void Update()
	{
		if (!dialogueIsActive) return;
		HandleInputs();
		LoadDialogueText();
		DisplayOptions();
	}

	public void ChangeCondition(string name, int value)
	{
		condidtions[name] = value;
	}

}
