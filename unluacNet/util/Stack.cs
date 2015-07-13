using System.Collections;
using System.Collections.Generic;

namespace unluac.util
{


	public class Stack<T>
	{

	  private readonly List<T> data;

        public int Count
      {
          get { return data.Count; }
      }

	  public Stack()
	  {
		data = new List<T>();
	  }

	  public virtual bool isEmpty()
	  {
		return data.Count == 0;
	  }

	  public virtual T peek()
	  {
		return data[data.Count - 1];
	  }

	  public virtual T pop()
	  {
          T toReturn = data[data.Count - 1];
          data.RemoveAt(data.Count-1);
		return toReturn;
	  }

	  public virtual void push(T item)
	  {
		data.Add(item);
	  }

	  public virtual int size()
	  {
		return data.Count;
	  }

	  public virtual void reverse()
	  {
		data.Reverse();
	  }

	}

}