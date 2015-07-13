using System;

namespace unluac.decompile.expression
{

	using Output = unluac.decompile.Output;

	public class BinaryExpression : Expression
	{

	  private readonly string op;
	  private readonly Expression left;
	  private readonly Expression right;
	  private readonly int associativity;

	  public BinaryExpression(string op, Expression left, Expression right, int precedence, int associativity) : base(precedence)
	  {
		this.op = op;
		this.left = left;
		this.right = right;
		this.associativity = associativity;
	  }

	  public override int getConstantIndex()
	  {
		return Math.Max(left.getConstantIndex(), right.getConstantIndex());
	  }

	  public override bool beginsWithParen()
	  {
		return leftGroup() || left.beginsWithParen();
	  }

	  public override void print(Output @out)
	  {
		bool leftGroup = this.leftGroup();
		bool rightGroup = this.rightGroup();
		if(leftGroup)
			@out.print("(");
		left.print(@out);
		if(leftGroup)
			@out.print(")");
		@out.print(" ");
		@out.print(op);
		@out.print(" ");
		if(rightGroup)
			@out.print("(");
		right.print(@out);
		if(rightGroup)
			@out.print(")");
	  }

	  private bool leftGroup()
	  {
		return precedence > left.precedence || (precedence == left.precedence && associativity == ASSOCIATIVITY_RIGHT);
	  }

	  private bool rightGroup()
	  {
		return precedence > right.precedence || (precedence == right.precedence && associativity == ASSOCIATIVITY_LEFT);
	  }

	}

}