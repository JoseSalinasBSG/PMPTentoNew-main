#!/bin/bash
#Path is "/BUILD_PATH/<ORG_ID>.<PROJECT_ID>.<BUILD_TARGET_ID>/.build/last/<BUILD_TARGET_ID>/build.ipa"
path="$WORKSPACE/.build/last/$TARGET_NAME/build.ipa"
echo "Uploading IPA to Appstore Connect..."
echo "Usuario: $ITUNES_USERNAME"
echo "Contraseña: $ITUNES_PASSWORD"
echo "worksapce: $WORKSPACE"
echo "path: $path"
if xcrun altool --upload-app -t ios -f $path -u $ITUNES_USERNAME -p $ITUNES_PASSWORD ; then
    echo "Upload IPA to Appstore Connect finished with success"
else
    echo "Upload IPA to Appstore Connect failed"