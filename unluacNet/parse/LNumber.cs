using System;

namespace unluac.parse
{

	public abstract class LNumber : LObject
	{

	  public static LNumber makeInteger(int number)
	  {
		return new LIntNumber(number);
	  }

	  public override abstract string ToString();

  //TODO: problem solution for this issue
	  public abstract double @value();
	}

	internal class LFloatNumber : LNumber
	{

	  public readonly float number;

	  public LFloatNumber(float number)
	  {
		this.number = number;
	  }

	  public override string ToString()
	  {
		if(number == (float) Math.Round(number))
		{
		  return Convert.ToString((int) number);
		}
		else
		{
		  return Convert.ToString(number);
		}
	  }

	  public override bool Equals(object o)
	  {
		if(o is LFloatNumber)
		{
		  return number == ((LFloatNumber) o).number;
		}
		else if(o is LNumber)
		{
		  return @value() == ((LNumber) o).value();
		}
		return false;
	  }

	  public override double @value()
	  {
		return number;
	  }

	}

	internal class LDoubleNumber : LNumber
	{

	  public readonly double number;

	  public LDoubleNumber(double number)
	  {
		this.number = number;
	  }

	  public override string ToString()
	  {
		if(number == (double) Math.Round(number))
		{
		  return Convert.ToString((long) number);
		}
		else
		{
            return Convert.ToString(number);
		}
	  }

	  public override bool Equals(object o)
	  {
		if(o is LDoubleNumber)
		{
		  return number == ((LDoubleNumber) o).number;
		}
		else if(o is LNumber)
		{
		  return @value() == ((LNumber) o).value();
		}
		return false;
	  }

	  public override double @value()
	  {
		return number;
	  }

	}

	internal class LIntNumber : LNumber
	{

	  public readonly int number;

	  public LIntNumber(int number)
	  {
		this.number = number;
	  }

	  public override string ToString()
	  {
		return Convert.ToString(number);
	  }

	  public override bool Equals(object o)
	  {
		if(o is LIntNumber)
		{
		  return number == ((LIntNumber) o).number;
		}
		else if(o is LNumber)
		{
		  return @value() == ((LNumber) o).value();
		}
		return false;
	  }

	  public override double @value()
	  {
		return number;
	  }

	}

	internal class LLongNumber : LNumber
	{

	  public readonly long number;

	  public LLongNumber(long number)
	  {
		this.number = number;
	  }

	  public override string ToString()
	  {
		return Convert.ToString(number);
	  }

	  public override bool Equals(object o)
	  {
		if(o is LLongNumber)
		{
		  return number == ((LLongNumber) o).number;
		}
		else if(o is LNumber)
		{
		  return @value() == ((LNumber) o).value();
		}
		return false;
	  }

	  public override double @value()
	  {
		return number;
	  }

	}
}