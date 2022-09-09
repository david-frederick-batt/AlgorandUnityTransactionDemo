# Algorand Transaction Demo in Unity
 This project is a proof of concept for using the [Dotnet Algorand SDK](https://github.com/FrankSzendzielarz/dotnet-algorand-sdk) to make payment transactions between multiple accounts in Unity. The demo allows the user to generate new accounts or access prefunded accounts from [Algorand Sandbox](https://github.com/algorand/sandbox).
 
 ## Prerequisites
 - **[Algorand Sandbox](https://github.com/algorand/sandbox) must be installed and running**.
 - **Unity version `2021.3` or higher must be installed**.

 # Using the Dotnet Algorand SDK in Unity
1. In Visual Studio create a new C# Library
2. From the NuGet command line execute:
```powershell
Install-Package Algorand2_Unity
```
3. Next, execute the following in the developer command prompt:
```
dotnet publish
```
4. Locate `Algorand.dll`, `BouncyCastle.Crypto.dll`, and `RestSharp.dll` in `<project-root>\bin\Debug\netstandard2.0\publish`
5. Move the assemblies over into your Unity project.


You should now be able to use the SDK in your Unity project. 
