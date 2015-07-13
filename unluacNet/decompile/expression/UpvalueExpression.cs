namespace unluac.decompile.expression
{

	using Output = unluac.decompile.Output;

	public class UpvalueExpression : Expression
	{

	  private readonly string name;

	  public UpvalueExpression(string name) : base(PRECEDENCE_ATOMIC)
	  {
		this.name = name;
	  }

	  public override int getConstantIndex()
	  {
		return -1;
	  }

	  public override bool isDotChain()
	  {
		return true;
	  }

	  public override void print(Output @out)
	  {
		@out.print(name);
	  }

	  public override bool isBrief()
	  {
		return true;
	  }

	}

}