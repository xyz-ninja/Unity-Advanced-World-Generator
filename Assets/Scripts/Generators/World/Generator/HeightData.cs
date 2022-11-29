using System;
using UnityEngine;

[Serializable]
public class HeightData {
        
	public string Name = "Height Data";

	public bool IsFluid = false;
	
	[SerializeField] private int _index = 0;
	[SerializeField] private float _heightValue;
	[SerializeField] private Color _color = Color.white;

	#region getters

	public int Index => _index;
	public float HeightValue => _heightValue;
	public Color Color => _color;

	#endregion
}