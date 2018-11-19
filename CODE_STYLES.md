# C# Coding Conventions

Coding conventions serve the following purposes:

- They create a consistent look to the code, so that readers can focus
on content, not layout.
- They enable readers to understand the code more quickly by making
assumptions based on previous experience.
- They facilitate copying, changing, and maintaining the code.
- They demonstrate C# best practices.

This document is based on Microsoft's [C# Coding Conventions][guide].
Some differences do exist, however.

## Table of Contents

 - [Naming Conventions](#naming-conventions)
 - [Layout Conventions](#layout-conventions)
 - [Commenting Conventions](#commenting-conventions)
 - [Language Guidelines](#language-guidelines)
     - [General Code Style](#general-code-style)
     - [Indentation](#indentation)
     - [New Lines](#new-lines)
     - [Spacing](#spacing)
     - [String Data Type](#string-data-type)
     - [Unsigned Data Type](#unsigned-data-type)
     - [Arrays](#arrays)
     - [Delegates](#delegates)
     - [try-catch and using Statements](#try-catch-and-using-statements)
     - [&& and || Operators](#-and--operators)
     - [New operator](#new-operator)
     - [Event Handling](#event-handling)
     - [Static Members](#static-members)
     - [LINQ Queries](#linq-queries)
 - [Solution Folder Structure](#solution-folder-structure)
 - [Unit Tests](#unit-tests)

## Naming Conventions

- In short examples that do not include [using directives][using], use
namespace qualifications. If you know that a namespace is imported by
default in a project, you do not have to fully qualify the names from
that namespace. Qualified names can be broken after a dot `.` if they
are too long for a single line, as shown in the following example.

```C#
var currentPerformanceCounterCategory = new System.Diagnostics.
    PerformanceCounterCategory();
```

- You do not have to change the names of objects that were created by
using the Visual Studio designer tools (and are not used outside of the
designer) to make them fit other guidelines.
- All [interfaces][interface] must begin with `I`.

```C#
// Right
public interface IFoo
{
}

// Wrong! Don't name interface without an 'I' prefix.
public interface Foo
{
}

```

- All [Attribute][attribute] class names must end with `Attribute`.

```C#
// Right
public class FooAttribute : Attribute
{
}

// Wrong!
public class Foo : Attribute
{
}
```

- All types must be `PascalCase`.

```C#
// Right
public class FooBar
{
}

// Wrong! Do not use camelCase.
public class fooBar
{
}

// Wrong! Never use alllowercase.
public class foobar
{
}

// Wrong! Don't use snake_case.
public class foo_bar
{
}
```

- All locals, parameters, and field members must be `camelCase`.

```C#
public void Foo()
{
    // Right!
    int fooBar;

    // Wrong! Never user alllowercase.
    int foobar;

    // Wrong! Do not use snake_case.
    int foo_bar;

    // Wrong! Do not use PascalCase.
    int FooBar;

    // Wrong!
    int _fooBar;
}
```

- Private field members must begin with an underscore `_`.

``` C#
public class Foo
{
    // Right
    private int _bar;

    // Wrong! Prefix an underscore.
    private int bar;
}
```

- [Class][classes] field members can only be private. In place of public
or protected fields, use [properties][properties].

```C#
public class Foo
{
    // Right
    print int _foo;

    // Wrong! Public field is forbidden.
    public int _bar;
}
```

- [Structs][structs] can have public fields if direct access is
required. Such a field must also be a struct.
- Explicitly declare all [access modifiers][access].

## Layout Conventions

 - Good layout uses formatting to emphasize the structure of your code
 and to make the code easier to read.
- Use smart indenting, four-character indents, and tabs saved as spaces.
These are the Visual Studio default Code Editor settings.
- Comment lines have a character limit of 72 characters.
- Code lines have a character limit of 79 characters.
- Write only one statement per line.

```C#
// Right
x += y;
y += z;
```

```C#
// Wrong!
x += y; y += z;
```

- Write only one declaration per line.

```C#
// Right
var a = 0;
var b = -1;
```

```C#
// Wrong!
int a = 0, b = -1;
```

- If continuation lines are not indented automatically, indent them one
tab stop (four spaces).
- Add at least one blank line between method definitions and property
definitions.

```C#
// Right
void Foo()
{
}

void Bar()
{
}
```

```C#
// Wrong!
void Foo()
{
}
void Bar()
{
}
```


- Use parentheses to make clauses in an expression apparent, as shown in
the following code.

```C#
if ((val1 > val2) && (val1 > val3))
{
    // Take appropriate action.
}
```

## Commenting Conventions

- Place the comment on a separate line, not at the end of a line of
code.

```C#
// Place comments here.
var a = 0;

a++; // Do not write comment here.
```

- Begin comment text with an uppercase letter.
- End comment text with a period.
- Insert one space between the comment delimiter '//' and the comment
text, as shown in the following example.

```C#
// The following declaration creates a query. It does not run
// the query.
```

- Do not create formatted blocks of asterisks around comments.

## Language Guidelines

The following sections describe practices that you should follow when
writing code.

### General Code Style

- Do _not_ qualify [Members][members] with `this.`.
- Use `var` where possible.
- For locals, parameters, and members, use the predefined type (e.g.
`int`, `string`, etc.).
- For member access expressions, use the framework type (e.g.
`Int32.MaxValue`).
- Uses braces for single-line code blocks

```C#
if (test)
{
    Display();
}
```

- Use [object and collection initializers][initializers].

```C#
// Right
var list = new List<int>()
{
    0,
    1,
    2,
};
```

```C#
// Wrong!
var list = new List<int>()
list.Add(0);
list.Add(1);
list.Add(2);
```

- Use [pattern matching][patterns] in `is` and `as` statements (.NET 4.7
required).

```C#
// Right
if (obj is int i)
{
    // Do stuff.
}
```

```C#
// Wrong
if (obj is int)
{
    var i = (int)obj;
    // Do stuff.
}
```

- Use [explicit tuple names][tuples].

```C#
// Right
var named = (first: "one", second: "two");
```

```C#
// Wrong!
var unnamed = ("one", "two");
```

- Use simple [default expressions][defaults].

```C#
// Right
public static readonly Vector2f Empty = default;
```

- Do _not_ use [expression bodies][expressions] except for single-line
lambda expressions.
- Use [inlined variable declaration][outvar] (.NET 4.7 required).

```C#
// Right
if (Int32.TryParse(text, out var value))
{
    // Do stuff.
}
```

```C#
// Wrong!
var value = default(int);
if (Int32.TryParse(text, out value))
{
    // Do stuff.
}
```

- Use `?` and [`??`][null_op] for `null` checking.

### Indentation

- Always indent block and case contents.
- Do _not_ indent open and close braces or case labels.

```C#
switch (value):
{
case 0:
    break;
default:
    return;
}
```

- Place labels one indent less than current. Place a newline before a
`goto` label. Labels must begin with an underscore.

```C#
void Foo()
{
    var a = 0;

_loop:
    a++;
    goto _loop;
}
```

### New Lines

- Place open and close braces on new lines.
- Place `else`, `catch`, and `finally` on new lines.
- Place members in object initializers and anonymous types on new line.
- Place query expression clauses on new line.

### Spacing

- Do _not_ insert space between method name and its opening parenthesis,
within parameter/argument list parentheses, or within empty
parameter/argument list parentheses.
- Insert space after keywords in control flow statements (`for`,
`foreach`, `if`, etc.).
- Do not insert space within parentheses of expressions, type casts, or
control flow statements.
- Do not insert space after a cast.
- Extra space in declaration statements is okay for alignment purposes.
- Do not insert space before or within square brackets.
- Insert space before and after colon delimiters.
- Insert space after (but not before) comma delimiters and semicolon
delimiters in `for` statements.
- Insert space before and after binary operators.

### String Data Type

- Use the `+` operator to concatenate short strings, as shown in the
following code.

```C#
string displayName = nameList[n].LastName + ", " + nameList[n].FirstName;
```

- To append strings in loops, especially when you are working with large
amounts of text, use a [StringBuilder][stringbuilder] object.

```C#
var phrase = "lalalalalalalalalalalalalalalalalalalalalalalalalalalalalala";
var manyPhrases = new StringBuilder();
for (var i = 0; i < 10000; i++)
{
    manyPhrases.Append(phrase);
}
//Console.WriteLine("tra" + manyPhrases);
```

### Unsigned Data Type

- In general, use `int` rather than unsigned types. The use of `int` is
common throughout C#, and it is easier to interact with other libraries
when you use `int`.

### Arrays

- Use the concise syntax when you initialize arrays on the declaration
line.

```C#
// Preferred syntax. Note that you cannot use var here instead of string[].
string[] vowels1 = { "a", "e", "i", "o", "u" };


// If you use explicit instantiation, you can use var.
var vowels2 = new string[] { "a", "e", "i", "o", "u" };
```

### Delegates

- Use the concise syntax to create instances of a delegate type.

```C#
// First, in class Program, define the delegate type and a method that
// has a matching signature.

// Define the type.
public delegate void Del(string message);

// Define a method that has a matching signature.
public static void DelMethod(string str)
{
    Console.WriteLine("DelMethod argument: {0}", str);
}
```

```C#
// In the Main method, create an instance of Del.

// Preferred: Create an instance of Del by using condensed syntax.
Del exampleDel2 = DelMethod;

// The following declaration uses the full syntax.
Del exampleDel1 = new Del(DelMethod);
```

### try-catch and using Statements

- Use a [try-catch][try_catch] statement for most exception handling.

```C#
static string GetValueFromArray(string[] array, int index)
{
    try
    {
        return array[index];
    }
    catch (System.IndexOutOfRangeException ex)
    {
        Console.WriteLine("Index is out of range: {0}", index);
        throw;
    }
}
```

- Simplify your code by using the C# [using statement][using_statement].
If you have a [try-finally][try_finally] statement in which the only
code in the `finally` block is a call to the [Dispose][dispose] method,
use a `using` statement instead.

```C#
// This try-finally statement only calls Dispose in the finally block.
Font font1 = new Font("Arial", 10.0f);
try
{
    byte charset = font1.GdiCharSet;
}
finally
{
    if (font1 != null)
    {
        ((IDisposable)font1).Dispose();
    }
}


// You can do the same thing with a using statement.
using (Font font2 = new Font("Arial", 10.0f))
{
    byte charset = font2.GdiCharSet;
}
```

### && and || Operators

- To avoid exceptions and increase performance by skipping unnecessary
comparisons, use [&&][conditional_and] instead of [&][boolean_and] and
[||][logical_or] instead of [|][boolean_or] when you perform
comparisons, as shown in the following example.

```C#
Console.Write("Enter a dividend: ");
var dividend = Convert.ToInt32(Console.ReadLine());

Console.Write("Enter a divisor: ");
var divisor = Convert.ToInt32(Console.ReadLine());

// If the divisor is 0, the second clause in the following condition
// causes a run-time error. The && operator short circuits when the
// first expression is false. That is, it does not evaluate the
// second expression. The & operator evaluates both, and causes
// a run-time error when divisor is 0.
if ((divisor != 0) && (dividend / divisor > 0))
{
    Console.WriteLine("Quotient: {0}", dividend / divisor);
}
else
{
    Console.WriteLine("Attempted division by 0 ends up here.");
}
```

### New Operator

- Use the concise form of object instantiation, with implicit typing, as
shown in the following declaration.

```C#
var instance1 = new ExampleClass();
```

The previous line is equivalent to the following declaration.

```C#
ExampleClass instance2 = new ExampleClass();
```

- Use object initializers to simplify object creation.

```C#
// Object initializer.
var instance3 = new ExampleClass { Name = "Desktop", ID = 37414,
    Location = "Redmond", Age = 2.3 };

// Default constructor and assignment statements.
var instance4 = new ExampleClass();
instance4.Name = "Desktop";
instance4.ID = 37414;
instance4.Location = "Redmond";
instance4.Age = 2.3;
```

### Event Handling

- If you are defining an event handler that you do not need to remove
later, use a lambda expression.

```C#
public Form2()
{
    // You can use a lambda expression to define an event handler.
    this.Click += (s, e) =>
        {
            MessageBox.Show(
                ((MouseEventArgs)e).Location.ToString());
        };
}
```

```C#
// Using a lambda expression shortens the following traditional definition.
public Form1()
{
    this.Click += new EventHandler(Form1_Click);
}

void Form1_Click(object sender, EventArgs e)
{
    MessageBox.Show(((MouseEventArgs)e).Location.ToString());
}
```

### Static Members

- Call [static][static] members by using the class name:
_ClassName.StaticMember_. This practice makes code more readable by
making static access clear.
- Do not qualify a static member defined in a base class with the name
of a derived class. While that code compiles, the code readability is
misleading, and the code may break in the future if you add a static
member with the same name to the derived class.

### LINQ Queries

- Use meaningful names for query variables. The following example uses
`seattleCustomers` for customers who are located in Seattle.

```C#
var seattleCustomers = from cust in customers
                       where cust.City == "Seattle"
                       select cust.Name;
```

- Use aliases to make sure that property names of anonymous types are
correctly capitalized, using Pascal casing.

```C#
var localDistributors =
    from customer in customers
    join distributor in distributors on customer.City equals distributor.City
    select new { Customer = customer, Distributor = distributor };
```

- Rename properties when the property names in the result would be
ambiguous. For example, if your query returns a customer name and a
distributor ID, instead of leaving them as `Name` and `ID` in the
result, rename them to clarify that `Name` is the name of a customer,
and `ID` is the ID of a distributor.

```C#
var localDistributors2 =
    from cust in customers
    join dist in distributors on cust.City equals dist.City
    select new { CustomerName = cust.Name, DistributorID = dist.ID };
```

- Use implicit typing in the declaration of query variables and range
variables.

```C#
var seattleCustomers = from cust in customers
                       where cust.City == "Seattle"
                       select cust.Name;
```

- Align query clauses under the from clause, as shown in the previous
examples.
- Use [where][where] clauses before other query clauses to ensure that
later query clauses operate on the reduced, filtered set of data.

```C#
var seattleCustomers2 = from cust in customers
                        where cust.City == "Seattle"
                        orderby cust.Name
                        select cust;
```

- Use multiple `from` clauses instead of a [join][join] clause to access
inner collections. For example, a collection of `Student` objects might
each contain a collection of test scores. When the following query is
executed, it returns each score that is over 90, along with the last
name of the student who received the score.

```C#
// Use a compound from to access the inner sequence within each element.
var scoreQuery = from student in students
                 from score in student.Scores
                 where score > 90
                 select new { Last = student.LastName, score };
```

### Solution Folder Structure

- All projects must comply with the following code structure.

```
SolutionName\
    .github\
        PULL_REQUEST_TEMPLATE.md
        ISSUE_TEMPLATE\
            bug_report.md
            feature_request.md
    doc\
    src\
        ProjectName1\
            ProjectName1.csproj
            ClassName.cs
        ProjectName2\
    test\
        ProjectName1\
            ProjectName1.Tests.csproj
            ClassNameTests.cs
        ProjectName2\
    sub\
        SubmoduleSolution1\
        SubmoduleSolution2\
    SolutionName.ruleset
    SolutionName.sln
    SolutionName.sln.licenseheader
    CODE_OF_CONDUCT.md
    CODE_STYLES.md
    CONTRIBUTING.md
    LICENSE.md
    README.md
    VISION.md
```

- A class of name `ClassName` will exist in the file
`SolutionName\src\ProjectName1\ClassName.cs`. Its fully qualified name
will be `OrganizationName.SolutionName.ProjectName1.ClassName`.
- Any unit tests that belong to `ClassName` will exist in the file
`SolutionName\test\ProjectName1\ClassNameTests.cs`, which will contain
the class `ClassNameTests` in the same namespace as `ClassName`,
`OrganizationName.SolutionName.ProjectName1`.
- Output assemblies will be the same name as their .csproj file names
and exist in a folder with the same name as your organization.

### Unit Tests

- Use [XUnit][xunit] for all tests.
- To test a method named `Foo` in class `Class1`, create a test method
named `Foo` in the test class `Class1Tests`.
- Use `Theory` attributes over multiple tests methods where possible.
The `Theory` attribute should almost always be able to fully test a
method in one test method. In cases where this is not possible, consider
refactoring the `Foo` method to make it possible. If this cannot be
done, name the multiple test methods after what is being tested. For
example `FooThrowNull` and `FooThrowOverflow`.

[guide]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions
[using]: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-directive
[interface]: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/interface
[classes]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/classes
[properties]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/properties
[structs]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/structs
[access]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers
[members]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/members
[initializers]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/object-and-collection-initializers
[patterns]: https://docs.microsoft.com/en-us/dotnet/csharp/pattern-matching
[tuples]: https://docs.microsoft.com/en-us/dotnet/csharp/tuples
[defaults]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/default-value-expressions
[expressions]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members
[outvar]: https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-7#out-variables
[null_op]: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/expressions#the-null-coalescing-operator
[stringbuilder]: https://docs.microsoft.com/en-us/dotnet/api/system.text.stringbuilder?view=netframework-4.7
[try_catch]: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/try-catch
[using_statement]: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-statement
[try_finally]: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/try-finally
[dispose]: https://docs.microsoft.com/en-us/dotnet/api/system.idisposable.dispose?view=netframework-4.7
[conditional_and]: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-and-operator
[boolean_and]: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/and-operator
[logical_or]: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-or-operator
[boolean_or]: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/or-operator
[static]: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/static
[where]: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/where-clause
[join]: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/join-clause
[xunit]: https://xunit.github.io/
[attribute]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/attributes
