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

public class MetadataTag : IDisposable {
  private HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal MetadataTag(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new HandleRef(this, cPtr);
  }

  internal static HandleRef getCPtr(MetadataTag obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  ~MetadataTag() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          MMCoreCSPINVOKE.delete_MetadataTag(swigCPtr);
        }
        swigCPtr = new HandleRef(null, IntPtr.Zero);
      }
      GC.SuppressFinalize(this);
    }
  }

  public string GetDevice() {
    string ret = MMCoreCSPINVOKE.MetadataTag_GetDevice(swigCPtr);
    return ret;
  }

  public string GetName() {
    string ret = MMCoreCSPINVOKE.MetadataTag_GetName(swigCPtr);
    return ret;
  }

  public bool IsReadOnly() {
    bool ret = MMCoreCSPINVOKE.MetadataTag_IsReadOnly(swigCPtr);
    return ret;
  }

  public void SetDevice(string device) {
    MMCoreCSPINVOKE.MetadataTag_SetDevice(swigCPtr, device);
  }

  public void SetName(string name) {
    MMCoreCSPINVOKE.MetadataTag_SetName(swigCPtr, name);
  }

  public void SetReadOnly(bool ro) {
    MMCoreCSPINVOKE.MetadataTag_SetReadOnly(swigCPtr, ro);
  }

  public virtual MetadataSingleTag ToSingleTag() {
    IntPtr cPtr = MMCoreCSPINVOKE.MetadataTag_ToSingleTag__SWIG_0(swigCPtr);
    MetadataSingleTag ret = (cPtr == IntPtr.Zero) ? null : new MetadataSingleTag(cPtr, false);
    return ret;
  }

  public virtual MetadataArrayTag ToArrayTag() {
    IntPtr cPtr = MMCoreCSPINVOKE.MetadataTag_ToArrayTag__SWIG_0(swigCPtr);
    MetadataArrayTag ret = (cPtr == IntPtr.Zero) ? null : new MetadataArrayTag(cPtr, false);
    return ret;
  }

  public virtual MetadataTag Clone() {
    IntPtr cPtr = MMCoreCSPINVOKE.MetadataTag_Clone(swigCPtr);
    MetadataTag ret = (cPtr == IntPtr.Zero) ? null : new MetadataTag(cPtr, false);
    return ret;
  }

  public virtual string Serialize() {
    string ret = MMCoreCSPINVOKE.MetadataTag_Serialize(swigCPtr);
    return ret;
  }

  public virtual bool Restore(string stream) {
    bool ret = MMCoreCSPINVOKE.MetadataTag_Restore(swigCPtr, stream);
    return ret;
  }

}

}
