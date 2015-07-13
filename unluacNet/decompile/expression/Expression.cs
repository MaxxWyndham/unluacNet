using System;
using System.Collections.Generic;

namespace unluac.decompile.expression
{


	using Constant = unluac.decompile.Constant;
	using Output = unluac.decompile.Output;
	using Target = unluac.decompile.target.Target;
	using LNil = unluac.parse.LNil;

	abstract public class Expression
	{

	  public const int PRECEDENCE_OR = 1;
	  public const int PRECEDENCE_AND = 2;
	  public const int PRECEDENCE_COMPARE = 3;
	  public const int PRECEDENCE_CONCAT = 4;
	  public const int PRECEDENCE_ADD = 5;
	  public const int PRECEDENCE_MUL = 6;
	  public const int PRECEDENCE_UNARY = 7;
	  public const int PRECEDENCE_POW = 8;
	  public const int PRECEDENCE_ATOMIC = 9;

	  public const int ASSOCIATIVITY_NONE = 0;
	  public const int ASSOCIATIVITY_LEFT = 1;
	  public const int ASSOCIATIVITY_RIGHT = 2;

	  public static readonly Expression NIL = new ConstantExpression(new Constant(LNil.NIL), -1);

	  public static BinaryExpression makeCONCAT(Expression left, Expression right)
	  {
		return new BinaryExpression("..", left, right, PRECEDENCE_CONCAT, ASSOCIATIVITY_RIGHT);
	  }

	  public static BinaryExpression makeADD(Expression left, Expression right)
	  {
		return new BinaryExpression("+", left, right, PRECEDENCE_ADD, ASSOCIATIVITY_LEFT);
	  }

	  public static BinaryExpression makeSUB(Expression left, Expression right)
	  {
		return new BinaryExpression("-", left, right, PRECEDENCE_ADD, ASSOCIATIVITY_LEFT);
	  }

	  public static BinaryExpression makeMUL(Expression left, Expression right)
	  {
		return new BinaryExpression("*", left, right, PRECEDENCE_MUL, ASSOCIATIVITY_LEFT);
	  }

	  public static BinaryExpression makeDIV(Expression left, Expression right)
	  {
		return new BinaryExpression("/", left, right, PRECEDENCE_MUL, ASSOCIATIVITY_LEFT);
	  }

	  public static BinaryExpression makeMOD(Expression left, Expression right)
	  {
		return new BinaryExpression("%", left, right, PRECEDENCE_MUL, ASSOCIATIVITY_LEFT);
	  }

	  public static UnaryExpression makeUNM(Expression expression)
	  {
		return new UnaryExpression("-", expression, PRECEDENCE_UNARY);
	  }

	  public static UnaryExpression makeNOT(Expression expression)
	  {
		return new UnaryExpression("not ", expression, PRECEDENCE_UNARY);
	  }

	  public static UnaryExpression makeLEN(Expression expression)
	  {
		return new UnaryExpression("#", expression, PRECEDENCE_UNARY);
	  }

	  public static BinaryExpression makePOW(Expression left, Expression right)
	  {
		return new BinaryExpression("^", left, right, PRECEDENCE_POW, ASSOCIATIVITY_RIGHT);
	  }

//  *
//   * Prints out a sequences of expressions with commas, and optionally
//   * handling multiple expressions and return value adjustment.
//   
	  public static void printSequence(Output @out, List<Expression> exprs, bool linebreak, bool multiple)
	  {
		int n = exprs.Count;
		int i = 1;
		foreach(Expression expr in exprs)
		{
		  bool last = (i == n);
		  if(expr.isMultiple())
		  {
			last = true;
		  }
		  if(last)
		  {
			if(multiple)
			{
			  expr.printMultiple(@out);
			}
			else
			{
			  expr.print(@out);
			}
			break;
			}
		  else
		  {
			expr.print(@out);
			@out.print(",");
			if(linebreak)
			{
			  @out.println();
			}
			else
			{
			  @out.print(" ");
			}
		  }
		  i++;
		}
	  }

	  public readonly int precedence;

	  public Expression(int precedence)
	  {
		this.precedence = precedence;
	  }

	  protected internal static void printUnary(Output @out, string op, Expression expression)
	  {
		@out.print(op);
		expression.print(@out);
	  }

	  protected internal static void printBinary(Output @out, string op, Expression left, Expression right)
	  {
		left.print(@out);
		@out.print(" ");
		@out.print(op);
		@out.print(" ");
		right.print(@out);
	  }

	  abstract public void print(Output @out);

//  *
//   * Prints the expression in a context that accepts multiple values.
//   * (Thus, if an expression that normally could return multiple values
//   * doesn't, it should use parens to adjust to 1.)
//   
	  public virtual void printMultiple(Output @out)
	  {
		print(@out);
	  }

//  *
//   * Determines the index of the last-declared constant in this expression.
//   * If there is no constant in the expression, return -1.
//   
	  abstract public int getConstantIndex();

	  public virtual bool beginsWithParen()
	  {
		return false;
	  }

	  public virtual bool isNil()
	  {
		return false;
	  }

	  public virtual bool isClosure()
	  {
		return false;
	  }

	  public virtual bool isConstant()
	  {
		return false;
	  }

  // Only supported for closures
	  public virtual bool isUpvalueOf(int register)
	  {
          throw new Exception();
	  }

	  public virtual bool isBoolean()
	  {
		return false;
	  }

	  public virtual bool isInteger()
	  {
		return false;
	  }

	  public virtual int asInteger()
	  {
          throw new Exception();
	  }

	  public virtual bool isString()
	  {
		return false;
	  }

	  public virtual bool isIdentifier()
	  {
		return false;
	  }

//  *
//   * Determines if this can be part of a function name.
//   * Is it of the form: {Name . } Name
//   
	  public virtual bool isDotChain()
	  {
		return false;
	  }

	  public virtual int closureUpvalueLine()
	  {
          throw new Exception();
	  }

	  public virtual void printClosure(Output @out, Target name)
	  {
          throw new Exception();
	  }

	  public virtual string asName()
	  {
          throw new Exception();
	  }

	  public virtual bool isTableLiteral()
	  {
		return false;
	  }

	  public virtual void addEntry(TableLiteral.Entry entry)
	  {
          throw new Exception();
	  }

//  *
//   * Whether the expression has more than one return stored into registers.
//   
	  public virtual bool isMultiple()
	  {
		return false;
	  }

	  public virtual bool isMemberAccess()
	  {
		return false;
	  }

	  public virtual Expression getTable()
	  {
          throw new Exception();
	  }

	  public virtual string getField()
	  {
          throw new Exception();
	  }

	  public virtual bool isBrief()
	  {
		return false;
	  }

	}

}