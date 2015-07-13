namespace unluac.decompile
{

	using UpvalueExpression = unluac.decompile.expression.UpvalueExpression;
	using LUpvalue = unluac.parse.LUpvalue;

	public class Upvalues
	{

	  private readonly LUpvalue[] upvalues;

	  public Upvalues(LUpvalue[] upvalues)
	  {
		this.upvalues = upvalues;
	  }

	  public virtual string getName(int index)
	  {
		if(index < upvalues.Length && upvalues[index].name != null)
		{
		  return upvalues[index].name;
		}
		else
		{
	  //TODO: SET ERROR
		  return "_UPVALUE" + index + "_";
		}
	  }

	  public virtual UpvalueExpression getExpression(int index)
	  {
		return new UpvalueExpression(getName(index));
	  }

	}

}