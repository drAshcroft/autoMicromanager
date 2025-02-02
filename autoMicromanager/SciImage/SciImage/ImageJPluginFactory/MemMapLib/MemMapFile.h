/* DO NOT EDIT THIS FILE - it is machine generated */
#include <jni.h>
/* Header for class com_stanley_memmap_MemMapFile */

#ifndef _Included_com_stanley_memmap_MemMapFile
#define _Included_com_stanley_memmap_MemMapFile
#ifdef __cplusplus
extern "C" {
#endif
#undef com_stanley_memmap_MemMapFile_PAGE_READONLY
#define com_stanley_memmap_MemMapFile_PAGE_READONLY 2L
#undef com_stanley_memmap_MemMapFile_PAGE_READWRITE
#define com_stanley_memmap_MemMapFile_PAGE_READWRITE 4L
#undef com_stanley_memmap_MemMapFile_PAGE_WRITECOPY
#define com_stanley_memmap_MemMapFile_PAGE_WRITECOPY 8L
#undef com_stanley_memmap_MemMapFile_FILE_MAP_COPY
#define com_stanley_memmap_MemMapFile_FILE_MAP_COPY 1L
#undef com_stanley_memmap_MemMapFile_FILE_MAP_WRITE
#define com_stanley_memmap_MemMapFile_FILE_MAP_WRITE 2L
#undef com_stanley_memmap_MemMapFile_FILE_MAP_READ
#define com_stanley_memmap_MemMapFile_FILE_MAP_READ 4L
/*
 * Class:     com_stanley_memmap_MemMapFile
 * Method:    createFileMapping
 * Signature: (IIILjava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_MemMapFile_createFileMapping
  (JNIEnv *, jclass, jint, jint, jint, jstring);

/*
 * Class:     com_stanley_memmap_MemMapFile
 * Method:    openFileMapping
 * Signature: (IZLjava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_MemMapFile_openFileMapping
  (JNIEnv *, jclass, jint, jboolean, jstring);

/*
 * Class:     com_stanley_memmap_MemMapFile
 * Method:    mapViewOfFile
 * Signature: (IIIII)I
 */
JNIEXPORT jint JNICALL Java_MemMapFile_mapViewOfFile
  (JNIEnv *, jclass, jint, jint, jint, jint, jint);

/*
 * Class:     com_stanley_memmap_MemMapFile
 * Method:    unmapViewOfFile
 * Signature: (I)Z
 */
JNIEXPORT jboolean JNICALL Java_MemMapFile_unmapViewOfFile
  (JNIEnv *, jclass, jint);

/*
 * Class:     com_stanley_memmap_MemMapFile
 * Method:    writeToMem
 * Signature: (ILjava/lang/String;)V
 */
JNIEXPORT void JNICALL Java_MemMapFile_writeToMem
  (JNIEnv *, jclass, jint, jstring);

/*
 * Class:     com_stanley_memmap_MemMapFile
 * Method:    readFromMem
 * Signature: (I)Ljava/lang/String;
 */
JNIEXPORT jstring JNICALL Java_MemMapFile_readFromMem
  (JNIEnv *, jclass, jint);

/*
 * Class:     com_stanley_memmap_MemMapFile
 * Method:    closeHandle
 * Signature: (I)Z
 */
JNIEXPORT jboolean JNICALL Java_MemMapFile_closeHandle
  (JNIEnv *, jclass, jint);

/*
 * Class:     com_stanley_memmap_MemMapFile
 * Method:    broadcast
 * Signature: ()V
 */
JNIEXPORT void JNICALL Java_MemMapFile_broadcast
  (JNIEnv *, jclass);

/*
 * Class:     com_stanley_memmap_MemMapFile
 * Method:    broadcast
 * Signature: ()V
 */
JNIEXPORT jobject JNICALL Java_MemMapFile_ReadImage
	(JNIEnv * pEnv, jclass, jint lpBaseAddress);

/*
 * Class:     com_stanley_memmap_MemMapFile
 * Method:    broadcast
 * Signature: ()V
 */
JNIEXPORT jint JNICALL Java_MemMapFile_EasyWriteImageByte
   (JNIEnv *, jclass, jstring, jbyteArray , jint , jint , jint , jint , jint );
JNIEXPORT jint JNICALL Java_MemMapFile_EasyWriteImageShort
   (JNIEnv *, jclass, jstring, jshortArray , jint , jint , jint , jint , jint );
JNIEXPORT jint JNICALL Java_MemMapFile_EasyWriteImageInt
   (JNIEnv *, jclass, jstring, jintArray , jint , jint , jint , jint , jint );

JNIEXPORT void JNICALL Java_MemMapFile_EasyWriteImage2(JNIEnv *,jclass);

JNIEXPORT void JNICALL Java_MemMapFile_WriteImage
    (JNIEnv *env,jclass, jobject cl, jint lpBaseAddress, jbyteArray Image, jint BPP , jint numChannels, jint Width, jint Height);

JNIEXPORT jobject JNICALL Java_MemMapFile_ReadImageInfo
(JNIEnv * pEnv, jclass, jint lpBaseAddress);
#ifdef __cplusplus
}
#endif
#endif
