# Nexus - An Embeddable Programming Language 
Nexus is an easy-to-learn programming language designed to be embedded into other applications via its dependency-free C# library. The syntax and features are inspired by Lua. Nexus shares many features with Lua, especially its data types such as tables. The library is in **early development** and many of the language's features have not yet been implemented (about 1/2 through week 1 as of writing this README).



One of its design goals is to keep features to a minimum, even fewer than Lua's. This leads to minimal syntactic sugar and design choices that work within its limitations. Nexus avoids multiple ways of doing the same thing while remaining fully capable. For example, object-oriented programming is absent in Nexus, unlike in Lua. However, similar features can be achieved using tables, which stay true to the design goals mentioned above.



Building on these principles, the current primary focus is to complete the library (Nexus.csproj). Nexus’s interpreter processes code by first splitting the source code into tokens. Using these tokens, it builds a syntax tree; that tree is then flattened/compiled into bytecode, and finally, the compiled code is passed to the virtual machine for execution. These steps—parsing, compiling, and executing—are handled by the library’s primary interpreter class, allowing compilation and execution of source code at runtime.

# Data Types
Nexus is a dynamically typed language with the following primitives:

<b>number</b>: 8 byte value-type floating point number (can round)

<b>bool</b>: 2 byte value-type boolean (true/false)

<b>text</b>: 2 byte Unicode characters and a 4 byte address.

<b>function</b>: A callable function that optionally can accept a value and/or return an optional value.

<b>table</b>: Data structure that can indexed elements of multiple types that can also be referenced if given names. Code can also access an element using dot notation + its name or its name inside a string runtime.

> [!NOTE]
>  Nil and null do not exist. Variables must have an initial value when declared. Accessing a non-existent element in a table throws an error, though functionality to check for an element is being planned/designed. Because the language supports dynamic typing, you can mimic null-like values. For example, if you initially set a variable to a table but later want it to represent empty or undefined, use false instead of null.

# Progress
- Currently lexical analysis, parsing, bytecode compilation, and the virtual machine support top-level return expressions (nested or otherwise). These expressions can be composed of arithmetic, comparison, and/or logical operators. Return statements and expressions still need further testing.

- Both booleans and number value have been implemented with expression coercion support. For example, `return true == 1` will return true because `true` is coerced to `1`. If it was `return false == 1` it would return false because `false` is coerced to `0`.

# Syntax
## Basics
### Hello World
```
print("Hello World")
```

### Single-Line Comments
```
# This is a comment
print("Hello World") # You can end a line with single line comments
```
### Multi-Line Comments
```
## This is a multi-line comment. You can End 
   the comment using two pound/hashtag symbols ##

print("before")## Multi-line commnts can be before or after
   code on the same line ## print("after")
```

## Variables
### Variable Declarations
```
# Variables are implicitly typed
num = 25

# Variables MUST BE initialized when declared 
flag = true

# This will is a syntax error
noInitVar
```

### Variable Assignment
```
myVar = 2 # Declaration

myVar = 3 # Reassignment

myVar = "hello" # Changes variable type to string
```

## Control Structures
### Conditional Statements
```
if myVar is 20
{
	print("Variable is 20")
}
```

### If-Other (If-Else) Statements
```
if myVar is not 5 
{

}
other 
{

}
```

## Other (Else) Statements
```
if myVar is not 2 
{

}
other myVar is 20 
{

}
```

## While
```
while myVar is 21 
{

}
```
