// dll implementation file MemMapLib.cpp
//
////////////////////////////////////////////////////////////////////////////////////

#include <windows.h>
#include <tchar.h>
#include <jni.h>

#include "MemMapFile.h"

#define UWM_DATA_READY_MSG _T("UWM_DATA_READY_MSG-{7FDB2CB4-5510-4d30-99A9-CD7752E0D680}")

UINT UWM_DATA_READY;

BOOL APIENTRY DllMain(HINSTANCE hinstDll, DWORD dwReasion, LPVOID lpReserved) {

	if(dwReasion == DLL_PROCESS_ATTACH) 
		UWM_DATA_READY = RegisterWindowMessage(UWM_DATA_READY_MSG);

	return TRUE; 
}

void ErrorHandler(LPCTSTR pszErrorMessage) {
	MessageBox(NULL, pszErrorMessage, _T("Error"), MB_OK | MB_ICONERROR);
}

JNIEXPORT jint JNICALL Java_MemMapFile_createFileMapping
	(JNIEnv * pEnv, jclass, jint lProtect, jint dwMaximumSizeHigh, 
	jint dwMaximumSizeLow, jstring name) {
	
	HANDLE hFile = INVALID_HANDLE_VALUE;
	HANDLE hMapFile = NULL;

	LPCSTR lpName = pEnv->GetStringUTFChars(name, NULL);

	__try {
		hMapFile = CreateFileMapping(hFile, NULL, lProtect, dwMaximumSizeHigh, 
									 dwMaximumSizeLow, lpName);

		if(hMapFile == NULL) {
			ErrorHandler(_T("Can not create file mapping object"));
			__leave;
		}

		if(GetLastError() == ERROR_ALREADY_EXISTS) {
			ErrorHandler(_T("File mapping object already exists"));
			CloseHandle(hMapFile);		
			__leave;
		}
	}
	__finally {
	}

	pEnv->ReleaseStringUTFChars(name, lpName);

	// if hMapFile is NULL, just return NULL, or return the handle
	return reinterpret_cast<jint>(hMapFile);
}

JNIEXPORT jint JNICALL Java_MemMapFile_openFileMapping
	(JNIEnv * pEnv, jclass, jint dwDesiredAccess, 
	jboolean bInheritHandle, jstring name) {
	
	HANDLE hMapFile = NULL;

	LPCSTR lpName = pEnv->GetStringUTFChars(name, NULL);
	hMapFile = OpenFileMapping(dwDesiredAccess, bInheritHandle, lpName);
	if(hMapFile == NULL) ErrorHandler(_T("Can not open file mapping object"));
	pEnv->ReleaseStringUTFChars(name, lpName);

	return reinterpret_cast<jint>(hMapFile);
}

JNIEXPORT jint JNICALL Java_MemMapFile_mapViewOfFile
	(JNIEnv *, jclass, jint hMapFile, jint dwDesiredAccess, 
	jint dwFileOffsetHigh, jint dwFileOffsetLow, jint dwNumberOfBytesToMap) {
	
	PVOID pView = NULL;
	pView = MapViewOfFile(reinterpret_cast<HANDLE>(hMapFile), dwDesiredAccess, 
						  dwFileOffsetHigh, dwFileOffsetLow, dwNumberOfBytesToMap);
	if(pView == NULL) ErrorHandler(_T("Can not map view of file"));

	return reinterpret_cast<jint>(pView);
}

JNIEXPORT jboolean JNICALL Java_MemMapFile_unmapViewOfFile
	(JNIEnv *, jclass, jint lpBaseAddress) {

	BOOL bRet = UnmapViewOfFile(reinterpret_cast<PVOID>(lpBaseAddress));
	if(!bRet) ErrorHandler(_T("Can not unmap view of file"));

	return bRet;
}

JNIEXPORT jboolean JNICALL Java_MemMapFile_closeHandle
	(JNIEnv *, jclass, jint hObject) {

	return CloseHandle(reinterpret_cast<HANDLE>(hObject));
}

JNIEXPORT void JNICALL Java_MemMapFile_writeToMem
	(JNIEnv * pEnv, jclass, jint lpBaseAddress, jstring content) {

	
	LPCSTR pszContent = pEnv->GetStringUTFChars(content, NULL);
	PVOID pView = reinterpret_cast<PVOID>(lpBaseAddress);
	LPSTR pszCopy = reinterpret_cast<LPSTR>(pView);
	lstrcpy(pszCopy, pszContent);
	pEnv->ReleaseStringUTFChars(content, pszContent);
}

JNIEXPORT jstring JNICALL Java_MemMapFile_readFromMem
	(JNIEnv * pEnv, jclass, jint lpBaseAddress) {

	PVOID pView = reinterpret_cast<PVOID>(lpBaseAddress);
	LPSTR pszContent = reinterpret_cast<LPSTR>(pView);
	return pEnv->NewStringUTF(pszContent);
}

JNIEXPORT void JNICALL Java_MemMapFile_broadcast
	(JNIEnv *, jclass) {
	
	//SendMessage(HWND_BROADCAST, UWM_DATA_READY, 0, 0);
}

JNIEXPORT jobject JNICALL Java_MemMapFile_ReadImageInfo
(JNIEnv * pEnv, jclass, jint lpBaseAddress){

   PVOID pView = reinterpret_cast<PVOID>(lpBaseAddress);
   unsigned char * result=(unsigned char*)pView;

   int lBPP=(int)(*result);
   result++;
   int numChannels=(int)(*result);  
   result++;
   long *pwidth=(long*)result;
   long width=*pwidth;
   result+=4;
   long *pHeight=(long*)result;
   long height=(*pHeight);
   result+=4;

   jlong lSize=4;
   jlong * info = new jlong[(unsigned int)lSize];
   info[0]=(jlong)lBPP;
   info[1]=(jlong)numChannels;
   info[2]=(jlong)width;
   info[3]=(jlong)height;


   jobject jresult = 0 ;
   JNIEnv * jenv=pEnv;

      // create a new short[] object in Java
      jlongArray data = jenv->NewLongArray((jsize)4);
      if (data == 0)
      {
		 
        jclass excep = jenv->FindClass("java/lang/Exception");
        if (excep)
        jenv->ThrowNew(excep, "The system ran out of memory!");
        
        jresult = 0;
        return jresult;
      }
      
      // copy pixels from the image buffer
      jenv->SetLongArrayRegion(data, 0,(jsize) 4, info);
      
      jresult = data;
    return jresult;
}//Java_MemMapFile_
JNIEXPORT jobject JNICALL Java_MemMapFile_ReadImage(JNIEnv * pEnv, jclass, jint lpBaseAddress)
{
	
	//, jint lBPP, jlong lSize, jint numChannels) {

   PVOID pView = reinterpret_cast<PVOID>(lpBaseAddress);
   jbyte * result=(jbyte*)pView;

  int lBPP=(int)(*result);
   result++;
   int numChannels=(int)(*result);  
   result++;
   long *pwidth=(long*)result;
   long width=*pwidth;
   result+=4;
   long *pHeight=(long*)result;
   long height=(*pHeight);
   result+=4;
   int * s=(int *)result;
   long * ll=(long *)result;
   

   jlong lSize=(jlong)(*ll);
   s=0;
   result+=4;

   jobject jresult = 0 ;
   JNIEnv * jenv=pEnv;

    if (lBPP == 1)
    {
      // create a new byte[] object in Java
      jbyteArray data = jenv->NewByteArray((jsize)lSize);
      if (data == 0)
      {
		 
        jclass excep = jenv->FindClass("java/lang/Exception");
        if (excep)
        jenv->ThrowNew(excep, "The system ran out of memory!");
        
        jresult = 0;
        return jresult;
      }
      
      // copy pixels from the image buffer
      jenv->SetByteArrayRegion(data, 0,(jsize) lSize, (jbyte*)result);
      
      jresult = data;
    }
    else if (lBPP == 2)
    {
      // create a new short[] object in Java
      jshortArray data = jenv->NewShortArray((jsize)lSize);
      if (data == 0)
      {
        jclass excep = jenv->FindClass("java/lang/Exception");
        if (excep)
        jenv->ThrowNew(excep, "The system ran out of memory!");
        jresult = 0;
        return jresult;
      }
	  jenv->SetShortArrayRegion(data, 0,(jsize) lSize, (jshort*)result);
      
	  jresult=data; 
    }
	else if (lBPP=4)
	{
        jintArray data = jenv->NewIntArray((jsize)lSize);
        if (data == 0)
        {
			jclass excep = jenv->FindClass("java/lang/Exception");
			if (excep)
			jenv->ThrowNew(excep, "The system ran out of memory!");
			jresult = 0;
			return jresult;

		}
		// copy pixels from the image buffer
        jenv->SetIntArrayRegion(data, 0,(jsize) lSize, (jint*)result);
        jresult = data;
	}
    else
    {
      // don't know how to map
      // TODO: thow exception?
      jresult = 0;
    }
	return jresult;
  }



 

JNIEXPORT jint JNICALL Java_MemMapFile_EasyWriteImageByte(JNIEnv *env,jclass jj, jstring name, jbyteArray Image, jint lSize, jint BPP , jint numChannels, jint Width, jint Height)
{
	
	HANDLE m_hFileMMF = 0, m_pViewMMFFile = 0;

	LPCSTR lpName = env->GetStringUTFChars(name, NULL);
    //Creation and Mapping of Memory Mapped File
	m_hFileMMF = CreateFileMapping(INVALID_HANDLE_VALUE,NULL,PAGE_READWRITE,0,(DWORD)(lSize+200), lpName);              
    DWORD dwError = GetLastError();
    if ( ! m_hFileMMF )
    {
		jclass excep = env->FindClass("java/lang/Exception");
			if (excep)
			   env->ThrowNew(excep, "Creation of file mapping failed");
        
    }
    else
    {
        m_pViewMMFFile = MapViewOfFile(m_hFileMMF,FILE_MAP_ALL_ACCESS,0,0,0);                         // map all file

        if(! m_pViewMMFFile )
        {
			jclass excep =  env->FindClass("java/lang/Exception");
			if (excep)
			   env->ThrowNew(excep, "MapViewOfFile function failed" );
			
           // MessageBox(0, "MapViewOfFile function failed" ,0,0);
        }
		else 
		{
				int nRows ;
			
				nRows= env->GetArrayLength( Image);
				unsigned char  * buffer;
				buffer = (unsigned char*)(env)->GetByteArrayElements(Image, false);

				PVOID pView = reinterpret_cast<PVOID>(m_pViewMMFFile);//(lpBaseAddress);
			    
				long * head=(long*)pView;
				head[0]=(long)BPP;
				head[1]=(long)numChannels;
				head[2]=(long)Width;
				head[3]=(long)Height;
				head[4]=(long)nRows;
				head+=5;
				byte * pszCopy = (byte*)(head);
				memcpy(pszCopy, buffer,(size_t)nRows);

				env->ReleaseByteArrayElements(Image,(jbyte*)  buffer,0) ;


		}
    }
	
	   
    return (jint)m_hFileMMF;
	//return m_pViewMMFFile!=NULL;

}

JNIEXPORT jint JNICALL Java_MemMapFile_EasyWriteImageShort(JNIEnv *env,jclass jj, jstring name, jshortArray Image, jint lSize, jint BPP , jint numChannels, jint Width, jint Height)
{
	
	HANDLE m_hFileMMF = 0, m_pViewMMFFile = 0;
    int nRows ;
			
	nRows= env->GetArrayLength( Image);
	nRows=nRows*2;

	LPCSTR lpName = env->GetStringUTFChars(name, NULL);
    //Creation and Mapping of Memory Mapped File
	m_hFileMMF = CreateFileMapping(INVALID_HANDLE_VALUE,NULL,PAGE_READWRITE,0,(DWORD)(nRows+200), lpName);              
    DWORD dwError = GetLastError();
    if ( ! m_hFileMMF )
    {
		jclass excep = env->FindClass("java/lang/Exception");
			if (excep)
			   env->ThrowNew(excep, "Creation of file mapping failed");
        
    }
    else
    {
        m_pViewMMFFile = MapViewOfFile(m_hFileMMF,FILE_MAP_ALL_ACCESS,0,0,0);                         // map all file

        if(! m_pViewMMFFile )
        {
			jclass excep =  env->FindClass("java/lang/Exception");
			if (excep)
			   env->ThrowNew(excep, "MapViewOfFile function failed" );
			
           // MessageBox(0, "MapViewOfFile function failed" ,0,0);
        }
		else 
		{
				
				unsigned char  * buffer;
				buffer = (unsigned char*)(env)->GetShortArrayElements(Image, false);

				PVOID pView = reinterpret_cast<PVOID>(m_pViewMMFFile);//(lpBaseAddress);
			    
				long * head=(long*)pView;
				head[0]=(long)BPP;
				head[1]=(long)numChannels;
				head[2]=(long)Width;
				head[3]=(long)Height;
				head[4]=(long)nRows;
				head+=5;
				byte * pszCopy = (byte*)(head);
				memcpy(pszCopy, buffer,(size_t)nRows);

				env->ReleaseShortArrayElements(Image,(jshort*)  buffer,0) ;


		}
    }
	
	   
    return (jint)m_hFileMMF;
	//return m_pViewMMFFile!=NULL;

}

JNIEXPORT jint JNICALL Java_MemMapFile_EasyWriteImageInt(JNIEnv *env,jclass jj, jstring name, jintArray Image, jint lSize, jint BPP , jint numChannels, jint Width, jint Height)
{
	int nRows ;
			
	nRows= env->GetArrayLength( Image);
	nRows=nRows*4;

	HANDLE m_hFileMMF = 0, m_pViewMMFFile = 0;

	LPCSTR lpName = env->GetStringUTFChars(name, NULL);
    //Creation and Mapping of Memory Mapped File
	m_hFileMMF = CreateFileMapping(INVALID_HANDLE_VALUE,NULL,PAGE_READWRITE,0,(DWORD)(nRows+200), lpName);              
    DWORD dwError = GetLastError();
    if ( ! m_hFileMMF )
    {
		jclass excep = env->FindClass("java/lang/Exception");
			if (excep)
			   env->ThrowNew(excep, "Creation of file mapping failed");
        
    }
    else
    {
        m_pViewMMFFile = MapViewOfFile(m_hFileMMF,FILE_MAP_ALL_ACCESS,0,0,0);                         // map all file

        if(! m_pViewMMFFile )
        {
			jclass excep =  env->FindClass("java/lang/Exception");
			if (excep)
			   env->ThrowNew(excep, "MapViewOfFile function failed" );
			
           // MessageBox(0, "MapViewOfFile function failed" ,0,0);
        }
		else 
		{
				
				unsigned char  * buffer;
				buffer = (unsigned char*)(env)->GetIntArrayElements(Image, false);

				PVOID pView = reinterpret_cast<PVOID>(m_pViewMMFFile);//(lpBaseAddress);
			    
				long * head=(long*)pView;
				head[0]=(long)BPP;
				head[1]=(long)numChannels;
				head[2]=(long)Width;
				head[3]=(long)Height;
				head[4]=(long)nRows;
				head+=5;
				byte * pszCopy = (byte*)(head);
				memcpy(pszCopy, buffer,(size_t)nRows);

				env->ReleaseIntArrayElements(Image,(jint*)  buffer,0) ;


		}
    }
	
	   
    return (jint)m_hFileMMF;
	//return m_pViewMMFFile!=NULL;

}

JNIEXPORT void JNICALL Java_MemMapFile_WriteImage
    (JNIEnv *env,jclass, jobject cl, jint lpBaseAddress, jbyteArray Image, jint BPP , jint numChannels, jint Width, jint Height)
{

		int nRows ;
	
		nRows= env->GetArrayLength( Image);
		unsigned char  * buffer;
	    buffer = (unsigned char*)(env)->GetByteArrayElements(Image, false);

	    PVOID pView = reinterpret_cast<PVOID>(lpBaseAddress);
	    
		long * head=(long*)pView;
        head[0]=(long)BPP;
		head[1]=(long)numChannels;
		head[2]=(long)Width;
		head[3]=(long)Height;
		head[4]=(long)nRows;
	    head+=5;
		byte * pszCopy = (byte*)(head);
	    memcpy(pszCopy, buffer,(size_t)nRows);

	    env->ReleaseByteArrayElements(Image,(jbyte*)  buffer,0) ;


}

//  return( jresult);

