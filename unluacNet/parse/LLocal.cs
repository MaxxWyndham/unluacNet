namespace unluac.parse
{


	public class LLocal : BObject
	{

	  public readonly LString name;
	  public readonly int Start;
	  public readonly int end;

  // Used by the decompiler for annotation. 
	  public bool forLoop = false;

	  public LLocal(LString name, BInteger start, BInteger end)
	  {
		this.name = name;
		this.Start = start.asInt();
		this.end = end.asInt();
	  }

	  public override string ToString()
	  {
		return name.deref();
	  }

	}

}