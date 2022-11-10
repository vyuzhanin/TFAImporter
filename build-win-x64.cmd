set location=%cd%
echo Your current dir is %location%

echo "Clear previous built binary"
rm -rf publish/win-x64

echo "Building project"
dotnet publish -r win-x64 --self-contained true -c Release -o publish/win-x64

echo "Done. Check for publish folder"

pause