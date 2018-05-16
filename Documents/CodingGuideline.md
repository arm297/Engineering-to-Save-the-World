# Contribution Details and Guidelines

## General

This project is implemented using Unity. Anyone wishing to learn Unity can look through and attempt their [tutorials](https://unity3d.com/learn/tutorials). The [Unity API](https://docs.unity3d.com/ScriptReference/) provides further reference on using their library. 

The behavioral scripts are writen in C#. A good tutorial for learning C# can found [here](http://csharp.net-tutorials.com/).

### C# Coding Style

This project should follow [Unity's C# Coding guidelines](http://wiki.unity3d.com/index.php/Csharp_Coding_Guidelines).

A quick summary:
1. Use camelCase for class variables, local variables and method parameters, i.e. `thisVariableName`.
2. Use PascalCase for function names, event names, and class names, i.e. `ThisNameFormat`.
3. For custom methods and variables, add informative names, i.e. `ResetsClockTime()` instead of  `ChangeClock()`.
4. Group C# script sections according to the following order: Fields, Constructors, Properties, Events, Methods, Private interface implementations, Nested types.
5. For more complex functions and non-obvious class variables, document the code by adding a brief description of the variable's or function's purpose.

### GitHub Commits and Branches

For larger changes and modifying files multiple people are working on, commit your changes to branches.

1. Add informative comments explaining what you changed and why, instead of how.
2. When you create a pull request for your branch, make sure it is rebased, or merged on top of master. See more information about rebasing vs merging [here](https://hackernoon.com/git-merge-vs-rebase-whats-the-diff-76413c117333).
3. Before creating a pull request, be sure that your modifications still build and run the project.
4. If you changed a file someone else is working on, be sure to notify then, and try to make them review your changes.
5. Please do not add any of the folders or files specified in the .gitignore. You should only add your changes to the Assets directory.

For more information, refer to the [GitHub Help Page](https://help.github.com/).

### General Issues

If you notice an issue or can think of a good feature to add, place a description of the task under Issues.
