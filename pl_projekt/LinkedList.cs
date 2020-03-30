/* If you're reading this, I just wanted to say I appreciated the fact */
/* that I was given the chance to develop this app under Linux instead */
/* of having to install Windows as well as Visual Studio just to       */
/* uninstall it again                                                  */

using System;
using System.Collections;
using System.Collections.Generic;

namespace pl_projekt
{
  public class LinkedList<Type> : IEnumerable<Type> where Type : IComparable
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

    void Push(Type value)
    {
      var node = new Node(null, begin, value);
      if (begin == null)
      {
        begin = node;
        end = begin;
      }
      else
      {
        node.Next = begin;
        begin.Prev = node;
        begin = node;
      }
      ++Size;
    }
    
    void Append(Type value)
    {
      var node = new Node(end, null, value);
      if (end == null)
      {
        begin = node;
        end = begin;
      }
      else
      {
        node.Prev = end;
        end.Next = node;
        end = node;
      }
      ++Size;
    }

    public void Insert(Type value)
    {

      if (begin == null || begin.Value.CompareTo(value) > 0)
      {
        Push(value);
        return;
      }

      if (end.Value.CompareTo(value) < 0)
      {
        Append(value);
        return;
      }
      
      var val = begin;
      while (val.Value.CompareTo(value) < 0)
      {
        val = val.Next;
      }

      var node = new Node(val.Prev, val, value);
      val.Prev.Next = node;
      val.Prev = node;
    }

    public Type this[int key]
    {
      get => Get(key);
      set => Set(key, value);
    }

    public bool Empty
    {
      get => Size == 0;
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













