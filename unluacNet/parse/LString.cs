namespace unluac.parse
{


	public class LString : LObject
	{

	  public readonly BSizeT size;
	  public readonly string @value;

	  public LString(BSizeT size, string @value)
	  {
		this.size = size;
		this.value = @value.Length == 0 ? "" : @value.Substring(0, @value.Length - 1);
	  }

	  public override string deref()
	  {
		return @value;
	  }

	  public override string ToString()
	  {
		return "\"" + @value + "\"";
	  }

	  public override bool Equals(object o)
	  {
		if(o is LString)
		{
		  LString os = (LString) o;
		  return os.value.Equals(@value);
		}
		return false;
	  }

	}

}