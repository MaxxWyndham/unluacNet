namespace unluac.parse
{

	public class LNil : LObject
	{

	  public static readonly LNil NIL = new LNil();

	  private LNil()
	  {

	  }

	  public override bool Equals(object o)
	  {
		return this == o;
	  }

	}

}