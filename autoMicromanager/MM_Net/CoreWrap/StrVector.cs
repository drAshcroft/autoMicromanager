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

public class StrVector : IDisposable, System.Collections.IEnumerable
#if !SWIG_DOTNET_1
    , System.Collections.Generic.IList<string>
#endif
 {
  private HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal StrVector(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new HandleRef(this, cPtr);
  }

  internal static HandleRef getCPtr(StrVector obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  ~StrVector() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          MMCoreCSPINVOKE.delete_StrVector(swigCPtr);
        }
        swigCPtr = new HandleRef(null, IntPtr.Zero);
      }
      GC.SuppressFinalize(this);
    }
  }

  public StrVector(System.Collections.ICollection c) : this() {
    if (c == null)
      throw new ArgumentNullException("c");
    foreach (string element in c) {
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

  public string this[int index]  {
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
  public void CopyTo(string[] array)
#endif
  {
    CopyTo(0, array, 0, this.Count);
  }

#if SWIG_DOTNET_1
  public void CopyTo(System.Array array, int arrayIndex)
#else
  public void CopyTo(string[] array, int arrayIndex)
#endif
  {
    CopyTo(0, array, arrayIndex, this.Count);
  }

#if SWIG_DOTNET_1
  public void CopyTo(int index, System.Array array, int arrayIndex, int count)
#else
  public void CopyTo(int index, string[] array, int arrayIndex, int count)
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
  System.Collections.Generic.IEnumerator<string> System.Collections.Generic.IEnumerable<string>.GetEnumerator() {
    return new StrVectorEnumerator(this);
  }
#endif

  System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
    return new StrVectorEnumerator(this);
  }

  public StrVectorEnumerator GetEnumerator() {
    return new StrVectorEnumerator(this);
  }

  // Type-safe enumerator
  /// Note that the IEnumerator documentation requires an InvalidOperationException to be thrown
  /// whenever the collection is modified. This has been done for changes in the size of the
  /// collection but not when one of the elements of the collection is modified as it is a bit
  /// tricky to detect unmanaged code that modifies the collection under our feet.
  public sealed class StrVectorEnumerator : System.Collections.IEnumerator
#if !SWIG_DOTNET_1
    , System.Collections.Generic.IEnumerator<string>
#endif
  {
    private StrVector collectionRef;
    private int currentIndex;
    private object currentObject;
    private int currentSize;

    public StrVectorEnumerator(StrVector collection) {
      collectionRef = collection;
      currentIndex = -1;
      currentObject = null;
      currentSize = collectionRef.Count;
    }

    // Type-safe iterator Current
    public string Current {
      get {
        if (currentIndex == -1)
          throw new InvalidOperationException("Enumeration not started.");
        if (currentIndex > currentSize - 1)
          throw new InvalidOperationException("Enumeration finished.");
        if (currentObject == null)
          throw new InvalidOperationException("Collection modified.");
        return (string)currentObject;
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
    MMCoreCSPINVOKE.StrVector_Clear(swigCPtr);
  }

  public void Add(string x) {
    MMCoreCSPINVOKE.StrVector_Add(swigCPtr, x);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
  }

  private uint size() {
    uint ret = MMCoreCSPINVOKE.StrVector_size(swigCPtr);
    return ret;
  }

  private uint capacity() {
    uint ret = MMCoreCSPINVOKE.StrVector_capacity(swigCPtr);
    return ret;
  }

  private void reserve(uint n) {
    MMCoreCSPINVOKE.StrVector_reserve(swigCPtr, n);
  }

  public StrVector() : this(MMCoreCSPINVOKE.new_StrVector__SWIG_0(), true) {
  }

  public StrVector(StrVector other) : this(MMCoreCSPINVOKE.new_StrVector__SWIG_1(StrVector.getCPtr(other)), true) {
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
  }

  public StrVector(int capacity) : this(MMCoreCSPINVOKE.new_StrVector__SWIG_2(capacity), true) {
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
  }

  private string getitemcopy(int index) {
    string ret = MMCoreCSPINVOKE.StrVector_getitemcopy(swigCPtr, index);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  private string getitem(int index) {
    string ret = MMCoreCSPINVOKE.StrVector_getitem(swigCPtr, index);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  private void setitem(int index, string val) {
    MMCoreCSPINVOKE.StrVector_setitem(swigCPtr, index, val);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
  }

  public void AddRange(StrVector values) {
    MMCoreCSPINVOKE.StrVector_AddRange(swigCPtr, StrVector.getCPtr(values));
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
  }

  public StrVector GetRange(int index, int count) {
    IntPtr cPtr = MMCoreCSPINVOKE.StrVector_GetRange(swigCPtr, index, count);
    StrVector ret = (cPtr == IntPtr.Zero) ? null : new StrVector(cPtr, true);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void Insert(int index, string x) {
    MMCoreCSPINVOKE.StrVector_Insert(swigCPtr, index, x);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
  }

  public void InsertRange(int index, StrVector values) {
    MMCoreCSPINVOKE.StrVector_InsertRange(swigCPtr, index, StrVector.getCPtr(values));
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
  }

  public void RemoveAt(int index) {
    MMCoreCSPINVOKE.StrVector_RemoveAt(swigCPtr, index);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
  }

  public void RemoveRange(int index, int count) {
    MMCoreCSPINVOKE.StrVector_RemoveRange(swigCPtr, index, count);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
  }

  public static StrVector Repeat(string value, int count) {
    IntPtr cPtr = MMCoreCSPINVOKE.StrVector_Repeat(value, count);
    StrVector ret = (cPtr == IntPtr.Zero) ? null : new StrVector(cPtr, true);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void Reverse() {
    MMCoreCSPINVOKE.StrVector_Reverse__SWIG_0(swigCPtr);
  }

  public void Reverse(int index, int count) {
    MMCoreCSPINVOKE.StrVector_Reverse__SWIG_1(swigCPtr, index, count);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
  }

  public void SetRange(int index, StrVector values) {
    MMCoreCSPINVOKE.StrVector_SetRange(swigCPtr, index, StrVector.getCPtr(values));
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
  }

  public bool Contains(string value) {
    bool ret = MMCoreCSPINVOKE.StrVector_Contains(swigCPtr, value);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public int IndexOf(string value) {
    int ret = MMCoreCSPINVOKE.StrVector_IndexOf(swigCPtr, value);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public int LastIndexOf(string value) {
    int ret = MMCoreCSPINVOKE.StrVector_LastIndexOf(swigCPtr, value);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public bool Remove(string value) {
    bool ret = MMCoreCSPINVOKE.StrVector_Remove(swigCPtr, value);
    if (MMCoreCSPINVOKE.SWIGPendingException.Pending) throw MMCoreCSPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

}

}
