using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
	public List<InteractiveObject> possibleInteractions = new List<InteractiveObject>();
	InteractiveObject currentInteraction;
	int ignoreValue = 0;

	InteractiveObject FindTheNearestInteraction()
	{
		Transform interactionTransform;
		InteractiveObject theNearestInteraction = possibleInteractions[0];
		float minDistance = 1000000;
		float distance = 0;

		foreach (InteractiveObject interaction in possibleInteractions)
		{
			interactionTransform = interaction.transform;
			distance = Vector3.Distance(transform.position, interactionTransform.position);
			if (distance < minDistance)
			{
				minDistance = distance;
				theNearestInteraction = interaction;
			}
		}
		return theNearestInteraction;
	}

	public void SetIngoreValue(int value)
	{
		ignoreValue = value;
	}

	void Update()
    {
		if (possibleInteractions.Count == 0) return;

        currentInteraction = FindTheNearestInteraction();
		if (Input.GetKeyDown("e"))
		{
			if (ignoreValue > 0)
			{
				ignoreValue--;
				return;
			}
			currentInteraction.TriggerInteraction();
		}
    }
}
