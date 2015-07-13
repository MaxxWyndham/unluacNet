namespace unluac.decompile.branch
{

	using Registers = unluac.decompile.Registers;
	using BinaryExpression = unluac.decompile.expression.BinaryExpression;
	using Expression = unluac.decompile.expression.Expression;

	public class EQNode : Branch
	{

	  private readonly int left;
	  private readonly int right;
	  private readonly bool _invert;

	  public EQNode(int left, int right, bool _invert, int line, int begin, int end) : base(line, begin, end)
	  {
		this.left = left;
		this.right = right;
		this._invert = _invert;
	  }

	  public override Branch invert()
	  {
		return new EQNode(left, right, !_invert, line, end, begin);
	  }

	  public override int getRegister()
	  {
		return -1;
	  }

	  public override Expression asExpression(Registers r)
	  {
		bool transpose = false;
		string op = _invert ? "~=" : "==";
		return new BinaryExpression(op, r.getKExpression(!transpose ? left : right, line), r.getKExpression(!transpose ? right : left, line), Expression.PRECEDENCE_COMPARE, Expression.ASSOCIATIVITY_LEFT);
	  }

	  public override void useExpression(Expression expression)
	  {
	// Do nothing 
	  }

	}

}