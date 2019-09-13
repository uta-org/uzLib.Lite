In case you need the dependencies as an Unity git submodule (as a Nuget package), just use this command:

`$ git submodule add -b unity-<ver> https://github.com/uta-org/<project_name>.git`

In this case:

`$ git submodule add -b unity-1.2.0 https://github.com/uta-org/uzLib.Lite.git`

In case you need to update, then just use:

`$ git submodule update --remote --merge`