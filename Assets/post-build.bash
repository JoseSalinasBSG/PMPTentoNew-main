#!/bin/bash
echo "Uploading IPA to Appstore Connect..."
echo "Usuario: $ITUNES_USERNAME"
echo "Contraseña: $ITUNES_PASSWORD"
echo "xcrun altool --upload-app --type ios -f \"$path\" -u \"$ITUNES_USERNAME\" -p \"$ITUNES_PASSWORD\""

path="${UNITY_PLAYER_PATH}"
if xcrun altool --upload-app --type ios -f "$path" -u "$ITUNES_USERNAME" -p "$ITUNES_PASSWORD" -v; then
    echo "Upload IPA to Appstore Connect finished with success"
else
    echo "Upload IPA to Appstore Connect failed"
fi