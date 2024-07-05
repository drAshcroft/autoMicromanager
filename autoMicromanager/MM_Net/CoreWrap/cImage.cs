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

public class cImage : IDisposable {
  private HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal cImage(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new HandleRef(this, cPtr);
  }

  internal static HandleRef getCPtr(cImage obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  ~cImage() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          MMCoreCSPINVOKE.delete_cImage(swigCPtr);
        }
        swigCPtr = new HandleRef(null, IntPtr.Zero);
      }
      GC.SuppressFinalize(this);
    }
  }

  public int lSize {
    set {
      MMCoreCSPINVOKE.cImage_lSize_set(swigCPtr, value);
    } 
    get {
      int ret = MMCoreCSPINVOKE.cImage_lSize_get(swigCPtr);
      return ret;
    } 
  }

  public IntPtr Data {
    get {
      IntPtr cPtr = MMCoreCSPINVOKE.cImage_Data_get(swigCPtr);
      return cPtr;
    } 
  }

  public bool RGB32 {
    set {
      MMCoreCSPINVOKE.cImage_RGB32_set(swigCPtr, value);
    } 
    get {
      bool ret = MMCoreCSPINVOKE.cImage_RGB32_get(swigCPtr);
      return ret;
    } 
  }

  public int NumComponents {
    set {
      MMCoreCSPINVOKE.cImage_NumComponents_set(swigCPtr, value);
    } 
    get {
      int ret = MMCoreCSPINVOKE.cImage_NumComponents_get(swigCPtr);
      return ret;
    } 
  }

  public int Width {
    set {
      MMCoreCSPINVOKE.cImage_Width_set(swigCPtr, value);
    } 
    get {
      int ret = MMCoreCSPINVOKE.cImage_Width_get(swigCPtr);
      return ret;
    } 
  }

  public int Height {
    set {
      MMCoreCSPINVOKE.cImage_Height_set(swigCPtr, value);
    } 
    get {
      int ret = MMCoreCSPINVOKE.cImage_Height_get(swigCPtr);
      return ret;
    } 
  }

  public int BitsPerPixel {
    set {
      MMCoreCSPINVOKE.cImage_BitsPerPixel_set(swigCPtr, value);
    } 
    get {
      int ret = MMCoreCSPINVOKE.cImage_BitsPerPixel_get(swigCPtr);
      return ret;
    } 
  }

  public int Stride {
    set {
      MMCoreCSPINVOKE.cImage_Stride_set(swigCPtr, value);
    } 
    get {
      int ret = MMCoreCSPINVOKE.cImage_Stride_get(swigCPtr);
      return ret;
    } 
  }

  public cImage() : this(MMCoreCSPINVOKE.new_cImage(), true) {
  }

}

}