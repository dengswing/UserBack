#include <jni.h>
#include <android/log.h>
#include <stdio.h>

#include "google_breakpad/src/client/linux/handler/exception_handler.h"
#include "google_breakpad/src/client/linux/handler/minidump_descriptor.h"

JNIEnv * jEnv = 0;
static google_breakpad::ExceptionHandler* exceptionHandler;

bool DumpCallback(const google_breakpad::MinidumpDescriptor& descriptor,
                  void* context,
                  bool succeeded) {

    __android_log_print(ANDROID_LOG_DEBUG, "FranticJni", "Dump path: %s\n", descriptor.path());

    jclass cls = jEnv->FindClass("com/pluu/jni/NativeController");


    if(cls == NULL) {
        __android_log_print(ANDROID_LOG_DEBUG, "FranticJni", "err");
    return false;
    }

    jmethodID mhthod = jEnv->GetStaticMethodID(cls, "NativeCrashCallback", "(Ljava/lang/String;)I");
    if(mhthod == NULL) {
        __android_log_print(ANDROID_LOG_DEBUG, "FranticJni", "err22");
        return false;
    }

    const char* path = descriptor.path();

    jstring jstr = jEnv->NewStringUTF(path);
    if (jstr == NULL) {
        __android_log_print(ANDROID_LOG_DEBUG, "FranticJni", "err33");
        return false;
    }

    jint i = jEnv->CallStaticIntMethod(cls, mhthod, jstr);
    __android_log_print(ANDROID_LOG_DEBUG, "FranticJni", "err44");
    return succeeded;
}

void Crash() {
    volatile int* a = reinterpret_cast<volatile int*>(NULL);
    *a = 1;
}

extern "C" {
    void Java_com_shinezone_crashreport_JniActivity_initNative(JNIEnv* env, jobject obj, jstring filepath)
    {
        jEnv = env;

        const char *path = env->GetStringUTFChars(filepath, 0);
        google_breakpad::MinidumpDescriptor descriptor(path);
        exceptionHandler = new google_breakpad::ExceptionHandler(descriptor, NULL, DumpCallback, NULL, true, -1);

        __android_log_print(ANDROID_LOG_DEBUG, "FranticJni", "initNative cal");
    }

    void Java_com_shinezone_crashreport_JniActivity_crashService(JNIEnv* env, jobject obj)
    {
        __android_log_print(ANDROID_LOG_DEBUG, "FranticJni", "crashService call");

        Crash();
    }
}
