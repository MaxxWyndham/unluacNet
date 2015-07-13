namespace unluac.parse
{

	public class LUpvalue : BObject
	{

	  public bool instack;
	  public int idx;

	  public string name;

	  public override bool Equals(object obj)
	  {
		if(obj is LUpvalue)
		{
		  LUpvalue upvalue = (LUpvalue) obj;
		  if(!(instack == upvalue.instack && idx == upvalue.idx))
		  {
			return false;
		  }
		  if(name == upvalue.name)
		  {
			return true;
		  }
		  return name != null && name.Equals(upvalue.name);
		  }
		else
		{
		  return false;
		}
	  }

	}

}