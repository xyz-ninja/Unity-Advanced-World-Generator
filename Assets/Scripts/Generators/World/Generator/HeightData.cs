using System;
using UnityEngine;

[Serializable]
public class HeightData {
        
	public string Name = "Height Data";

	[SerializeField] private float _heightValue;
	[SerializeField] private Color _color = Color.white;

	#region getters

	public float HeightValue => _heightValue;
	public Color Color => _color;

	#endregion
}