# Copyright (c) 2012, Google Inc.
# All rights reserved.
#
# Redistribution and use in source and binary forms, with or without
# modification, are permitted provided that the following conditions are
# met:
#
#     * Redistributions of source code must retain the above copyright
# notice, this list of conditions and the following disclaimer.
#     * Redistributions in binary form must reproduce the above
# copyright notice, this list of conditions and the following disclaimer
# in the documentation and/or other materials provided with the
# distribution.
#     * Neither the name of Google Inc. nor the names of its
# contributors may be used to endorse or promote products derived from
# this software without specific prior written permission.
#
# THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
# "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
# LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
# A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
# OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
# SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
# LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
# DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
# THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
# (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
# OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

LOCAL_PATH := $(call my-dir)

include $(CLEAR_VARS)
LOCAL_MODULE := breakpad
LOCAL_ARM_MODE := arm
LOCAL_SRC_FILES := \
         google_breakpad/src/client/linux/microdump_writer/microdump_writer.cc \
         google_breakpad/src/client/linux/log/log.cc \
         google_breakpad/src/client/linux/crash_generation/crash_generation_client.cc \
         google_breakpad/src/client/linux/handler/exception_handler.cc \
         google_breakpad/src/client/linux/handler/minidump_descriptor.cc \
         google_breakpad/src/client/linux/minidump_writer/linux_dumper.cc \
         google_breakpad/src/client/linux/minidump_writer/linux_ptrace_dumper.cc \
         google_breakpad/src/client/minidump_file_writer.cc \
         google_breakpad/src/common/android/breakpad_getcontext.S \
         google_breakpad/src/common/convert_UTF.c \
         google_breakpad/src/common/md5.cc \
         google_breakpad/src/common/linux/elfutils.cc \
         google_breakpad/src/common/linux/file_id.cc \
         google_breakpad/src/common/linux/guid_creator.cc \
         google_breakpad/src/common/linux/linux_libc_support.cc \
         google_breakpad/src/common/linux/memory_mapped_file.cc \
         google_breakpad/src/client/linux/dump_writer_common/seccomp_unwinder.cc \
         google_breakpad/src/client/linux/dump_writer_common/thread_info.cc \
         google_breakpad/src/client/linux/dump_writer_common/ucontext_reader.cc \
         google_breakpad/src/client/linux/minidump_writer/linux_core_dumper.cc \
         google_breakpad/src/client/linux/minidump_writer/minidump_writer.cc \
         google_breakpad/src/common/linux/elf_core_dump.cc \
         google_breakpad/src/common/linux/safe_readlink.cc \
         google_breakpad/src/common/string_conversion.cc \

LOCAL_SRC_FILES += \
    tandroid_breakpad.cpp


LOCAL_ALLOW_UNDEFINED_SYMBOLS := true
LOCAL_LDLIBS := -llog
LOCAL_C_INCLUDES        := $(LOCAL_PATH)/google_breakpad/src/common/android/include \
                           $(LOCAL_PATH)/google_breakpad/src

LOCAL_EXPORT_C_INCLUDES := $(LOCAL_C_INCLUDES)
include $(BUILD_SHARED_LIBRARY)

# If NDK_MODULE_PATH is defined, import the module, otherwise do a direct
# includes. This allows us to build in all scenarios easily.

include $(LOCAL_PATH)/google_breakpad/Android.mk

