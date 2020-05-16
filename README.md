# Conzole
Conzole is a collection of static helper functions to simplify data display and input from the command line in DotNet Core.

## Usage
The following contains examples of library usage. These functions can be accessed through the "ConzoleUtils" static class.

### Count
The Count function takes a collection of items and displays the size of the collection.

### List
The List function takes a collection of items and displays the as a list. The simplest usage will take a collection of any objects and convert them to their string representation. For example, the following will list three integers and display with the default format:
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

### Menu
The menu function allows the user to define a navigable menu structure and attach hooks for actions to be performed when entering those menus.
