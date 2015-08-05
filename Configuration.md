**Note: All examples are based on the latest version of NHaml. Check the [GettingNHaml](GettingNHaml.md) for information about getting the latest version.**

# Introduction #

An example App.config for nhaml.

```

<?xml version="1.0"?>

<configuration>

	<configSections>
		<section name="nhaml" type="NHaml.Configuration.NHamlConfigurationSection, NHaml"/>
	</configSections>

  <nhaml autoRecompile="false" templateCompiler="CSharp3" encodeHtml="false" useTabs="false" indentSize="2">
		<assemblies>
			<clear/>
			<add assembly="System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=B77A5C561934E089"/>
		</assemblies>
		<namespaces>
			<clear/>
			<add namespace="System.Collections"/>
		</namespaces>
	</nhaml>

</configuration>

```

# autoRecompile #

NHaml views are compiled. The first time an NHaml view is requested it is parsed and compiled into a .NET Type capable of rendering the view. This Type is then cached. The view engine can then be set to autoRecompile. When autoRecompile is set to true the view engine will always recompile a view . When autoRecompile is set to false the view engine will only recompile a cached view if any of it's constituent haml files are modified. autoRecompile may be enabled through the web.config file like so:

# templateCompiler #

This controls the temaplte compiler that will be used for the inline code used in views. Shorthand is supported for C# i.e. CSharp2, CSharp2 and CSharp4. For all other compilers fully qualified type name is required. The currently supported compilers are

  * NHaml.Compilers.CSharp2.CSharp2TemplateCompiler, NHaml
  * NHaml.Compilers.CSharp3.CSharp3TemplateCompiler, NHaml
  * NHaml.Compilers.CSharp4.CSharp4TemplateCompiler, NHaml
  * NHaml.Compilers.IronRuby.IronRubyTemplateCompiler, NHaml.Compilers.IronRuby
  * NHaml.Compilers.VisualBasic.VisualBasicTemplateCompiler, NHaml.Compilers.VisualBasic
  * NHaml.Compilers.Boo.BooTemplateCompiler, NHaml.Compilers.Boo
  * NHaml.Compilers.FSharp.FSharpTemplateCompiler, NHaml.Compilers.FSharp

# useTabs #

nhaml is a indentation sensitive lanugage. i.e. the level of indentation is used to control logic. The _useTabs_ setting is used to define what type of indentation character to use.

_true_ to use tabs _false_ (or non-existant) for spaces.

# indentSize #

The number of tabs or spaces to indicate an indent.

# outputDebugFiles #

Used to output temporary source code files for debugging purposes.See [DebugExceptionsOrStepThroughTemplates](DebugExceptionsOrStepThroughTemplates.md)

# assemblies #

All assemblies that are necessary for compilation.

# namespaces #

All namespaces that are necessary for compilation.