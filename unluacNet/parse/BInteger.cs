using System;
using System.Numerics;
namespace unluac.parse
{


	public class BInteger : BObject
	{

	  private readonly BigInteger big;
	  private readonly int n;

	  private static BigInteger MAX_INT = 0;
	  private static BigInteger MIN_INT = 0;

	  public BInteger(BInteger b)
	  {
		this.big = b.big;
		this.n = b.n;
	  }

	  public BInteger(int n)
	  {
		this.big = 0;
		this.n = n;
	  }

	  public BInteger(BigInteger big)
	  {
		this.big = big;
		this.n = 0;
		if(MAX_INT == null)
		{
            
		  MAX_INT = int.MaxValue;
		  MIN_INT = int.MinValue;
		}
	  }

	  public virtual int asInt()
	  {
		if(big == 0 && n > 0)
		{
		  return n;
		}
		else if(big.CompareTo(MAX_INT) > 0 || big.CompareTo(MIN_INT) < 0)
		{
		  throw new Exception("The size of an integer is outside the range that unluac can handle.");
		}
		else
		{
		  return (int)big;
		}
	  }

	  public virtual void iterate(Action thunk)
	  {
		if(big == 0 && n > 0)
		{
		  int i = n;
		  while(i-- != 0)
		  {
			thunk();
		  }
		  }
		else
		{
		  BigInteger i = big;
		  while(i.Sign > 0)
		  {
			thunk();
			i = i - BigInteger.One;
		  }
		}
	  }

	}

}