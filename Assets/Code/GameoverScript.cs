using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverScript : MonoBehaviour
{
	private void Awake()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	public void BackToMenu()
	{
		FadeManager.Instance.start_fade(new int[] { 0 });
	}
}
