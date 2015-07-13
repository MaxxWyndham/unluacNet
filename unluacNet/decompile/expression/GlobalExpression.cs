namespace unluac.decompile.expression
{

	using Output = unluac.decompile.Output;

	public class GlobalExpression : Expression
	{

	  private readonly string name;
	  private readonly int index;

	  public GlobalExpression(string name, int index) : base(PRECEDENCE_ATOMIC)
	  {
		this.name = name;
		this.index = index;
	  }

	  public override int getConstantIndex()
	  {
		return index;
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