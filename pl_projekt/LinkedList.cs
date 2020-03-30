/* If you're reading this, I just wanted to say I appreciated the fact */
/* that I was given the chance to develop this app under Linux instead */
/* of having to install Windows as well as Visual Studio just to       */
/* uninstall it again                                                  */

using System;
using System.Collections;
using System.Collections.Generic;

namespace pl_projekt
{
  public class LinkedList<Type> : IEnumerable<Type> where Type : Serializable
  {
    private class Node
    {
      public Node? Prev;
      public Node? Next;
      public Type Value;

      public Node(Node prev, Node next, Type value)
      {
        Prev = prev;
        Next = next;
        Value = value;
      }
    }

    public int Size { get; private set; }
    private Node? begin = null;
    private Node? end = null;
    
    public void Append(Type val)
    {
      if (begin == null)
      {
        begin = new Node(null, null, val);
        end = begin;
      }
      else
      {
        var node = new Node(end, null, val);
        end.Next = node;
        end = node;
      }

      ++Size;
    }

    public void Push(Type val)
    {
      if (begin == null)
      {
        Append(val);
      }
      else
      {
        var node = new Node(null, begin, val);
        begin.Prev = node;
        begin = node;
        ++Size;
      }
    }

    private Node GetNode(int index)
    {
      if (index >= Size || index < 0)
      {
        throw new ArgumentException("Index out of bounds");
      }

      var val = begin;

      for (var i = 0; i < index; ++i)
      {
        val = val.Next;
      }

      return val;

    }

    public Type Get(int index)
    {
      return GetNode(index).Value;
    }

    public void Set(int index, Type value)
    {
      GetNode(index).Value = value;
    }


    public Type this[int key]
    {
      get => Get(key);
      set => Set(key, value);
    }


    public IEnumerator<Type> GetEnumerator()
    {
      for (Node n = begin; n != null; n = n.Next)
      {
        yield return n.Value;
      }
    }

     IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}













