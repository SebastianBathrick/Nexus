# Nexus - An Embeddable Programming Language 
Nexus is an easy-to-learn programming language meant to be embedded into other applications via its dependency-free C# library. The syntax and features are inspired by Lua and have many of the same features, such as its data types, most notably tables. One of its language design goals is to keep features to a minimum, even fewer than Lua's. That means it has minimal syntactic sugar, makes design choices that work within its limitations, and avoids multiple ways of doing the same thing; all without limiting its capabilities.  For example, object-oriented programming is entirely absent in Nexus, unlike in Lua, but similar features of the paradigm can be achieved using tables; therefore, abiding by the design goals mentioned previously.

Building on these principles, the current primary focus is to complete the library (Nova.csproj). Nova’s interpreter processes code by first converting a string of source code into a syntax tree, then compiling the syntax tree into bytecode, and finally executing the bytecode using a virtual machine. These steps—parsing, compiling, and executing—are handled by the library’s primary interpreter class, allowing compilation and execution of source code at runtime.

Following the completion of the library, creating a console application that uses the library to execute source code is planned, but it is not a priority. This will include a CLI designed to be as simple and quick to use as possible.

# Data Types
<b>number</b>: 8 byte value-type floating point number (can round)

<b>bool</b>: 2 byte value-type boolean (true/false)

<b>text</b>: 2 byte Unicode characters and a 4 byte address.

<b>function</b>: A callable function that optionally can accept a value and/or return an optional value.

<b>table</b>: Data structure that can indexed elements of multiple types that can also be referenced if given names. Code can also access an element using dot notation + its name or its name inside a string runtime.

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
if myVar is 20 {
	print("Variable is 20")
}
```

### If-Other (If-Else) Statements
```
if myVar is not 5 {

}
other {

}
```

## Other (Else) Statements
```
if myVar is not 2 {

}
other myVar is 20 {
}
```

## While
```
while myVar is 21 {
}
```