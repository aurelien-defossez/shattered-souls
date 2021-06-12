﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FractiRetinae
{
	public class ScreenLayout : MonoBehaviourSingleton<ScreenLayout>
	{
		[SerializeField] private MeshRenderer[] screens;

		private float xLeft, xRight, yTop, yBottom;

		protected override void Awake()
		{
			xLeft = screens[0].transform.localPosition.x;
			xRight = screens[1].transform.localPosition.x;
			yTop = screens[0].transform.localPosition.y;
			yBottom = screens[2].transform.localPosition.y;
		}

		public void Setup(int screenCount)
		{
			for (int i = 0; i < screens.Length; i++)
			{
				screens[i].gameObject.SetActive(i < screenCount);
			}

			// X position of 1st screen
			if (screenCount == 1)
			{
				screens[0].transform.localPosition = Vector3.zero;
			}
			else if (screenCount == 2)
			{
				screens[0].transform.localPosition = new Vector3(xLeft, 0, 0);
				screens[1].transform.localPosition = new Vector3(xRight, 0, 0);
			}
			else
			{
				screens[0].transform.localPosition = new Vector3(xLeft, yTop, 0);
				screens[1].transform.localPosition = new Vector3(xRight, yTop, 0);
				screens[2].transform.localPosition = screenCount == 3 ? new Vector3(0, yBottom, 0) : new Vector3(xLeft, yBottom, 0);
			}
		}
	}
}