namespace unluac.decompile.expression
{

	using Output = unluac.decompile.Output;

	public class Vararg : Expression
	{

	  private readonly int length;
	  private readonly bool multiple;

	  public Vararg(int length, bool multiple) : base(PRECEDENCE_ATOMIC)
	  {
		this.length = length;
		this.multiple = multiple;
	  }

	  public override int getConstantIndex()
	  {
		return -1;
	  }

	  public override void print(Output @out)
	  {
	//out.print("...");
		@out.print(multiple ? "..." : "(...)");
	  }

	  public override void printMultiple(Output @out)
	  {
		@out.print(multiple ? "..." : "(...)");
	  }

	  public override bool isMultiple()
	  {
		return multiple;
	  }

	}

}