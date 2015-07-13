namespace unluac.decompile.expression
{

	using Declaration = unluac.decompile.Declaration;
	using Output = unluac.decompile.Output;

	public class LocalVariable : Expression
	{

	  private readonly Declaration decl;

	  public LocalVariable(Declaration decl) : base(PRECEDENCE_ATOMIC)
	  {
		this.decl = decl;
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
		@out.print(decl.name);
	  }

	  public override bool isBrief()
	  {
		return true;
	  }

	}

}