namespace unluac.decompile.branch
{

	using Registers = unluac.decompile.Registers;
	using BinaryExpression = unluac.decompile.expression.BinaryExpression;
	using Expression = unluac.decompile.expression.Expression;
	using UnaryExpression = unluac.decompile.expression.UnaryExpression;

	public class LTNode : Branch
	{

	  private readonly int left;
	  private readonly int right;
	  private readonly bool _invert;

	  public LTNode(int left, int right, bool _invert, int line, int begin, int end) : base(line, begin, end)
	  {
		this.left = left;
		this.right = right;
		this._invert = _invert;
	  }

	  public override Branch invert()
	  {
		return new LTNode(left, right, !_invert, line, end, begin);
	  }

	  public override int getRegister()
	  {
		return -1;
	  }

	  public override Expression asExpression(Registers r)
	  {
		bool transpose = false;
		Expression leftExpression = r.getKExpression(left, line);
		Expression rightExpression = r.getKExpression(right, line);
		if(((left | right) & 256) == 0)
		{
		  transpose = r.getUpdated(left, line) > r.getUpdated(right, line);
		}
		else
		{
		  transpose = rightExpression.getConstantIndex() < leftExpression.getConstantIndex();
		}
		string op = !transpose ? "<" : ">";
		Expression rtn = new BinaryExpression(op, !transpose ? leftExpression : rightExpression, !transpose ? rightExpression : leftExpression, Expression.PRECEDENCE_COMPARE, Expression.ASSOCIATIVITY_LEFT);
		if(_invert)
		{
		  rtn = new UnaryExpression("not ", rtn, Expression.PRECEDENCE_UNARY);
		}
		return rtn;
	  }

	  public override void useExpression(Expression expression)
	  {
	// Do nothing 
	  }

	}
}