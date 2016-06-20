#!/bin/sh   
HOST=172.17.73.107 
USER=u3d    
PASS=u3d2016    
echo "Starting to sftp..."  
lftp -u ${USER},${PASS} sftp://${HOST} <<EOF   
cd /kagou/datafile    
lcd /StreamingAssets
prompt
mget *.*    
bye    
EOF    
echo "done"