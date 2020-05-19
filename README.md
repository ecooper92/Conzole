![GitHub release (latest by date)](https://img.shields.io/github/v/release/ecooper92/Conzole)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](/LICENSE.md)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Conzole?label=NuGet%20Downloads)](https://www.nuget.org/packages/Conzole/)

# Conzole
Conzole is a collection of static helper functions to simplify data display and input from the command line in DotNet Core.

## Usage
The following contains examples of library usage. These functions can be accessed through the `ConzoleUtils` static class.

### Count
The Count function takes a collection of items and displays the size of the collection. The simplest usage will take a collection of objects and display the count in the collection using the default format:
```
var items = new int[] { 3, 4, 2 };
ConzoleUtils.Count(items);
```
Output:
```
3 result(s)
```
Alternative formatting can be applied for a line by supplying a custom formatter through the `CountOptions` class:
```
var items = new int[] { 3, 4, 2 };
var countOptions = new CountOptions();
countOptions.ResultFormatter = count => $"{count}!!!";

ConzoleUtils.Count(items, countOptions);
```
Output:
```
3!!!
```

### List
The List function takes a collection of items and displays the as a list. The simplest usage will take a collection of objects and convert them to their string representation. For example, the following will list three integers and display with the default format:
```
var items = new int[] { 3, 4, 2 };
ConzoleUtils.List(items);
```
Output:
```
1) 3
2) 4
3) 2
```
Alternative formatting can be applied for a line by supplying a custom formatter through the `LineOptions` class:
```
var items = new int[] { 3, 4, 2 };
var listOptions = new ListOptions<int>();
listOptions.LineFormatter = (index, item) => $"{item}::{index}";

ConzoleUtils.List(items, listOptions);
```
Output:
```
1::3
2::4
3::2
```

### Prompt
The Prompt functions request the user to input a value on console. The simplest case for a prompt takes an optional text prompt to display to the user then reads from the console and returns the result:
```
var result = ConzoleUtils.Prompt("Enter your name here:");
```
The other prompt functions are built on top of this function and also convert the resulting string into another data type:
```
var success1 = ConzoleUtils.PromptInt("Enter your int here:", out var result);
var success2 = ConzoleUtils.PromptDouble("Enter your double here:", out var result);
```
Since these fuctions are built on top of the TryParse paradigm they follow the pattern of returning a boolean indicating whether a valid value was provided.

### Repeat Until Success
The Repeat Until Success function attempts an action in a loop until either the action returns true or the user provides the negative response. 
```
var success = await ConzoleUtils.RepeatUntilSuccess(() =>
{
    return Task.FromResult(true);
});
```

### Menu
The menu function allows the user to define a navigable menu structure and attach hooks for actions to be performed when entering those menus. For example, the following defines a navigable root menu and nested menu:
```
var nestedMenu = new Menu("nested menu");
nestedMenu.AddMenuItem("a", new MenuItem("a1", () => Task.CompletedTask));
nestedMenu.AddMenuItem("b", new MenuItem("b1", () => Task.FromResult(true)));
nestedMenu.AddMenuItem("c", new MenuItem("c1", () => Task.FromResult(false)));

var rootMenu = new Menu("root menu");
rootMenu.SetExitMenuItem("Exit", "X");
rootMenu.AddMenuItem("I", nestedMenu);
rootMenu.AddMenuItem("II", new MenuItem("root action", () => Task.CompletedTask));
```
Initially this menu will display:
```
root menu
I) nested menu
II) root action
X) Exit

Enter selection:
```
Entering "II" will cause the menu to repeat since the Action provided simply returns a completed `Task`, but entering "I" will navigate to the nested menu and display:
```
nested menu
a) a1
b) b1
c) c1
0) Back

Enter selection:
```
Entering either "a" or "b" will cause the menu to repeat since returning a completed `Task` or true wrapped in a `Task` have the same effect. However entering "c" returns a false wrapped in a `Task` which has the result of exiting the current menu and returning to the parent menu. The default exit action "0" always returns false and exits the current menu, returning to the parent.

Functions can be substituted for the lambdas in the example above to provide hooks into the menu items.
