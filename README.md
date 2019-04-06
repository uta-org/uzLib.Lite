# uzLib.Lite

> The Lite version of my paid uzLib (Unity3D) API used as dependency of my open-source repos.

## Contents

This API has an extensive list of methods to extend funcionality of .NET Framework classes, WinForms, System.Console and Unity3D, also it contains own/third party implementation:

### List of interesting contents by namespaces

- **uzLib.Lite.Core**
	- Contains a Singleton implementation.
	- Contains a Console wrapper to read lines or keys at the same time. (**ConsoleOutput** class, *method: ReadLineOrKey*)
	- Inside of the Input namespace we can find some class that contains an implementation (**SmartPsswrd class**) to read `passwords` in screen without displaying it (as expected).

- **uzLib.Lite.Extensions**
	- Contains some *array/collection extensions* (add element to an array, check if *Dictionary/List/HashSet/IEnumerable* is null or empty (`IsNullOrEmpty`), ForEach extensions to execute `Action` callbacks (included with index) (expanded on paid API), `DisctintBy`...)
	- Contains some Dictionary extensions methods:
		- **FindIndex** (find item index by predicate)
		- **GetIndex** (get index of existing item, if not, returns -1)
		- **AddOrSet** (add value if key doesn't exists, if not update its value)
			- **AddOrAppend** (the same as before, but for `List<T>/T[]` as **TValue**)
		- **AddAdGet** (if key doesn't exist add it with specified value, then return it)
			- **Get** (the same as before, but force generic types with constraints to be classes (`where TValue : class, new()`))
			- **SafeGet** (try to get value if key exists if not returns default value (`default(TValue)`))
	- Contains some *Bitmap extensions*: get an `IEnumerable` of Colors from a `Bitmap` class or `Save` it to a path (or to a Stream (**TODO**)).
	- Contains some *byte extensions*: `RoundOff` method (to get a rounded byte)...
	- Contains some *Color extensions*: **get threshold** between two colors, compare `color1` to **get similar color** from a IEnumerable of colors (2), *posterize convolution*, get percentage of similarity, round color...
	- Contains a method extension to **compile \*.sln files**... 
	- Contains *compression extensions*: zip/unzip files from it's path, streams or objects (sync/async)...
	- Contains a *CodeDOM extension* to add `static` literal to new classes declared through `CodeTypeDeclaration` class.
	- Contains a *DateTime extension* to get its *UNIX timestamp*(in `DateTime` not `DateTimeOffset` class).

- **uzLib.Lite.Plugins.SymLinker**

- **uzLib.Lite.Shells**

- **uzLib.Lite.Unity**

**TODO:** Write all the folders from the API and interesting things that it has.

## Setup

Need help? Just clone this repository into your Unity project or in your solution.

For documentation, just check the [Documentation API](http://dev.z3nth10n.net/dev/assets/uzlib.lite/docs).

## Issues

Having issues? Just report in [the issue section](/issues). **Thanks for the feedback!**

## Contribute

Fork this repository, make your changes and then issue a pull request. If you find bugs or have new ideas that you do not want to implement yourself, file a bug report.

## Donate

Become a patron, by simply clicking on this button (**very appreciated!**):

[![](https://c5.patreon.com/external/logo/become_a_patron_button.png)](https://www.patreon.com/z3nth10n)

... Or if you prefer a one-time donation:

[![](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://paypal.me/z3nth10n)

## Copyright

Copyright (c) 2019 z3nth10n (United Teamwork Association).

License: GNU General Public License v3.0