#!/bin/sh


if [ $# != 1 ] ; then
echo "xx.app.dSYM parameter does not!"
exit 1;
fi

code=$1
demo=${code%%.*}

echo ${code%%.*}

urfile=${demo}.sym
./dump_syms $code >$urfile

read str < $urfile
echo $str

OLD_IFS="$IFS"
IFS=" "
arr=($str)
IFS="$OLD_IFS"

name=${arr[3]}
echo $name


mkdir -p symbols/$demo/$name/
mv $urfile symbols/$demo/$name/


for file in `ls *.dmp`
do
echo $file
./minidump_stackwalk $file symbols > `echo "${file}.txt"`
done