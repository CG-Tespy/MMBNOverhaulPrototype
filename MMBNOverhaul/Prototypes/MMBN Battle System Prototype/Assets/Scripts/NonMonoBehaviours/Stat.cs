using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class Stat : System.IEquatable<Stat>, System.IComparable<Stat>
{
	#region Fields
	public string name, description;
	public float minVal, maxVal;
	

	#region Backing Fields
	[SerializeField] float _val = 0;

	[Header("For debugging")]
	[SerializeField] float _effectiveVal = 0;

	#endregion

	public float effectiveMinVal, effectiveMaxVal;
	

	#endregion

	#region Properties
	public float val
	{
		get { return _val; }
		set { _val = Mathf.Clamp(value, minVal, maxVal); }
	}

	public float effectiveVal 
	{
		get { return _effectiveVal; }
		set { _effectiveVal = Mathf.Clamp(value, effectiveMinVal, effectiveMaxVal); }
	}

	#endregion

	public Stat(float val = 0, float minVal = 0, float maxVal = 0, 
				string name = "Stat", string description = "A self-managing stat")
	{
		this.val = val;
		this.minVal = minVal;
		this.maxVal = maxVal;

		this.name = name;
		this.description = description;

		this.effectiveVal = val;
		this.effectiveMinVal = minVal;
		this.effectiveMaxVal = maxVal;
	}



	public bool Equals(Stat other)
	{
		return (other.effectiveVal == this.effectiveVal);
	}

	public int CompareTo(Stat other)
	{
		if (other.effectiveVal < this.effectiveVal)
			return 1;
		else if (other.effectiveVal > this.effectiveVal)
			return -1;
		
		return 0;
	}

	public Stat Copy()
	{
		Stat newStat = new Stat(val, minVal, maxVal, name, description);
		
		newStat.effectiveVal = this.effectiveVal;
		newStat.val = this.val;

		return newStat;
	}

	#region Overloaded operators

	public static Stat operator+(Stat stat1, float num1)
	{
		stat1.val += num1;
		stat1.effectiveVal += num1;

		return stat1;
	}

	public static float operator+(float num1, Stat stat1)
	{
		num1 += stat1.effectiveVal;

		return num1;
	}

	public static int operator+(int num1, Stat stat1)
	{
		num1 += (int)stat1.effectiveVal;

		return num1;
	}

	public static Stat operator-(Stat stat1, float num1)
	{
		stat1.val -= num1;
		stat1.effectiveVal -= num1;

		return stat1;
	}

	public static float operator-(float num1, Stat stat1)
	{
		num1 -= stat1.effectiveVal;

		return num1;
	}

	public static int operator-(int num1, Stat stat1)
	{
		num1 -= (int)stat1.effectiveVal;

		return num1;
	}

	public static Stat operator*(Stat stat1, float num1)
	{
		stat1.val *= num1;
		stat1.effectiveVal *= num1;

		return stat1;
	}

	public static float operator*(float num1, Stat stat1)
	{
		num1 *= stat1.effectiveVal;

		return num1;
	}

	public static int operator*(int num1, Stat stat1)
	{
		num1 *= (int)stat1.effectiveVal;

		return num1;
	}

	public static Stat operator/(Stat stat1, float num1)
	{
		stat1.val /= num1;
		stat1.effectiveVal /= num1;

		return stat1;
	}

	public static float operator/(float num1, Stat stat1)
	{
		num1 /= stat1.effectiveVal;

		return num1;
	}

	public static int operator/(int num1, Stat stat1)
	{
		num1 /= (int)stat1.effectiveVal;

		return num1;
	}

	public static Stat operator%(Stat stat1, float num1)
	{
		stat1.val %= num1;
		stat1.effectiveVal %= num1;

		return stat1;
	}

	public static float operator%(float num1, Stat stat1)
	{
		num1 %= stat1.effectiveVal;

		return num1;
	}

	public static int operator%(int num1, Stat stat1)
	{
		num1 %= (int)stat1.effectiveVal;

		return num1;
	}


	#endregion
	
}
