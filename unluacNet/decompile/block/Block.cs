using System;
using System.Collections;
using System.Collections.Generic;
using unluac.decompile.operation;
using unluac.decompile.statement;

namespace unluac.decompile.block
{

	using Decompiler = unluac.decompile.Decompiler;
	using Registers = unluac.decompile.Registers;
	using Statement = unluac.decompile.statement.Statement;
	using LFunction = unluac.parse.LFunction;

	abstract public class Block : Statement, IComparable<Block>
	{

	  protected internal readonly LFunction function;
	  public int begin;
	  public int end;
	  public bool loopRedirectAdjustment = false;

	  public Block(LFunction function, int begin, int end)
	  {
		this.function = function;
		this.begin = begin;
		this.end = end;
	  }

	  abstract public void addStatement(Statement statement);

	  public virtual bool contains(Block block)
	  {
		return begin <= block.begin && end >= block.end;
	  }

	  public virtual bool contains(int line)
	  {
		return begin <= line && line < end;
	  }

	  public virtual int scopeEnd()
	  {
		return end - 1;
	  }

//  *
//   * An unprotected block is one that ends in a JMP instruction.
//   * If this is the case, any inner statement that tries to jump
//   * to the end of this block will be redirected.
//   * 
//   * (One of the lua compiler's few optimizations is that is changes
//   * any JMP that targets another JMP to the ultimate target. This
//   * is what I call redirection.)
//   
	  abstract public bool isUnprotected();

	  abstract public int getLoopback();

	  abstract public bool breakable();

	  abstract public bool isContainer();

	  public virtual int CompareTo(Block block)
	  {
		if(this.begin < block.begin)
		{
		  return -1;
		}
		else if(this.begin == block.begin)
		{
		  if(this.end < block.end)
		  {
			return 1;
		  }
		  else if(this.end == block.end)
		  {
			if(this.isContainer() && !block.isContainer())
			{
			  return -1;
			}
			else if(!this.isContainer() && block.isContainer())
			{
			  return 1;
			}
			else
			{
			  return 0;
			}
			}
		  else
		  {
			return -1;
		  }
		  }
		else
		{
		  return 1;
		}
	  }

	  public virtual Operation process(Decompiler d)
	  {
		Statement statement = this;
        return new BlockOperation(end - 1, statement);
	  }

	}

    public class BlockOperation : Operation
    {
        private Statement _statement;
        public BlockOperation(int line, Statement statement) : base(line)
        {
            _statement = statement;
        }
        public override Statement process(unluac.decompile.Registers r, unluac.decompile.block.Block block)
        {
 	        return _statement;
        }
    }

}