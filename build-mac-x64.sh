#!/bin/zsh

PUBLISH_FOLDER=./publish/osx-x64

echo "Clear previous built binary"
rm -rf $PUBLISH_FOLDER

echo "Building project"
dotnet publish -r osx-x64 --self-contained true -c Release -o $PUBLISH_FOLDER

echo "Renaming executable file"
mv $PUBLISH_FOLDER/Cerberos.TfaImporter $PUBLISH_FOLDER/2FAImporter

echo "Clearing old app"
rm -rf ./publish/2FAImporter.app

echo "Creating folder structure"
mkdir ./publish/2FAImporter.app
mkdir ./publish/2FAImporter.app/Contents
mkdir ./publish/2FAImporter.app/Contents/MacOS
mkdir ./publish/2FAImporter.app/Contents/Resources

echo "Copy files"
cp -aR $PUBLISH_FOLDER/. ./publish/2FAImporter.app/Contents/MacOS

cp ./bundle/Info.plist ./publish/2FAImporter.app/Contents

echo "Making icons"
cd bundle
mkdir icon.iconset
sips -z 16 16     Icon1024.png --out icon.iconset/icon_16x16.png
sips -z 32 32     Icon1024.png --out icon.iconset/icon_16x16@2x.png
sips -z 32 32     Icon1024.png --out icon.iconset/icon_32x32.png
sips -z 64 64     Icon1024.png --out icon.iconset/icon_32x32@2x.png
sips -z 128 128   Icon1024.png --out icon.iconset/icon_128x128.png
sips -z 256 256   Icon1024.png --out icon.iconset/icon_128x128@2x.png
sips -z 256 256   Icon1024.png --out icon.iconset/icon_256x256.png
sips -z 512 512   Icon1024.png --out icon.iconset/icon_256x256@2x.png
sips -z 512 512   Icon1024.png --out icon.iconset/icon_512x512.png
cp Icon1024.png icon_512x512@2x.png
mv icon_512x512@2x.png icon.iconset/

iconutil -c icns "icon.iconset"

rm -R icon.iconset
cd ..                        
mv ./bundle/icon.icns ./publish/2FAImporter.app/Contents/Resources/

rm -rf $PUBLISH_FOLDER/*

cp -aR ./publish/2FAImporter.app $PUBLISH_FOLDER/

rm -rf ./publish/2FAImporter.app

echo "Done. Check for publish folder"