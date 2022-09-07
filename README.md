# Algorand Transaction Demo in Unity
 This project is a proof of concept for using the [Dotnet Algorand SDK](https://github.com/FrankSzendzielarz/dotnet-algorand-sdk) to make payment transactions between multiple accounts in Unity. The demo allows the user to generate new accounts or access prefunded accounts from [Algorand Sandbox](https://github.com/algorand/sandbox).
 
 ## Prerequisites
 - **[Algorand Sandbox](https://github.com/algorand/sandbox) must be installed and running**.
 - **Unity version `2021.3` or higher must be installed**.

 # Using the Dotnet Algorand SDK in Unity
 1. Download the following NuGet packages: [Algorand2_Unity](https://www.nuget.org/packages/Algorand2_Unity/1.0.0.10#readme-body-tab), [BouncyCastle.NetCore](https://www.nuget.org/packages/Algorand2_Unity/1.0.0.10#readme-body-tab), and [RestSharp](https://www.nuget.org/packages/RestSharp/)
 2. For each package, replace the `.nupkg` file extension with `.zip`.
 3. Extract the contents of all the packages.
 4. Locate and copy the `.dll` file from the following directory of every package: `<package-root>\lib\netstandard2.0`.
 5. Move over the `.dll` files into your Unity project.

You should now be able to use the SDK in your Unity project. 
