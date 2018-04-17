# VuejsConverter

## About VuejsConverter

VuejsConverter is a project that came from the need to convert a vuejs project from the old 0.12 version to the most recent vuejs version. Since in a larger application this is not an easy process and would require a slow down in new developments if not an actual stagnation until the process is completed I decided to create my own converter to help in this situations.
It is intended that this project converts at least 90% of the project from the 0.12 to the 1.0.
Later more versions will be included.

## How to run VuejsConverter

### Debug

To run this project in debug your either create your own "EntryPoint" and call directly the tool:
```C#
MigrationTool.MigrationTool tool = new MigrationTool.MigrationTool(type, path);
```

or add to the existent EntryPoint the two required arguments under the debug arguments.

### Release

My own preference is to create a bat file that runs the EntryPoint.exe with two arguments.
Ex: 
```batch
.\EntryPoint.exe "C:\Projects\vuejs-project" 1
```

### Arguments:

On the first argument the actual project directory.
On the second argument the converter to use, for now only 1 is available which corresponds to the MigrationTool0_12_16To1_0_7

## Contributing

Since this is a quite ambitious goal to archive any help would be greatly appreciated!

MigrationTool is the project to look for if any modification to the actual converter mechanism is intended.
The EntryPoint, has its name suggests is just an entry point to the converter and for now is a console but maybe later will be converted to a Windows App or something.

## License

The VuejsConverter is open-sourced software licensed under the [MIT license](https://opensource.org/licenses/MIT).