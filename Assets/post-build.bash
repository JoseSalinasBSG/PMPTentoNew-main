#!/bin/bash
echo "Uploading IPA to Appstore Connect..."
echo "Usuario: $ITUNES_USERNAME"
echo "Contraseña: $ITUNES_PASSWORD"
echo "path: $UNITY_PLAYER_PATH"
echo "worksapce: $WORKSPACE"
echo "target name: $TARGET_NAME"
#path="$WORKSPACE/.build/last/$TARGET_NAME/build.ipa"
#Path is "/BUILD_PATH/<ORG_ID>.<PROJECT_ID>.<BUILD_TARGET_ID>/.build/last/<BUILD_TARGET_ID>/build.ipa"
path="${UNITY_PLAYER_PATH}"
if xcrun altool --upload-app --type ios -f $path -u $ITUNES_USERNAME -p $ITUNES_PASSWORD ; then
    echo "Upload IPA to Appstore Connect finished with success"
else
    echo "Upload IPA to Appstore Connect failed"
fi