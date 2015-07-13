namespace unluac.decompile.expression
{

	using Constant = unluac.decompile.Constant;
	using Output = unluac.decompile.Output;

	public class ConstantExpression : Expression
	{

	  private readonly Constant constant;
	  private readonly int index;

	  public ConstantExpression(Constant constant, int index) : base(PRECEDENCE_ATOMIC)
	  {
		this.constant = constant;
		this.index = index;
	  }

	  public override int getConstantIndex()
	  {
		return index;
	  }

	  public override void print(Output @out)
	  {
		constant.print(@out);
	  }

	  public override bool isConstant()
	  {
		return true;
	  }

	  public override bool isNil()
	  {
		return constant.isNil();
	  }

	  public override bool isBoolean()
	  {
		return constant.isBoolean();
	  }

	  public override bool isInteger()
	  {
		return constant.isInteger();
	  }

	  public override int asInteger()
	  {
		return constant.asInteger();
	  }

	  public override bool isString()
	  {
		return constant.isString();
	  }

	  public override bool isIdentifier()
	  {
		return constant.isIdentifier();
	  }

	  public override string asName()
	  {
		return constant.asName();
	  }

	  public override bool isBrief()
	  {
		return !constant.isString() || constant.asName().Length <= 10;
	  }

	}

}