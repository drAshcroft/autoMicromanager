/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 1.3.40
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */

namespace CWrapper {

using System;
using System.Runtime.InteropServices;

public class IntVector : IDisposable, System.Collections.IEnumerable
#if !SWIG_DOTNET_1
    , System.Collections.Generic.IList<int>
#endif
 {
  private HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal IntVector(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new HandleRef(this, cPtr);
  }

  internal static HandleRef getCPtr(IntVector obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  ~IntVector() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          MMCoreCSPINVOKE.delete_IntVector(swigCPtr);
        }
        swigCPtr = new HandleRef(null, IntPtr.Zero);
      }
      GC.SuppressFinalize(this);
    }
  }

  public IntVector(System.Collections.ICollection c) : this() {
    if (c == null)
      throw new ArgumentNullException("c");
    foreach (int element in c) {
      this.Add(element);
    }
  }

  public bool IsFixedSize {
    get {
      return false;
    }
  }

  public bool IsReadOnly {
    get {
      return false;
    }
  }

  public int this[int index]  {
    get {
      return getitem(index);
    }
    set {
      setitem(index, value);
    }
  }

  public int Capacity {
    get {
      return (int)capacity();
    }
    set {
      if (value < size())
        throw new ArgumentOutOfRangeException("Capacity");
      reserve((uint)value);
    }
  }

  public int Count {
    get {
      return (int)size();
    }
  }

  public bool IsSynchronized {
    get {
      return false;
    }
  }

#if SWIG_DOTNET_1
  public void CopyTo(System.Array array)
#else
  public void CopyTo(int[] array)
#endif
  {
    CopyTo(0, array, 0, this.Count);
  }

#if SWIG_DOTNET_1
  public void CopyTo(System.Array array, int arrayIndex)
#else
  public void CopyTo(int[] array, int arrayIndex)
#endif
  {
    CopyTo(0, array, arrayIndex, this.Count);
  }

#if SWIG_DOTNET_1
  public void CopyTo(int index, System.Array array, int arrayIndex, int count)
#else
  public void CopyTo(int index, int[] array, int arrayIndex, int count)
#endif
  {
    if (array == null)
      throw new ArgumentNullException("array");
    if (index < 0)
      throw new ArgumentOutOfRangeException("index", "Value is less than zero");
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException("arrayIndex", "Value is less than zero");
    if (count < 0)
      throw new ArgumentOutOfRangeException("count", "Value is less than zero");
    if (array.Rank > 1)
      throw new ArgumentException("Multi dimensional array.", "array");
    if (index+count > this.Count || arrayIndex+count > array.Length)
      throw new ArgumentException("Number of elements to copy is too large.");
    for (int i=0; i<count; i++)
      array.SetValue(getitemcopy(index+i), arrayIndex+i);
  }

#if !SWIG_DOTNET_1
  System.Collections.Generic.IEnumerator<int> System.Collections.Generic.IEnumerable<int>.GetEnumerator() {
    return new IntVectorEnumerator(this);
  }
#endif

  System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
    return new IntVectorEnumerator(this);
  }

  public IntVectorEnumerator GetEnumerator() {
    return new IntVectorEnumerator(this);
  }

  // Type-safe enumerator
  /// Note that the IEnumerator documentation requires an InvalidOperationException to be thrown
  /// whenever the collection is modified. This has been done for changes in the size of the
  /// collection but not when one of the elements of the collection is modified as it is a bit
  /// tricky to detect unmanaged code that modifies the collection under our feet.
  public sealed class IntVectorEnumerator : System.Collections.IEnumerator
#if !SWIG_DOTNET_1
    , System.Collections.Generic.IEnumerator<int>
#endif
  {
    private IntVector collectionRef;
    private int currentIndex;
    private object currentObject;
    private int currentSize;

    public IntVectorEnumerator(IntVector collection) {
      collectionRef = collection;
      currentIndex = -1;
      currentObject = null;
      currentSize = collectionRef.Count;
    }

    // Type-safe iterator Current
    public int Current {
      get {
        if (currentIndex == -1)
          throw new InvalidOperationException("Enumeration not started.");
        if (currentIndex > currentSize - 1)
          throw new InvalidOperationException("Enumeration finished.");
        if (currentObject == null)
          throw new InvalidOperationException("Collection modified.");
        return (int)currentObject;
      }
    }

    // Type-unsafe IEnumerator.Current
    object System.Collections.IEnumerator.Current {
      get {
        return Current;
      }
    }

    public bool MoveNext() {
      int size = collectionRef.Count;
      bool moveOkay = (currentIndex+1 < size) && (size == currentSize);
      if (moveOkay) {
        currentIndex++;
        currentObject = collectionRef[currentIndex];
      } else {
        currentObject = null;
      }
      return moveOkay;
    }

    public void Reset() {
      currentIndex = -1;
      currentObject = null;
      if (collectionRef.Count != currentSize) {
        throw new InvalidOperationException("Collection modified.");
      }
    }

#if !SWIG_DOTNET_1
    public void Dispose() {
        currentIndex = -1;
        currentObject = null;
    }
#endif
  }

  public void Clear() {
    MMCoreCSPINVOKE.IntVector_Clear(swigCPtr);
  }

  public void Add(int x) {
    MMCoreCSPINVOKE.IntVector_Add(swigCPtr, x);
  }

  private uint size() {
    uint ret = MMCoreCSPINVOKE.IntVector_size(swigCPtr);
    return ret;
  }

  private uint capacity() {
    uint ret = MMCoreCSPINVOKE.IntVector_capacity(swigCPtr);
    return ret;
  }

  private void reserve(uint n) {
    MMCoreCSPINVOKE.IntVector_reserve(swigCPtr, n);
  }

  public IntVector() : this(MMCoreCSPINVOKE.new_IntVector__SWIG_0(), true) {
  }

  public IntVector(IntVector other) : this(MMCoreCSPINVOKE.new_IntVector__SWIG_1(IntVector.getCPtr(other)), true) {
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
  }

  public IntVector(int capacity) : this(MMCoreCSPINVOKE.new_IntVector__SWIG_2(capacity), true) {
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
  }

  private int getitemcopy(int index) {
    int ret = MMCoreCSPINVOKE.IntVector_getitemcopy(swigCPtr, index);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  private int getitem(int index) {
    int ret = MMCoreCSPINVOKE.IntVector_getitem(swigCPtr, index);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  private void setitem(int index, int val) {
    MMCoreCSPINVOKE.IntVector_setitem(swigCPtr, index, val);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
  }

  public void AddRange(IntVector values) {
    MMCoreCSPINVOKE.IntVector_AddRange(swigCPtr, IntVector.getCPtr(values));
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
  }

  public IntVector GetRange(int index, int count) {
    IntPtr cPtr = MMCoreCSPINVOKE.IntVector_GetRange(swigCPtr, index, count);
    IntVector ret = (cPtr == IntPtr.Zero) ? null : new IntVector(cPtr, true);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void Insert(int index, int x) {
    MMCoreCSPINVOKE.IntVector_Insert(swigCPtr, index, x);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
  }

  public void InsertRange(int index, IntVector values) {
    MMCoreCSPINVOKE.IntVector_InsertRange(swigCPtr, index, IntVector.getCPtr(values));
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
  }

  public void RemoveAt(int index) {
    MMCoreCSPINVOKE.IntVector_RemoveAt(swigCPtr, index);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
  }

  public void RemoveRange(int index, int count) {
    MMCoreCSPINVOKE.IntVector_RemoveRange(swigCPtr, index, count);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
  }

  public static IntVector Repeat(int value, int count) {
    IntPtr cPtr = MMCoreCSPINVOKE.IntVector_Repeat(value, count);
    IntVector ret = (cPtr == IntPtr.Zero) ? null : new IntVector(cPtr, true);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void Reverse() {
    MMCoreCSPINVOKE.IntVector_Reverse__SWIG_0(swigCPtr);
  }

  public void Reverse(int index, int count) {
    MMCoreCSPINVOKE.IntVector_Reverse__SWIG_1(swigCPtr, index, count);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
  }

  public void SetRange(int index, IntVector values) {
    MMCoreCSPINVOKE.IntVector_SetRange(swigCPtr, index, IntVector.getCPtr(values));
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
  }

  public bool Contains(int value) {
    bool ret = MMCoreCSPINVOKE.IntVector_Contains(swigCPtr, value);
    return ret;
  }

  public int IndexOf(int value) {
    int ret = MMCoreCSPINVOKE.IntVector_IndexOf(swigCPtr, value);
    return ret;
  }

  public int LastIndexOf(int value) {
    int ret = MMCoreCSPINVOKE.IntVector_LastIndexOf(swigCPtr, value);
    return ret;
  }

  public bool Remove(int value) {
    bool ret = MMCoreCSPINVOKE.IntVector_Remove(swigCPtr, value);
    return ret;
  }

}

}
