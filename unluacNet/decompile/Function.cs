namespace unluac.decompile
{

	using ConstantExpression = unluac.decompile.expression.ConstantExpression;
	using GlobalExpression = unluac.decompile.expression.GlobalExpression;
	using LFunction = unluac.parse.LFunction;

	public class ULFunction
	{

	  private Constant[] constants;

	  public ULFunction(LFunction function)
	  {
		constants = new Constant[function.constants.Length];
		for(int i = 0; i < constants.Length; i++)
		{
		  constants[i] = new Constant(function.constants[i]);
		}
	  }

	  public virtual string getGlobalName(int constantIndex)
	  {
		return constants[constantIndex].asName();
	  }

	  public virtual ConstantExpression getConstantExpression(int constantIndex)
	  {
		return new ConstantExpression(constants[constantIndex], constantIndex);
	  }

	  public virtual GlobalExpression getGlobalExpression(int constantIndex)
	  {
		return new GlobalExpression(getGlobalName(constantIndex), constantIndex);
	  }

	}

}