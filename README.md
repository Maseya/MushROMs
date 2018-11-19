# Editors

C# .NET Class Library for creating 2D game editors

## Table of Contents

- [What is the Editors library?](#what-is-the-editors-library)
- [Installation](#installation)
    - [Visual Studio](#visual-studio)
- [Contributions](#contributions)
- [Credits](#credits)
- [License](#license)

## What is the Editors library?

The Editors class library provides utilities for creating common editors
like text editors, image editors, and level editors. While these editors
can be used in any scenario, the design principle focuses on using them
for 2D game editing purposes.

## Installation

Presently, the [Visual Studio 2017 IDE][vs17] is the only supported
environment. Users are encouraged to suggest new environments in our
[Issues][issues] section.

### Visual Studio
- Get the [latest][vs_latest] version of Visual Studio. At the time of
writing this, it should be Visual Studio 2017. You have three options:
[Community, Professional, and Enterprise][vs_compare]. Any of these
three are fine. The collaborators presently build against community
since it is free. See that you meet the
[System Requirements][vs_req] for Visual Studio for best interaction.
- When installing Visual Studio, under the _Workloads_ tab, select
**.NET desktop development**. Under the _Individual Components_ tab,
select .NET Framework 4.7 SDK and .NET Framework 4.7 targeting pack if
they weren't already selected. If a newer SDK is available, be sure to
select that one as well. We will keep up to date with the SDK as
frequently as possible.
- Click Install and let the installer do it's thing.
- Clone our repository and open the solution file in Visual Studio.
- Open the test explorer and make sure all tests are passing.

## Contributions

Do you want to add a feature, report a bug, or propose a change to the
project? That's awesome! First, please refer to our
[Contributing](CONTRIBUTING.md) file. We use it in hopes having the best
working environment we can.

## Credits

* [Nelson Garcia](https://github.com/bonimy): Project leader and main
programmer

## License

C# .NET Class Library for creating 2D game editors
Copyright (C) 2018 Nelson Garcia

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published
by the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program. If not, see http://www.gnu.org/licenses/.

[vs17]: https://www.visualstudio.com/en-us/news/releasenotes/vs2017-relnotes
[issues]: https://github.com/Maseya/Editors/issues
[vs_latest]: https://www.visualstudio.com/downloads
[vs_compare]: https://www.visualstudio.com/vs/compare
[vs_req]: https://www.visualstudio.com/en-us/productinfo/vs2017-system-requirements-vs
