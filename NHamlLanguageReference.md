**Note: All examples are based on the latest version of NHaml. Check the [GettingNHaml](GettingNHaml.md) for information about getting the latest version.**

**Note: All examples where code is used are in C# unless specified otherwise**

Disclaimer: Most of this reference was lifted from the main Haml site.

# Tags '%' #

The '%' character is placed at the beginning of a line. It‘s followed immediately by the name of an element, then optionally by modifiers (see below), a space, and text to be rendered inside the element. It creates an element in the form of `<element></element>`.

For example:

```
%one
  %two
    %three Hey there
```

is compiled to:

```
<one>
  <two>
    <three>Hey there</three>
  </two>
</one>
```

Any string is a valid element name; NHaml will automatically generate opening and closing tags for any element.

# Attributes "{}" #

## NHaml 2.0 Beta 1 Syntax ##

Braces represent the .net anonymous type object initializer statement that is used for specifying the attributes of an element. It is evaluated as an anonymous type, so
logic will work in it and local variables may be used. The braces are placed after the tag is defined. For example if you are using the C# anonymous syntax:

```
%head
  %title My Sample MVC Application
  %link{ href="../../Content/Sites", rel="stylesheet", type="text/css" }
```

is compiled to:

```
<head>
  <title>My Sample MVC Application</title>
  <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
</head>
```


## NHaml 2.0 Beta 2 Syntax ##

In Beta 2 the attribute syntax has been brough inline with where the next version haml.

Braces represent a HTML style of attribute of an element with a few special syntax.
  * quoted values are interpreted as strings
  * single and double quotes area allowed
  * unquotes values (without spaces) are interpreted as code
  * the syntax #{...} inside quoted values are interpreted as code. so this effectively allows you to inline code and concatenate it with text.
  * **No commas required for separation**

```
a                       output is a="a"
a = b                   output is a="[value of variable b]"
a = "b"                 output is a="b"
a = 'b'                 output is a="b"
a = #{1+1}              output is a="2"
a = "#{1+1}abc"         output is a="2abc"
```

For example:

```
%head
  %title My Sample MVC Application
  %link{ href="../../Content/Sites" rel="stylesheet" type="text/css" }
```

is compiled to:

```
<head>
  <title>My Sample MVC Application</title>
  <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
</head>
```

# Self-closing Tags '/' #

The '/' character, when placed at the end of a tag definition, causes the tag to be self-closed. For example:

```
%br/
%meta{ http-equiv="Content-Type" content="text/html"}/
```

is compiled to:

```
<br />
<meta http-equiv="Content-Type" content="text/html" />
```

Some tags are automatically closed, as long as they have no content. meta, img, link, script, br, input and hr tags are closed by default.

```
%br
%meta{ http-equiv="Content-Type" content="text/html" }
```

is also compiled to:

```
<br />
<meta http-equiv="Content-Type" content="text/html" />
```

# Class and Id '.' and '#' #

The ’.’ and ’#’ are borrowed from CSS. They are used as shortcuts to specify the class and id attributes of an element, respectively. Multiple class names can be specified in a similar way to CSS, by chaining the class names together with periods. They are placed immediately after the tag and before an attributes hash. For example:

```
%div#things
  %span#rice Chicken Fried
  %p.beans{ food = "true" } The magical fruit
  %h1.class.otherclass#id La La La
```

is compiled to:

```
<div id="things">
  <span id="rice">Chicken Fried</span>
  <p class="beans" food="true">The magical fruit</p>
  <h1 class="class otherclass" id="id">La La La</h1>
</div>
```

And,

```
#content
  .articles
    .article.title
      Doogie Howser Comes Out
    .article.date
      2006-11-05
    .article.entry
      Neil Patrick Harris would like to dispel any rumors that he is straight
```

is compiled to:

```
<div id="content">
  <div class="articles">
    <div class="article title">Doogie Howser Comes Out</div>
    <div class="article date">2006-11-05</div>
    <div class="article entry">
      Neil Patrick Harris would like to dispel any rumors that he is straight
    </div>
  </div>
</div>
```

# Implicit Div Elements #

Because the div element is used so often, it is the default element. If you only define a class and/or id using the . or # syntax, a div element is automatically
used. For example:

```
#collection
  .item
    .description What a cool item!
```

is the same as:

```
%div{ id = "collection" }
  %div{ class = "item" }
    %div{ class = "description" } What a cool item!
```

and is compiled to:

```
<div id="collection">
  <div class="item">
    <div class="description">What a cool item!</div>
  </div>
</div>
```

# No Special Character #

If no special character appears at the beginning of a line, the line is rendered as plain text. For example:

```
%gee
  %whiz
    Wow this is cool!
```

is compiled to:

```
<gee>
  <whiz>
    Wow this is cool!
  </whiz>
</gee>
```

# DOCTYPES "!!!" #

When describing XHTML documents with NHaml, you can have a document type or XML prolog generated automatically by including the characters !!!. For example:

```
!!! XML
!!!
%html
  %head
    %title Myspace
  %body
    %h1 I am the international space station
    %p Sign my guestbook
```

is compiled to:

```
<?xml version="1.0" encoding="utf-8" ?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
  <head>
    <title>Myspace</title>
  </head>
  <body>
    <h1>I am the international space station</h1>
    <p>Sign my guestbook</p>
  </body>
</html>
```

You can also specify the version and type of XHTML after the !!!. XHTML 1.0 Strict, Transitional, and Frameset and XHTML 1.1 are supported. The default version is 1.0 and the default type is Transitional. For example:

```
!!! 1.1
```

is compiled to:

```
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
```

and

```
!!! Strict
```

is compiled to:

```
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
```

If you‘re not using the UTF-8 characterset for your document, you can specify which encoding should appear in the XML prolog in a similar way. For example:

```
!!! XML iso-8859-1
```

is compiled to:

```
<?xml version="1.0" encoding="iso-8859-1" ?>
```

# XHTML Comments '/' #

The ’/’ character, when placed at the beginning of a line, wraps all text after it in an HTML comment. For example:

```
%billabong
  / This is the billabong element
  I like billabongs!
```

is compiled to:

```
<billabong>
  <!-- This is the billabong element -->
  I like billabongs!
</billabong>
```

The forward slash can also wrap indented sections of code. For example:

```
/
  %p This doesn't render...
  %div
    %h1 Because it's commented out!
```

is compiled to:

```
<!--
  <p>
This doesn't render...</p>
  <div>
    <h1>Because it"s commented out!</h1>
  </div>
-->
```

You can also use Internet Explorer conditional comments (about) by enclosing the condition in square brackets after the /. For example:

```
/[if IE]
  %a{ :href = "http://www.mozilla.com/en-US/firefox/" }
    %h1 Get Firefox
```

is compiled to:

```
<!--[if IE]>

  <a href="http://www.mozilla.com/en-US/firefox/">
    <h1>Get Firefox</h1>
  </a>
<![endif]-->
```

# Escape Sequence '\' #

The backslash character escapes the first character of a line, allowing use of otherwise interpreted characters as plain text. For example:

```
%title
  = @title
  \- MySite
```

is compiled to:

```
<title>
  MyPage
  - MySite
</title>
```

# Line Continuation '|' #

The pipe character designates a multiline string. It‘s placed at the end of a line and means that all following lines that end with '|' will be evaluated as though they were on the same line. For example:

```
%whoo
  %hoo I think this might get |
    pretty long so I should |
    probably make it |
    multiline so it doesn"t |
    look awful. |
  %p This is short.
```

is compiled to:

```
<whoo>
  <hoo>
    I think this might get pretty long so I should probably make it multiline so it doesn"t look awful.
  </hoo>
  <p>This is short</p>
</whoo>
```

# Partials & Layouts '`_`' #

Use an '`_`' to render a partial or the content of a layout. For a partial, specify the name of the partial after the underscore like so:

```
%p
  _ Customer
```

will render the _Customer.haml partial file._

An `_` on it”s own is used to specify where the content within a layout will be inserted:

```
%p
  _
```


# Evaluate Output '`=`' #
The '`=`' character is followed by code, which is evaluated and the output inserted into the document as plain text. For example:

```
%p
  = "hi" + " there" + " reader!" 
  = "yo"
```

is compiled to:

```
<p>
  hi there reader!
  yo
</p>
```

When '`=`' is placed at the end of a tag definition, after class, id, and attribute declarations. It‘s just a shortcut for inserting code into an element. It works the same as = without a tag: it inserts the result of the code into the template. For example:

```
%p= string.Join(" ", new string[]{"He", "braid", "runner!"})
```

Results in:

```
<p>He braid runner!</p>
```

# HTML Encode Output '`&=`' #
The '`&=`' string is followed by code, which is evaluated and the output inserted into the document after being HTML encoded. For example:

```
&= "LessThan <"
&= "GreaterThan >"
```

is compiled to:

```
LessThan &lt;
GreaterThan &gt;
```

'`&`' can also be used on its own so that #{} interpolation is escaped. For example,

```
& I like #{"cheese & crackers"}
```

is compiled to:

```
I like cheese &amp; crackers
```

# Not HTML Encode Output '`!=`' #

The '!=' string will result in the output never being html encoded.

By default, the single equals doesn‘t sanitize HTML either. However, if the EncodeHtml option is set, = will sanitize the HTML, but != still won‘t. For example, if EncodeHtml is set:

```
= "I feel <strong>!"
!= "I feel <strong>!"
```

is compiled to:

```
I feel &lt;strong&gt;!
I feel <strong>!
```

'!' can also be used on its own so that #{} interpolation is unescaped. For example,

```
! I feel #{"<strong>"}!
```

is compiled to:

```
I feel <strong>!
```

# Evaluate Silent '-' #

The '-' character makes the text following it into “silent” code: code that is evaluated, but not output.

It is not recommended that you use this widely; almost all processing code and logic should be restricted to the Controller, Helpers, or partials.

For example:

```
- string foo = "hello" 
- foo += " there" 
- foo += " you!" 
%p= foo
```

is compiled to:

```
<p>
  hello there you!
</p>
```


# Universal interpolation #

Code can be embedded in text throught he use of #{}. For example:

```
%p This is a really cool #{h what_is_this}!
But is it a #{h what_isnt_this}?
```

In addition, to escape or unescape the interpolated code, you can just add & or !, respectively, to the beginning of the line:

```
%p& This is a really cool #{what_is_this}!
& But is it a #{what_isnt_this}?
```

# Code Blocks #

Code blocks, like XHTML tags, don‘t need to be explicitly closed in NHaml. Rather, they‘re automatically closed, based on indentation. A block begins whenever the indentation is increased after a silent script command. It ends when the indentation decreases.

```
- for (int i=42; i<47; i++)
  %p= i
%p See, I can count!
```

is compiled to:

```
<p>
  42
</p>
<p>
  43
</p>
<p>
  44
</p>
<p>
  45
</p>
<p>
  46
</p>
<p>See, I can count!</p>
```

# Silent Comments '-//' #
The hyphen followed immediately by a code comment has the effect of a silent comment. Any text following this isn‘t rendered in the resulting document at all.

For example:

```
%p foo 
  -// This is a comment %p bar
```

is compiled to:

```
<p>
  foo
</p>
```


Source from http://andrewpeters.net/2007/12/19/introducing-nhaml-an-aspnet-mvc-view-engine/#reference