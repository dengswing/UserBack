#!/bin/bash
for file in `ls *.dmp`
do
echo $file
./minidump_stackwalk $file symbols > `echo "${file}.txt"`
done