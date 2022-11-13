# 2FAImporter

Provides two factor authentication (2FA) accounts importing from services like Google Authenticator

![image](https://user-images.githubusercontent.com/596514/201524251-6df6e7a2-f6c2-4bbc-b16d-51ac76e73c6a.png)
![image](https://user-images.githubusercontent.com/596514/201524255-ec9d7bab-78ee-4b51-923c-8054f418aa45.png)

## Pre-Requisites

The application has developed on [.net 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) which requires to build the project from source code.

## Building

Project Contains build scripts for most popular platforms:

| Script|Plaform  |
|---| --- |
| [build-mac-arm64.sh](build-mac-arm64.sh) |Mac OS 11 Apple Silicon|
| [build-mac-x64.sh](build-mac-x64.sh)     |Mac OS 11 Intel|
| [build-win-x64.cmd](build-win-x64.cmd)   |Windows x64|

Each script builds the project to the `/publish/<platform-name>` folder.

## Releases

Check for [releases section](https://github.com/vyuzhanin/TFAImporter/releases/latest) to to get the latest build. For macOS users the application isn't notarized so after you will put 2FAImporter to Aplications folder run the following command:
```shell
$sudo xattr -rd com.apple.quarantine /Applications/2FAImporter.app
```

## Contributing

Any contributions are **greatly appreciated**. If you have a suggestion that would make this better, please open an issue or fork the repository and create a pull request.

## License

[GNU GENERAL PUBLIC LICENSE Version 3](https://www.gnu.org/licenses/gpl-3.0.txt)
The licenses for most software and other practical works are designed to take away your freedom to share and change the works.  By contrast, the GNU General Public License is intended to guarantee your freedom to share and change all versions of a program--to make sure it remains free software for all its users.
Also some third party libraries provided by the following licenses:avalom [Apache-2.0](https://www.apache.org/licenses/LICENSE-2.0), [Google Proprietarity](https://github.com/protocolbuffers/protobuf/blob/main/LICENSE) and [MIT](https://opensource.org/licenses/MIT).
