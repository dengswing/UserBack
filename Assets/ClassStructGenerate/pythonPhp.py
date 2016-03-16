#coding:utf-8
import os
import re
file1 = 'orm.config.php'
file2 = 'C:\\Users\\Administrator\\Desktop\\test1.php'
def read_file(source_file):
    dic = dict()
    if os.path.exists(source_file):
        content = read_content(source_file)

        preg = re.compile('(array\(.*\))',re.DOTALL)
        rtn = preg.search(content)
        if len(rtn.groups()) > 0:
            rtn = rtn.groups()[0]
            rtn = rtn.replace("array(","").replace(")","")
            rtns = rtn.split("\n")
            for line in rtns:
                if len(line.strip()) == 0:
                    continue
                lines = line.split("=>")
                lines[0] = lines[0].strip()
                lines[1] = lines[1].strip()
                if lines[0].find("\"") == 0:
                    lines[0] = lines[0][lines[0].find("\"")+1:lines[0].rfind("\"")]
                elif lines[0].find("'") == 0:
                    lines[0] = lines[0][lines[0].find("'")+1:lines[0].rfind("'")]
                if lines[1].find("\"") == 0:
                    lines[1] = lines[1][lines[1].find("\"")+1:lines[1].rfind("\"")]
                elif lines[1].find("'") == 0:
                    lines[1] = lines[1][lines[1].find("'")+1:lines[1].rfind("'")]
                dic[lines[0]] = lines[1]

    return dic

def read_content(source_file):
    handle = open(source_file,'r')
    content = handle.read()
    handle.close() 
    return content

def write_file(dst_file,content,dic):
    handle = open(dst_file,'w')
    handle.write(content)
    for key in dic.keys():
        line = "\n    \"%s\"=>\"%s\","%(key,dic[key])
        handle.write(line)
    handle.write("\n);\n?>")
    handle.close()

def merge():
	print("==>")
    #read_content(file1)

if __name__ == '__main__':
    #merge()
	handle = open(file1,'r')
    content = handle.read()
    handle.close() 
    print ('done!')