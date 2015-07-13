namespace unluac.parse
{

	public class LBoolean : LObject
	{

	  public static readonly LBoolean LTRUE = new LBoolean(true);
	  public static readonly LBoolean LFALSE = new LBoolean(false);

	  private readonly bool @value;

	  private LBoolean(bool @value)
	  {
		this.value = @value;
	  }

	  public override string ToString()
	  {
		return @value.ToString();
	  }

	  public override bool Equals(object o)
	  {
		return this == o;
	  }

	}

}