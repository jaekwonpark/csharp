Build a package used for Nutanix Csharp SDK.
Nutanix Csharp SDK will import this for serializing/deserializing enum types with custom enum name.

To build a package,
bash build.sh <build number>
which will build a package CustomEnumNameConverter-1.0.0-<build number>.
To bumpup the version change <VersionPrefix>1.0.0</VersionPrefix> in .csproj file

To push the package to private repository,
bash push.sh <package name ends with .nupkg>