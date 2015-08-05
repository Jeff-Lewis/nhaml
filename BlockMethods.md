**Note: All examples are based on the latest version of NHaml. Check the [GettingNHaml](GettingNHaml.md) for information about getting the latest version.**

An example (albeit contrived) to demonstrate:

```
- Html.Tag("div") () =>
  Some regular NHaml markup here...
```
which results in:

```
<div>
  Some regular NHaml markup here...
</div>
```
And the Tag method looks like this:

```
public void Tag(string name, Action yield)
{
  _output.WriteLine("<" + name + ">");
  yield();
  _output.WriteLine("</" + name + ">");
}
```
As we can see, NHaml wraps up the nested block markup into a lambda (or anonymous method if targeting C# 2.0) and passes a delegate to the block as the last argument of the helper method. Invoking the delegate causes the markup within the block to be rendered. Notice too, we can write directly to the output stream: _output is an instance of NHaml’s IOutputWriter which is passed to our helper class when it’s instantiated. It has the following methods:_

```
void WriteLine(string value);
void Write(string value);
void Indent();
void Outdent();
```
Output written using these methods will be correctly indented and we can also further control the indent level if we need to write nested output.

Worth mentioning is that we can also pass arguments to a block:

```
- Html.Tag("div", 21, 21) (i, j) =>
  = "N was: " + (i + j)
```
which yields:

```
<div>
  N was: 42
</div>
```
And our helper method:

```
public void Tag<T1, T2>(string name, T1 t1, T2 t2, Action<T1, T2> yield)
{
  _output.WriteLine("<" + name + ">");
  yield(t1, t2);
  _output.WriteLine("</" + name + ">");
}
```

source from http://andrewpeters.net/2008/08/21/nhaml-block-methods-and-restful-helpers/